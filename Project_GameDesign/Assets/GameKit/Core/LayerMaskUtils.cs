using System;
using UnityEngine;

public static class LayerMaskUtils
{
	public const int Empty = 0;
	public const int TotalLayerCount = 32;

	#region Methods

	/// <summary>
	/// 	Determines if a is layer defined (i.e. has a name).
	/// </summary>
	/// <returns><c>true</c> if layer is defined; otherwise, <c>false</c>.</returns>
	/// <param name="index">Index.</param>
	public static bool IsLayerDefined(int index)
	{
		string name = LayerMask.LayerToName(index);
		return (!string.IsNullOrEmpty(name));
	}

	/// <summary>
	/// 	Gets the layer names.
	/// </summary>
	/// <param name="output">Output.</param>
	public static void GetLayerNames(ref string[] output)
	{
		Array.Resize(ref output, TotalLayerCount);

		for (int index = 0; index < TotalLayerCount; index++)
			output[index] = LayerMask.LayerToName(index);
	}

	/// <summary>
	/// 	Gets the layer count.
	/// </summary>
	/// <returns>The layer count.</returns>
	public static int GetLayerCount()
	{
		int output = 0;

		for (int index = 0; index < TotalLayerCount; index++)
		{
			if (IsLayerDefined(index))
				output++;
		}

		return output;
	}

	/// <summary>
	/// 	Gets the non empty layer names.
	/// </summary>
	/// <param name="output">Output.</param>
	public static void GetMaskFieldNames(ref string[] output)
	{
		Array.Resize(ref output, GetLayerCount());

		int index = 0;

		for (int layerIndex = 0; layerIndex < TotalLayerCount; layerIndex++)
		{
			if (!IsLayerDefined(layerIndex))
				continue;

			string name = LayerMask.LayerToName(layerIndex);
			output[index] = name;
			index++;
		}
	}

	/// <summary>
	/// 	Maps the 32 bit layer mask to match the number of non-empty layers
	/// </summary>
	/// <returns>The mapped mask.</returns>
	/// <param name="mask">Mask.</param>
	public static int MapToMaskField(LayerMask mask)
	{
		if (mask <= 0)
			return mask;

		int output = 0;
		int index = 0;

		for (int layerIndex = 0; layerIndex < TotalLayerCount; layerIndex++)
		{
			if (!IsLayerDefined(layerIndex))
				continue;

			bool bit = (mask & (1 << layerIndex)) != 0;
			if (bit)
				output += 1 << index;

			index++;
		}

		return output;
	}

	/// <summary>
	/// 	Maps the non-empty layer mask to a 32 bit layer mask.
	/// </summary>
	/// <returns>The unmapped mask.</returns>
	/// <param name="mask">Mask.</param>
	public static LayerMask MapFromMaskField(int mask)
	{
		if (mask <= 0)
			return mask;

		int output = 0;
		int index = 0;

		for (int layerIndex = 0; layerIndex < TotalLayerCount; layerIndex++)
		{
			if (!IsLayerDefined(layerIndex))
				continue;

			bool bit = (mask & (1 << index)) != 0;
			if (bit)
				output += 1 << layerIndex;

			index++;
		}

		return output;
	}

	#endregion
}