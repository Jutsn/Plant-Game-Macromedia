
using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemySpawnPoints;
    public GameObject[] resource2SpawnPoints;

	[SerializeField] private int smallEnemyAmountA;
	[SerializeField] private int bigEnemyAmountA;
	[SerializeField] private int smallEnemyAmountB;
	[SerializeField] private int bigEnemyAmountB;							//neuen Enemy-Pool erstellen, falls neuer Gegnertyp hinzukommt
	[SerializeField] private float spawnRatePointA;
	[SerializeField] private float spawnRatePointB;

	void Start()
    {
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("Enemy Spawn Point");
        resource2SpawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point Resource 2");
        StartCoroutine(SpawnCoroutineA());
		StartCoroutine(SpawnCoroutineB());
    }

    IEnumerator SpawnCoroutineA() //Diesen Block hier kopieren und anpassen, falls Spawnpunkt C hinzukommt. Variablen erstellen und Array-Nummer anpassen (spawnPoints [2]) nicht vergessen. 
    {
		yield return new WaitForSeconds(spawnRatePointA); //Durch warten sicherstellen, dass die Pools vollständig befüllt sind. Verhindert index out of Range-Bug.

		while (!GameManager.Instance.gameOver)
		{
			for (int i = 0; i < smallEnemyAmountA; i++)
			{
				GameObject smallEnemy = SmallEnemyPool.Instance.GetPooledObject();

				if (smallEnemy != null)
				{
					smallEnemy.transform.position = enemySpawnPoints[0].transform.position;
					smallEnemy.transform.rotation = enemySpawnPoints[0].transform.rotation;
					smallEnemy.SetActive(true);
				}

				if (GameManager.Instance.gameOver)
				{
					yield break; //für sofortigen SpawnStopp bei GameOver 
				}
				yield return new WaitForSeconds(spawnRatePointA);
			}

			for (int i = 0; i < bigEnemyAmountA; i++)
			{
				GameObject bigEnemy = BigEnemyPool.Instance.GetPooledObject();

				if (bigEnemy != null)
				{
					bigEnemy.transform.position = enemySpawnPoints[0].transform.position;
					bigEnemy.transform.rotation = enemySpawnPoints[0].transform.rotation;
					bigEnemy.SetActive(true);
				}

				if (GameManager.Instance.gameOver)
				{
					yield break; //für sofortigen SpawnStopp bei GameOver
				}
				yield return new WaitForSeconds(spawnRatePointA);
			}
		}
	}

	IEnumerator SpawnCoroutineB()
	{
		yield return new WaitForSeconds(spawnRatePointB); //Durch warten sicherstellen, dass die Pools vollständig befüllt sind. Verhindert index out of Range-Bug.

		while (!GameManager.Instance.gameOver)
		{
			for (int i = 0; i < smallEnemyAmountB; i++)
			{
				GameObject smallEnemy = SmallEnemyPool.Instance.GetPooledObject();

				if (smallEnemy != null)
				{
					smallEnemy.transform.position = enemySpawnPoints[1].transform.position;
					smallEnemy.transform.rotation = enemySpawnPoints[1].transform.rotation;
					smallEnemy.SetActive(true);
				}

				if (GameManager.Instance.gameOver)
				{
					yield break; //für sofortigen SpawnStopp bei GameOver
				}
				yield return new WaitForSeconds(spawnRatePointB);
			}

			for (int i = 0; i < bigEnemyAmountB; i++)
			{
				GameObject bigEnemy = BigEnemyPool.Instance.GetPooledObject();

				if (bigEnemy != null)
				{
					bigEnemy.transform.position = enemySpawnPoints[1].transform.position;
					bigEnemy.transform.rotation = enemySpawnPoints[1].transform.rotation;
					bigEnemy.SetActive(true);
				}

				if (GameManager.Instance.gameOver)
				{
					yield break; //für sofortigen SpawnStopp bei GameOver
				}
				yield return new WaitForSeconds(spawnRatePointB);
			}
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
}
