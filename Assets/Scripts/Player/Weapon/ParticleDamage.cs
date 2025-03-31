using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
	[SerializeField] private float waterPerParticleWaterBeam = 0.1f;  // Wasserregeneration pro Partikel
	[SerializeField] private float damagePerParticleWaterBeam = 0.1f;  // Schaden pro Partikel
	[SerializeField] private float waterPerParticleShotgun = 1f;  // Wasserregeneration pro Partikel
	[SerializeField] private float damagePerParticleShotgun = 0.8f;  // Schaden pro Partikel


	private ParticleSystem beamParticles;
	private ParticleSystem shotgunParticles;
	private void Awake()
	{
		if (gameObject.name == "Water Beam")
		{
			beamParticles = GetComponent<ParticleSystem>();
			//beamParticles.Stop();
		}	
		else
			shotgunParticles = GetComponent<ParticleSystem>();
	}
	private void Start()
	{
		
	}
	void OnParticleCollision(GameObject other)
	{
		if (other.gameObject.name == "Great Plant" && !GameManager.Instance.gameOver)
		{
			MainPlant mainPlantSkript = other.GetComponent<MainPlant>();
			if (mainPlantSkript != null && beamParticles != null)
			{
				mainPlantSkript.GetWater(waterPerParticleWaterBeam);
			}
			if (mainPlantSkript != null && shotgunParticles != null)
			{
				mainPlantSkript.GetWater(waterPerParticleShotgun);
			}
		}

		if (other.gameObject.CompareTag("Enemy"))
		{
			EnemyBehaviour enemyBehaviourSkript = other.GetComponent<EnemyBehaviour>();
			if (enemyBehaviourSkript != null && beamParticles != null)
			{
				enemyBehaviourSkript.GetDamage(damagePerParticleWaterBeam);
			}
			if (enemyBehaviourSkript != null && shotgunParticles != null)
			{
				enemyBehaviourSkript.GetDamage(damagePerParticleShotgun);
			}
		}
	}
}
