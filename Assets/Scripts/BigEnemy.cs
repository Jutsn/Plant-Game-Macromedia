using UnityEngine;

public class BigEnemy : EnemyBehaviour
{
	[SerializeField] private int damage; //persönlichen Damage des Kindes im Inspector festlegen
	
	protected override void DoDamage(int damage) //Hier kann man die Basis Do-Damage-Funktion des Eltern-Skripts überschreiben
	{
		base.DoDamage(damage); //Basis-DoDamage-Funktion des Eltern-Skripts ausführen
		PoisonPlant(); // persönliche PoisonPlant-Funktion durchführen
		gameObject.SetActive(false); //Setze große Gegner inaktiv für Object-Pooling
	}
	protected override void PoisonPlant() //Basis PoisonPlant-Funktion des Elternskripts wird hier überschrieben
	{
		mainPlantScript.mainPlantState = MainPlantState.poisened; //Zustand der Main Plant auf "poisened" setzen
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!GameManager.Instance.gameOver)
		{
			if (collision.gameObject == mainPlant) //mainPlant wurde bereits im Eltern-Skript Enemy Behaviour befüllt
			{
				DoDamage(damage); //Basis DoDamage-Funktion von Eltern-Skript mit persönlichem Damage-Wert des Kindes ausführen
			}
		}
		
	}
	
}
