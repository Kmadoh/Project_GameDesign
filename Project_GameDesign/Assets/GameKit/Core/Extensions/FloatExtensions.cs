using UnityEngine;

public static class FloatExtensions
{
	/// <summary>
	/// Remaps a value from a min and a max to another min and max value ranges
	/// </summary>
	/// <param name="value">Value we remap from</param>
	/// <param name="from1">Min value to remap from</param>
	/// <param name="to1">Min value to remap to</param>
	/// <param name="from2">Max value to remap from</param>
	/// <param name="to2">Max value to remap to</param>
	/// <returns>Remapped value</returns>
	public static float Remap (this float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public static float RotationNormalizedDeg(this float rotation) {
		rotation = rotation % 360f;
		if (rotation < 0)
			rotation += 360f;
		return rotation;
	}

	public static float RotationNormalizedRad(this float rotation) {
		rotation = rotation % Mathf.PI;
		if (rotation < 0)
			rotation += Mathf.PI;
		return rotation;
	}


	/// <summary>Round to nearest number of float points<br></br>
	/// <param><b>roundVal:</b> Number of decimal points to round value.(i.e. 2 = .00, 3 = .000, 4 = .0000, etc.)</param></summary>
	public static float RoundF(this float num,int roundVal) => (float) System.Math.Round((decimal) num,roundVal);


	/// <summary>Round float value to nearest half fraction (i.e. 2 = half, 3 = third, 4 = quarter, etc.)</summary>
	public static float RoundHalf(this float num,int roundVal) => (float) System.Math.Round(num * roundVal,System.MidpointRounding.AwayFromZero) / roundVal;


	/// <summary>
	/// Converts minutes to seconds
	/// <param name="minutes"><br></br>Minutes</param>
	/// </summary>
	/// <returns>Seconds from input in minutes</returns>
	public static float ToSeconds(this float minutes) => minutes * 60f;


	/// <summary>
	/// Awards or penalizes percentage points based on input
	/// </summary>
	/// <param name="value"></param>
	/// <param name="percent"></param>
	/// <returns></returns>
	public static int AdjustByPercent(this int value, float percent) => Mathf.RoundToInt(((float)value * percent) + value);


	/// <summary>
	/// Returns the ratio of current number max
	/// </summary>
	/// <param name="value"></param>
	/// <param name="maxValue"></param>
	/// <returns></returns>
	public static float Ratio(this float value, float maxValue) => (int)value / Mathf.Clamp(maxValue, Mathf.Epsilon, int.MaxValue);
	
	/// <summary>
	/// Check if time elapsed is superior to cooldown. Best suited for non periodic cooldowns.
	/// </summary>
	/// <param name="timer">Timer reference</param>
	/// <param name="cooldown">Cooldown used for reference</param>
	/// <returns>Is the cooldown over ?</returns>
	public static bool PunctualCooldownCheck(this ref float timer, float cooldown)
	{
		if(Time.time < timer) return false;
	
		timer = Time.time + cooldown;
		return true;
	}

	/// <summary>
	/// Check if time elapsed is superior to cooldown. Best suited for periodic cooldowns.
	/// </summary>
	/// <param name="timer">Timer reference</param>
	/// <param name="cooldown">Cooldown used for reference</param>
	/// <returns>Is the cooldown over ?</returns>
	public static bool PeriodicCooldownCheck (this ref float timer, float cooldown)
	{
		if (Time.time < timer) return false;

		timer += cooldown;
		return true;
	}
}