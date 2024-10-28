using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace UnistrokeGestureRecognition {
    public static class GestureMath {

        #region - Normalize - 

        /// <summary>
        /// Normalize path at native slice buffer inside rect. 
        /// </summary>
        /// <param name="rect">Normalize path rect</param>
        /// <param name="path">Path to normalize</param>
        public static void NormalizePath(float4 rect, NativeSlice<float2> path) {
            for (int o = 0; o < path.Length; o++) {
                var point = path[o];
                var nPoint = new float2((point.x - rect.x) / rect.z, (point.y - rect.y) / rect.w);
                path[o] = nPoint;
            }
        }

        /// <summary>
        /// Normalize path at list inside rect. 
        /// </summary>
        /// <param name="rect">Normalize path rect</param>
        /// <param name="path">Path to normalize</param>
        public static void NormalizePath(Rect rect, List<Vector2> path) {
            for (int i = 0; i < path.Count; i++) {
                var nPoint = Rect.PointToNormalized(rect, path[i]);
                path[i] = nPoint;
            }
        }

        #endregion

        #region - FindGestureRect - 

        /// <summary>
        /// Find gesture un uniform rect in native slice path.
        /// </summary>
        /// <param name="path">Gesture path</param>
        /// <returns>Un uniform rect</returns>
        public static float4 FindGestureUnUniformRect(NativeSlice<float2> path) {
            var bounds = FindPathBounds(path);
            return BoundsToUnUniformRect(bounds);
        }

        /// <summary>
        /// Find gesture un uniform rect in read only collection.
        /// </summary>
        /// <param name="path">Gesture path</param>
        /// <returns>Un uniform rect</returns>
        public static Rect FindGestureUnUniformRect(ReadOnlyCollection<Vector2> path) {
            var bounds = FindPathBounds(path);
            var rect = BoundsToUnUniformRect(bounds);
            return new(rect.x, rect.y, rect.z, rect.w);
        }

        /// <summary>
        /// Find gesture uniform rect in native slice path.
        /// </summary>
        /// <param name="path">Gesture path</param>
        /// <returns>Uniform rect</returns>
        public static float4 FindGestureUniformRect(NativeSlice<float2> path) {
            var bounds = FindPathBounds(path);
            return BoundsToUniformRect(bounds);
        }

        /// <summary>
        /// Find gesture uniform rect in read only collection.
        /// </summary>
        /// <param name="path">Gesture path</param>
        /// <returns>Uniform rect</returns>
        public static Rect FindGestureUniformRect(ReadOnlyCollection<Vector2> path) {
            var bounds = FindPathBounds(path);
            var rect = BoundsToUniformRect(bounds);
            return new(rect.x, rect.y, rect.z, rect.w);
        }

        # endregion

        #region - Resample -

        /// <summary>
        /// Resample gesture path and finds path bounds.
        /// </summary>
        /// <param name="readBuffer">Read path buffer</param>
        /// <param name="writeBuffer">Write resample path buffer</param>
        /// <returns>Path bounds</returns>
        public static float4 ResamplePath(NativeArray<float2> readBuffer, NativeArray<float2> writeBuffer) 
            => ResamplePath(readBuffer.Slice(), writeBuffer.Slice());

        /// <inheritdoc cref="ResamplePath"/>
        public static float4 ResamplePath(NativeArray<float2> readBuffer, NativeSlice<float2> writeBuffer) 
            => ResamplePath(readBuffer.Slice(), writeBuffer);

        /// <inheritdoc cref="ResamplePath"/>
        public static float4 ResamplePath(NativeSlice<float2> readBuffer, NativeSlice<float2> writeBuffer) {
            int writePointsCount = writeBuffer.Length;
            int readPointsCount = readBuffer.Length;

            if (writePointsCount < 2 || readPointsCount < 2) return float4.zero;

            float xMin = float.MaxValue;
            float yMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMax = float.MinValue;

            float interval = CalculatePathLength(readBuffer) / (writePointsCount - 1);

            writeBuffer[0] = readBuffer[0];

            var previousPoint = readBuffer[0];
            var currentPoint = readBuffer[1];

            int writeIndex = 1;
            int readIndex = 2;

            int counter = readPointsCount;

            float path = 0f;
            for (int i = 1; i < counter; i++) {
                float d = math.distance(previousPoint, currentPoint);

                if (path + d < interval) {
                    if (readIndex == readPointsCount)
                        break;

                    path += d;
                    previousPoint = currentPoint;
                    currentPoint = readBuffer[readIndex++];

                    continue;
                }

                var newPoint = previousPoint + (interval - path) / d * (currentPoint - previousPoint);

                if (newPoint.x > xMax) xMax = newPoint.x;
                if (newPoint.x < xMin) xMin = newPoint.x;
                if (newPoint.y > yMax) yMax = newPoint.y;
                if (newPoint.y < yMin) yMin = newPoint.y;

                writeBuffer[writeIndex++] = newPoint;
                previousPoint = newPoint;
                path = 0f;
                counter++;
            }

            writeBuffer[^1] = readBuffer[^1];

            return new(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// Calculate gesture path length by summing 
        /// distance between adjacent points. 
        /// </summary>
        /// <param name="path">Gesture path</param>
        /// <returns>Length of the path</returns>
        public static float CalculatePathLength(NativeSlice<float2> path) {
            float pathLength = 0f;
            for (int o = 1; o < path.Length; o++) {
                pathLength += math.distance(path[o - 1], path[o]);
            }
            return pathLength;
        }

        #endregion

        #region  - Resample and Normalize -

        /// <summary>
        /// Resample gesture path and normalize it with selected strategy.
        /// </summary>
        /// <param name="readBuffer">Read path buffer</param>
        /// <param name="writeBuffer">Write resample path buffer</param>
        /// <param name="isUniform">Normalize strategy</param>
        public static void ResampleAndNormalizePath(NativeArray<float2> readBuffer, NativeArray<float2> writeBuffer, bool isUniform) 
            => ResampleAndNormalizePath(readBuffer.Slice(), writeBuffer.Slice(), isUniform);

        /// <inheritdoc cref="ResampleAndNormalizePath"/>
        public static void ResampleAndNormalizePath(NativeArray<float2> readBuffer, NativeSlice<float2> writeBuffer, bool isUniform) 
            => ResampleAndNormalizePath(readBuffer.Slice(), writeBuffer, isUniform);

        /// <inheritdoc cref="ResampleAndNormalizePath"/>
        public static void ResampleAndNormalizePath(NativeSlice<float2> readBuffer, NativeSlice<float2> writeBuffer, bool isUniform) {
            var bounds = ResamplePath(readBuffer, writeBuffer);
            var rect = isUniform ? BoundsToUniformRect(bounds) : BoundsToUnUniformRect(bounds);
            NormalizePath(rect, writeBuffer);
        }

        #endregion

        #region  - Bounds math - 

        /// <summary>
        /// Find gesture path bounds (bottom left and top right points) in NativeSlice.
        /// </summary>
        /// <param name="path">Gesture path</param>
        /// <returns>
        /// Path bounds (x - left point, y - bottom point, z - right point, w - top point) 
        /// </returns>
        public static float4 FindPathBounds(NativeSlice<float2> path) {
            float xMin = float.MaxValue;
            float yMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMax = float.MinValue;

            foreach (var point in path) {
                if (point.x > xMax) xMax = point.x;
                if (point.x < xMin) xMin = point.x;
                if (point.y > yMax) yMax = point.y;
                if (point.y < yMin) yMin = point.y;
            }

            return new(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// Find gesture path bounds (bottom left and top right points) in ReadOnlyCollection.
        /// </summary>
        /// <inheritdoc cref="FindPathBounds"/>
        public static float4 FindPathBounds(ReadOnlyCollection<Vector2> path) {
            float xMin = float.MaxValue;
            float yMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMax = float.MinValue;

            foreach (var point in path) {
                if (point.x > xMax) xMax = point.x;
                if (point.x < xMin) xMin = point.x;
                if (point.y > yMax) yMax = point.y;
                if (point.y < yMin) yMin = point.y;
            }

            return new(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// Transform bounds to Uniform rect 
        /// (top left point, width and height, where width = height)
        /// </summary>
        /// <param name="bounds">Gesture bounds</param>
        /// <returns>
        /// Uniform rect (x - left point, y - top point, z - width, w - height)
        /// </returns>
        public static float4 BoundsToUniformRect(float4 bounds) 
            => BoundsToUniformRect(bounds.x, bounds.y, bounds.z, bounds.w);

        /// <summary>
        /// <inheritdoc cref="BoundsToUniformRect"/>
        /// </summary>
        /// <param name="xMin">Left point</param>
        /// <param name="yMin">Bottom point</param>
        /// <param name="xMax">Right point</param>
        /// <param name="yMax">Top point</param>
        /// <inheritdoc cref="BoundsToUniformRect"/>
        public static float4 BoundsToUniformRect(float xMin, float yMin, float xMax, float yMax) {
            float width = xMax - xMin;
            float height = yMax - yMin;
            if (width > height) {
                float halfWith = width / 2;
                return new(xMin, (yMax + yMin) / 2 - halfWith, width, width);
            }
            float halfHeight = height / 2;
            return new((xMax + xMin) / 2 - halfHeight, yMin, height, height);
        }

        /// <summary>
        /// Transform bounds to Un uniform rect 
        /// (top left point, width and height)
        /// </summary>
        /// <param name="bounds">Gesture bounds</param>
        /// <returns>
        /// Un uniform rect (x - left point, y - top point, z - width, w - height)
        /// </returns>
        public static float4 BoundsToUnUniformRect(float4 bounds) 
            => BoundsToUnUniformRect(bounds.x, bounds.y, bounds.z, bounds.w);

        /// <summary>
        /// <inheritdoc cref="BoundsToUnUniformRect"/>
        /// </summary>
        /// <param name="xMin">Left point</param>
        /// <param name="yMin">Bottom point</param>
        /// <param name="xMax">Right point</param>
        /// <param name="yMax">Top point</param>
        /// <inheritdoc cref="BoundsToUnUniformRect"/>
        public static float4 BoundsToUnUniformRect(float xMin, float yMin, float xMax, float yMax) {
            float width = xMax - xMin;
            float height = yMax - yMin;
            return new(xMin, yMin, width, height);
        }

        #endregion
    }
}