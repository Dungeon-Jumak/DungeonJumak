using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class DragState : EditorState {
        private readonly VisualElement _point;
        private readonly IGestureEditorState _prevState;

        private Vector2 _startPosition;

        public DragState(GestureEditorWindow editor, VisualElement point, IGestureEditorState prevState) : base(editor) {
            _prevState = prevState;
            _point = point;
            _startPosition = _point.transform.position;
        }

        public override void Enter() {
            Editor.PointerLeaveTrackArea += ResetPosition;
            Editor.PointerMoveAtTrackArea += MovePoint;
            Editor.MouseLBUpAtTrackArea += PlacePoint;
        }

        public override void Exit() {
            Editor.PointerLeaveTrackArea -= ResetPosition;
            Editor.PointerMoveAtTrackArea -= MovePoint;
            Editor.MouseLBUpAtTrackArea -= PlacePoint;
        }

        private void PlacePoint(MouseUpEvent e) {
            Editor.ChangeState(_prevState);
        }

        private void ResetPosition(PointerLeaveEvent e) {
            Editor.MovePoint(_point, _startPosition);

        }

        private void MovePoint(PointerMoveEvent e) {
            Editor.MovePoint(_point, Editor.CalculatePosition(e.localPosition));
        }
    }
}