using UnityEngine;
using UnityEngine.UI;

namespace _09_ColorPaletteShifter.Scripts.Runtime
{
    [ExecuteInEditMode]
    public class ColorSwitcher : MonoBehaviour, IColorSwitcher
    {
        public string nameInPalette;
        public bool useSharedMaterial = false;
        private SpriteRenderer _spriteRenderer;

        private MeshRenderer _meshRenderer;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private Image _image;

        private ColorPaletteManager _colorPaletteManager;

#if UNITY_EDITOR
        private Material tempMaterial;
#endif

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            _image = GetComponent<Image>();

            _colorPaletteManager = ColorPaletteManager.Instance;
#if UNITY_EDITOR
            //in the editor the singleton might not be set yet.
            if (_colorPaletteManager == null)
            {
                _colorPaletteManager = FindObjectOfType<ColorPaletteManager>();
            }

            //also create a temp material so we dont have any material leakage
            if (_meshRenderer)
            {
                tempMaterial = new Material(_meshRenderer.sharedMaterial);
                _meshRenderer.sharedMaterial = tempMaterial;
            }
            if (_skinnedMeshRenderer)
            {
                tempMaterial = new Material(_skinnedMeshRenderer.sharedMaterial);
                _skinnedMeshRenderer.sharedMaterial = tempMaterial;
            }

#endif

            if (_colorPaletteManager == null)
            {
                Debug.LogWarning("ColorSwitcher could not find a ColorPaletteManager.", this);
                return;
            }

            _colorPaletteManager.Subscribe(this);


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
                    if (useSharedMaterial)
                        _meshRenderer.sharedMaterial.color = newPalette.GetColor(nameInPalette);
                    else
                        _meshRenderer.material.color = newPalette.GetColor(nameInPalette);
                }
                if (_skinnedMeshRenderer != null)
                {
                    if (useSharedMaterial)
                        _skinnedMeshRenderer.sharedMaterial.color = newPalette.GetColor(nameInPalette);
                    else
                        _skinnedMeshRenderer.material.color = newPalette.GetColor(nameInPalette);
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