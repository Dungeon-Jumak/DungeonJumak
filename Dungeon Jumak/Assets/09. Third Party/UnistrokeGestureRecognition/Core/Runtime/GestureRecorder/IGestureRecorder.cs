using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// Records gesture path by points.
    /// </summary>
    public interface IGestureRecorder {
        /// <summary>
        /// Current length of the recorded path.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Recorded gesture path.
        /// </summary>
        NativeSlice<float2> Path { get; }

        /// <summary>
        /// Add a new point to the gesture path.
        /// </summary>
        void RecordPoint(float2 point);

        /// <inheritdoc cref="RecordPoint"/>
        void RecordPoint(Vector2 point);

        /// <summary>
        /// Reset the recorder to start recording new gesture.
        /// </summary>
        void Reset();

        /// <summary>
        /// Dispose the recorder to free memory and prevent leaks.
        /// Do not use this recorder after dispose.
        /// </summary>
        void Dispose();
    }
}
