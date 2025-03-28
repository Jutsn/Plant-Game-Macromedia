using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
	[SerializeField] private float waterPerParticle = 0.1f;  // Schaden pro Partikel

	private ParticleSystem beamParticles;
	private void Awake()
	{
		beamParticles = GetComponent<ParticleSystem>();
		beamParticles.Stop();
	}
	void OnParticleCollision(GameObject other)
	{
		if (other.gameObject.name == "Great Plant" && !GameManager.Instance.gameOver)
		{
			MainPlant mainPlantSkript = other.GetComponent<MainPlant>();
			if (mainPlantSkript != null)
			{
				mainPlantSkript.GetWater(waterPerParticle);
			}
		}
	}
}
