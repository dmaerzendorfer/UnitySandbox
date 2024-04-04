using UnityEditor;
using UnityEngine;
using System;

//Because unity won't let you use CustomEditor multiple times & property drawers work differently than inspectors
[CustomPropertyDrawer(typeof(EyeSync.StareProps))]
public class StarePropsDrawer : PropertyDrawer{

	//Height of a property
	const float height = 17;
	//Is the foldout expanded
	bool show = false;

	//"conditional properties"
	SerializedProperty flat;
	SerializedProperty mode;

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		//Debug.Log (base.GetPropertyHeight (prop, label));
		if (!show)
			return base.GetPropertyHeight (prop, label);
		if (mode.enumValueIndex == 0)
			return base.GetPropertyHeight (prop, label) * (flat.boolValue ? 8 : 7) + height;

		return base.GetPropertyHeight (prop, label) * (flat.boolValue ? 9 : 8) + height;
	}

	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label){
		//Uncomment for prefab logic (no idea how it works)
		//EditorGUI.BeginProperty (pos, label, prop);

		pos.height = height;
		show = EditorGUI.Foldout (pos, show, label.text, true);

		//Foldout
		if (show) {
			//Indent properly
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 1;

			//Re standardise height
			pos.height = height;

			//Set up refs to "conditional properties"
			flat = prop.FindPropertyRelative ("flat");
			mode = prop.FindPropertyRelative ("mode");

			//Increment y position before each property
			pos.y += height;
			//Had issue w/ just using property field where selection did nothing
			mode.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup (pos, "Target Mode", (PupilLook.LookMode)mode.enumValueIndex));

			pos.y += height;
			EditorGUI.PropertyField (pos, prop.FindPropertyRelative ("useLids"));

			pos.y += height;
			flat.boolValue = EditorGUI.Toggle (pos, "2D", flat.boolValue);


			pos.y += height;
			//Transform mode
			if (mode.enumValueIndex == 0)
				EditorGUI.PropertyField (pos, prop.FindPropertyRelative ("targTrans"), new GUIContent ("Transform"));

			//Point mode
			else {

				//Display vector2
				if (flat.boolValue) {
					prop.FindPropertyRelative ("targPoint").vector3Value = EditorGUI.Vector2Field (pos, "Point", (Vector2)prop.FindPropertyRelative ("targPoint").vector3Value);

				}

				//Display vector3
				else
					EditorGUI.PropertyField (pos, prop.FindPropertyRelative ("targPoint"), new GUIContent ("Point"));

				//Padding to keep field out of other fields when inspector is small
				pos.y += height;
			}

			//Show maxDist
			if (flat.boolValue) {
				pos.y += height;
				EditorGUI.PropertyField (pos, prop.FindPropertyRelative ("maxDist"));
			}

			pos.y += height;
			EditorGUI.PropertyField (pos, prop.FindPropertyRelative ("randomOffset"));
			pos.y += height;
			EditorGUI.PropertyField (pos, prop.FindPropertyRelative ("randomSpeed"));


			///Reset indents
			EditorGUI.indentLevel = indent;
		}

		//Uncomment for prefab logic (no idea how it works)
		//EditorGUI.EndProperty ();
	}
}