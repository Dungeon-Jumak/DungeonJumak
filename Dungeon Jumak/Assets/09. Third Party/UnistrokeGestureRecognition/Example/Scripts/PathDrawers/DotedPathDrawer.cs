using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnistrokeGestureRecognition.Example {
    public sealed class DotedPathDrawer : PathDrawerBase {
        [SerializeField] private GameObject _dotPrefab;
        private readonly List<GameObject> _dotList = new();
        private int _count = 0;

        public override void Show() => gameObject.SetActive(true);

        public override void Hide() => gameObject.SetActive(false);

        public override void Clear() {
            foreach (var dot in _dotList) {
                dot.SetActive(false);
            }
            _count = 0;
        }

        public override void SetPath(ReadOnlySpan<Vector2> path) {
            if (_count < path.Length) {
                for (int i = 0; i < path.Length - _count; i++) {
                    CreateDot();
                }
            }

            foreach (var point in path) {
                AddPoint(point);
            }
        }

        public override void AddPoint(Vector2 position) {
            if (_count == _dotList.Count) {
                var go = CreateDot();
                go.transform.position = new Vector3(position.x, position.y);
                go.SetActive(true);
            }
            else {
                var dot = _dotList[_count];
                dot.transform.position = new Vector3(position.x, position.y);
                dot.SetActive(true);
            }
            _count++;
        }

        private GameObject CreateDot() {
            var go = Instantiate(_dotPrefab, transform, true);
            go.SetActive(false);
            _dotList.Add(go);
            return go;
        }
    }
}
