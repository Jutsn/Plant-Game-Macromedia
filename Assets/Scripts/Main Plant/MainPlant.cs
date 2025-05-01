using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainPlant : MonoBehaviour
{
    public MainPlantState mainPlantState;


    void Start()
    {

        mainPlantState = MainPlantState.normal; //MainPlantState(Enum) auf normal setzen
        StartCoroutine(PassiveWaterLossCoroutine()); 
        StartCoroutine(PassiveHealthLossCoroutine()); //Verdurstung + Vergiftung
        //StartCoroutine(PassiveHealthRegeneration(StatsManager.Instance.stats.plantMaxHealth)); //maxHealth durchgeben
    }

    IEnumerator PassiveWaterLossCoroutine()
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (StatsManager.Instance.stats.plantWater > 0) //Wenn Wasserstand ueber null
			{
				StatsManager.Instance.stats.plantWater -= StatsManager.Instance.stats.waterLoss; //Wasserverlust
				UIManager.Instance.UpdatePlantWaterBar(StatsManager.Instance.stats.plantWater);
			}
			if (StatsManager.Instance.stats.plantWater < 0) //Wenn Wasserstand unter 0
			{
				StatsManager.Instance.stats.plantWater = 0; //Verhindere Negativwasserstand
			}
			yield return new WaitForSeconds(StatsManager.Instance.stats.waterLossRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
	}

    IEnumerator PassiveHealthLossCoroutine() 
    {
        while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (StatsManager.Instance.stats.plantWater == 0 && StatsManager.Instance.stats.health > 0) // Wenn kein Wasser mehr vorhanden
			{
				StatsManager.Instance.stats.health -= StatsManager.Instance.stats.noWaterDamage; //Lebensverlust
				UIManager.Instance.UpdatePlantHealthBar(StatsManager.Instance.stats.health);
			}
			if (mainPlantState == MainPlantState.poisened && StatsManager.Instance.stats.health > 0) //Wenn vergiftet
			{
				StatsManager.Instance.stats.health -= StatsManager.Instance.stats.poisonDamage; //Lebensverlust
				UIManager.Instance.UpdatePlantHealthBar(StatsManager.Instance.stats.health);
				UIManager.Instance.ChangeHealthBarColor(Color.magenta);
			}
			if (StatsManager.Instance.stats.health < 0) //Wenn Lebenszahl negativ
			{
				StatsManager.Instance.stats.health = 0; //Verhindere Negativleben 
			}
			if (StatsManager.Instance.stats.health == 0) //Wenn keine Leben
			{
				GameManager.Instance.gameOver = true; //gameOver-Variable in GameManager true setzen und Schleife nicht nochmal wiederholen
				GameManager.Instance.GameOver(); //GameOver-Methode in GameManager callen 
			}
			yield return new WaitForSeconds(StatsManager.Instance.stats.passiveHealthLossRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
	}

    IEnumerator PassiveHealthRegeneration() 
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (StatsManager.Instance.stats.plantWater > 0 && StatsManager.Instance.stats.health < StatsManager.Instance.stats.plantMaxHealth) //Wenn Wasser vorhanden und maxHealth nicht �berschritten
			{
				StatsManager.Instance.stats.health += StatsManager.Instance.stats.healthRegen; // Heile Leben
				UIManager.Instance.UpdatePlantHealthBar(StatsManager.Instance.stats.health);
			}
			if (StatsManager.Instance.stats.health >  StatsManager.Instance.stats.plantMaxHealth)
			{
				StatsManager.Instance.stats.health =  StatsManager.Instance.stats.plantMaxHealth;
				UIManager.Instance.UpdatePlantHealthBar(StatsManager.Instance.stats.health);
			}
			yield return new WaitForSeconds(StatsManager.Instance.stats.healthRegenRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
    }
	public void GetActiveDamage(int damage)
	{
		StatsManager.Instance.stats.health -= damage;
		UIManager.Instance.UpdatePlantHealthBar(StatsManager.Instance.stats.health);
	}

	public void GetWater(float waterInAmmunation)
	{
		if (StatsManager.Instance.stats.plantWater < StatsManager.Instance.stats.maxPlantWater)
		{
			StatsManager.Instance.stats.plantWater += waterInAmmunation;
			UIManager.Instance.UpdatePlantWaterBar(StatsManager.Instance.stats.plantWater);
		}
		else if(StatsManager.Instance.stats.plantWater > StatsManager.Instance.stats.maxPlantWater)
		{
			StatsManager.Instance.stats.plantWater = StatsManager.Instance.stats.maxPlantWater;
		}
	}

	public void DetoxPlant()
	{
		mainPlantState = MainPlantState.normal;
		UIManager.Instance.UpdatePlantHealthBar(StatsManager.Instance.stats.health);
		UIManager.Instance.ChangeHealthBarColor(Color.green);
	}


	
}
