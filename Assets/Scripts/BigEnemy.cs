using UnityEngine;

public class BigEnemy : EnemyBehaviour
{
	[SerializeField] private int damage; //persönlichen Damage des Kindes im Inspector festlegen
	
	protected override void DoDamage() //Hier kann man die Basis Do-Damage-Funktion des Eltern-Skripts überschreiben
	{
		damageMade = damage;
		base.DoDamage(); //Basis-DoDamage-Funktion des Eltern-Skripts ausführen
		PoisonPlant(); // persönliche PoisonPlant-Funktion durchführen
		gameObject.SetActive(false); //Setze große Gegner inaktiv für Object-Pooling
	}
	protected override void PoisonPlant() //Basis PoisonPlant-Funktion des Elternskripts wird hier überschrieben
	{
		mainPlantScript.mainPlantState = MainPlantState.poisened; //Zustand der Main Plant auf "poisened" setzen
	}
}
