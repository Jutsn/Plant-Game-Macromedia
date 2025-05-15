using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MainPlant : MonoBehaviour
{
	public int plantMaxHealth;
    public int  health = 50;
    public float plantMaxWater  = 50;
    public float plantWater = 50;
    public float waterLoss = 2;
    public float waterLossRate = 1;
    public int noWaterDamage = 1; 
    public int poisonDamage = 1; 
	public float passiveHealthLossRate = 1;
    public int healthRegen = 1; 
    public float healthRegenRate = 3;

    public MainPlantState mainPlantState;

    void OnEnable()
    {
        StatsManager.OnStatsChanged += RefreshStats;
    }

    void OnDisable()
    {
        StatsManager.OnStatsChanged += RefreshStats;
    }

	void RefreshStats(StatsManager stats)
	{
		plantMaxHealth = StatsManager.Instance.stats.plantMaxHealth;
		health = StatsManager.Instance.stats.health;
		plantMaxWater = StatsManager.Instance.stats.plantMaxWater;
		plantWater = StatsManager.Instance.stats.plantWater;
		waterLoss = StatsManager.Instance.stats.waterLoss;
		waterLossRate = StatsManager.Instance.stats.waterLossRate;
		noWaterDamage = StatsManager.Instance.stats.noWaterDamage;
		poisonDamage = StatsManager.Instance.stats.poisonDamage;
		passiveHealthLossRate = StatsManager.Instance.stats.passiveHealthLossRate;
		healthRegen = StatsManager.Instance.stats.healthRegen;
		healthRegenRate = StatsManager.Instance.stats.healthRegenRate;
	}
	// holt sich die stats aus dem statsmanager

    void Start()
    {
		plantMaxHealth = StatsManager.Instance.stats.plantMaxHealth;
		plantMaxWater = StatsManager.Instance.stats.plantMaxWater;
		waterLoss = StatsManager.Instance.stats.waterLoss;
		waterLossRate = StatsManager.Instance.stats.waterLossRate;
		noWaterDamage = StatsManager.Instance.stats.noWaterDamage;
		poisonDamage = StatsManager.Instance.stats.poisonDamage;
		passiveHealthLossRate = StatsManager.Instance.stats.passiveHealthLossRate;
		healthRegen = StatsManager.Instance.stats.healthRegen;
		healthRegenRate = StatsManager.Instance.stats.healthRegenRate;

		
        mainPlantState = MainPlantState.normal; //MainPlantState(Enum) auf normal setzen
        StartCoroutine(PassiveWaterLossCoroutine()); 
        StartCoroutine(PassiveHealthLossCoroutine()); //Verdurstung + Vergiftung
        StartCoroutine(PassiveHealthRegeneration()); //maxHealth durchgeben
    }

    IEnumerator PassiveWaterLossCoroutine()
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater > 0) //Wenn Wasserstand ueber null
			{
				plantWater -= waterLoss; //Wasserverlust
				StatsManager.Instance.SetPlantWater(plantWater);
				UIManager.Instance.UpdatePlantWaterBar(plantWater);
			}
			if (plantWater < 0) //Wenn Wasserstand unter 0
			{
				plantWater = 0; //Verhindere Negativwasserstand
			}
			yield return new WaitForSeconds(waterLossRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
	}

    IEnumerator PassiveHealthLossCoroutine() 
    {
        while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater == 0 && health > 0) // Wenn kein Wasser mehr vorhanden
			{
				health -= noWaterDamage; //Lebensverlust
				StatsManager.Instance.SetHealth(health);
				UIManager.Instance.UpdatePlantHealthBar(health);
			}
			if (mainPlantState == MainPlantState.poisened && health > 0) //Wenn vergiftet
			{
				health -= poisonDamage; //Lebensverlust
				UIManager.Instance.ChangeHealthBarColor(Color.magenta);
				StatsManager.Instance.SetHealth(health);
				UIManager.Instance.UpdatePlantHealthBar(health);
				
			}
			if (health < 0) //Wenn Lebenszahl negativ
			{
				health = 0; //Verhindere Negativleben 
			}
			if (health == 0) //Wenn keine Leben
			{
				GameManager.Instance.gameOver = true; //gameOver-Variable in GameManager true setzen und Schleife nicht nochmal wiederholen
				GameManager.Instance.GameOver(); //GameOver-Methode in GameManager callen 
			}
			yield return new WaitForSeconds(passiveHealthLossRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
	}

    IEnumerator PassiveHealthRegeneration() 
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater > 0 && health < plantMaxHealth) //Wenn Wasser vorhanden und maxHealth nicht �berschritten
			{
				health += healthRegen; // Heile Leben
				StatsManager.Instance.SetHealth(health);
				UIManager.Instance.UpdatePlantHealthBar(health);
			}
			if (health >  plantMaxHealth)
			{
				health =  plantMaxHealth;
				StatsManager.Instance.SetHealth(health);
				UIManager.Instance.UpdatePlantHealthBar(health);
			}
			yield return new WaitForSeconds(healthRegenRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
    }
	public void GetActiveDamage(int damage)
	{
		health -= damage;
		StatsManager.Instance.SetHealth(health);
		UIManager.Instance.UpdatePlantHealthBar(health);
	}

	public void GetWater(float waterInAmmunation)
	{
		if (plantWater < plantMaxWater)
		{
			plantWater += waterInAmmunation;
			StatsManager.Instance.SetPlantWater(plantWater);
			UIManager.Instance.UpdatePlantWaterBar(plantWater);
		}
		else if(plantWater > plantMaxWater)
		{
			plantWater = plantMaxWater;
		}
	}

	public void DetoxPlant()
	{
		mainPlantState = MainPlantState.normal;
		StatsManager.Instance.SetHealth(health);
		UIManager.Instance.UpdatePlantHealthBar(health);
		UIManager.Instance.ChangeHealthBarColor(Color.green);
	}


	
}
