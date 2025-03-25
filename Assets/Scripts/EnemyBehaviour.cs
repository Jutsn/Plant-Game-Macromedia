using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
	protected GameObject mainPlant;
	protected NavMeshAgent navMeshAgent;
	protected MainPlant mainPlantScript;
	//protected Collider enemyCollider; //Man könnte einen größeren Collider um die Gegner herum ziehen, um Spielerannäherung zu erkennen und hn statt der Main Plant anzugreifen

	
	protected void OnEnable()
	{
		mainPlant = GameObject.Find("Great Plant");
		mainPlantScript = mainPlant.GetComponent<MainPlant>();

		navMeshAgent = GetComponent<NavMeshAgent>();
		if (GameManager.Instance != null) //verhindert Missing Object-Reference Bug beim ersten OnEnable-Call durch Poolerstellung
		{
			StartCoroutine(SetDestinationCoroutine());
		}
		
	}

	protected void OnDisable()
	{
		StopCoroutine(SetDestinationCoroutine());
	}

	protected IEnumerator SetDestinationCoroutine() //Jede Sekunde Ziel neu ermitteln und hinlaufen
	{
		while (!GameManager.Instance.gameOver)
		{
			if (mainPlant.transform != null)
			{
				navMeshAgent.SetDestination(mainPlant.transform.position);
			}
			yield return new WaitForSeconds(1);
		}

	}

	protected virtual void DoDamage(int damage) //Höhe des Damages wird aber in den Kinder-Skripten festgelegt 
	{
		mainPlantScript.GetActiveDamage(damage); //Leitet den Damage an das MainPlant-Skript weiter,
	}
	protected virtual void PoisonPlant() 
	{
		//ist hier erstmal leer gelassen, kann aber in den erbenden Kinder-Skripten überschrieben werden (siehe BigEnemy-Skript)
	}
}
