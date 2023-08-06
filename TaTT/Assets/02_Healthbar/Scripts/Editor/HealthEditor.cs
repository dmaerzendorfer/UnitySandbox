using System;
using UnityEditor;
using UnityEngine;

namespace _02_Healthbar.Scripts.Editor
{
    [CustomEditor(typeof(Health))]
    public class HealthEditor : UnityEditor.Editor
    {
        private Health _health;

        public override void OnInspectorGUI()
        {
            //todo: screw the base, we do our own shit now!
            base.OnInspectorGUI();
            GUILayout.Space(5f);
            GUILayout.Label("Display Options", EditorStyles.boldLabel);
            _health.DisplayBar = EditorGUILayout.Toggle("DisplayBar", _health.DisplayBar);
            
            //todo: add test dmg etc button, show current health (readonly tho!), show more display options
        }

        void OnEnable()
        {
            _health = (Health)target;
        }
    }
}