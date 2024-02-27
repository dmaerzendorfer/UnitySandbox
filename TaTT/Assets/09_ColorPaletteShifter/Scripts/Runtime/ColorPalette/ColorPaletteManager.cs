using System.Collections.Generic;
using _09_ColorPaletteShifter.Scripts.Runtime;
using _09_ColourPaletteShifter.Scripts.Runtime;
using _Generics.Scripts.Runtime;
using UnityEngine;


[ExecuteInEditMode]
public class ColorPaletteManager : SingletonMonoBehaviour<ColorPaletteManager>
{
    //has a dictionary of color pallets -> scriptable objects! -> key is an int
    // also make a dictionary for saving string as a key -> just another lookup table
    //not sure if it makes sense but i like the idea of "readable" code eG change palette to "lightsOut" instead of changing it to palette no. 12

    [SerializeField]
    private List<ColorPalette> colorPalettes;

    [SerializeField]
    private int currentPalette = 0;

    private List<IColorSwitcher> _colorSwitchers = new List<IColorSwitcher>();

    public ColorPalette GetCurrentPalette()
    {
        return colorPalettes[currentPalette];
    }

    public void ChangePalette(int paletteIndex)
    {
        currentPalette = paletteIndex;
        Notify();
    }

    [ContextMenu("ChangeToNextPalette")]
    public void ChangeToNextPalette()
    {
        currentPalette++;
        currentPalette %= colorPalettes.Count;
        Notify();
    }

    public void Notify()
    {
        foreach (var switcher in _colorSwitchers)
        {
            switcher.ApplyPalette(colorPalettes[currentPalette]);
        }
    }


    public void Subscribe(IColorSwitcher switcher)
    {
        _colorSwitchers.Add(switcher);
    }

    public void Unsubscribe(IColorSwitcher switcher)
    {
        _colorSwitchers.Remove(switcher);
    }
}