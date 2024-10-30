using System;
using UnistrokeGestureRecognition;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// <inheritdoc cref="IGestureRecorder"/>
    /// It uses a resampling algorithm to capture long paths to a limited size buffer.
    /// </summary>
    public sealed class GestureRecorder : IGestureRecorder {
        public int Length => _pointsCounter;

        private NativeArray<float2> _pathBuffer1;
        private NativeArray<float2> _pathBuffer2;

        private readonly int _maxLength;

        private int _currentBuffer = 0;
        private int _pointsCounter = 0;

        private readonly float _newPointMinDistance;
        private float2? _previousPoint = null;

        private NativeArray<float2> CurrentBuffer => _currentBuffer == 0 ?
            _pathBuffer1 :
            _pathBuffer2;

        private (NativeArray<float2> currentBuffer, NativeArray<float2> nextBuffer) Buffers
        => _currentBuffer == 0 ?
                (_pathBuffer1, _pathBuffer2) :
                (_pathBuffer2, _pathBuffer1);

        public NativeSlice<float2> Path => CurrentBuffer.Slice(0, _pointsCounter);

        /// <summary>
        /// <inheritdoc cref="GestureRecorder"/>
        /// </summary>
        /// <param name="pathMaxLength">
        /// The first value specifies the maximum number of points in the path buffer.
        /// A higher value gives a more accurate path, but requires more memory.
        /// </param>
        /// <param name="newPointMinDistance">
        /// If the distance is less than required, the new point will not be added to the recorded path.
        /// Useful when you want to exclude points from a path when the user keeps the cursor in one place.
        /// </param>
        public GestureRecorder(int pathMaxLength, float newPointMinDistance = 0f) {
            if (pathMaxLength < 2) throw new ArgumentOutOfRangeException(nameof(pathMaxLength));

            _maxLength = pathMaxLength;
            _pathBuffer1 = new NativeArray<float2>(pathMaxLength, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            _pathBuffer2 = new NativeArray<float2>(pathMaxLength, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            if (newPointMinDistance < 0) throw new ArgumentOutOfRangeException(nameof(newPointMinDistance));

            _newPointMinDistance = newPointMinDistance;
        }

        /// <summary>
        /// Add a new point to the gesture path.
        /// If newPointMinDistance is not 0, the distance between the previous and new point is also checked. 
        /// If the distance is less than the required value, no new point will be added.
        /// </summary>
        /// <param name="point">New point</param>
        public void RecordPoint(Vector2 point) => RecordPoint((float2)point);

        /// <inheritdoc cref="RecordPoint"/>
        public void RecordPoint(float2 point) {
            if (!IsCanRecord(point)) return;

            _previousPoint = point;

            if (_pointsCounter < _maxLength) {
                if (_currentBuffer == 0) _pathBuffer1[_pointsCounter] = point;
                else _pathBuffer2[_pointsCounter] = point;

                _pointsCounter += 1;
                return;
            }

            (var currentBuffer, var nextBuffer) = Buffers;

            ResamplePath(currentBuffer, nextBuffer, point);
            ChangeCurrentBuffer();
        }

        public void Reset() {
            _currentBuffer = 0;
            _pointsCounter = 0;

            _previousPoint = null;
        }

        public void Dispose() {
            _pathBuffer1.Dispose();
            _pathBuffer2.Dispose();
        }

        private void ChangeCurrentBuffer() {
            _currentBuffer = _currentBuffer == 0 ? 1 : 0;
        }

        private void ResamplePath(NativeArray<float2> pathBuffer, NativeArray<float2> writeBuffer, Vector2 point) {
            int halfLengths = _maxLength / 2;
            GestureMath.ResamplePath(pathBuffer, writeBuffer.Slice(0, halfLengths));
            writeBuffer[halfLengths] = point;
            _pointsCounter = halfLengths + 1;
        }

        private bool IsCanRecord(float2 point) {
            if (!_previousPoint.HasValue) return true;

            var pp = _previousPoint.Value;
            if (math.abs(pp.x - point.x) < _newPointMinDistance &&
                math.abs(pp.y - point.y) < _newPointMinDistance) return false;

            return true;
        }
    }
}
