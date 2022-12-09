using System.Collections;
using UnityEngine;

public class AttackOnInput : MonoBehaviour
{
	[Tooltip("Input name used for attacking (InputManager)")]
	public string inputName = "Fire1";

	[Tooltip("Layers affected by the attack")]
	public LayerMask attackLayerMask = 1;

	[Range(0f,360f)]
	[Tooltip("Attack effective angle")]
	public float attackAngle = 90f;

	[Tooltip("Range of the attack")]
	public float attackRange = 3f;

	[Tooltip("Delay before dealing damage. Useful for syncing with animation")]
	[SerializeField] private float damageDelay = 0.5f;

	[Tooltip("Cooldown between each attack")]
	public float attackCooldown = 1f;

	[Tooltip("Damage applied to hit GameObjects with a Life Component")]
	[SerializeField] private int attackDamage = 5;

	[Tooltip("Force applied to hit Gameobjects")]
	[SerializeField] private float attackKnockback = 10f;

	[Tooltip("Vertical force applied to hit Gameobjects")]
	[SerializeField] private float attackUpwardsKnockback = 5f;

	[Tooltip("FX Instantiated on hit GameObjects")]
	[SerializeField] private GameObject hitFX = null;


	[Tooltip("Reference to the Animator Component")]
	public Animator animator = null;

	[Tooltip("Name of the Trigger parameter called during the attack")]
	[SerializeField] private string attackTriggerParameterName = "Attack";

	private float effectiveRange;
	[HideInInspector] public float timer = 0f;
	public bool displayDebugInfo = false;
	public int inputChoiceIndex;


	private void Start()
    {
		effectiveRange = attackAngle.Remap( 0, 360f, 1, -1f);

		if (animator == null && !TryGetComponent(out animator))
		{
			Debug.LogWarning("No Animator found on this GameObject ! Please add one", gameObject);
		}
    }

	private void Attack()
	{
		timer = attackCooldown;

		if(animator != null)
		{
			animator.SetTrigger(attackTriggerParameterName);
		}

		StartCoroutine(DamageDeal());
	}

	private  IEnumerator DamageDeal()
	{
		yield return new WaitForSeconds(damageDelay);

		Collider[] angleEntities = Physics.OverlapSphere(transform.position, attackRange, attackLayerMask);
		foreach (Collider entity in angleEntities)
		{
			Vector3 toTarget = entity.transform.position - transform.position;
			Vector3 knockbackDir = toTarget.normalized * attackKnockback;
			knockbackDir.y = attackUpwardsKnockback;

			float dot = Vector3.Dot(transform.forward, toTarget.normalized);

			// If entity is within range and in the right angle
			if (!(dot >= effectiveRange)) continue;
			
			Life entityLife = entity.GetComponent<Life>();
			Rigidbody entityRigid = entity.gameObject.GetComponent<Rigidbody>();

			if (entityLife)
			{
				entityLife.ModifyLife(attackDamage * -1);
			}

			if (entityRigid != null)
			{
				entityRigid.AddForce(knockbackDir, ForceMode.Impulse);
			}

			if (hitFX == null || (entityLife == null && entityRigid == null)) continue;

			Transform entityTransform = entity.transform;
			GameObject fx = Instantiate(hitFX, entityTransform.position, entityTransform.rotation);
			Destroy(fx, 3f);
		}
	}

    private void Update()
    {
        if(timer > 0f)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			if(Input.GetButtonDown(inputName))
			{
				Attack();
			}
		}
    }
}
