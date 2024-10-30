using System;
using UnityEngine;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// Stores gesture data.
    /// </summary>
    public interface IGesturePattern {
        /// <summary>
        /// Gesture scaling mode
        /// </summary>
        GestureScalingMode ScalingMode { get; }

        /// <summary>
        /// Path of the pattern.
        /// </summary>
        ReadOnlySpan<Vector2> Path { get; }
    }
}