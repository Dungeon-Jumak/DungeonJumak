using System;
using System.Collections;
using UnityEngine;

namespace UnistrokeGestureRecognition.Example {
    public sealed class LinePathDrawer : PathDrawerBase {
        [SerializeField] private LineRenderer _brash;

        private void Awake() {
            _brash = Instantiate(_brash, transform);
            Clear();
        }

        public override void Show() => _brash.gameObject.SetActive(true);

        public override void Hide() => _brash.gameObject.SetActive(false);

        public override void Clear() {
            _brash.positionCount = 0;
        }

        public override void SetPath(ReadOnlySpan<Vector2> path) {
            foreach (var point in path) {
                AddPoint(point);
            }
        }

        public override void AddPoint(Vector2 position) {
            _brash.positionCount++;
            _brash.SetPosition(_brash.positionCount - 1, position);
        }
    }
}
