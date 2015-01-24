using UnityEngine;
using System.Collections;

public static class BasicUtils
{

	public static string GetTimeString(float t) 
	{ 
		string minSec = string.Format("{0}:{1:00}", (int)t / 60, (int)t % 60);
		
		return minSec;
	}
}
