
using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemySpawnPoints;
    public GameObject[] resource2SpawnPoints;

	[SerializeField] private int smallEnemySpawnAmount;
	[SerializeField] private int bigEnemySpawnAmount;
	[SerializeField] private float enemySpawnRate;
	[SerializeField] private float pauseBetweenSpawnPointSwitches;

	public bool newWave = true;
	public int currentSpawnPointIndex;

	void Start()
    {
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("Enemy Spawn Point");
        resource2SpawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point Resource 2");
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine() //Diesen Block hier kopieren und anpassen, falls Spawnpunkt C hinzukommt. Variablen erstellen und Array-Nummer anpassen (spawnPoints [2]) nicht vergessen. 
    {
		yield return new WaitForSeconds(enemySpawnRate); //Durch warten sicherstellen, dass die Pools vollständig befüllt sind. Verhindert index out of Range-Bug.

		while (!GameManager.Instance.gameOver)
		{
			while (GameManager.Instance.waveActive)
			{
				if (newWave)
				{
					newWave = false;
					int newSpawnPointIndex = Random.Range(0,enemySpawnPoints.Length);
					yield return new WaitForEndOfFrame();

					if (currentSpawnPointIndex == newSpawnPointIndex)
					{
						newWave = true; //Wähle erneut einen Punkt aus
					}
					else if (currentSpawnPointIndex != newSpawnPointIndex)
					{
						currentSpawnPointIndex = newSpawnPointIndex; //Speichere den neuen Punkt als aktuellen und starte das Spawnen
					}
					yield return new WaitForSeconds(1);
				}
				else if (!newWave)
				{
					for (int i = 0; i < smallEnemySpawnAmount; i++)
					{
						GameObject smallEnemy = SmallEnemyPool.Instance.GetPooledObject();

						if (smallEnemy != null)
						{
							smallEnemy.transform.position = enemySpawnPoints[currentSpawnPointIndex].transform.position;
							smallEnemy.transform.rotation = enemySpawnPoints[currentSpawnPointIndex].transform.rotation;
							smallEnemy.SetActive(true);
						}

						if (GameManager.Instance.gameOver)
						{
							yield break; //für sofortigen SpawnStopp bei GameOver 
						}
						else if (!GameManager.Instance.waveActive)
						{
							break;
						}
						yield return new WaitForSeconds(enemySpawnRate);
					}

					for (int i = 0; i < bigEnemySpawnAmount; i++)
					{
						GameObject bigEnemy = BigEnemyPool.Instance.GetPooledObject();

						if (bigEnemy != null)
						{
							bigEnemy.transform.position = enemySpawnPoints[currentSpawnPointIndex].transform.position;
							bigEnemy.transform.rotation = enemySpawnPoints[currentSpawnPointIndex].transform.rotation;
							bigEnemy.SetActive(true);
						}

						if (GameManager.Instance.gameOver)
						{
							yield break; //für sofortigen SpawnStopp bei GameOver
						}
						else if (!GameManager.Instance.waveActive)
						{
							break;
						}
						yield return new WaitForSeconds(enemySpawnRate);
					}

					newWave = true;
				}
				yield return new WaitForSeconds(pauseBetweenSpawnPointSwitches);
			}

			yield return new WaitForSeconds(2);
		}
	}

	public void SpawnResource2()
	{
		GameObject resource2 = Resource2Pool.Instance.GetPooledObject();

		if (resource2 != null)
		{
			resource2.transform.position = GetResource2SpawnPoint();
			resource2.transform.rotation = resource2.transform.rotation;
			resource2.SetActive(true);
		}
	}

	private Vector3 GetResource2SpawnPoint()
	{
		int i = Random.Range(0, resource2SpawnPoints.Length);
		Vector3 spawnPos = resource2SpawnPoints[i].transform.position;

		return spawnPos;
	}

	public void SpawnEliteEnemy() //Funktion zum Spawnen des Elitengegners
	{
		GameObject bigEnemyElite = BigEnemyElitePool.Instance.GetPooledObject();

		if (bigEnemyElite != null)
		{
			//int spawnPointIndex = Random.Range(0, enemySpawnPoints.Length);
			bigEnemyElite.transform.position = enemySpawnPoints[currentSpawnPointIndex].transform.position;
			bigEnemyElite.transform.rotation = enemySpawnPoints[currentSpawnPointIndex].transform.rotation;
			bigEnemyElite.SetActive(true);
		}
	}
}
