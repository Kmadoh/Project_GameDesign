using UnityEngine;

public static class ColorExtensions
{
    //Set transparency on color
    public static Color SetTransparency(this Color color,float alpha) 
    {
        color = new Color(color.r,color.g,color.b,alpha);
        return color;
    }
}