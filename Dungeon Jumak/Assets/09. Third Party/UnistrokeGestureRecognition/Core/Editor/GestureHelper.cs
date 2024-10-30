using System.Collections.Generic;
using UnistrokeGestureRecognition;
using UnityEditor;
using UnityEngine;

namespace UnistrokeGestureRecognition.Editors {
    static class GestureHelper {
        private const string _editorPathPropertyName = "_editorPath";
        private const string _pathPropertyName = "_path";

        private static SerializedProperty GetEditorPathArrayProperty(SerializedObject so) {
            return so.FindProperty(_editorPathPropertyName);
        }

        public static int Size(SerializedObject so) => GetEditorPathArrayProperty(so).arraySize;

        public static Vector2 GetPoint(SerializedObject so, int index) {
            return GetEditorPathArrayProperty(so).GetArrayElementAtIndex(index).vector2Value;
        }

        public static void ClearPath(SerializedObject so) {
            GetEditorPathArrayProperty(so).ClearArray();
        }

        public static void DeletePointAtPath(SerializedObject so, int index) {
            GetEditorPathArrayProperty(so).DeleteArrayElementAtIndex(index);
        }

        public static void InsertPoint(SerializedObject so, int index, Vector2 position) {
            var sp = GetEditorPathArrayProperty(so);
            sp.InsertArrayElementAtIndex(index);
            sp.GetArrayElementAtIndex(index).vector2Value = position;
        }

        public static void SetPointPosition(SerializedObject so, int index, Vector2 position) {
            GetEditorPathArrayProperty(so).GetArrayElementAtIndex(index).vector2Value = position;
        }

        public static List<Vector2> GetEditorPath(SerializedObject so) {
            var sp = GetEditorPathArrayProperty(so);
            return GetPathFromProperty(sp);
        }

        public static List<Vector2> GetPath(SerializedObject so) {
            var sp = so.FindProperty(_pathPropertyName);
            return GetPathFromProperty(sp);
        }

        private static List<Vector2> GetPathFromProperty(SerializedProperty sp) {
            var size = sp.arraySize;
            var path = new List<Vector2>(size);

            for (int i = 0; i < size; i++) {
                path.Add(sp.GetArrayElementAtIndex(i).vector2Value);
            }

            return path;
        }
    }
}