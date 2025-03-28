using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public StandardWeaponMode standardWeaponMode;

	public Transform firePoint; // Punkt, von dem der Strahl ausgeht
	public ParticleSystem beamParticles; // Das Partikel-System
	[SerializeField] private int waterConsumptionBeam;
	[SerializeField] private float waterConsumptionRate = 1;
	private bool isFiring;


	void Update()
	{
		if (Input.GetButtonDown("Fire1") && standardWeaponMode == StandardWeaponMode.beam && WaterTank.Instance.waterLevel > 0 && !GameManager.Instance.gameOver) // Schießen solange Maustaste gedrückt
		{
			beamParticles.Play();
			isFiring = true;
			StartCoroutine(WaterConsumptionCoroutine(waterConsumptionBeam));
		}
		else if(Input.GetButtonUp("Fire1"))
		{
			isFiring = false;
			beamParticles.Stop(); // Partikel stoppen
		}

		if(Input.GetButtonDown("Fire1") && standardWeaponMode == StandardWeaponMode.shotgun && WaterTank.Instance.waterLevel > 0)
		{
			//Shoot Shotgun
		}
	}
	IEnumerator WaterConsumptionCoroutine(int waterConsumption)
	{
		while (isFiring)
		{
			WaterTank.Instance.waterLevel -= waterConsumption;
			yield return new WaitForSeconds(waterConsumptionRate);
		}
		
	}
}