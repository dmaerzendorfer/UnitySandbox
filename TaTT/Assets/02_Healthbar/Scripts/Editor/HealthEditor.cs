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
            // base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Health Settings", EditorStyles.boldLabel);
            _health.MaxHealth = EditorGUILayout.FloatField("Max Health", _health.MaxHealth);
            //show greyed out currentHealth
            GUI.enabled = false;
            EditorGUILayout.FloatField("Current Health", _health.CurrentHealth);
            GUI.enabled = true;

            //death settings
            GUILayout.Space(5f);
            GUILayout.Label("Death Settings", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Death Response");
            _health.deathHandling = (Health.DeathResponse)EditorGUILayout.EnumPopup(_health.deathHandling);
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            _health.showEventFoldout = EditorGUILayout.Foldout(_health.showEventFoldout,
                new GUIContent("Event Settings"), true);
            if (_health.showEventFoldout)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onDeath"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onDamage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onHeal"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onHealthChange"));
            }


            GUILayout.Space(5f);
            _health.showDisplayFoldout = EditorGUILayout.Foldout(_health.showDisplayFoldout,
                new GUIContent("Display Options"), true);
            if (_health.showDisplayFoldout)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Display Mode");
                _health.healthDisplay = (Health.DisplayOption)EditorGUILayout.EnumPopup(_health.healthDisplay);
                GUILayout.EndHorizontal();

                if (_health.healthDisplay != Health.DisplayOption.Hidden)
                {
                    _health.DisplayBar = EditorGUILayout.Toggle("Display UI", _health.DisplayBar);
                    _health.hideOnFullHealth = EditorGUILayout.Toggle("Hide on full Health", _health.hideOnFullHealth);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("displayCanvas"));
                }

                if (_health.healthDisplay == Health.DisplayOption.Bar)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("healthBarSlider"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("healthBarColor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("healthBarFill"));
                }

                if (_health.healthDisplay == Health.DisplayOption.Hearts)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("healthHeartBar"));
                }
            }

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Deal 1 Damage") && Application.isPlaying)
            {
                _health.TakeDamage(1);
            }

            if (GUILayout.Button("Heal 1 Damage") && Application.isPlaying)
            {
                _health.HealHealth(1);
            }
        }

        void OnEnable()
        {
            _health = (Health)target;
        }
    }
}