using System.Collections.Generic;
using UnityEngine;

public static class LogExtensions
{
	/// <summary>
	/// Print contents of array[]<br></br>
	/// </summary>
	public static string Print<T>(this T[] array,bool isMute = false) 
	{
		string output = "[",
			comma = ", ";
		for(int i = 0; i < array.Length; i++) {
			if(i == array.Length - 1) comma = "";
			output += array[i] + comma;
		}
		output += "]"; //add end bracket
		if(!isMute) Debug.Log(output);

		return output;
	}

	/// <summary>
	/// Print contents of a List as string<br></br>
	/// Usage: myList.Print();<br></br>
	/// isLogOutput: true = Debug.Log(output). Meant for printing standalone. Use when you don't want plan to use in print() statement.<br></br>
	/// Great shortcut for debugging lists wilst saving code
	/// </summary>
	public static string Print<T>(this List<T> array) 
	{
		string output = "[",
			comma = ", ";
		for(int i = 0; i < array.Count; i++) {
			if(i == array.Count - 1) comma = "";
			output += array[i] + comma;
		}

		output += "]"; //add end bracket

		return output;
	}
}
