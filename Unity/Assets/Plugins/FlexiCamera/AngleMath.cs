using UnityEngine;
using System.Collections;

public class AngleMath : object {

	// Project v1, v2 onto plane defined by normal n.
	// Return right handed signed angle between projected vectors v1.v2
	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
	    return Mathf.Atan2(
	        Vector3.Dot(n, Vector3.Cross(v1, v2)),
	        Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}
