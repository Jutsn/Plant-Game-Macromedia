using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
	[Header("AR")]
	[SerializeField] private float damagePerParticleAR = 0.8f;  // Schaden pro Partikel
	[SerializeField] private float impactSplashPlayRateAR = 0.3f;
	[SerializeField] private float plantWaterPerParticleAR = 1f;  // Wasserregeneration pro Partikel

	[Header("Shotgun")]
	[SerializeField] private float damagePerParticleShotgun = 0.8f;  // Schaden pro Partikel
	[SerializeField] private float impactSplashPlayRateShotgun = 0.1f;
	[SerializeField] private float plantWaterPerParticleShotgun = 1f;  // Wasserregeneration pro Partikel

	[Header("Beam")]
	[SerializeField] private float damagePerParticleWaterBeam = 0.1f;  // Schaden pro Partikel
	[SerializeField] private float impactSplashPlayRateBeam = 0.3f;
	[SerializeField] private float plantWaterPerParticleWaterBeam = 0.1f;  // Wasserregeneration pro Partikel



	private ParticleSystem beamParticles;
	private ParticleSystem shotgunParticles;
	private ParticleSystem automaticRifleParticles;

	private List<ParticleCollisionEvent> collisionEventsBeam;
	private List<ParticleCollisionEvent> collisionEventsShotgun;
	private List<ParticleCollisionEvent> collisionEventsAR;

	
	
	
	//[SerializeField] private float impactSplashHearingDistance = 15f;

	//private GameObject playerCam;

	private void Awake()
	{
		if (gameObject.CompareTag("Water Bullet Shotgun"))
		{
			Debug.Log("Shotgun Bullet gefunden");
			shotgunParticles = GetComponent<ParticleSystem>();
		}
		else if (gameObject.CompareTag("Water Bullet AR"))
		{
			Debug.Log("AR Bullet gefunden");
			automaticRifleParticles = GetComponent<ParticleSystem>();
		}
		else if (gameObject.name == "Water Beam")
		{
			beamParticles = GetComponent<ParticleSystem>();
			//beamParticles.Stop();
		}
		
			
		//playerCam = GameObject.Find("Player Camera");
	}

	private void Start()
	{
		collisionEventsBeam = new List<ParticleCollisionEvent>();
		collisionEventsShotgun = new List<ParticleCollisionEvent>();
		collisionEventsAR = new List<ParticleCollisionEvent>();
	}

	void OnParticleCollision(GameObject other)
	{
		if (other.gameObject.name == "Great Plant" && !GameManager.Instance.gameOver)
		{
			WaterPlant(other);
		}
		else if (other.gameObject.CompareTag("Enemy"))
		{
			MakeEnemyDamageAndShowHitmarker(other);
		}

		if (other != null) 
		{
			PlaySplashSound(other);
		}
	}

	void WaterPlant(GameObject other)
	{
		MainPlant mainPlantSkript = other.GetComponent<MainPlant>();
		if (mainPlantSkript != null && beamParticles != null)
		{
			mainPlantSkript.GetWater(plantWaterPerParticleWaterBeam);
		}
		if (mainPlantSkript != null && shotgunParticles != null)
		{
			mainPlantSkript.GetWater(plantWaterPerParticleShotgun);
		}
		if (mainPlantSkript != null && automaticRifleParticles != null)
		{
			mainPlantSkript.GetWater(plantWaterPerParticleAR);
		}
	}

	void MakeEnemyDamageAndShowHitmarker(GameObject other)
	{
		EnemyBehaviour enemyBehaviourSkript = other.GetComponent<EnemyBehaviour>();
		if (enemyBehaviourSkript != null && beamParticles != null)
		{
			float rate = 0.3f;
			enemyBehaviourSkript.GetDamage(damagePerParticleWaterBeam);
			UIManager.Instance.ShowHitmarker(rate);
			//Play Splash Sound
		}
		if (enemyBehaviourSkript != null && shotgunParticles != null)
		{
			float rate = 0.1f;
			enemyBehaviourSkript.GetDamage(damagePerParticleShotgun);
			UIManager.Instance.ShowHitmarker(rate);
			//Play Splash Sound
		}
		if (enemyBehaviourSkript != null && automaticRifleParticles != null)
		{
			float rate = 0.1f;
			enemyBehaviourSkript.GetDamage(damagePerParticleAR);
			UIManager.Instance.ShowHitmarker(rate);
			//Play Splash Sound
		}
	}

	void PlaySplashSound(GameObject other)
	{
		if (beamParticles != null)
		{
			int numCollisionEventsBeam = beamParticles.GetCollisionEvents(other, collisionEventsBeam);

			for (int i = 0; i < numCollisionEventsBeam; i++)
			{
				Vector3 collisionPoint = collisionEventsBeam[i].intersection;
				//float distanceToBeamCollision = (collisionPoint - playerCam.transform.position).magnitude;

				//if (distanceToBeamCollision < impactSplashHearingDistance) //Wenn näher dran als maximale Hördistanz
				
				float impactSoundPlayRate = impactSplashPlayRateBeam;
				SoundManager.Instance.PlaySplashSound(impactSoundPlayRate, collisionPoint); //Play Splash Sound in einer bestimmten Rate
				
			}
		}

		if (shotgunParticles != null)
		{
			int numCollisionEventsShotgun = shotgunParticles.GetCollisionEvents(other, collisionEventsShotgun);

			for (int i = 0; i < numCollisionEventsShotgun; i++)
			{
				Vector3 collisionPoint = collisionEventsShotgun[i].intersection;
				//float distanceToShotgunCollision = (collisionPoint - playerCam.transform.position).magnitude;

				//if (distanceToShotgunCollision < impactSplashHearingDistance) //Wenn näher dran als maximale Hördistanz
				
				float impactSoundPlayRate = impactSplashPlayRateShotgun;
				SoundManager.Instance.PlaySplashSound(impactSoundPlayRate, collisionPoint); //Play Splash Sound in einer bestimmten Rate
				
			}
		}

		if (automaticRifleParticles != null)
		{
			int numCollisionEventsAR = automaticRifleParticles.GetCollisionEvents(other, collisionEventsAR);

			for (int i = 0; i < numCollisionEventsAR; i++)
			{
				Vector3 collisionPoint = collisionEventsAR[i].intersection;
				//float distanceToShotgunCollision = (collisionPoint - playerCam.transform.position).magnitude;

				//if (distanceToShotgunCollision < impactSplashHearingDistance) //Wenn näher dran als maximale Hördistanz
				
				float impactSoundPlayRate = impactSplashPlayRateAR;
				SoundManager.Instance.PlaySplashSound(impactSoundPlayRate, collisionPoint); //Play Splash Sound in einer bestimmten Rate
				
			}
		}
	}
}
