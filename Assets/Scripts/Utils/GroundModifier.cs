using Shudder.Configs;
using Shudder.Factories;
using Shudder.Models;
using Shudder.Models.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    [ExecuteInEditMode]
    public sealed class GroundModifier : MonoBehaviour
    {
        private IGround _ground;
        private GridConfig _config;
        private Transform _parent;
        private Vector3 _offsetPosition;
        private GroundFactory _groundFactory;

        public void Init(Ground ground, GroundFactory groundFactory, GridConfig config, Transform parent,
            Vector3 offsetPosition)
        {
            _ground = ground;
            _groundFactory = groundFactory;
            _config = config;
            _parent = parent;
            _offsetPosition = offsetPosition;
        }

        private void Start()
        {
            if (Application.isPlaying) return;
#if UNITY_EDITOR
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
#endif
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                SceneView.duringSceneGui -= OnSceneGUI;
#endif
            }
        }

#if UNITY_EDITOR

        private void OnSceneGUI(SceneView sceneView)
        {
            var isOnMe = Selection.activeGameObject == gameObject;
            if (!isOnMe)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    if (TrySelect(child, ref isOnMe))
                        break;
                    
                    for (int j = 0; j < child.childCount; j++)
                    {
                        if (TrySelect(child.GetChild(j), ref isOnMe))
                            break;
                    }
                }
            }

            if (!isOnMe)
                return;
            
            SceneViewGUI(SceneView.currentDrawingSceneView);
        }
        
        
        private void SceneViewGUI(SceneView sceneView) {
            var width = sceneView.camera.pixelRect.width / 7;
            var height = sceneView.camera.pixelRect.height / 4;
            var x = sceneView.camera.pixelRect.width - width - 7;
            var y = sceneView.camera.pixelRect.height / 2;
            
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(x, y, width,height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GetButtonModify(GroundType.Wall);
            GetButtonModify(GroundType.TileHigh);
            GetButtonModify(GroundType.TileMedium);
            GetButtonModify(GroundType.TileLow);
            GetButtonModify(GroundType.Portal);
            GetButtonModify(GroundType.Pit);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
            Handles.EndGUI();
        }

        private bool TrySelect(Transform child, ref bool isOnMe)
        {
            if (Selection.activeTransform != child)
                return false;
            
            Selection.activeGameObject = gameObject;
            isOnMe = true;
            return true;
        }

        private void GetButtonModify(GroundType groundTypeTo)
        {
            GUI.backgroundColor = GetColor(groundTypeTo);

            if (_ground.GroundType == groundTypeTo)
            {
                if (GUILayout.Button(" "))
                {
                }
            }
            else
            {
                if (GUILayout.Button($"{groundTypeTo}"))
                {
                    Debug.Log($"Modify to {groundTypeTo}");
                    ModifyGround(groundTypeTo);
                }
            }
        }
        
        private bool Modify(GroundType groundTypeTo)
        {
            var offsetPos = (int)groundTypeTo * 0.3f;
            var btnPosition = transform.position + new Vector3(0.4f, offsetPos);

            if (_ground.GroundType == groundTypeTo)
            {
                Handles.color = Color.white;
                Handles.Button(btnPosition, Quaternion.identity, 0.3f, 0.15f, Handles.SphereHandleCap);
            }
            else
            {
                Handles.color = GetColor(groundTypeTo);

                if (Handles.Button(btnPosition, Quaternion.identity, 0.3f, 0.15f, Handles.CubeHandleCap))
                {
                    Debug.Log($"Modify to {groundTypeTo}");
                    ModifyGround(groundTypeTo);
                    return true;
                }
            }

            var handleSize = HandleUtility.GetHandleSize(btnPosition);
            var buttonStyle = new GUIStyle
            {
                fontSize = (int)(8f / handleSize)
            };
            Handles.color = Color.cyan;
            Handles.Label(btnPosition + new Vector3(0.2f, 0.2f), $"{groundTypeTo}", buttonStyle);

            return false;
        }

        private void ModifyGround(GroundType groundTypeTo)
        {
            var newGround = _groundFactory.Create(
                _config,
                _parent,
                _ground.Id.x,
                _ground.Id.y,
                _offsetPosition,
                groundTypeTo);

            newGround.Presenter.View.gameObject
                .AddComponent<GroundModifier>()
                .Init(newGround, _groundFactory, _config, _parent, _offsetPosition);

            DestroyImmediate(_ground.Presenter.View.gameObject);
            _ground = newGround;
        }

        private static Color GetColor(GroundType groundTypeTo)
        {
            return groundTypeTo switch
            {
                GroundType.Pit => Color.black,
                GroundType.Portal => Color.magenta,
                GroundType.TileLow => Color.cyan,
                GroundType.TileMedium => Color.blue,
                GroundType.TileHigh => Color.green,
                GroundType.Wall => Color.grey,
                _ => Color.yellow
            };
        }
#endif
    }
}