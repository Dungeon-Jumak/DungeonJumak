using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class PointConnectorDrawer : ImmediateModeElement
    {
        private static readonly Color _linesColor = new Color32(146, 20, 12, 255);

        public List<VisualElement> Points { get; set; }

        public PointConnectorDrawer(List<VisualElement> points)
        {
            Points = points;
            this.StretchToParentSize();
        }

        protected override void ImmediateRepaint()
        {
            if (Points.Count < 2) return;

            var currentPoint = Points[0];
            for (int i = 1; i < Points.Count; i++) {
                var nextPoint = Points[i];
                DrawLine(currentPoint.GetCenter(), nextPoint.GetCenter());
                currentPoint = nextPoint;
            }
        }

        private void DrawLine(Vector2 positionA, Vector2 positionB) {
            Handles.color = _linesColor;
            EditorHelper.DrawStrongLine(positionA, positionB);
        }
    }
}