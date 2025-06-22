using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using static UnityEngine.GraphicsBuffer;

public abstract class EnemyBehaviour : MonoBehaviour
{
	protected GameObject mainPlant;
	protected NavMeshAgent navMeshAgent;
	protected MainPlant mainPlantScript;
	protected VisualEffect deathSmoke;
	protected int damageMade;
	protected Rigidbody rb;
	protected Collider collider;
	protected float enemyHealth;
	[SerializeField]  protected bool attackActive;
	[SerializeField] protected float distanceToAttackGoal;
	[SerializeField] protected float attackRange;
	[SerializeField] protected Vector3 attackGoal;
	[SerializeField] protected float deathAnimationDuration;
	[SerializeField] protected float attackAnimationDuration;
	[SerializeField] protected float attackAnimationDurationAfterDamage;
	[SerializeField] protected float deathSmokeDuration;
	[SerializeField] protected Animator enemyAnimator;


	//protected Collider enemyCollider; //Man könnte einen größeren Collider um die Gegner herum ziehen, um Spielerannäherung zu erkennen und ihn statt der Main Plant anzugreifen

	
	protected virtual void OnEnable()
	{
		if (mainPlant == null)
		{
			mainPlant = GameObject.Find("Great Plant");
		}
		
		mainPlantScript = mainPlant.GetComponent<MainPlant>();
		deathSmoke = GetComponentInChildren<VisualEffect>();
		enemyAnimator = GetComponentInChildren<Animator>();
		collider = GetComponent<Collider>();
		navMeshAgent = GetComponent<NavMeshAgent>();

		collider.enabled = true;

		if (GameManager.Instance != null && !GameManager.Instance.gameOver && mainPlant.transform != null) //verhindert Missing Object-Reference Bug beim ersten OnEnable-Call durch Poolerstellung
		{
			navMeshAgent.SetDestination(mainPlant.transform.position);
			enemyAnimator.SetBool("isWalking", true);
			Vector3 direction = (mainPlant.transform.position - navMeshAgent.transform.position).normalized;

			// Rückwärtssuche vom Zielpunkt in Richtung Agent
			Vector3 sampleStart = mainPlant.transform.position - direction * attackRange;

			if (NavMesh.SamplePosition(sampleStart, out NavMeshHit hit, attackRange, NavMesh.AllAreas))
			{
				navMeshAgent.SetDestination(hit.position);
				attackGoal = hit.position;
			}
			else
			{
				attackGoal = mainPlant.transform.position;
			}

				
		}
		rb = GetComponent<Rigidbody>();
		rb.linearDamping = 4;

		GameManager.GameOverEvent += DeactivateAgent;
	}

	protected virtual void OnDisable()
	{
		GameManager.GameOverEvent -= DeactivateAgent;
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (!GameManager.Instance.gameOver)
		{
			if (collision.gameObject == mainPlant && !attackActive) //mainPlant wurde bereits im Eltern-Skript Enemy Behaviour befüllt
			{
				attackActive = true;
				navMeshAgent.isStopped = true;
				enemyAnimator.SetBool("isWalking", false);
				StartCoroutine(AttackPlantCoroutine());
					
			}
			else
			{
				distanceToAttackGoal = (attackGoal - transform.position).magnitude;
				if (distanceToAttackGoal > attackRange)
				{
					navMeshAgent.SetDestination(mainPlant.transform.position);
					enemyAnimator.SetBool("isWalking", true);
				}
			}
					
		}
	}
	protected virtual void OnTriggerEnter(Collider other)
	{
		if (!GameManager.Instance.gameOver)
		{
			if (other.gameObject == mainPlant && !attackActive) //mainPlant wurde bereits im Eltern-Skript Enemy Behaviour befüllt
			{
				attackActive = true;
				navMeshAgent.isStopped = true;
				enemyAnimator.SetBool("isWalking", false);
				StartCoroutine(AttackPlantCoroutine());	
			}
					
		}
	}


	protected virtual IEnumerator AttackPlantCoroutine()
	{
		while (attackActive && !GameManager.Instance.gameOver)
		{
			enemyAnimator.SetTrigger("isAttacking");
			yield return new WaitForSeconds(attackAnimationDuration);
			DoDamage(); //DoDamage-Funktion des Kindes mit persönlichem Damage-Wert des Kindes ausführen
			yield return new WaitForSeconds(attackAnimationDurationAfterDamage);

			distanceToAttackGoal = (attackGoal - transform.position).magnitude;
			if (distanceToAttackGoal > attackRange)
			{
				attackActive = false;
				navMeshAgent.isStopped = false;
				navMeshAgent.SetDestination(mainPlant.transform.position);
				enemyAnimator.SetBool("isWalking", true);
			}
		}
	}

	public void DeactivateAgent()
	{
		navMeshAgent.isStopped = true;
	}

	protected virtual void DoDamage() //Höhe des Damages wird aber in den Kinder-Skripten festgelegt 
	{
		mainPlantScript.GetActiveDamage(damageMade); //Leitet den Damage an das MainPlant-Skript weiter,
	}
	protected virtual void PoisonPlant() 
	{
		//ist hier erstmal leer gelassen, kann aber in den erbenden Kinder-Skripten überschrieben werden (siehe BigEnemy-Skript)
	}
	
	public virtual void GetDamage(float damageOfAmmo)
	{
		enemyHealth -= damageOfAmmo;
		if (enemyHealth < 0)
		{
			Death();
		}
	}

	public virtual void Death()
	{
		StartCoroutine(DeathCoroutine());
	}

	protected virtual IEnumerator DeathCoroutine()
	{
		DeactivateAgent();
		enemyAnimator.SetBool("isDead", true);
		collider.enabled = false;
		yield return new WaitForSeconds(deathAnimationDuration);
		deathSmoke.Play();
		yield return new WaitForSeconds(deathSmokeDuration);
		GameManager.Instance.killedEnemies += 1;
		DropResources();
		gameObject.SetActive(false);
	}

	protected virtual void DropResources()
	{
		int randomNumber = Random.Range(1, 101);
		int dropChance = GetDropChance();
		if (randomNumber >= 1 && randomNumber <= dropChance)
		{
			GameObject resource1 = Resource1Pool.Instance.GetPooledObject();

			if (resource1 != null)
			{
				resource1.transform.position = transform.position;
				resource1.transform.rotation = transform.rotation;
				resource1.SetActive(true);
			}
		}
	}

	protected virtual int GetDropChance()
	{
		int dropChance = 0;
		return dropChance;
	}
}
