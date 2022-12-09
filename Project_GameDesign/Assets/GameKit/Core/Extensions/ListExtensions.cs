using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
	/// <summary>
	/// Cast Transform with child nodes to List()
	/// </summary>
	public static List<Transform> CastToList(this Transform trans) => trans.Cast<Transform>().ToList();


	/// <summary>
	/// Shuffle any (I)List with an extension method based on the Fisher-Yates shuffle:<br>
	/// The algorithm effectively puts all the elements into a hat; it continually determines the next element by
	/// randomly drawing an element from the hat until no elements remain. The algorithm produces an unbiased permutation:
	///  every permutation is equally likely. The modern version of the algorithm is efficient: it takes time proportional
	///  to the number of items being shuffled and shuffles them in place.</br>
	/// </summary>
	public static List<T> Shuffle<T>(this IList<T> list) 
	{
		System.Random rng = new System.Random();
		int n = list.Count;
		while (n > 1) 
		{
			n--;
			int k = rng.Next(n + 1);
			(list[k], list[n]) = (list[n], list[k]);
		}
		return list.ToList();
	}

	public static T GetRandomItem<T>(this IList<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static T RemoveRandom<T>(this IList<T> list)
	{
		if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
		int index = UnityEngine.Random.Range(0, list.Count);
		T item = list[index];
		list.RemoveAt(index);
		return item;
	}
}