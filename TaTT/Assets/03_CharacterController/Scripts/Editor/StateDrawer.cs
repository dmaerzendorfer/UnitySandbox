using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace _03_StateMachine.Scripts.Editor
{
    // [CustomPropertyDrawer(typeof(State))]
    public class StateDrawer  : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
        
            
            return container;
        }
        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //     // base.OnGUI(position, property, label);
        //     GUILayout.Label("Test");
        //
        // }
        
    }
}