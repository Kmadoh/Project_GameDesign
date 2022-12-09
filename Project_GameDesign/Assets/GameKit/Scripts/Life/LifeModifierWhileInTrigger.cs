using UnityEngine;

public class LifeModifierWhileInTrigger : MonoBehaviour
{
	[SerializeField] private int lifeAmountChanged = 1;
	[SerializeField] private float damageFrequency = 0.5f;
	[SerializeField] private bool resetCooldownOnLeave = true;

	public bool useTag = false;
	public string tagName = "Player";

	private float timer;

	private void Start ()
	{
		timer = damageFrequency;
	}

	bool CanDamage ()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			return false;
		}
		else
		{
			return true;
		}
	}

	private void OnTriggerStay (Collider other)
	{
		Life lifeComponent = other.GetComponent<Life>();
		if (lifeComponent == null)
		{
			lifeComponent = other.GetComponentInParent<Life>();
		}

		if (lifeComponent == null) return;
		if (useTag && !other.CompareTag(tagName)) return;
		if (!CanDamage()) return;
			
		lifeComponent.ModifyLife(lifeAmountChanged);
		timer = damageFrequency;
	}

	private void OnTriggerExit (Collider other)
	{
		if (!resetCooldownOnLeave) return;
		
		if (useTag && !other.CompareTag(tagName)) return;
		
		timer = damageFrequency;
	}
}
