using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterBulletParticlePool : MonoBehaviour
{
	public static WaterBulletParticlePool Instance {  get; private set; }

	public ParticleSystem objectToPool;
	[SerializeField] int amountToPool;

	public List<ParticleSystem> pooledObjects;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		////////////////////////////////////////////
		

		for (int i = 0; i < amountToPool; i++)
		{
			ParticleSystem obj = Instantiate(objectToPool);
			obj.gameObject.SetActive(false);
			pooledObjects.Add(obj);
		}
	}

	public ParticleSystem GetPooledParticleSystem()
	{
		foreach (ParticleSystem obj in pooledObjects)
		{
			if (!obj.gameObject.activeInHierarchy)
			{
				obj.gameObject.SetActive(true);
				return obj;
				
			}
		}
		return null;
	}
}
