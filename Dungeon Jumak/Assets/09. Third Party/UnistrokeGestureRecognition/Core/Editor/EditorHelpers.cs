using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnistrokeGestureRecognition.Editors {
    static class EditorHelper {
        public static void DrawStrongLine(Vector2 positionA, Vector2 positionB) {
            Handles.DrawLine(positionA + Vector2.up, positionB + Vector2.up);
            Handles.DrawLine(positionA + Vector2.down, positionB + Vector2.down);
            Handles.DrawLine(positionA + Vector2.left, positionB + Vector2.left);
            Handles.DrawLine(positionA + Vector2.right, positionB + Vector2.right);
        }

        public static Vector2 RotateLine(Vector2 direction, Vector2 rotatePoint, float angle, float length) {
            return rotatePoint + (Vector2)(Quaternion.Euler(0, 0, angle) * direction) * length;
        }

        public static Vector2 GetCenter(this VisualElement element) {
            var position = element.transform.position;
            var rect = element.contentRect;
            return new Vector2(position.x + rect.xMax * .5f, position.y + rect.yMax * .5f);
        }
    }
}