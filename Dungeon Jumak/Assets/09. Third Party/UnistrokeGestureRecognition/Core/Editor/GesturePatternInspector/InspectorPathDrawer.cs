using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Inspector {
    sealed class InspectorPathDrawer : ImmediateModeElement {
        private static readonly Color _linesColor = new Color32(146, 20, 12, 255);

        public SerializedObject Gesture { get; set; }

        public InspectorPathDrawer() {
            this.StretchToParentSize();
        }

        protected override void ImmediateRepaint() {
            Gesture.Update();
            var points = GestureHelper.GetPath(Gesture);

            if (points.Count < 1) return;

            var currentPoint = NormalizedToPointYFlip(contentRect, points[0]);
            Handles.color = _linesColor;
            Handles.DrawSolidDisc(currentPoint, Vector3.forward, 3f);

            for (int i = 1; i < points.Count; i++) {
                var nextPoint = NormalizedToPointYFlip(contentRect, points[i]);

                EditorHelper.DrawStrongLine(currentPoint, nextPoint);

                currentPoint = nextPoint;
                Handles.DrawSolidDisc(currentPoint, Vector3.forward, 3f);
            }

            if (points.Count < 2) return;
            var prevPoint = NormalizedToPointYFlip(contentRect, points[^2]);

            var dir = (currentPoint - prevPoint).normalized;
            var va = EditorHelper.RotateLine(dir, currentPoint, 90 + 70, 20);
            var vb = EditorHelper.RotateLine(dir, currentPoint, -90 - 70, 20);

            EditorHelper.DrawStrongLine(currentPoint, va);
            EditorHelper.DrawStrongLine(currentPoint, vb);
        }

        private Vector2 NormalizedToPointYFlip(Rect rect, Vector2 point) {
            return Rect.NormalizedToPoint(rect, new(point.x, 1 - point.y));
        }
    }
}