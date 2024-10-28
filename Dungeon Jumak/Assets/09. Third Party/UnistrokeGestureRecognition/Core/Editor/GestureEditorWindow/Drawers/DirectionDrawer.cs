using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class DirectionDrawer : ImmediateModeElement {
        private static readonly Color _linesColor = new Color32(146, 20, 12, 255);

        public List<VisualElement> Points { get; set; }

        public bool IsDrawing { get; set; }

        public DirectionDrawer(List<VisualElement> points) {
            Points = points;
            this.StretchToParentSize();
        }

        protected override void ImmediateRepaint() {
            const float length = 30;
            const float angle = 70;

            if (!IsDrawing || Points.Count < 2) return;

            var v1 = Points[^2].GetCenter();
            var v2 = Points[^1].GetCenter();

            var dir = (v2 - v1).normalized;
            var va = EditorHelper.RotateLine(dir, v2, 90 + angle, length);
            var vb = EditorHelper.RotateLine(dir, v2, -90 - angle, length);
            // var va = v2 + (Vector2)(Quaternion.Euler(0, 0, 90 + angle) * dir) * length;
            // var vb = v2 + (Vector2)(Quaternion.Euler(0, 0, -90 - angle) * dir) * length;

            DrawLine(v2, va);
            DrawLine(v2, vb);
        }

        private void DrawLine(Vector2 positionA, Vector2 positionB) {
            Handles.color = _linesColor;
            EditorHelper.DrawStrongLine(positionA, positionB);
        }
    }
}