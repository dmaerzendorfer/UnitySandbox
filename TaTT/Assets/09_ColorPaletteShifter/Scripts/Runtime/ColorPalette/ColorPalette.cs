using System;
using System.Collections.Generic;
using System.Linq;
using _09_ColourPaletteShifter.Scripts.Runtime;
using UnityEngine;

namespace _09_ColorPaletteShifter.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField]
        private List<ColorItem> colors = new List<ColorItem>();


        public Color GetColor(string name)
        {
            var item = colors.FirstOrDefault(x => x.name == name);

            if (item != null)
            {
                return item.color;
            }

            throw new Exception($"Could not find color with name {name} in the palette.");
        }
    }
}