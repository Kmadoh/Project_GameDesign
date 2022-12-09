using UnityEngine;
using UnityEngine.UI;

public static class GUIExtensions
{
	/// <summary>
    /// Preserves Color while taking the button text and changing it's opacity.
    /// </summary>
    /// <param name="button"></param>
    /// <param name="opacity"></param>
    public static void SetButtonTextOpacity(this Button button, float[] opacitySwitch, bool isActive) 
	{
        Color textColor = button.transform.GetComponentInChildren<Text>().color;
        button.transform.GetComponentInChildren<Text>().color = textColor.SetTransparency(opacitySwitch[System.Convert.ToInt32(!isActive)]);
    }


    //GREAT EXAMPLE OF FORMATS FOR SUMMARY
    /// <summary>
    /// Testing Summary examples<br></br>
    ///<b>Bold</b> = Custom way to doc summary.<br></br>
    ///<i>Italic = Custom way to doc summary.</i><br></br>
    ///<c>Indicates Code</c>
    ///<code>code example 2</code>
    ///See ref example: <see cref="RandomRoll100"/>
    ///<example> Example doesn't work</example>
    ///<para>Paragraph to be used in summary</para>
    ///<remarks>Remarks are debateable</remarks>
    /// <list type="bullet">
    /// <item>
    /// <description>Item 1. from list example</description>
    /// </item>
    /// <item>
    /// <description>Item 2. from list example</description>
    /// </item>
    /// </list>
    /// <b><paramref name="thisVar"/></b> parameter takes a number.
    ///</summary>
    ///<typeparam name="int">The element type of the array doesn't work</typeparam>
    ///<returns> returns remark - int </returns>
    ///<exception cref="System.Exception">exception description</exception>
    ///<param name="Int1">Parameters don't work in or out of summary</param>
    ///<permission cref="System.Security.PermissionSet">Permission doesn't work</permission>
    ///<value>property-description of c# type Value</value>
    public static void CommentSummaryExample(int thisVar) { }
    
	//Secondary extension class to Input methods from UnityEngine
	public static class KeyInput 
	{
	    /// <summary>
	    /// Go through all keyboard array on key press
	    /// </summary>
	    /// <returns>
	    /// Input Key found
	    /// </returns>
	    public static KeyCode Get() 
	    {
	        KeyCode key = KeyCode.None;
	        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
	        {
	            if(Input.GetKey(vKey)) key = vKey;
	        }

	        return key;
	    }
	}
	
	/// <summary>
	/// Converts rect transform position into world space coordinates
	/// </summary>
	/// <param name="element">Element we need to get world position of</param>
	/// <returns>World Space Coordinates of rect transform</returns>
	public static Vector2 GetWorldPositionOfCanvasElement(this RectTransform element)
	{
		RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main, out Vector3 result);
		return result;
	}
}