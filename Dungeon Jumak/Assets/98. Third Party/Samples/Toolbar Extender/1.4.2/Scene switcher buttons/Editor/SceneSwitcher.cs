using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.Examples
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        protected static int SceneCheck;

        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("Start", "Start Scene")))
            {
                SceneHelper.OpenScene("Start");
            }

            if (GUILayout.Button(new GUIContent("Wait", "Waiting Scene")))
            {
                SceneHelper.OpenScene("Waiting");
            }

            if (GUILayout.Button(new GUIContent("Market", "Market Scene")))
            {
                SceneHelper.OpenScene("Market");
            }


            if (GUILayout.Button(new GUIContent("Stage1", "Stage1 Scene")))
            {
                SceneHelper.OpenScene("Stage1", 1);
            }

            if (GUILayout.Button(new GUIContent("Jumak", "Jumak Scene")))
            {
                SceneHelper.OpenScene("Jumak", 2);
            }
        }
    }

    static class SceneHelper
    {
        public static void OpenScene(string name)
        {
            var saved = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (saved)
            {
                _ = EditorSceneManager.OpenScene($"Assets/Scenes/{name}.unity");
            }
        }

        public static void OpenScene(string name, int SceneCheck)
        {
            var saved = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (saved && SceneCheck == 1)
            {
                _ = EditorSceneManager.OpenScene($"Assets/Scenes/Dungeon/{name}.unity");
            }
            else if (saved && SceneCheck == 2)
            {
                _ = EditorSceneManager.OpenScene($"Assets/Scenes/Jumak/{name}.unity");
            }
        }
    }
}
