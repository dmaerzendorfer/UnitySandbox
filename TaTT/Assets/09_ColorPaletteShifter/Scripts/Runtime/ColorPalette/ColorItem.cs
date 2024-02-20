using System;
using UnityEngine;

namespace _09_ColourPaletteShifter.Scripts.Runtime
{
    [Serializable]
    public class ColorItem
    {
        public String name;

        [ColorUsage(true, true)]
        public Color color;
    }
}