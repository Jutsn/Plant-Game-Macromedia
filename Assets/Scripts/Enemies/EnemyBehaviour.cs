using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
	protected GameObject mainPlant;
	protected NavMeshAgent navMeshAgent;
	protected MainPlant mainPlantScript;
	protected int damageMade;
	protected Rigidbody rb;
	//protected Collider enemyCollider; //Man könnte einen größeren Collider um die Gegner herum ziehen, um Spielerannäherung zu erkennen und hn statt der Main Plant anzugreifen

	
	protected void OnEnable()
	{
		mainPlant = GameObject.Find("Great Plant");
		mainPlantScript = mainPlant.GetComponent<MainPlant>();

		navMeshAgent = GetComponent<NavMeshAgent>();
		if (GameManager.Instance != null && !GameManager.Instance.gameOver && mainPlant.transform != null) //verhindert Missing Object-Reference Bug beim ersten OnEnable-Call durch Poolerstellung
		{
			navMeshAgent.SetDestination(mainPlant.transform.position);
		}
		rb = GetComponent<Rigidbody>();
		rb.linearDamping = 4;
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (!GameManager.Instance.gameOver)
		{
			if (collision.gameObject == mainPlant) //mainPlant wurde bereits im Eltern-Skript Enemy Behaviour befüllt
			{
				DoDamage(); //DoDamage-Funktion des Kindes mit persönlichem Damage-Wert des Kindes ausführen
			}
			else
			{
				navMeshAgent.SetDestination(mainPlant.transform.position);
			}
		}
		else
		{
			navMeshAgent.isStopped = true;
		}
	}

	protected virtual void DoDamage() //Höhe des Damages wird aber in den Kinder-Skripten festgelegt 
	{
		mainPlantScript.GetActiveDamage(damageMade); //Leitet den Damage an das MainPlant-Skript weiter,
	}
	protected virtual void PoisonPlant() 
	{
		//ist hier erstmal leer gelassen, kann aber in den erbenden Kinder-Skripten überschrieben werden (siehe BigEnemy-Skript)
	}
	
}
