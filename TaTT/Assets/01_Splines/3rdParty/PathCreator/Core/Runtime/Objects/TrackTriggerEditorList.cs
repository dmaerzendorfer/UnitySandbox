using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PathCreation
{
    public static class TrackTriggerEditorList
    {
        public static void Show(SerializedProperty list)
        {
            EditorGUI.indentLevel += 1;
            List<int> toBeDeleted = new List<int>();
            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedProperty item = list.GetArrayElementAtIndex(i);

                var foldedOut = item.FindPropertyRelative("_foldedOut");
                foldedOut.boolValue =
                    EditorGUILayout.Foldout(foldedOut.boolValue, new GUIContent($"Trigger {i}"), true);
                if (foldedOut.boolValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(item.FindPropertyRelative("enabled"));
                    EditorGUILayout.PropertyField(item.FindPropertyRelative("position"));
                    GUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(item.FindPropertyRelative("trackEvent"));
                    if (GUILayout.Button("Delete"))
                    {
                        toBeDeleted.Add(i);
                    }
                }
            }

            foreach (var idx in toBeDeleted)
            {
                list.DeleteArrayElementAtIndex(idx);
            }

            EditorGUI.indentLevel -= 1;
        }
    }
}