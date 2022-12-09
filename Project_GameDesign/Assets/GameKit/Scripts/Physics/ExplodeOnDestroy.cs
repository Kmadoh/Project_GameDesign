using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDestroy : MonoBehaviour
{
	[Tooltip("Damage dealt to GameObjects in range")]
	public int damage;
	[Tooltip("Range of the explosion")]
	public float range = 5f;
	[Tooltip("Directional force applied to GameObjects in range")]
	public float bumpForce = 10f;
	[Tooltip("Upwards modifier applied to the directional force")]
	public float upwardsModifier = 2f;
	[Tooltip("Layers affected by the explosion")]
	public LayerMask explosionLayerMask = 1;

	[Header("FX")]
	[SerializeField] private GameObject fxToSpawn = null;
	[SerializeField] private float fxLifeTime = 3f;

	private void OnDestroy ()
	{
		Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, range, explosionLayerMask);
		if(nearbyObjects.Length > 0)
		{
			if(damage > 0)
			{
				foreach (Collider c in nearbyObjects)
				{
					Life life = c.GetComponent<Life>();
					if(life != null)
					{
						life.ModifyLife(damage * -1);
					}
				}
			}
			
			foreach(Collider hit in nearbyObjects)
			{
				Rigidbody rigid = hit.GetComponent<Rigidbody>();
				if(rigid != null)
				{
					rigid.AddExplosionForce(bumpForce, transform.position, range, upwardsModifier, ForceMode.Impulse);
				}
			}
			
		}

		if (fxToSpawn == null) return;

		Transform t = transform;
		GameObject fx = Instantiate(fxToSpawn, t.position, t.rotation);
		
		if(fxLifeTime > 0 )
		{
			Destroy(fx, fxLifeTime);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}