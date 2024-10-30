using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace  UnistrokeGestureRecognition.Editors.Window {
    sealed class GridBackgroundDrawer : ImmediateModeElement {
        private static readonly Color _2LineColor = new Color(0f, 0f, 0f, 0.48f);
        private static readonly Color _8lineColor = new Color(0f, 0f, 0f, 0.38f);
        private static readonly Color _16LineColor = new Color(0f, 0f, 0f, 0.28f);
        private static readonly Color _32LineColor = new Color(0f, 0f, 0f, 0.18f);

        private static readonly Color _gridBackgroundColor = new Color(0.17f, 0.17f, 0.17f, 1f);

        public GridBackgroundDrawer() {
            this.StretchToParentSize();
        }

        private void DrawGrid(int division) {
            for (int i = 1; i < division; i++) {
                float t = Mathf.InverseLerp(0, division, i);
                float tx = Mathf.Lerp(layout.xMin, layout.xMax, t);
                float ty = Mathf.Lerp(layout.yMin, layout.yMax, t);


                Handles.DrawLine(new Vector3(layout.xMin, ty), new Vector3(layout.xMax, ty));
                Handles.DrawLine(new Vector3(tx, layout.yMin), new Vector3(tx, layout.yMax));
            }
        }

        protected override void ImmediateRepaint() {

            Handles.DrawSolidRectangleWithOutline(layout, _gridBackgroundColor, _gridBackgroundColor);

            Handles.color = _2LineColor;
            DrawGrid(2);

            Handles.color = _8lineColor;
            DrawGrid(8);

            Handles.color = _16LineColor;
            DrawGrid(16);

            Handles.color = _32LineColor;
            DrawGrid(32);
        }
    }
}