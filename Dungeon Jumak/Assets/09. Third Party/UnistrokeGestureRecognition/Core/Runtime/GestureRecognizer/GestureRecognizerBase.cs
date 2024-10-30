using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnistrokeGestureRecognition {
    /// <inheritdoc cref="IGestureRecognizer"/>
    public abstract class GestureRecognizerBase<G> : IGestureRecognizer<G> where G : IGesturePattern {
        protected readonly int _ResamplePointsNumber;
        public abstract RecognizeResult<G> Result { get; }
        public abstract RecognizeResult<G> Recognize(NativeSlice<float2> path, bool runParallel = true);
        public abstract JobHandle ScheduleRecognition(NativeSlice<float2> path, bool runParallel = true);

        protected GestureRecognizerBase(int resamplePointsNumber) {
            _ResamplePointsNumber = resamplePointsNumber;
        }

        public abstract void Dispose();

        protected void PreparePatterns(List<G> patterns, NativeArray<float2> resampleBuffer) {
            for (int i = 0; i < patterns.Count; i++) {
                var pattern = patterns[i];
                var patternPath = pattern.Path;

                if (patternPath.Length < 2) {
                    throw new ArgumentException("Patter must have at least 2 points!");
                }

                var tmpPathBuffer = new NativeArray<float2>(patternPath.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

                for (int o = 0; o < tmpPathBuffer.Length; o++) {
                    tmpPathBuffer[o] = patternPath[o];
                }

                var slice = resampleBuffer.Slice(i * _ResamplePointsNumber, _ResamplePointsNumber);
                GestureMath.ResampleAndNormalizePath(tmpPathBuffer, slice, pattern.ScalingMode == GestureScalingMode.Uniform);
            }
        }

        static protected (float distance, int index) FindBestDistance(NativeArray<float> distanceBuffer) {
            float bestDistance = float.MaxValue;
            int bestDistanceIndex = 0;

            for (int i = 0; i < distanceBuffer.Length; i++) {
                var distance = distanceBuffer[i];
                if (distance < bestDistance) {
                    bestDistance = distance;
                    bestDistanceIndex = i;
                }
            }

            return (bestDistance, bestDistanceIndex);
        }
    }
}