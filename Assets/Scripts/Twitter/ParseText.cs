using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ParseText : MonoBehaviour
{
	public static string RemoveURLs(string source)
	{
		return Regex.Replace(source, @"http[^\s]+", "");
	}


	public static string RemoveUsernames(string source)
	{
		return Regex.Replace(source, @"@(?<=@)\w+", "");
	}


	public static string RemoveHashtags(string source)
	{
		//return Regex.Replace(source, @"(?<=#)\w+$", "");
		return Regex.Replace(source, @"#", "");
	}

	public static string CleanUp(string source)
	{
		string cleanedSource = source;

		cleanedSource = cleanedSource.Replace("&amp; ", "");

		return cleanedSource;
	}
}
