using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class PointsEditState : EditorState {
        public PointsEditState(GestureEditorWindow editor) : base(editor) { }

        public override void Enter() {
            Editor.PreviewDrawer.IsDrawing = true;

            Editor.PointerEnterTrackArea += ShowMarkerOnAreaEnter;
            Editor.PointerLeaveTrackArea += HideMarkerOnEnterArea;
            Editor.MouseMBDownAtTrackArea += ChangeModeToCut;
            Editor.PointerMoveAtTrackArea += MoveMarker;
            Editor.MouseLBDownAtTrackArea += AddPoint;

            Editor.PointerEnterPoint += HideMarkerOnPointEnter;
            Editor.PointerLeavePoint += ShowMarkerOnPointLeave;
            Editor.MouseLBDownAtPoint += StartDragPoint;
            Editor.MouseRBDownAtPoint += DeletePoint;
        }

        public override void Exit() {
            Editor.HideMarker();

            Editor.PreviewDrawer.IsDrawing = false;

            Editor.PointerEnterTrackArea -= ShowMarkerOnAreaEnter;
            Editor.PointerLeaveTrackArea -= HideMarkerOnEnterArea;
            Editor.MouseMBDownAtTrackArea -= ChangeModeToCut;
            Editor.PointerMoveAtTrackArea -= MoveMarker;
            Editor.MouseLBDownAtTrackArea -= AddPoint;

            Editor.PointerEnterPoint -= HideMarkerOnPointEnter;
            Editor.PointerLeavePoint -= ShowMarkerOnPointLeave;
            Editor.MouseLBDownAtPoint -= StartDragPoint;
            Editor.MouseRBDownAtPoint -= DeletePoint;
        }

        private void ShowMarkerOnAreaEnter(PointerEnterEvent e) => Editor.ShowMarker();

        private void HideMarkerOnEnterArea(PointerLeaveEvent e) => Editor.HideMarker();

        private void HideMarkerOnPointEnter(PointerEnterEvent e, VisualElement point) => Editor.HideMarker();
        private void ShowMarkerOnPointLeave(PointerLeaveEvent e, VisualElement point) => Editor.ShowMarker();

        private void MoveMarker(PointerMoveEvent e) => Editor.SetMarkerPosition(Editor.CalculatePosition(e.localPosition));

        private void DeletePoint(MouseDownEvent e, VisualElement point) {
            Editor.RemovePoint(point);
        }

        private void StartDragPoint(MouseDownEvent e, VisualElement point) {
            Editor.ChangeState(new DragState(Editor, point, this));
        }

        private void AddPoint(MouseDownEvent e) {
            var point = Editor.AddPoint(Editor.CalculatePosition(e.localMousePosition));
            Editor.ChangeState(new DragState(Editor, point, this));
        }

        private void ChangeModeToCut(MouseDownEvent e) {
            Editor.ChangeState(new CutLinesState(Editor));
        }
    }
}