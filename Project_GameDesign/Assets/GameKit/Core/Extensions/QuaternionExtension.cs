using System;
using UnityEngine;

public static class QuaternionExtensions
{
    /// <summary>
    /// Returns a Quaternion slerped between two values according to an AnimationCurve.
    /// </summary>
    public static Quaternion CurvedSlerp(this Quaternion rotation, Quaternion fromRotation, Quaternion toRotation, AnimationCurve curve, float t)
    {
        float curveEvaluate = curve.Evaluate(t);

        return Quaternion.Slerp(fromRotation, toRotation, curveEvaluate);
    }
    
    
    /// <summary>
    /// Returns a Quaternion slerped between two values according to an AnimationCurve.
    /// </summary>
    public static Quaternion CurvedSlerp(this Quaternion rotation, Quaternion fromRotation, Quaternion toRotation, float t)
    {
        AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);
        float curveEvaluate = curve.Evaluate(t);

        return Quaternion.Slerp(fromRotation, toRotation, curveEvaluate);
    }
}