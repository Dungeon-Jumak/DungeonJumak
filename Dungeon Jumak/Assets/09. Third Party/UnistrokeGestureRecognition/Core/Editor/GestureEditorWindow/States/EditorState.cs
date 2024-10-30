namespace UnistrokeGestureRecognition.Editors.Window {
    abstract class EditorState : IGestureEditorState {
        public GestureEditorWindow Editor { get; private set; }

        public EditorState(GestureEditorWindow editor) => Editor = editor;

        public abstract void Enter();

        public abstract void Exit();
    }
}
