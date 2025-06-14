using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public abstract class EnemyBehaviour : MonoBehaviour
{
	protected GameObject mainPlant;
	protected NavMeshAgent navMeshAgent;
	protected MainPlant mainPlantScript;
	protected int damageMade;
	protected Rigidbody rb;
	protected float enemyHealth;
	[SerializeField]  protected bool attackActive;
	[SerializeField] protected float distanceToAttackGoal;
	[SerializeField] protected float attackRange;
	[SerializeField] protected Vector3 attackGoal;
	//protected Collider enemyCollider; //Man könnte einen größeren Collider um die Gegner herum ziehen, um Spielerannäherung zu erkennen und ihn statt der Main Plant anzugreifen

	
	protected virtual void OnEnable()
	{
		mainPlant = GameObject.Find("Great Plant");
		mainPlantScript = mainPlant.GetComponent<MainPlant>();

		navMeshAgent = GetComponent<NavMeshAgent>();
		if (GameManager.Instance != null && !GameManager.Instance.gameOver && mainPlant.transform != null) //verhindert Missing Object-Reference Bug beim ersten OnEnable-Call durch Poolerstellung
		{
			navMeshAgent.SetDestination(mainPlant.transform.position);
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

		GameManager.GameOverEvent += DeactivateEnemy;
	}

	protected virtual void OnDisable()
	{
		GameManager.GameOverEvent -= DeactivateEnemy;
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (!GameManager.Instance.gameOver)
		{
			if (collision.gameObject == mainPlant && !attackActive) //mainPlant wurde bereits im Eltern-Skript Enemy Behaviour befüllt
			{
				attackActive = true;
				navMeshAgent.isStopped = true;
				StartCoroutine(AttackPlantCoroutine());
					
			}
			else
			{
				distanceToAttackGoal = (attackGoal - transform.position).magnitude;
				if (distanceToAttackGoal > attackRange)
				{
					navMeshAgent.SetDestination(mainPlant.transform.position);
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
				StartCoroutine(AttackPlantCoroutine());	
			}
					
		}
	}


	protected virtual IEnumerator AttackPlantCoroutine()
	{
		while (attackActive && !GameManager.Instance.gameOver)
		{
			DoDamage(); //DoDamage-Funktion des Kindes mit persönlichem Damage-Wert des Kindes ausführen
			yield return new WaitForSeconds(2);
			distanceToAttackGoal = (attackGoal - transform.position).magnitude;
			if (distanceToAttackGoal > attackRange)
			{
				attackActive = false;
				navMeshAgent.isStopped = false;
				navMeshAgent.SetDestination(mainPlant.transform.position);
			}
		}
	}

	public void DeactivateEnemy()
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
