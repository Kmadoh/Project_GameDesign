using System;
using UnityEngine;

public static class VectorExtensions
{
	public static Vector2 ToVector2(this Vector3 vector) 
	{
		return vector;
    }
    
    /// <summary>
    /// Adds a float to a Vector3's x, y and z values.
    /// </summary>
    /// <param name="v">Vector3 we want to increment</param>
    /// <param name="f">float value to add to x, y and z values</param>
    /// <returns></returns>
    public static Vector3 PlusFloat (this Vector3 v, float f)
    {
	    return new Vector3(v.x + f, v.y + f, v.z + f);
    }
    
     /// <summary>
     /// Converts World space coordinates to Local Space
     /// </summary>
     /// <param name="t">Transform used for reference</param>
     /// <param name="world">World Space Coordinates</param>
     /// <returns>Local Space coordinates</returns>
     public static Vector3 WorldToLocalSpace(this Vector3 world, Transform t)
     {
	     Vector3 localPos = t.right * world.x + t.up * world.y + t.forward * world.z;

	     return localPos;
     }
     
    //Easy way to round Vector 3 by decimal
    public static Vector3 Round(this UnityEngine.Vector3 vector3,int decimalPlaces = 2) 
    {
        float multiplier = 1;
        for(int i = 0; i < decimalPlaces; i++) 
        {
            multiplier *= 10f;
        }

        return new UnityEngine.Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }
    
    public static Vector3 Flattened(this Vector3 vector)
    {
	    return new Vector3(vector.x, 0f, vector.z);
    }

    public static float DistanceFlat(this Vector3 origin, Vector3 destination)
    {
	    return Vector3.Distance(origin.Flattened(), destination.Flattened());
    }
    
    public static Vector2 SetX(this Vector2 vector, float x)
    {
	    return new Vector2(x, vector.y);
    }
    public static Vector3 SetX(this Vector3 vector, float x)
    {
	    return new Vector3(x, vector.y, vector.z);
    }
    
    public static Vector2 SetY(this Vector2 vector, float y)
    {
	    return new Vector2(vector.x, y);
    }
    public static Vector3 SetY(this Vector3 vector, float y)
    {
	    return new Vector3(vector.x, y, vector.z);
    }
    
    public static Vector3 SetZ(this Vector3 vector, float z)
    {
	    return new Vector3(vector.x, vector.y, z);
    }
    
    public static Vector2 AddX(this Vector2 vector, float x)
    {
	    return new Vector2(vector.x + x, vector.y);
    }    
    public static Vector3 AddX(this Vector3 vector, float x)
    {
	    return new Vector3(vector.x + x, vector.y, vector.z);
    }
    
    public static Vector2 AddY(this Vector2 vector, float y)
    {
	    return new Vector2(vector.x , vector.y + y);
    }
    public static Vector3 AddY(this Vector3 vector, float y)
    {
	    return new Vector3(vector.x , vector.y + y, vector.z);
    }
    
    public static Vector3 AddZ(this Vector3 vector, float z)
    {
	    return new Vector3(vector.x, vector.y, vector.z + z);
    }
    
    public static Vector2 XY(this Vector3 v) 
    {
	    return new Vector2(v.x, v.y);
    }
    
    public static Vector2 XZ(this Vector3 v) 
    {
	    return new Vector2(v.x, v.z);
    }

    public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
    {
	    if (!isNormalized) axisDirection.Normalize();
	    float d = Vector3.Dot(point, axisDirection);
	    return axisDirection * d;
    }
    
    public static Vector2 GetClosestVector2From(this Vector2 vector, Vector2[] otherVectors)
    {
	    if (otherVectors.Length == 0) throw new Exception("The list of other vectors is empty");
	    float minDistance = Vector2.Distance(vector, otherVectors[0]);
	    Vector2 minVector = otherVectors[0];
	    for (int i = otherVectors.Length - 1; i > 0; i--)
	    {
		    float newDistance = Vector2.Distance(vector, otherVectors[i]);
		    
		    if (!(newDistance < minDistance)) continue;
		    
		    minDistance = newDistance;
		    minVector = otherVectors[i];
	    }
	    return minVector;
    }

    public static Vector3 GetClosestVector3From(this Vector3 vector, Vector3[] otherVectors)
    {
	    if (otherVectors.Length == 0) throw new Exception("The list of other vectors is empty");
	    float minDistance = Vector3.Distance(vector, otherVectors[0]);
	    Vector3 minVector = otherVectors[0];
	    for (int i = otherVectors.Length - 1; i > 0; i--)
	    {
		    float newDistance = Vector3.Distance(vector, otherVectors[i]);
		    
		    if (!(newDistance < minDistance)) continue;
		    
		    minDistance = newDistance;
		    minVector = otherVectors[i];
	    }
	    return minVector;
    }
    
    public static Vector3 GetClosestTransformFrom(this Vector3 vector, Transform[] otherTransforms)
    {
	    if (otherTransforms.Length == 0) throw new Exception("The list of other vectors is empty");
	    
	    float minDistance = Vector3.Distance(vector, otherTransforms[0].position);
	    Vector3 minVector = otherTransforms[0].position;
	    for (int i = otherTransforms.Length - 1; i > 0; i--)
	    {
		    float newDistance = Vector3.Distance(vector, otherTransforms[i].position);
		    
		    if (!(newDistance < minDistance)) continue;
		    
		    minDistance = newDistance;
		    minVector = otherTransforms[i].position;
	    }
	    return minVector;
    }
    
    public static Vector3 GetClosestColliderFrom(this Vector3 vector, Collider[] otherColliders)
    {
	    if (otherColliders.Length == 0) throw new Exception("The list of other vectors is empty");
	    
	    float minDistance = Vector3.Distance(vector, otherColliders[0].transform.position);
	    Vector3 minVector = otherColliders[0].transform.position;
	    for (int i = otherColliders.Length - 1; i > 0; i--)
	    {
		    float newDistance = Vector3.Distance(vector, otherColliders[i].transform.position);
		    
		    if (!(newDistance < minDistance)) continue;
		    
		    minDistance = newDistance;
		    minVector = otherColliders[i].transform.position;
	    }
	    return minVector;
    }
    
    /// <summary>
    /// Returns a float lerped between two values according to an AnimationCurve.
    /// </summary>
    public static float CurvedLerp(this Vector2 minMaxValue, AnimationCurve curve, float t)
    {
	    float curveEvaluate = curve.Evaluate(t);

	    float lerpedValue = Mathf.Lerp(minMaxValue.x, minMaxValue.y, curveEvaluate);

	    return lerpedValue;
    }
	
    /// <summary>
    /// Returns a float lerped between two values according to an AnimationCurve.
    /// </summary>
    public static float CurvedLerp (this Vector2 minMaxValue, float t)
    {
	    AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	    float curveEvaluate = animCurve.Evaluate(t);

	    float lerpedValue = Mathf.Lerp(minMaxValue.x, minMaxValue.y, curveEvaluate);

	    return lerpedValue;
    }

    public static Vector3 Towards(this Vector3 from, Vector3 towards, bool normalized)
    {
	    return normalized ? (towards - from).normalized : (towards - from);
    }
    
    public static Vector3 Towards(this Vector3 from, Vector3 towards)
    {
	    return towards - from;
    }
}