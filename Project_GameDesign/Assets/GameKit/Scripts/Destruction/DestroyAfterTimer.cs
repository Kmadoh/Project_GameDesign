using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour
{
	[Tooltip("Time before destroying the object")]
	public float lifeTime = 1f;

	void Awake ()
	{
		Destroy(gameObject, lifeTime);
	}
}