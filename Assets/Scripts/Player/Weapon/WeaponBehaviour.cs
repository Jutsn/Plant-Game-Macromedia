using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public StandardWeaponMode standardWeaponMode;

	public Transform firePoint; // Punkt, von dem der Strahl ausgeht
	public ParticleSystem beamParticles; // Das Partikel-System
	[SerializeField] private int waterConsumptionBeam;
	[SerializeField] private float waterConsumptionRateBeam = 1;
	[SerializeField] private int waterConsumptionShotgun = 3;
	[SerializeField] private float cooldownTimeShotgun = 0.5f;
	private bool isFiringBeam;
	private bool isFiringShotgun;

	
	public GameObject muzzle;

	private void Start()
	{
		standardWeaponMode = StandardWeaponMode.beam;
	}
	void Update()
	{
		if (Input.GetButtonDown("Fire1") && standardWeaponMode == StandardWeaponMode.beam && WaterTank.Instance.waterLevel > 0 && !GameManager.Instance.gameOver) // Schie�en solange Maustaste gedr�ckt
		{
			beamParticles.Play();
			isFiringBeam = true;
			StartCoroutine(WaterConsumptionCoroutine(waterConsumptionBeam));
		}
		else if(Input.GetButtonUp("Fire1"))
		{
			isFiringBeam = false;
			beamParticles.Stop(); // Partikel stoppen
		}

		if(Input.GetButtonDown("Fire1") && isFiringShotgun == false && standardWeaponMode == StandardWeaponMode.shotgun && WaterTank.Instance.waterLevel > 0 && !GameManager.Instance.gameOver)
		{
			isFiringShotgun = true;
			StartCoroutine(ShootShotgun()); //Shoot Shotgun
			StartCoroutine(WaterConsumptionCoroutine(waterConsumptionShotgun));
		}
	}
	IEnumerator WaterConsumptionCoroutine(int waterConsumption)
	{
		while (isFiringBeam && standardWeaponMode == StandardWeaponMode.beam)
		{
			WaterTank.Instance.waterLevel -= waterConsumption;
			if (WaterTank.Instance.waterLevel < 0)
			{
				isFiringBeam = false;
				beamParticles.Stop();
			}
			yield return new WaitForSeconds(waterConsumptionRateBeam);
		}

		if (isFiringShotgun && standardWeaponMode == StandardWeaponMode.shotgun)
		{
			WaterTank.Instance.waterLevel -= waterConsumption;
			yield return new WaitForSeconds(cooldownTimeShotgun);
			isFiringShotgun = false;
		}
		
	}

	IEnumerator ShootShotgun()
	{
		ParticleSystem ps0 = WaterBulletParticlePool.Instance.GetPooledParticleSystem(); //1.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps0.transform.position = muzzle.transform.position;
		ps0.transform.forward = muzzle.transform.forward;
		// Partikel aktivieren
		ps0.gameObject.SetActive(true);
		ps0.Play();

		yield return new WaitForEndOfFrame();

		ParticleSystem ps1 = WaterBulletParticlePool.Instance.GetPooledParticleSystem(); //2.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps1.transform.position = muzzle.transform.position;
		ps1.transform.forward = muzzle.transform.forward + new Vector3(-0.05f, 0f, 0f);
		// Partikel aktivieren
		ps1.gameObject.SetActive(true);
		ps1.Play();

		yield return new WaitForEndOfFrame();

		ParticleSystem ps2 = WaterBulletParticlePool.Instance.GetPooledParticleSystem(); //3.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps2.transform.position = muzzle.transform.position;
		ps2.transform.forward = muzzle.transform.forward + new Vector3(0.05f, 0f, 0f);
		// Partikel aktivieren
		ps2.gameObject.SetActive(true);
		ps2.Play();

		yield return new WaitForEndOfFrame();

		ParticleSystem ps3 = WaterBulletParticlePool.Instance.GetPooledParticleSystem(); //4.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps3.transform.position = muzzle.transform.position;
		ps3.transform.forward = muzzle.transform.forward + new Vector3(0f, 0.05f, 0f);
		// Partikel aktivieren
		ps3.gameObject.SetActive(true);
		ps3.Play();
		
		yield return new WaitForEndOfFrame();

		ParticleSystem ps4 = WaterBulletParticlePool.Instance.GetPooledParticleSystem(); //5.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps4.transform.position = muzzle.transform.position;
		ps4.transform.forward = muzzle.transform.forward + new Vector3(0f, -0.05f, 0f);
		// Partikel aktivieren
		ps4.gameObject.SetActive(true);
		ps4.Play();
	}

	public void SwitchWeaponMode()
	{
		if (standardWeaponMode == StandardWeaponMode.beam) //Wechsel zur Shotgun
		{
			beamParticles.Stop();
			standardWeaponMode = StandardWeaponMode.shotgun;
		}
		else if (standardWeaponMode == StandardWeaponMode.shotgun) //Wechsel zum Beam
		{
			standardWeaponMode = StandardWeaponMode.beam;
		}
	}
}