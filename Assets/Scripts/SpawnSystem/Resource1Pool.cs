using System.Collections.Generic;
using UnityEngine;

public class Resource1Pool : MonoBehaviour
{
	public static Resource1Pool Instance { get; private set; }

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
		GameObject resource;
		for (int i = 0; i < amountToPool; i++)
		{
			resource = Instantiate(objectToPool);
			resource.SetActive(false);
			pooledObjects.Add(resource);
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
