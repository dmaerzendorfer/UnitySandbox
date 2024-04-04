using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(PupilLook), true)]
[CanEditMultipleObjects]
public class PupilLookEditor : Editor {

	SerializedProperty useLids;
	SerializedProperty flat;
	SerializedProperty mode;
	SerializedProperty transform;
	SerializedProperty point;
	SerializedProperty maxDist;

	SerializedProperty randomOffset;
	SerializedProperty randomSpeed;

	void OnEnable(){
		useLids = serializedObject.FindProperty ("useLids");
		flat = serializedObject.FindProperty ("flat");
		mode = serializedObject.FindProperty ("mode");
		transform = serializedObject.FindProperty("targTrans");
		point = serializedObject.FindProperty ("targPoint");
		maxDist = serializedObject.FindProperty ("maxDist");

		randomOffset = serializedObject.FindProperty ("randomOffset");
		randomSpeed = serializedObject.FindProperty ("randomSpeed");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update ();
		EditorGUILayout.PropertyField (useLids);


		flat.boolValue = EditorGUILayout.Toggle ("2D", flat.boolValue);
		EditorGUILayout.PropertyField (mode);

		//Transform mode
		if (mode.enumValueIndex == 0)
			EditorGUILayout.PropertyField (transform);

		//Point mode
		else {
			//Display vector2
			if (flat.boolValue) {
				point.vector3Value = EditorGUILayout.Vector2Field ("Point", (Vector2)point.vector3Value);
			}

			//Display vector3
			else
				EditorGUILayout.PropertyField (point);
		}

		//Show maxDist
		if(flat.boolValue)
			EditorGUILayout.PropertyField (maxDist);


		EditorGUILayout.PropertyField (randomOffset);
		EditorGUILayout.PropertyField (randomSpeed);


		serializedObject.ApplyModifiedProperties();
	}
}