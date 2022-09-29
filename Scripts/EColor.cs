using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EColor
{
    None = 0b_000,
    Red = 0b_001,
    Blue = 0b_010,
    Yellow = 0b_100,
    Purple = Red + Blue,
    Green = Blue + Yellow, 
    Orange = Yellow + Red,
    Black = Red + Blue + Yellow,
}

static class ColorSupport
{
    public static Color toColor(this EColor c)
    {
        //123
        Color result=Color.white;
       
        switch (c)
        {
            case EColor.None:
                break;

            case EColor.Red:
                result = Color.red;
                break;

            case EColor.Blue:
                result = Color.blue;
                break;

            case EColor.Yellow:
                result = Color.yellow;
                break;

            case EColor.Purple:
                result = new Color(144.0f/255.0f, 9.0f/255.0f, 249.0f / 255.0f, 255.0f / 255.0f);
                break;

            case EColor.Green:
                result = new Color(18.0f / 255.0f, 158.0f / 255.0f, 7.0f / 255.0f);
                break;

            case EColor.Orange:
                result = new Color(253.0f / 255.0f, 120.0f / 255.0f, 2.0f / 255.0f, 255.0f / 255.0f);
                break;

            case EColor.Black:
                result = Color.black;
                break;

            default:
                break;
        }


        return result;
    }

}
