using Unity.Mathematics;
using UnityEngine;
using System;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnPoints;

	[SerializeField] private int smallEnemyAmountA;
	[SerializeField] private int bigEnemyAmountA;
	[SerializeField] private int smallEnemyAmountB;
	[SerializeField] private int bigEnemyAmountB;							//neuen Enemy-Pool erstellen, falls neuer Gegnertyp hinzukommt
	[SerializeField] private float spawnRatePointA;
	[SerializeField] private float spawnRatePointB;

	void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
        StartCoroutine(SpawnCoroutineA());
		StartCoroutine(SpawnCoroutineB());
    }

    IEnumerator SpawnCoroutineA() //Diesen Block hier kopieren und anpassen, falls Spawnpunkt C hinzukommt. Variablen erstellen und Array-Nummer anpassen (spawnPoints [2]) nicht vergessen. 
    {
		while (!GameManager.Instance.gameOver)
		{
			for (int i = 0; i < smallEnemyAmountA; i++)
			{
				GameObject smallEnemy = SmallEnemyPool.Instance.GetPooledObject();

				if (smallEnemy != null)
				{
					smallEnemy.transform.position = spawnPoints[0].transform.position;
					smallEnemy.transform.rotation = spawnPoints[0].transform.rotation;
					smallEnemy.SetActive(true);
				}
				yield return new WaitForSeconds(spawnRatePointA);
			}

			for (int i = 0; i < bigEnemyAmountA; i++)
			{
				GameObject bigEnemy = BigEnemyPool.Instance.GetPooledObject();

				if (bigEnemy != null)
				{
					bigEnemy.transform.position = spawnPoints[0].transform.position;
					bigEnemy.transform.rotation = spawnPoints[0].transform.rotation;
					bigEnemy.SetActive(true);
				}
				yield return new WaitForSeconds(spawnRatePointA);
			}
		}
	}

	IEnumerator SpawnCoroutineB()
	{
		while (!GameManager.Instance.gameOver)
		{
			for (int i = 0; i < smallEnemyAmountB; i++)
			{
				GameObject smallEnemy = SmallEnemyPool.Instance.GetPooledObject();

				if (smallEnemy != null)
				{
					smallEnemy.transform.position = spawnPoints[1].transform.position;
					smallEnemy.transform.rotation = spawnPoints[1].transform.rotation;
					smallEnemy.SetActive(true);
				}
				yield return new WaitForSeconds(spawnRatePointB);
			}

			for (int i = 0; i < bigEnemyAmountB; i++)
			{
				GameObject bigEnemy = BigEnemyPool.Instance.GetPooledObject();

				if (bigEnemy != null)
				{
					bigEnemy.transform.position = spawnPoints[1].transform.position;
					bigEnemy.transform.rotation = spawnPoints[1].transform.rotation;
					bigEnemy.SetActive(true);
				}
				yield return new WaitForSeconds(spawnRatePointB);
			}
		}
	}
}
