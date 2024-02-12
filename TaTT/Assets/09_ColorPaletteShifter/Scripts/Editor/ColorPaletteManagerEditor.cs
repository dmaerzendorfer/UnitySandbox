using UnityEditor;

namespace _09_ColorPaletteShifter.Scripts.Editor
{
    [CustomEditor(typeof(ColorPaletteManager))]
    public class ColorPaletteManagerEditor : UnityEditor.Editor
    {
        public SerializedProperty currentPaletteId;

        private ColorPaletteManager _target;

        public void OnEnable()
        {
            currentPaletteId = serializedObject.FindProperty("currentPalette");
            _target = (ColorPaletteManager)target;
        }

        public override void OnInspectorGUI()
        {
            //get value before change
            int previousValue = currentPaletteId.intValue;

            // Make all the public and serialized fields visible in Inspector
            base.OnInspectorGUI();

            // load changed values
            serializedObject.Update();

            //call notify if value changed
            if (previousValue != currentPaletteId.intValue)
            {
                _target.Notify();
            }
        }
    }
}