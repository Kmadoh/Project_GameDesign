using UnityEngine;

public static class StringExtensions
{
	// This is the extension method.
    // The first parameter takes the "this" modifier
    // and specifies the type for which the method is defined.
    public static string Bold(this string str) => "<b>" + str + "</b>";

    /// <summary>
    /// Rich Text Format Colors. Here's a list of currently supported:<br></br><br></br>
    /// aqua (same as cyan), black, blue,brown,cyan(same as aqua),darkblue,fuchsia(same as magenta),green,grey,lightblue<br></br>	
    ///lime,magenta(same as fuchsia),maroon,navy,olive,orange,purple,red,silver,teal,white,and yellow	
    /// </summary>
    /// <param name="str"></param>
    /// <param name="clr"></param>
    /// <returns></returns>
    public static string Color(this string str,string clr) => string.Format("<color={0}>{1}</color>",clr,str);
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str,int size) => string.Format("<size={0}>{1}</size>",size,str);
    public static string LogHeader(this string str) => str.Bold().Color("Blue").ToUpper();
    public static void LogHeader(this string str, bool isPrint = true)
    {
	    Debug.Log(isPrint ? str.Bold().Color("Blue").ToUpper() : "");
    }

    /// <summary>
    /// Convert int number into percent format string.<br></br>
    /// Note: Percent whole number hardcoded since it's int based.
    /// </summary>
    public static string Percent(this int num) => (num * 0.01f).ToString("P0");


    /// <summary>
    /// Convert float number into percent format string. Input should be in fraction 0.0f - 1.0f <br></br>
    /// Example: 0.34 - > 34% <br></br>
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Percent(this float num) => (num).ToString("P0");


    /// <summary>
    /// Convert any string into int by using TryParse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int ParseInt(this string input) {
        int.TryParse(input,out int value);
        return value;
    }

    public static string Truncate(this string value, int maxLength)
    {
	    if (string.IsNullOrEmpty(value)) return value;
	    return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Replaces underscore with spacing for GUI clean format. Later can be used for any strange character replacements. Meant to make save easier yet GUI friendly.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string CleanString(this string name) => name.Replace("_", " ");

    /// <summary>
    /// Returns message with args
    /// Example: myMsg("I have {0} args in my string").Format("at least one");
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string Format(this string msg, params object[] args) => string.Format(msg, args);
}