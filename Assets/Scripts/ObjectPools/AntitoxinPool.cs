using System.Collections.Generic;
using UnityEngine;

public class AntitoxinPool : MonoBehaviour
{
	public static AntitoxinPool Instance { get; private set; }

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
