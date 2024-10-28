using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// <inheritdoc cref="IGesturePattern"/>
    /// Implement your pattern class from this one to add data and keep editor support.
    /// </summary>
    public abstract class GesturePatternBase : ScriptableObject, IGesturePattern {
        [HideInInspector]
        [SerializeField]
        private Vector2[] _path;
        public ReadOnlySpan<Vector2> Path => _path;

        [SerializeField]
        private GestureScalingMode _scalingMode = GestureScalingMode.UnUniform;
        public GestureScalingMode ScalingMode => _scalingMode;

#if UNITY_EDITOR

        // Editor data

        /// <summary>
        /// Points inside editor window.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private List<Vector2> _editorPath;

        /// <summary>
        /// Process editor path to normalized pattern path.
        /// </summary>
        private void OnValidate() {
            var path = new List<Vector2>(_editorPath);
            var rect = _scalingMode == GestureScalingMode.UnUniform ?
                    GestureMath.FindGestureUnUniformRect(new ReadOnlyCollection<Vector2>(path)) :
                    GestureMath.FindGestureUniformRect(new ReadOnlyCollection<Vector2>(path));

            GestureMath.NormalizePath(rect, path);

            for (int i = 0; i < path.Count; i++) {
                var point = path[i];
                path[i] = new Vector2(point.x, 1 - point.y);
            }

            _path = path.ToArray();
        }

#endif
    }
}