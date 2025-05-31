using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterBulletParticlePool : MonoBehaviour
{
	public static WaterBulletParticlePool Instance {  get; private set; }

	public ParticleSystem shotgunBulletToPool;
	public ParticleSystem ARBulletToPool;
	[SerializeField] int shotgunBulletAmountToPool;
	[SerializeField] int ARBulletAmountToPool;

	public List<ParticleSystem> pooledShotgunBullets;
	public List<ParticleSystem> pooledARBullets;

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
		

		for (int i = 0; i < shotgunBulletAmountToPool; i++)
		{
			ParticleSystem obj = Instantiate(shotgunBulletToPool);
			obj.gameObject.SetActive(false);
			pooledShotgunBullets.Add(obj);
		}

		for (int i = 0; i < ARBulletAmountToPool; i++)
		{
			ParticleSystem obj = Instantiate(ARBulletToPool);
			obj.gameObject.SetActive(false);
			pooledARBullets.Add(obj);
		}
	}

	public ParticleSystem GetPooledShotgunBullet()
	{
		foreach (ParticleSystem obj in pooledShotgunBullets)
		{
			if (!obj.gameObject.activeInHierarchy)
			{
				obj.gameObject.SetActive(true);
				return obj;
				
			}
		}
		return null;
	}

	public ParticleSystem GetPooledARBullet()
	{
		foreach (ParticleSystem obj in pooledARBullets)
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
