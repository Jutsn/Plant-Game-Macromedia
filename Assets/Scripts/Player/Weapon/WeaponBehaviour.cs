using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public StandardWeaponMode standardWeaponMode;

	public StatsSO baseStats;

	public Transform firePoint; // Punkt, von dem der Strahl ausgeht
	public ParticleSystem beamParticles; // Das Partikel-System
	public GameObject muzzle;
	public Animator animator;

	[Header("Beam")]
	[SerializeField] private int waterConsumptionBeam;
	[SerializeField] private float waterConsumptionRateBeam = 1;
	private bool isFiringBeam;

	[Header("Shotgun")]
	[SerializeField] private int waterConsumptionShotgun = 3;
	[SerializeField] private float cooldownTimeShotgun = 0.5f;
	private bool isFiringShotgun;

	[Header("AR")]
	[SerializeField] private int waterConsumptionAR = 3;
	[SerializeField] private float cooldownTimeAR = 0.5f;
	private bool isFiringAR;
	private bool ARFireReady;
	float arTimer = 0;
	
	private void Start()
	{
		standardWeaponMode = StandardWeaponMode.automaticRifle;
		
	}
	void Update()
	{
		if (Input.GetButtonDown("Fire1") && standardWeaponMode == StandardWeaponMode.beam && WaterTank.Instance.playerTankWaterLevel > 0 && !GameManager.Instance.gameOver && !GameManager.Instance.IsPaused) // Schie�en solange Maustaste gedr�ckt
		{
			animator.SetBool("isBeaming", true);
			beamParticles.Play();
			isFiringBeam = true;
			StartCoroutine(WaterConsumptionCoroutine(waterConsumptionBeam));
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			isFiringBeam = false;
			animator.SetBool("isBeaming", false);
			beamParticles.Stop(); // Partikel stoppen
		}

		if (Input.GetButtonDown("Fire1") && !isFiringShotgun && standardWeaponMode == StandardWeaponMode.shotgun && WaterTank.Instance.playerTankWaterLevel > 0 && !GameManager.Instance.gameOver && !GameManager.Instance.IsPaused)
		{
			isFiringShotgun = true;
			StartCoroutine(ShootShotgun()); //Shoot Shotgun
			StartCoroutine(WaterConsumptionCoroutine(waterConsumptionShotgun));
		}

		if (Input.GetButtonDown("Fire1") && !isFiringAR & standardWeaponMode == StandardWeaponMode.automaticRifle && WaterTank.Instance.playerTankWaterLevel > 0 && !GameManager.Instance.gameOver && !GameManager.Instance.IsPaused)
		{
			isFiringAR = true;
			StartCoroutine(WaterConsumptionCoroutine(waterConsumptionAR));
		}
		else if (Input.GetButtonUp("Fire1") && isFiringAR)
		{
			isFiringAR = false;
			arTimer = cooldownTimeAR;
		}
		if (isFiringAR)
		{
			
			ARTimer(); //Cooldown between bullets while holding
			ShootAR(); //Shoot AR
		}
		
	}
	

	IEnumerator WaterConsumptionCoroutine(int waterConsumption)
	{	
		while (isFiringBeam && standardWeaponMode == StandardWeaponMode.beam)
		{
			WaterTank.Instance.UseTankWater(waterConsumption);
			if (WaterTank.Instance.playerTankWaterLevel < 0)
			{
				isFiringBeam = false;
				beamParticles.Stop();
			}
			yield return new WaitForSeconds(waterConsumptionRateBeam);
		}

		if (isFiringShotgun && standardWeaponMode == StandardWeaponMode.shotgun)
		{
			WaterTank.Instance.UseTankWater(waterConsumption);
			yield return new WaitForSeconds(cooldownTimeShotgun);
			isFiringShotgun = false;
		}

		while (isFiringAR && standardWeaponMode == StandardWeaponMode.automaticRifle)
		{
			WaterTank.Instance.UseTankWater(waterConsumption);
			yield return new WaitForSeconds(cooldownTimeAR);
		}
		
	}

	IEnumerator ShootShotgun()
	{
		animator.SetTrigger("isShooting");
		yield return new WaitForSeconds(0.1f);
		ParticleSystem ps0 = WaterBulletParticlePool.Instance.GetPooledShotgunBullet(); //1.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps0.transform.position = muzzle.transform.position;
		ps0.transform.forward = muzzle.transform.forward;
		// Partikel aktivieren
		ps0.gameObject.SetActive(true);
		ps0.Play();

		yield return new WaitForEndOfFrame();

		ParticleSystem ps1 = WaterBulletParticlePool.Instance.GetPooledShotgunBullet(); //2.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps1.transform.position = muzzle.transform.position;
		ps1.transform.forward = muzzle.transform.forward + new Vector3(-0.05f, 0f, 0f);
		// Partikel aktivieren
		ps1.gameObject.SetActive(true);
		ps1.Play();

		yield return new WaitForEndOfFrame();

		ParticleSystem ps2 = WaterBulletParticlePool.Instance.GetPooledShotgunBullet(); //3.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps2.transform.position = muzzle.transform.position;
		ps2.transform.forward = muzzle.transform.forward + new Vector3(0.05f, 0f, 0f);
		// Partikel aktivieren
		ps2.gameObject.SetActive(true);
		ps2.Play();

		yield return new WaitForEndOfFrame();

		ParticleSystem ps3 = WaterBulletParticlePool.Instance.GetPooledShotgunBullet(); //4.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps3.transform.position = muzzle.transform.position;
		ps3.transform.forward = muzzle.transform.forward + new Vector3(0f, 0.05f, 0f);
		// Partikel aktivieren
		ps3.gameObject.SetActive(true);
		ps3.Play();
		
		yield return new WaitForEndOfFrame();

		ParticleSystem ps4 = WaterBulletParticlePool.Instance.GetPooledShotgunBullet(); //5.Kugel
		// Richte das Partikelsystem in die Richtung des Treffers aus
		ps4.transform.position = muzzle.transform.position;
		ps4.transform.forward = muzzle.transform.forward + new Vector3(0f, -0.05f, 0f);
		// Partikel aktivieren
		ps4.gameObject.SetActive(true);
		ps4.Play();
	}
	private void ShootAR()
	{
		
		if (ARFireReady)
		{
			StartCoroutine(ShootARBullet());
			ARFireReady = false;
		}
	}
	private void ARTimer()
	{
		arTimer += Time.deltaTime;

		if (arTimer >= cooldownTimeAR)
		{
			arTimer = 0;
			ARFireReady = true;
		}
	}
	IEnumerator ShootARBullet()
	{
		animator.SetTrigger("isShooting");
		yield return new WaitForSeconds(0.1f);
		ParticleSystem ps5 = WaterBulletParticlePool.Instance.GetPooledARBullet(); //1.Kugel
																				   // Richte das Partikelsystem in die Richtung des Treffers aus
		ps5.transform.position = muzzle.transform.position;
		ps5.transform.forward = muzzle.transform.forward;
		// Partikel aktivieren
		ps5.gameObject.SetActive(true);
		ps5.Play();
	}

	public void SwitchWeaponMode()
	{
		if (standardWeaponMode == StandardWeaponMode.beam) //Wechsel zur Shotgun
		{
			beamParticles.Stop();
			animator.SetTrigger("isSwitchingWeapon");
			standardWeaponMode = StandardWeaponMode.automaticRifle;
		}
		else if (standardWeaponMode == StandardWeaponMode.shotgun) //Wechsel zur AR
		{
			animator.SetTrigger("isSwitchingWeapon");
			if (!baseStats.beamUnlocked)
				standardWeaponMode = StandardWeaponMode.automaticRifle;
			else if (baseStats.beamUnlocked)
				standardWeaponMode = StandardWeaponMode.beam;
		}
		else if (standardWeaponMode == StandardWeaponMode.automaticRifle && baseStats.shotgunUnlocked) //Wechsel zum Beam
		{
			animator.SetTrigger("isSwitchingWeapon");
			standardWeaponMode = StandardWeaponMode.shotgun;
		}
	}
}