using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class MiscExtensions
{
	 /// <summary>
    /// Simple save list to text file and directory<br></br>
    /// <param><b>input:</b> List of string.</param><br></br>
    /// <param><b>dir:</b> Directory to save to.</param><br></br>
    /// <param><b>textFileName:</b> Name of file to save. Extension not included, please add.</param><br></br>
    /// </summary>
	 public static void SaveFile(this List<string> input,string dir,string textFileName)
	 {
		 /*
	        Simple save list to text file and directory

	        input: List of string.
	        dir: Directory to save to.
	        textFileName: Name of file to save. Extension not included, please add.
	    */
	    //print("saving file..." + dir + textFileName);
	    using TextWriter tw = new StreamWriter(dir + textFileName);
	    foreach(string s in input)
		    tw.WriteLine(s);
	 }


    /// <summary>
    /// Hard to say if this is already a built in function for C# or Unity
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool IsInRange(this int value,int min,int max) => Mathf.Clamp(value,min,max) == value;

    /// <summary>
    ///
    /// Searches directory and outputs list<br></br>
    /// 
    /// sDir: Directory to read from<br></br>
    /// return: List of files and subdirectories<br></br>
    /// 
    /// </summary>
    /// //ar files = Directory.GetFiles(@"C:\", "*").OrderByDescending(d => new FileInfo(d).CreationTime);
    public static List<string> GetAllDirFiles(string sDir,string exclusions = "", bool isSortByDate = false) 
    {
	    List<string> dirList = new List<string>();

	    try 
	    {
		    //Get all the files in the initial directory
		    //string[] initialFiles = Directory.GetFiles(sDir);
		    List<string> initialFiles = isSortByDate ? Directory.GetFiles(sDir).OrderByDescending(d => new FileInfo(d).CreationTime).ToList()
			    : Directory.GetFiles(sDir).ToList();
      

		    foreach (string f in initialFiles) 
		    {
			    if(!f.Contains(exclusions.Replace("*",""))) 
			    {
				    dirList.Add(f);
			    }
		    }

		    //Then grab everything in subdirectories
		    foreach(string d in Directory.GetDirectories(sDir)) 
		    {
			    //grab all files, list exclusions to a var, then add to var
			    string[] allFiles = Directory.GetFiles(d);

			    string[] filesToExclude = Directory.GetFiles(d,exclusions);
			    IEnumerable<string> wantedFiles = allFiles.Except(filesToExclude);

			    foreach(string f in wantedFiles) 
			    {
				    dirList.Add(f);
			    }
			    dirList.AddRange(GetAllDirFiles(d,exclusions));
		    }

	    } 
	    catch(System.Exception excpt) 
	    {
		    Debug.Log(excpt.Message);
	    }
	    return dirList;
    }
}