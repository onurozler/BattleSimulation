using System.Collections.Generic;
using Core.Model.Config;
using Core.Model.Config.Formation;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.UnitFormation
{
    public class UnitFormationEditor : EditorWindow
    {
        private const int FormationHeight = 5;
        private static string _formationName = "Default";
        private static int _formationSize = 20;
        private static bool _isSettingsCreated;
        private static bool[,] _formationCheck;

        private int _checkCounter;


        [MenuItem("Game Editor/Create Formation")]
        private static void CreateFormationEditor()
        {
            GetWindow<UnitFormationEditor>("Create Formation");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Formation Settings",EditorStyles.boldLabel);
            _formationName = EditorGUILayout.TextField(new GUIContent("Formation Name"), _formationName);
            _formationSize = EditorGUILayout.IntField(new GUIContent("Formation Size"), _formationSize);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Create Formation Order"))
            {
                _isSettingsCreated = true;
                _formationCheck = new bool[FormationHeight,_formationSize / 2];
            }

            if (_isSettingsCreated)
            {
                var guiLayoutOption = new []
                {
                    GUILayout.MaxHeight(15),
                    GUILayout.MaxWidth(15)
                };

                _checkCounter = 0;
                EditorGUILayout.LabelField("Formation Order", EditorStyles.boldLabel);
                for (int i = 0; i < _formationCheck.GetLength(0); i++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    for (int j = 0; j < _formationCheck.GetLength(1); j++)
                    {
                        _formationCheck[i,j] = EditorGUILayout.Toggle("", _formationCheck[i,j], guiLayoutOption);
                        _checkCounter += _formationCheck[i, j] ? 1 : 0;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (_checkCounter == _formationSize)
                {
                    if (GUILayout.Button("Create Formation!"))
                    {
                        var formationAsset = CreateInstance<FormationConfigData>();
                        formationAsset.FormationName = _formationName;
                        formationAsset.UnitSize = _formationSize;
                        var formationPosition = new List<Vector2>();

                        for (int i = 0; i < _formationCheck.GetLength(0); i++)
                        {
                            for (int j = 0; j < _formationCheck.GetLength(1); j++)
                            {
                                if (_formationCheck[i, j])
                                {
                                    formationPosition.Add(new Vector2(j,i));
                                }
                            }
                        }
                        formationAsset.Positions = formationPosition;

                        var path = AssetDatabase.GenerateUniqueAssetPath($"{Constants.FormationAssetPath}{_formationName}_FormationData.asset");
                        AssetDatabase.CreateAsset(formationAsset,path);
                        AssetDatabase.SaveAssets();
                        
                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = formationAsset;
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox($"You currently have {_checkCounter} units. You must have {_formationSize} units",MessageType.Error);
                }
            }
        }

        private void OnDisable()
        {
            _isSettingsCreated = false;
        }
    }   
}
