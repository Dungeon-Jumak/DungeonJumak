using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class CutLinesState : EditorState {
        private int _pointStartIndex = -1;
        private Vector2 _cutPosition = Vector2.zero;

        public CutLinesState(GestureEditorWindow editor) : base(editor) {
        }

        public override void Enter() {
            HideAllPoints();
            Editor.MouseMBDownAtTrackArea += ChangeStateToPointEdit;
            Editor.PointerMoveAtTrackArea += FindClipping;
            Editor.MouseLBDownAtTrackArea += CutLine;
        }

        public override void Exit() {
            ShowAllPoints();
            Editor.CutMarkerDrawer.IsDrawing = false;
            Editor.MouseMBDownAtTrackArea -= ChangeStateToPointEdit;
            Editor.PointerMoveAtTrackArea -= FindClipping;
            Editor.MouseLBDownAtTrackArea -= CutLine;
        }

        private void FindClipping(PointerMoveEvent e) {
            const float minAllowedDistance = 10f;
            const float mouseOffset = 0;

            if (Editor.Points.Count < 2)
                return;

            var mousePosition = new Vector2(e.localPosition.x, e.localPosition.y - mouseOffset);

            int resIndex = -1;
            Vector2 resCutPoint = default;
            float resDistance = float.MaxValue;

            var currentPoint = Editor.Points[0];
            for (int i = 1; i < Editor.Points.Count; i++) {
                var point = Editor.Points[i];

                var pointOnLine = HandleUtility.ProjectPointLine(mousePosition, currentPoint.GetCenter(), point.GetCenter());
                var distance = Vector2.Distance(pointOnLine, mousePosition);
                if (distance < resDistance && distance < minAllowedDistance) {
                    resCutPoint = pointOnLine;
                    resDistance = distance;
                    resIndex = i - 1;
                }

                currentPoint = point;
            }

            if (resIndex >= 0) {
                _cutPosition = resCutPoint;
                _pointStartIndex = resIndex;
                DrawCutMarker(resIndex, resCutPoint);
            }
            else {
                Editor.CutMarkerDrawer.IsDrawing = false;
            }

            Editor.Repaint();
        }

        private void DrawCutMarker(int startPointIndex, Vector2 cutPoint) {
            var startPoint = Editor.Points[startPointIndex].GetCenter();
            var endPoint = Editor.Points[startPointIndex + 1].GetCenter();

            Editor.CutMarkerDrawer.IsDrawing = true;

            Editor.CutMarkerDrawer.cutPoint = cutPoint;
            Editor.CutMarkerDrawer.start = startPoint;
            Editor.CutMarkerDrawer.end = endPoint;
        }

        private void HideAllPoints() {
            foreach (var point in Editor.Points) {
                point.visible = false;
            }
        }

        private void ShowAllPoints() {
            foreach (var point in Editor.Points) {
                point.visible = true;
            }
        }

        private void CutLine(MouseDownEvent e) {
            if (_pointStartIndex < 0)
                return;

            var point = Editor.AddPoint(_cutPosition, _pointStartIndex + 1);
            Editor.ChangeState(new DragState(Editor, point, this));
        }

        private void ChangeStateToPointEdit(MouseDownEvent e) {
            Editor.ChangeState(new PointsEditState(Editor));
        }
    }
}