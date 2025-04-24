using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toolbox.ShowSceneInToolbar
{
    [Overlay(typeof(SceneView), "Scene Selection")]
    public class ShowSceneInToolbar : ToolbarOverlay
    {
        public const string Icon = "Packages/com.game5mobile.showsceneintoolbar/Editor Default Resources/Unity32px.png";
        public const string Id = "ShowSceneInToolbarOverlay";

        ShowSceneInToolbar() : base(Id)
        {
        }

        [EditorToolbarElement(Id, typeof(SceneView))]
        class SceneDropDownToggle : EditorToolbarDropdownToggle, IAccessContainerWindow
        {
            public EditorWindow containerWindow { get; set; }

            SceneDropDownToggle()
            {
                Scene currentScene = EditorSceneManager.GetActiveScene();

                text = currentScene.name;
                tooltip = "Select scene to load";
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>(Icon);

                dropdownClicked += Dropdown_OnClicked;
            }

            private void Dropdown_OnClicked()
            {
                GenericMenu menu = new GenericMenu();
                Scene currentScene = EditorSceneManager.GetActiveScene();

                var scenes = EditorBuildSettings.scenes;
                foreach (var scene in scenes)
                {
                    string sceneName = Path.GetFileNameWithoutExtension(scene.path);

                    menu.AddItem(new GUIContent(sceneName), string.Compare(sceneName, currentScene.name) == 0, () => OpenScene(currentScene, scene.path));
                }

                menu.ShowAsContext();
            }

            private void OpenScene(Scene scene, string path)
            {
                if (scene.isDirty)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
                else
                {
                    EditorSceneManager.OpenScene(path);
                }

                OnSceneOpen();
            }

            private void OnSceneOpen()
            {
                Scene currentScene = EditorSceneManager.GetActiveScene();
                text = currentScene.name;
            }
        }
    }
}