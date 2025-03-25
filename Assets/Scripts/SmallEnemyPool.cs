using UnityEngine;
using System.Collections.Generic;

public class SmallEnemyPool : MonoBehaviour
{
	public static SmallEnemyPool Instance;
	public List<GameObject> pooledObjects;
	public GameObject objectToPool;
	public int amountToPool;

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
		GameObject smallEnemy;
		for (int i = 0; i < amountToPool; i++)
		{
			smallEnemy = Instantiate(objectToPool);
			smallEnemy.SetActive(false);
			pooledObjects.Add(smallEnemy);
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
