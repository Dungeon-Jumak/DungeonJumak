// System
using System;
using System.Collections.Generic;

// Unity
using Unity.Jobs;
using UnityEngine;

namespace UnistrokeGestureRecognition.Example 
{
    public sealed class RecognizerController : MonoBehaviour 
    {
        [Header("사용 패턴 목록")]
        [SerializeField] private List<ExampleGesturePattern> _patterns;

        [Header("새로운 포인트까지의 최소 거리")]
        [SerializeField, Range(0f, 10f)] private float _newPointMinDistance = 1f;

        [Header("Line Drawer")]
        [SerializeField] private PathDrawerBase _pathDrawer;

        [Header("Name Controller")]
        [SerializeField] private NameController _nameController;

        [Header("01: FireRing")]
        [SerializeField] private Skill.Controller.SkillController fireRing;

        private Camera _camera;

        private IGestureRecorder _gestureRecorder;

        private IGestureRecognizer<ExampleGesturePattern> _recognizer;

        private JobHandle? _recognizeJob;

        private float timer;

        private void Awake() {
            // Use this class to record the gesture path.
            // It uses a resampling algorithm to capture long paths to a limited size buffer.
            // The first value specifies the maximum number of points in the path buffer.
            // A higher value gives a more accurate path, but requires more memory.
            // The second value specifies the minimum distance between the previous and new point to record.
            // If the distance is less than required, the new point will not be added to the recorded path.
            // Useful when you want to exclude points from a path when the user keeps the cursor in one place.
            _gestureRecorder = new GestureRecorder(254, _newPointMinDistance);

            // Pass your patterns to the recognizer constructor.
            // You can also choose the number of points to resample.
            // A higher value gives more accurate results but takes longer to process.
            // 128 is the default and gives good results, but you can choose a better value for your pattern set.
            _recognizer = new GestureRecognizer<ExampleGesturePattern>(_patterns, 128);
        }

        private void Start() {
            _pathDrawer.Show();
            _camera = Camera.main;
        }

        private void OnDestroy() {
            // !!! IMPORTANT !!!
            // Do not forget to dispose the recognizer and recorder to prevent a memory leak, 
            // as they use native arrays to store the necessary data.
            _recognizer.Dispose();
            _gestureRecorder.Dispose();   
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.Mouse0)) {
                RecognizeRecordedGesture();
                Clear();
            }

            RecordNewPoint();
        }

        private void LateUpdate() {
            if (!_recognizeJob.HasValue) return;

            // Make sure to complete the recognition task
            _recognizeJob.Value.Complete();

            // Get recognition result
            RecognizeResult<ExampleGesturePattern> result = _recognizer.Result;

            // You can set the required precision value for the results.
            // A score is a value in the range of 0 to 1.
            // Where 1 means that the recorded path is exactly the same as the pattern path.
            // 0.7 is a good choice, but you can choose another value.
            if (result.Score >= .7f) 
            {
                // Get the recognized pattern from the result
                ExampleGesturePattern recognizedPattern = result.Pattern;
                _nameController.Set(recognizedPattern.Name);

                switch (recognizedPattern.Name)
                {
                    // 스킬 01. 불꽃 고리
                    case "FireRing":
                        fireRing.FireRing();
                        break;
                    default:
                        break;
                }

                Debug.Log($"{recognizedPattern.Name}");
            }

            _recognizeJob = null;
        }

        private void RecognizeRecordedGesture() {
            // Schedule recorded path recognition and get recognition results in the late update
            // The first value is the recorded gesture path.
            // The second value indicates whether the comparison should 
            // be performed for each pattern in parallel or sequentially.
            _recognizeJob = _recognizer.ScheduleRecognition(_gestureRecorder.Path, true);

            // If you want to get the result instantly, 
            // use _recognizer.Recognize(_gestureRecorder.Path) method instead
        }

        private void Clear() {
            _nameController.Clear();
            _pathDrawer.Clear();
            // Clear the gesture recorder buffer for the new path
            _gestureRecorder.Reset();
        }

        //Record New Point
        private void RecordNewPoint()
        {
            if (Input.GetMouseButton(0))
            {
                //Increase Timer
                timer += Time.deltaTime;

                if (timer >= 0.1f)
                {
                    var screenPosition = Input.mousePosition;

                    Vector2 point = _camera.ScreenToWorldPoint(screenPosition);
                    _gestureRecorder.RecordPoint(new Vector2(screenPosition.x, screenPosition.y));
                    // Show gesture path
                    _pathDrawer.AddPoint(point);
                }
            }
            else if (Input.GetMouseButtonUp(0))
                //Init Timer
                timer = 0f;
        }

        private void OnValidate() {
            if (Application.isPlaying && _recognizer != null) {
                _recognizer.Dispose();
                _recognizer = new GestureRecognizer<ExampleGesturePattern>(_patterns);
            }
        }
    }
}
