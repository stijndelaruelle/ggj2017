using System;
using System.Collections;
using System.Collections.Generic;

public static class StringExtension
{
	public static string Shuffle(this string str)
	{
		char[] array = str.ToCharArray();
		Random rng = new Random();
		int n = array.Length;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			var value = array[k];
			array[k] = array[n];
			array[n] = value;
		}
		return new string(array);
	}
}
