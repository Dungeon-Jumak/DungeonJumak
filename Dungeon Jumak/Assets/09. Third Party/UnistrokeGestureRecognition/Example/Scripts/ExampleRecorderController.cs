using UnityEngine;

namespace UnistrokeGestureRecognition.Example {
    public class ExampleRecorderController : MonoBehaviour {
        [SerializeField] private PathDrawerBase _actualPathDrawer;
        [SerializeField] private PathDrawerBase _recordedPathDrawer;

        private IGestureRecorder _recorder;
        private Camera _camera;
        private int _actualPathPoints = 0;

        private void Awake() {
            _recorder = new GestureRecorder(100, .01f);
            _camera = Camera.main;
        }

        private void Update() {
            Clear();
            RecordNewPoint();
        }

        void OnDestroy() {
            _recorder.Dispose();
        }

        private void RecordNewPoint() {
            if (!Input.GetKey(KeyCode.Mouse0)) return;

            var screenPosition = Input.mousePosition;
            Vector2 point = _camera.ScreenToWorldPoint(screenPosition);

            _actualPathPoints++;
            _actualPathDrawer.AddPoint(point);
            _recorder.RecordPoint(point);

            _recordedPathDrawer.Clear();

            foreach (var p in _recorder.Path) {
                _recordedPathDrawer.AddPoint(p);
            }

            Debug.Log("Actual path points count:" + _actualPathPoints);
            Debug.Log("Recorded path point count:" + _recorder.Length);
        }

        private void Clear() {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            _recorder.Reset();
            _actualPathDrawer.Clear();
            _recordedPathDrawer.Clear();
            _actualPathPoints = 0;
        }
    }
}