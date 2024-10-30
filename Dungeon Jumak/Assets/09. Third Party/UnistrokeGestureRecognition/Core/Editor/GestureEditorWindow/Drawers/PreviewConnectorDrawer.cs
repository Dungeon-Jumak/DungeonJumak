using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class PreviewConnectorDrawer : ImmediateModeElement {
        private static readonly Color _linesColor = new Color32(146, 20, 12, 180);

        public List<VisualElement> Points { get; set; }
        public VisualElement Marker { get; private set; }
        public bool IsDrawing { get; set; } = false;

        public PreviewConnectorDrawer(List<VisualElement> points, VisualElement marker) {
            Points = points;
            Marker = marker;
            this.StretchToParentSize();
        }

        protected override void ImmediateRepaint() {
            if (!IsDrawing || Points.Count < 1) return;
            if (!Marker.visible) return;

            var lastPoint = Points[^1];
            DrawLine(lastPoint.GetCenter(), Marker.GetCenter());
        }

        private void DrawLine(Vector2 positionA, Vector2 positionB) {
            Handles.color = _linesColor;
            Handles.DrawDottedLine(positionA, positionB, 1f);
        }
    }
}