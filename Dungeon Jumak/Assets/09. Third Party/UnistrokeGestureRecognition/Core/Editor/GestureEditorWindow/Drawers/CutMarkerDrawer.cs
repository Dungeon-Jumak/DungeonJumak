using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class CutMarkerDrawer : ImmediateModeElement {

        private static readonly Color _color = new Color32(146, 255, 12, 255);

        public bool IsDrawing { get; set; } = false;

        public Vector2 start;
        public Vector2 end;
        public Vector2 cutPoint;

        public CutMarkerDrawer() {
            this.StretchToParentSize();
        }

        protected override void ImmediateRepaint() {
            if (!IsDrawing) return;

            Handles.color = _color;
            Handles.DrawSolidDisc(cutPoint, Vector3.forward, 5f);

            DrawLine(start, cutPoint);
            DrawLine(end, cutPoint);
        }

        private void DrawLine(Vector2 positionA, Vector2 positionB) {
            Handles.color = _color;
            EditorHelper.DrawStrongLine(positionA, positionB);
        }
    }
}