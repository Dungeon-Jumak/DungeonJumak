using System;
using System.Collections.Generic;
using UnistrokeGestureRecognition;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors.Window {
    sealed class GestureEditorWindow : EditorWindow {
        public const float pointRadius = 20f;

        private const float _trackAreaSize = 400f;

        private const string _directionToggleName = "directionToggle";
        private const string _reverseButtonName = "reverseButton";
        private const string _snapDropdownName = "snapDropdown";
        private const string _snapToggleName = "snapToggle";
        private const string _flipXButtonName = "flipXButton";
        private const string _flipYButtonName = "flipYButton";
        private const string _trackAreaName = "trackArea";

        private static readonly List<string> _dropDownChoices = new() { "8", "16", "32" };
        private static readonly List<int> _dropDownValues = new() { 8, 16, 32 };

        private SerializedObject _gesture;

        private static int _snapFactor = 16;
        private bool _isSnap = false;

        private IGestureEditorState _currentState;

        public VisualElement TrackArea { get; private set; }
        public VisualElement Marker { get; private set; }

        public List<VisualElement> Points { get; private set; } = new();

        #region - Drawers -
        public PointConnectorDrawer ConnectionDrawer { get; private set; }
        public PreviewConnectorDrawer PreviewDrawer { get; private set; }
        public DirectionDrawer DirectionDrawer { get; private set; }
        public CutMarkerDrawer CutMarkerDrawer { get; private set; }
        #endregion

        #region - Events -
        // Track area events
        // Pointer events 
        public event Action<PointerEnterEvent> PointerEnterTrackArea;
        public event Action<PointerLeaveEvent> PointerLeaveTrackArea;
        public event Action<PointerMoveEvent> PointerMoveAtTrackArea;

        // Mouse events
        // LB
        public event Action<MouseDownEvent> MouseLBDownAtTrackArea;
        public event Action<MouseUpEvent> MouseLBUpAtTrackArea;

        // RB
        public event Action<MouseDownEvent> MouseRBDownAtTrackArea;
        public event Action<MouseUpEvent> MouseRBUpAtTrackArea;

        // MB
        public event Action<MouseDownEvent> MouseMBDownAtTrackArea;
        public event Action<MouseUpEvent> MouseMBUpAtTrackArea;

        // Point events
        // Pointer events
        public event Action<PointerEnterEvent, VisualElement> PointerEnterPoint;
        public event Action<PointerLeaveEvent, VisualElement> PointerLeavePoint;

        // Mouse events
        // LB
        public event Action<MouseDownEvent, VisualElement> MouseLBDownAtPoint;
        public event Action<MouseUpEvent, VisualElement> MouseLBUpAtPoint;

        // RB
        public event Action<MouseDownEvent, VisualElement> MouseRBDownAtPoint;
        public event Action<MouseUpEvent, VisualElement> MouseRBUpAtPoint;

        #endregion

        public void CreateGUI() {
            InitRoot();

            InitTrackArea();
            InitMarker();

            InitDrawers();

            InitDirectionToggle();
            InitSnapDropDown();
            IntiSnapToggle();

            InitReverseButton();
            InitFlipButtons();

            TrackArea.Add(Marker);

            InitEvents();

            ChangeState(new PointsEditState(this));

            Undo.undoRedoPerformed += OnUndoRedoPerformed;

            OnSelectionChange();
        }

        public void ChangeState(IGestureEditorState state) {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        # region - Inits - 
        private void InitRoot() {
            var root = rootVisualElement;

            var uxml = Resources.Load<VisualTreeAsset>("GestureEditorVisualTree");
            var uss = Resources.Load<StyleSheet>("GestureEditorStyleSheet");

            uxml.CloneTree(root);
            root.styleSheets.Add(uss);
        }

        private void InitDrawers() {
            CutMarkerDrawer = new CutMarkerDrawer();
            DirectionDrawer = new DirectionDrawer(Points);
            ConnectionDrawer = new PointConnectorDrawer(Points);
            PreviewDrawer = new PreviewConnectorDrawer(Points, Marker);
            TrackArea.Add(ConnectionDrawer);
            TrackArea.Add(PreviewDrawer);
            TrackArea.Add(DirectionDrawer);
            TrackArea.Add(CutMarkerDrawer);
        }

        private void IntiSnapToggle() {
            var toggle = rootVisualElement.Q<Toggle>(_snapToggleName);
            _isSnap = toggle.value;
            toggle.RegisterValueChangedCallback((e) => _isSnap = e.newValue);
        }

        private void InitDirectionToggle() {
            var toggle = rootVisualElement.Q<Toggle>(_directionToggleName);
            DirectionDrawer.IsDrawing = toggle.value;
            toggle.RegisterValueChangedCallback((e) => DirectionDrawer.IsDrawing = e.newValue);
        }

        private void InitTrackArea() {
            TrackArea = rootVisualElement.Q(_trackAreaName);
            TrackArea.Add(new GridBackgroundDrawer());
        }

        private void InitSnapDropDown() {
            var dropDown = rootVisualElement.Q<DropdownField>(_snapDropdownName);
            dropDown.choices = _dropDownChoices;
            dropDown.index = _dropDownValues.FindIndex((i) => i == _snapFactor);

            dropDown.RegisterValueChangedCallback((e) => _snapFactor = _dropDownValues[dropDown.index]);
        }

        private void InitMarker() {
            if (Marker is not null) {
                return;
            }

            VisualElement marker = new() {
                name = "marker",
                visible = false
            };
            marker.style.position = Position.Absolute;

            Marker = marker;
        }

        private void InitReverseButton() {
            var button = rootVisualElement.Q<Button>(_reverseButtonName);
            button.clicked += () => {
                Points.Reverse();
                _gesture.Update();
                GestureHelper.ClearPath(_gesture);
                foreach (var point in Points) {
                    AddPointToPath(CalculateOriginPosition(point.transform.position, point));
                }
                _gesture.ApplyModifiedProperties();
            };
        }

        private void InitFlipButtons() {
            var flipYButton = rootVisualElement.Q<Button>(_flipYButtonName);
            flipYButton.clicked += () => {
                _gesture.Update();
                GestureHelper.ClearPath(_gesture);
                foreach (var point in Points) {
                    var position = FlipY(CalculateOriginPosition(point.transform.position, point));
                    AddPointToPath(position);
                    point.transform.position = CalculateCenterPosition(position, point);
                }
                _gesture.ApplyModifiedProperties();
            };

            var flipXButton = rootVisualElement.Q<Button>(_flipXButtonName);
            flipXButton.clicked += () => {
                _gesture.Update();
                GestureHelper.ClearPath(_gesture);
                foreach (var point in Points) {
                    var position = FlipX(CalculateOriginPosition(point.transform.position, point));
                    AddPointToPath(position);
                    point.transform.position = CalculateCenterPosition(position, point);
                }
                _gesture.ApplyModifiedProperties();
            };
        }

        private void InitEvents() {
            TrackArea.RegisterCallback<MouseUpEvent>(OnMouseUpAtTrackArea);
            TrackArea.RegisterCallback<MouseDownEvent>(OnMouseDownAtTrackArea);
            TrackArea.RegisterCallback<PointerMoveEvent>(OnPointerMoveAtTrackArea);
            TrackArea.RegisterCallback<PointerEnterEvent>(OnPointerEnterTrackArea);
            TrackArea.RegisterCallback<PointerLeaveEvent>(OnPointerLeaveTrackArea);
        }

        #endregion

        # region - Points -
        public VisualElement AddPoint(Vector2 position) {
            var pointPosition = CalculateCenterPosition(position, pointRadius);
            var point = CreatePoint(pointPosition);
            Points.Add(point);

            AddPointToPath(position);

            return point;
        }

        public VisualElement AddPoint(Vector2 position, int index) {
            var pointPosition = CalculateCenterPosition(position, pointRadius);
            var point = CreatePoint(pointPosition);
            Points.Insert(index, point);

            AddPointToPath(position, index);

            return point;
        }

        public void RemovePoint(VisualElement point) {
            var index = Points.IndexOf(point);
            if (index == -1) return;

            Points.Remove(point);
            point.RemoveFromHierarchy();
            GestureHelper.DeletePointAtPath(_gesture, index);
            _gesture.ApplyModifiedProperties();
        }

        private void AddPointToPath(Vector2 position) {
            int pathIndex = GestureHelper.Size(_gesture);
            GestureHelper.InsertPoint(_gesture, pathIndex, position);
            _gesture.ApplyModifiedProperties();
        }

        private void AddPointToPath(Vector2 position, int index) {
            GestureHelper.InsertPoint(_gesture, index, position);
            _gesture.ApplyModifiedProperties();
        }

        private void SetPathPointPosition(Vector2 position, int index) {
            GestureHelper.SetPointPosition(_gesture, index, position);
        }

        public void MovePoint(VisualElement point, Vector2 position) {
            int pointIndex = Points.FindIndex((p) => point == p);
            if (pointIndex == -1) return;

            var pointPosition = CalculateCenterPosition(position, point);

            _gesture.Update();
            SetPathPointPosition(position, pointIndex);
            point.transform.position = pointPosition;
            _gesture.ApplyModifiedProperties();
        }

        private void InitPoint(Vector2 position) {
            var point = CreatePoint(CalculateCenterPosition(position, pointRadius));
            Points.Add(point);
        }

        private VisualElement CreatePoint(Vector2 position) {
            VisualElement point = new();
            point.AddToClassList("point");
            point.style.width = pointRadius;
            point.style.height = pointRadius;
            point.style.position = Position.Absolute;

            point.transform.position = position;

            point.RegisterCallback<MouseUpEvent, VisualElement>(OnMouseUpAtPoint, point);
            point.RegisterCallback<MouseDownEvent, VisualElement>(OnMouseDownAtPoint, point);
            point.RegisterCallback<PointerEnterEvent, VisualElement>(OnPointerEnterPoint, point);
            point.RegisterCallback<PointerLeaveEvent, VisualElement>(OnPointerLeavePoint, point);

            TrackArea.Add(point);

            return point;
        }

        #endregion

        #region - Event callbacks -
        private void OnPointerEnterTrackArea(PointerEnterEvent e) {
            PointerEnterTrackArea?.Invoke(e);
        }

        private void OnPointerLeaveTrackArea(PointerLeaveEvent e) {
            PointerLeaveTrackArea?.Invoke(e);
        }

        private void OnPointerMoveAtTrackArea(PointerMoveEvent e) {
            PointerMoveAtTrackArea?.Invoke(e);
        }

        private void OnMouseUpAtTrackArea(MouseUpEvent e) {
            if (e.button == 0) {
                MouseLBUpAtTrackArea?.Invoke(e);
            } else if (e.button == 1) {
                MouseRBUpAtTrackArea?.Invoke(e);
            } else if (e.button == 2) {
                MouseMBUpAtTrackArea?.Invoke(e);
            }
        }
        private void OnMouseDownAtTrackArea(MouseDownEvent e) {
            if (e.pressedButtons == 1) {

                MouseLBDownAtTrackArea?.Invoke(e);
            } else if (e.pressedButtons == 2) {
                MouseRBDownAtTrackArea?.Invoke(e);
            } else if (e.pressedButtons == 4) {
                MouseMBDownAtTrackArea?.Invoke(e);
            }
        }

        private void OnMouseDownAtPoint(MouseDownEvent e, VisualElement point) {
            if (e.pressedButtons == 1) {
                MouseLBDownAtPoint?.Invoke(e, point);
            } else if (e.pressedButtons == 2) {
                MouseRBDownAtPoint?.Invoke(e, point);
            }
            e.StopPropagation();
        }
        private void OnMouseUpAtPoint(MouseUpEvent e, VisualElement point) {
            if (e.pressedButtons == 1) {
                MouseLBUpAtPoint?.Invoke(e, point);
            } else if (e.pressedButtons == 2) {
                MouseRBUpAtPoint?.Invoke(e, point);
            }
        }
        private void OnPointerEnterPoint(PointerEnterEvent e, VisualElement point) {
            PointerEnterPoint?.Invoke(e, point);
        }
        private void OnPointerLeavePoint(PointerLeaveEvent e, VisualElement point) {
            PointerLeavePoint?.Invoke(e, point);
        }

        #endregion

        #region - Marker -

        public void HideMarker() {
            Marker.visible = false;
        }

        public void ShowMarker() {
            Marker.visible = true;
        }

        public void SetMarkerPosition(Vector2 newPosition) {
            Marker.transform.position = CalculateCenterPosition(newPosition, Marker);
        }

        #endregion

        #region  - Position Math -
        public Vector2 CalculateCenterPosition(Vector2 position, VisualElement element) {
            var rect = element.contentRect;
            return new Vector2(position.x - (rect.xMax * .5f), position.y - (rect.yMax * .5f));
        }

        public Vector2 CalculateCenterPosition(Vector2 newPosition, float radius) {
            return new Vector2(newPosition.x - (radius * .5f), newPosition.y - (radius * .5f));
        }

        public Vector2 CalculateOriginPosition(Vector2 position, VisualElement element) {
            var rect = element.contentRect;
            return new Vector2(position.x + (rect.xMax * .5f), position.y + (rect.yMax * .5f));
        }

        public Vector2 CalculateOriginPosition(Vector2 position, float radius) {
            return new Vector2(position.x + (radius * .5f), position.y + (radius * .5f));
        }

        public Vector2 FlipY(Vector2 position) {
            return new(position.x, _trackAreaSize - position.y);
        }

        public Vector2 FlipX(Vector2 position) {
            return new(_trackAreaSize - position.x, position.y);
        }

        public Vector2 CalculatePosition(Vector2 position) {
            if (!_isSnap) {
                return position;
            }

            Rect rect = TrackArea.contentRect;

            float xSnap = rect.xMax / _snapFactor;
            float ySnap = rect.yMax / _snapFactor;

            return new Vector2(SnapValue(position.x, xSnap), SnapValue(position.y, ySnap));

            static float SnapValue(float value, float snap) {
                return Mathf.Round(value / snap) * snap;
            }
        }

        #endregion

        private void EnableEditor() {
            rootVisualElement.SetEnabled(true);
            AddPointsFromGesture();
        }

        private void AddPointsFromGesture() {
            var path = GestureHelper.GetEditorPath(_gesture);
            foreach (var item in path) {
                InitPoint(item);
            }
        }

        private void DisableEditor() {
            rootVisualElement.SetEnabled(false);
            DeleteAllPoints();
        }

        private void DeleteAllPoints() {

            foreach (var point in Points) {
                point.RemoveFromHierarchy();
            }
            Points.Clear();
        }

        private void OnUndoRedoPerformed() {
            if (_gesture is null) return;

            _gesture.Update();
            DeleteAllPoints();
            AddPointsFromGesture();
        }

        #region - OpenWindow -

        [MenuItem("Tools/Gesture Editor")]
        public static void OpenWindow() {
            GestureEditorWindow wnd = GetWindow<GestureEditorWindow>();
            wnd.titleContent = new GUIContent("Gesture Editor");
            wnd.minSize = new(430f, 550f);
            wnd.maxSize = new(430f, wnd.maxSize.y);
        }

        #endregion

        public void OnSelectionChange() {
            if (Selection.activeObject is not IGesturePattern) {
                rootVisualElement.Unbind();

                _gesture = null;

                DisableEditor();

                return;
            }

            var so = new SerializedObject(Selection.activeObject);

            DeleteAllPoints();

            rootVisualElement.Bind(so);
            _gesture = so;
            EnableEditor();
        }
    }
}