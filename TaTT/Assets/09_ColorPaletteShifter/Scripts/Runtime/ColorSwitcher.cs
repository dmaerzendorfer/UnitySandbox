using UnityEngine;
using UnityEngine.UI;

namespace _09_ColorPaletteShifter.Scripts.Runtime
{
    [ExecuteInEditMode]
    public class ColorSwitcher : MonoBehaviour, IColorSwitcher
    {
        public string nameInPalette;
        private SpriteRenderer _spriteRenderer;

        private MeshRenderer _meshRenderer;
        private Image _image;

        private ColorPaletteManager _colorPaletteManager;

        private void Start()
        {
            _colorPaletteManager = ColorPaletteManager.Instance;
#if UNITY_EDITOR
            //in the editor the singleton might not be set yet.
            if (_colorPaletteManager == null)
            {
                _colorPaletteManager = FindObjectOfType<ColorPaletteManager>();
            }
#endif

            if (_colorPaletteManager == null)
            {
                Debug.LogWarning("ColorSwitcher could not find a ColorPaletteManager.", this);
                return;
            }

            _colorPaletteManager.Subscribe(this);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _image = GetComponent<Image>();

            ApplyPalette(_colorPaletteManager.GetCurrentPalette());
        }


        public void ApplyPalette(ColorPalette newPalette)
        {
            try
            {
                if (_spriteRenderer != null)
                {
                    _spriteRenderer.color = newPalette.GetColor(nameInPalette);
                }

                if (_meshRenderer != null)
                {
                    _meshRenderer.sharedMaterial.color = newPalette.GetColor(nameInPalette);
                }

                if (_image != null)
                {
                    _image.color = newPalette.GetColor(nameInPalette);
                }
            }
            catch { }
        }


        private void OnDestroy()
        {
            _colorPaletteManager.Unsubscribe(this);
        }
    }
}