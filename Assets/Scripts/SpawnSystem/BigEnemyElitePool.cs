using System.Collections.Generic;
using UnityEngine;

public class BigEnemyElitePool : MonoBehaviour
{
	public static BigEnemyElitePool Instance { get; private set; }

	[SerializeField] private List<GameObject> pooledObjects;
	[SerializeField] public GameObject objectToPool;
	[SerializeField] private int amountToPool;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		pooledObjects = new List<GameObject>();
		GameObject bigEnemyElite;
		for (int i = 0; i < amountToPool; i++)
		{
			bigEnemyElite = Instantiate(objectToPool);
			bigEnemyElite.SetActive(false);
			pooledObjects.Add(bigEnemyElite);
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
