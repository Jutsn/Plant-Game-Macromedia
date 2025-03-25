using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainPlant : MonoBehaviour
{
    public MainPlantState mainPlantState;

    public int health = 50;
    [SerializeField] private int plantWater = 50;
    [SerializeField] private int waterLoss = 2; 
    [SerializeField] private float waterLossRate = 1;
    [SerializeField] private int noWaterDamage = 1; 
	[SerializeField] private int poisonDamage = 1; 
	[SerializeField] private float passiveHealthLossRate = 1;
	[SerializeField] private int healthRegen = 1; 
	[SerializeField] private float healthRegenRate = 3;

    void Start()
    {
        int maxHealth = health; //maxHealth befüllen
        mainPlantState = MainPlantState.normal; //MainPlantState(Enum) auf normal setzen
        StartCoroutine(PassiveWaterLossCoroutine()); 
        StartCoroutine(PassiveHealthLossCoroutine()); //Verdurstung + Vergiftung
        StartCoroutine(PassiveHealthRegeneration(maxHealth)); //maxHealth durchgeben
    }

    IEnumerator PassiveWaterLossCoroutine()
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater > 0) //Wenn Wasserstand über null
			{
				plantWater -= waterLoss; //Wasserverlust
			}
			if (plantWater < 0) //Wenn Wasserstand unter 0
			{
				plantWater = 0; //Verhindere Negativwasserstand
			}
			yield return new WaitForSeconds(waterLossRate); //Warte vor nächster Wiederholung für ...Sekunden
		}
	}

    IEnumerator PassiveHealthLossCoroutine() 
    {
        while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater == 0 && health > 0) // Wenn kein Wasser mehr vorhanden
			{
				health -= noWaterDamage; //Lebensverlust
			}
			if (mainPlantState == MainPlantState.poisened && health > 0) //Wenn vergiftet
			{
				health -= poisonDamage; //Lebensverlust
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
			yield return new WaitForSeconds(passiveHealthLossRate); //Warte vor nächster Wiederholung für ...Sekunden
		}
	}

    IEnumerator PassiveHealthRegeneration(int maxHealth) 
    {
		while (!GameManager.Instance.gameOver) // Wiederhole, solange Spiel nicht GameOver ist
		{
			if (plantWater > 0 && health < maxHealth) //Wenn Wasser vorhanden und maxHealth nicht überschritten
			{
				health += healthRegen; // Heile Leben
			}
			if (health > maxHealth)
			{
				health = maxHealth;
			}
			yield return new WaitForSeconds(healthRegenRate); //Warte vor nächster Wiederholung für ...Sekunden
		}
    }
	public void GetActiveDamage(int damage)
	{
		health -= damage;
	}
}
