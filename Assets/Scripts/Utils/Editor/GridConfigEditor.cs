using System;
using Shudder.Configs;
using Shudder.Data;
using Shudder.Models;
using Shudder.Services;
using Shudder.Views;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.Editor
{
    [CustomEditor(typeof(GridConfig))]
    public class GridConfigEditor : UnityEditor.Editor
    {
        private SerializedObject _so;
        private SerializedProperty _level;
        private SerializedProperty _isBuilt;
        private SerializedProperty _spaceBetweenCells;
        private SerializedProperty _groundConfig;
        private SerializedProperty _chanceDestroy;
        private SerializedProperty _width;
        private SerializedProperty _height;
        private SerializedProperty _countCoin;
        private SerializedProperty _chanceCoin;
        private SerializedProperty _portalPositionForWidth;
        private SerializedProperty _portalPositionForHeight;
        private SerializedProperty _isKey;
        private SerializedProperty _keyPositionForWidth;
        private SerializedProperty _keyPositionForHeight;
        private SerializedProperty _isPit;
        private SerializedProperty _pitCount;
        private SerializedProperty _chanceOfPit;
        private SerializedProperty _pitPositionForWidth;
        private SerializedProperty _pitPositionForHeight;
        private SerializedProperty _isWall;
        private SerializedProperty _chanceOfWall;
        private SerializedProperty _wallPositionForWidth;
        private SerializedProperty _wallPositionForHeight;
        private bool _isBuilding;

        private void OnEnable()
        {
            var gridConf = (GridConfig)target;
            _so = new SerializedObject(gridConf);

            _isBuilt = _so.FindProperty("IsBuilt");
            _level = _so.FindProperty("Level");
            _spaceBetweenCells = _so.FindProperty("SpaceBetweenCells");
            _groundConfig = _so.FindProperty("GroundConfig");
            _chanceDestroy = _so.FindProperty("ChanceDestroy");
            _width = _so.FindProperty("Width");
            _height = _so.FindProperty("Height");
            _countCoin = _so.FindProperty("CountCoin");
            _chanceCoin = _so.FindProperty("ChanceCoin");
            _portalPositionForWidth = _so.FindProperty("HolePositionForWidth");
            _portalPositionForHeight = _so.FindProperty("HolePositionForHeight");
            _isKey = _so.FindProperty("IsKey");
            _keyPositionForWidth = _so.FindProperty("KeyPositionForWidth");
            _keyPositionForHeight = _so.FindProperty("KeyPositionForHeight");
            _isPit = _so.FindProperty("IsPit");
            _pitCount = _so.FindProperty("PitCount");
            _chanceOfPit = _so.FindProperty("ChanceOfPit");
            _pitPositionForWidth = _so.FindProperty("PitPositionForWidth");
            _pitPositionForHeight = _so.FindProperty("PitPositionForHeight");
            _isWall = _so.FindProperty("IsWall");
            _chanceOfWall = _so.FindProperty("ChanceOfWall");
            _wallPositionForWidth = _so.FindProperty("WallPositionForWidth");
            _wallPositionForHeight = _so.FindProperty("WallPositionForHeight");
            _isBuilding = true;
        }

        public override void OnInspectorGUI()
        {
            MakeLevel();
            MakeBase();
            MakeSizeGrid();
            MakeCoin();
            MakePortalPosition();
            MakeKey();
            MakePit();
            MakeWall();
            MakeBuilding();

            
            _so.ApplyModifiedProperties();
        }

        private void MakeBuilding()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.green;
            
            _isBuilding = EditorGUILayout.BeginFoldoutHeaderGroup(_isBuilding, "Is building");
            if (_isBuilding)
            {
                if (GUILayout.Button("Build"))
                {
                    new BuildingGridServiceEditor().BuildGrid((GridConfig)_so.targetObject);
                    _isBuilt.boolValue = true;
                }

                _isBuilt.boolValue = EditorGUILayout.Toggle(_isBuilt.boolValue);

                if (_isBuilt.boolValue)
                {
                    if (GUILayout.Button("Save Grid Config"))
                    {
                        SaveGrid();
                    }
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void MakeWall()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = new Color(0.7f, 0.6f, 0.6f, 1);

            _isWall.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(_isWall.boolValue, "Wall");
            if (_isWall.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Slider(_chanceOfWall, 0f, 1f, "Chance of Wall");

                EditorGUILayout.Space();
                _wallPositionForWidth.vector2IntValue =
                    EditorGUILayout.Vector2IntField("Position For Width", _wallPositionForWidth.vector2IntValue);
                float minPositionForWidth = _wallPositionForWidth.vector2IntValue.x;
                float maxPositionForWidth = _wallPositionForWidth.vector2IntValue.y;
                float maxWidth = _width.intValue;
                EditorGUILayout.MinMaxSlider(ref minPositionForWidth, ref maxPositionForWidth, 1, maxWidth);
                _wallPositionForWidth.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForWidth),
                    Mathf.RoundToInt(maxPositionForWidth));

                _wallPositionForHeight.vector2IntValue =
                    EditorGUILayout.Vector2IntField("Position For Height", _wallPositionForHeight.vector2IntValue);
                float minPositionForHeight = _wallPositionForHeight.vector2IntValue.x;
                float maxPositionForHeight = _wallPositionForHeight.vector2IntValue.y;
                float maxHeight = _height.intValue;
                EditorGUILayout.MinMaxSlider(ref minPositionForHeight, ref maxPositionForHeight, 1, maxHeight);
                _wallPositionForHeight.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForHeight),
                    Mathf.RoundToInt(maxPositionForHeight));
            }
                
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        private void MakePit()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.5f , 1);

            _isPit.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(_isPit.boolValue, "Pit");
            if (_isPit.boolValue)
            {
                EditorGUILayout.Space();
                _pitCount.intValue = EditorGUILayout.IntField("Count", _pitCount.intValue);
                EditorGUILayout.Slider(_chanceOfPit, 0f, 1f, "Chance of Pit");

                EditorGUILayout.Space();
                _pitPositionForWidth.vector2IntValue =
                    EditorGUILayout.Vector2IntField("Position For Width", _pitPositionForWidth.vector2IntValue);
                float minPositionForWidth = _pitPositionForWidth.vector2IntValue.x;
                float maxPositionForWidth = _pitPositionForWidth.vector2IntValue.y;
                float maxWidth = _width.intValue;
                EditorGUILayout.MinMaxSlider(ref minPositionForWidth, ref maxPositionForWidth, 1, maxWidth);
                _pitPositionForWidth.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForWidth),
                    Mathf.RoundToInt(maxPositionForWidth));

                _pitPositionForHeight.vector2IntValue =
                    EditorGUILayout.Vector2IntField("Position For Height", _pitPositionForHeight.vector2IntValue);
                float minPositionForHeight = _pitPositionForHeight.vector2IntValue.x;
                float maxPositionForHeight = _pitPositionForHeight.vector2IntValue.y;
                float maxHeight = _height.intValue;
                EditorGUILayout.MinMaxSlider(ref minPositionForHeight, ref maxPositionForHeight, 1, maxHeight);
                _pitPositionForHeight.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForHeight),
                    Mathf.RoundToInt(maxPositionForHeight));
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void MakeKey()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = new Color(0.5f, 0.1f, 0.5f , 1);

            _isKey.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(_isKey.boolValue, "Key");
            if (_isKey.boolValue)
            {
                EditorGUILayout.Space();
                _keyPositionForWidth.vector2IntValue =
                    EditorGUILayout.Vector2IntField("Position For Width", _keyPositionForWidth.vector2IntValue);
                float minPositionForWidth = _keyPositionForWidth.vector2IntValue.x;
                float maxPositionForWidth = _keyPositionForWidth.vector2IntValue.y;
                float maxWidth = _width.intValue;
                EditorGUILayout.MinMaxSlider(ref minPositionForWidth, ref maxPositionForWidth, 1, maxWidth);
                _keyPositionForWidth.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForWidth),
                    Mathf.RoundToInt(maxPositionForWidth));

                _keyPositionForHeight.vector2IntValue =
                    EditorGUILayout.Vector2IntField("Position For Height", _keyPositionForHeight.vector2IntValue);
                float minPositionForHeight = _keyPositionForHeight.vector2IntValue.x;
                float maxPositionForHeight = _keyPositionForHeight.vector2IntValue.y;
                float maxHeight = _height.intValue;
                EditorGUILayout.MinMaxSlider(ref minPositionForHeight, ref maxPositionForHeight, 1, maxHeight);
                _keyPositionForHeight.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForHeight),
                    Mathf.RoundToInt(maxPositionForHeight));
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void MakePortalPosition()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.magenta;
            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Portal");

            EditorGUILayout.Space();
            _portalPositionForWidth.vector2IntValue =
                EditorGUILayout.Vector2IntField("Position For Width", _portalPositionForWidth.vector2IntValue);
            float minPositionForWidth = _portalPositionForWidth.vector2IntValue.x;
            float maxPositionForWidth = _portalPositionForWidth.vector2IntValue.y;
            float maxWidth = _width.intValue;
            EditorGUILayout.MinMaxSlider(ref minPositionForWidth, ref maxPositionForWidth, 1, maxWidth);
            _portalPositionForWidth.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForWidth),
                Mathf.RoundToInt(maxPositionForWidth));

            _portalPositionForHeight.vector2IntValue = EditorGUILayout.Vector2IntField("Position For Height",
                _portalPositionForHeight.vector2IntValue);
            float minPositionForHeight = _portalPositionForHeight.vector2IntValue.x;
            float maxPositionForHeight = _portalPositionForHeight.vector2IntValue.y;
            float maxHeight = _height.intValue;
            EditorGUILayout.MinMaxSlider(ref minPositionForHeight, ref maxPositionForHeight, 1, maxHeight);
            _portalPositionForHeight.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minPositionForHeight),
                Mathf.RoundToInt(maxPositionForHeight));

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void MakeCoin()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.yellow;
            EditorGUILayout.BeginFoldoutHeaderGroup(false, "Coins");
            _countCoin.intValue = EditorGUILayout.IntField("Count Coin", _countCoin.intValue);
            EditorGUILayout.Slider(_chanceCoin, 0f, 1f, "Chance of Coin");
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void MakeSizeGrid()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.cyan;
            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Grid size");
            EditorGUILayout.IntSlider(_width, 2, 50, "Width grid");
            EditorGUILayout.IntSlider(_height, 2, 50, "Height grid");
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void MakeBase()
        {
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.cyan;
            _groundConfig.objectReferenceValue = 
                EditorGUILayout.ObjectField("Ground Config", _groundConfig.objectReferenceValue, typeof(GroundConfig), false);
            
            GUI.backgroundColor = Color.gray;
            EditorGUILayout.Slider(_spaceBetweenCells, 0.8f, 1.6f, "Space Between Cells");

            GUI.backgroundColor = Color.red;
            EditorGUILayout.Slider(_chanceDestroy, 0f, 1f, "Chance Destroy");
        }

        private void MakeLevel()
        {
            GUI.backgroundColor = Color.green;
            _level.intValue = EditorGUILayout.IntField("Level", _level.intValue);
        }

        private void SaveGrid()
        {
            var gridView = Object.FindFirstObjectByType<GridView>();
            Debug.Log(gridView);
            var width = gridView.Presenter.Grid.Grounds.GetLength(0);
            var height = gridView.Presenter.Grid.Grounds.GetLength(1);
                
            var cells = new Cell[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    foreach (var groundView in gridView.GetComponentsInChildren<GroundView>())
                    {
                        var id = groundView.Presenter.Ground.Id;
                        if (id.x == x && id.y == y)
                        {
                            cells[x, y] = new Cell(id, groundView.Presenter.Ground.GroundType);
                            break;
                        }
                    }
                }
            }
            
            SaveValue(_level.intValue, cells);

            Debug.Log($"Save level: {_level.intValue}, cells : {cells.Length}");
        }

        private void SaveValue(int level, Cell[,] cells)
        {
            var service = new JsonSaveLoadService();
            var levelsData = service.Load("LevelsData", new LevelsData());

            var levelGridData = new LevelGridData()
            {
                Level = _level.intValue,
                Width = _width.intValue,
                Height = _height.intValue,
                IsBuilt = _isBuilt.boolValue,
                SpaceBetweenCells = _spaceBetweenCells.floatValue,
                ChanceDestroy = _chanceDestroy.floatValue,
                CountCoin = _countCoin.intValue,
                ChanceCoin = _chanceCoin.floatValue,
                IsKey = _isKey.boolValue,
                Cells = cells
            };
            
            if (levelsData.Levels.ContainsKey(level))
                levelsData.Levels[level] = levelGridData;
            else
                levelsData.Add(level, levelGridData);

            service.Save(levelsData, "LevelsData");
        }
    }
}