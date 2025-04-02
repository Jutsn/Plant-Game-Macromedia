using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainPlant : MonoBehaviour
{
    public MainPlantState mainPlantState;

    [SerializeField]public int health = 50;
    [SerializeField] private float plantWater = 50;
	private float maxPlantWater;
	[SerializeField] private float waterLoss = 2; 
    [SerializeField] private float waterLossRate = 1;
    [SerializeField] private int noWaterDamage = 1; 
	[SerializeField] private int poisonDamage = 1; 
	[SerializeField] private float passiveHealthLossRate = 1;
	[SerializeField] private int healthRegen = 1; 
	[SerializeField] private float healthRegenRate = 3;

    void Start()
    {
		maxPlantWater = plantWater; 
        int maxHealth = health; //maxHealth bef�llen
        mainPlantState = MainPlantState.normal; //MainPlantState(Enum) auf normal setzen
        StartCoroutine(PassiveWaterLossCoroutine()); 
        StartCoroutine(PassiveHealthLossCoroutine()); //Verdurstung + Vergiftung
        StartCoroutine(PassiveHealthRegeneration(maxHealth)); //maxHealth durchgeben
    }

    IEnumerator PassiveWaterLossCoroutine()
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater > 0) //Wenn Wasserstand ueber null
			{
				plantWater -= waterLoss; //Wasserverlust
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
				UIManager.Instance.UpdatePlantHealthBar(health);
			}
			if (mainPlantState == MainPlantState.poisened && health > 0) //Wenn vergiftet
			{
				health -= poisonDamage; //Lebensverlust
				UIManager.Instance.UpdatePlantHealthBar(health);
				UIManager.Instance.ChangeHealthBarColor(Color.magenta);
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

    IEnumerator PassiveHealthRegeneration(int maxHealth) 
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater > 0 && health < maxHealth) //Wenn Wasser vorhanden und maxHealth nicht �berschritten
			{
				health += healthRegen; // Heile Leben
				UIManager.Instance.UpdatePlantHealthBar(health);
			}
			if (health > maxHealth)
			{
				health = maxHealth;
				UIManager.Instance.UpdatePlantHealthBar(health);
			}
			yield return new WaitForSeconds(healthRegenRate); //Warte vor n�chster Wiederholung f�r ...Sekunden
		}
    }
	public void GetActiveDamage(int damage)
	{
		health -= damage;
		UIManager.Instance.UpdatePlantHealthBar(health);
	}

	public void GetWater(float waterInAmmunation)
	{
		if (plantWater < maxPlantWater)
		{
			plantWater += waterInAmmunation;
			UIManager.Instance.UpdatePlantWaterBar(plantWater);
		}
		else if(plantWater > maxPlantWater)
		{
			plantWater = maxPlantWater;
		}
		
	}

	
}
