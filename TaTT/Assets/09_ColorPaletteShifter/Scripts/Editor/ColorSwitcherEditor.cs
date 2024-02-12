using System.Reflection;
using _09_ColorPaletteShifter.Scripts.Runtime;
using UnityEditor;

namespace _09_ColorPaletteShifter.Scripts.Editor
{
    [CustomEditor(typeof(ColorSwitcher))]
    public class ColorSwitcherEditor : UnityEditor.Editor
    {
        public SerializedProperty nameInPallete;

        private ColorSwitcher _target;

        private ColorPaletteManager _colorPaletteManager;

        public void OnEnable()
        {
            nameInPallete = serializedObject.FindProperty("nameInPalette");
            _target = (ColorSwitcher)target;
            _colorPaletteManager = FindObjectOfType<ColorPaletteManager>();
        }

        public override void OnInspectorGUI()
        {
            //get value before change
            string previousValue = nameInPallete.stringValue;

            // Make all the public and serialized fields visible in Inspector
            base.OnInspectorGUI();

            // load changed values
            serializedObject.Update();

            //change color if name changed
            if (previousValue != nameInPallete.stringValue && _colorPaletteManager != null)
            {
                _target.ApplyPalette(_colorPaletteManager.GetCurrentPalette());
            }
        }
    }
}