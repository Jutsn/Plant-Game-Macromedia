using System.Collections.Generic;
using UnityEngine;

public class BigEnemyPool : MonoBehaviour
{
	public static BigEnemyPool Instance;

	[SerializeField] private List<GameObject> pooledObjects;
	[SerializeField] public GameObject objectToPool;
	[SerializeField] private int amountToPool;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		pooledObjects = new List<GameObject>();
		GameObject bigEnemy;
		for (int i = 0; i < amountToPool; i++)
		{
			bigEnemy = Instantiate(objectToPool);
			bigEnemy.SetActive(false);
			pooledObjects.Add(bigEnemy);
		}
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0; i < amountToPool; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}
		return null;
	}
}
