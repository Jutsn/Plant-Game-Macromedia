using UnityEngine;

public class BigEnemy : EnemyBehaviour
{
	[SerializeField] private int damageOnPlant; //persönlichen Damage des Kindes im Inspector festlegen
	[SerializeField] private int health; //persönlichen Damage des Kindes im Inspector festlegen

	protected override void OnEnable()
	{
		enemyHealth = health;
		base.OnEnable();
	}

	protected override void DoDamage() //Hier kann man die Basis Do-Damage-Funktion des Eltern-Skripts überschreiben
	{
		damageMade = damageOnPlant;
		base.DoDamage(); //Basis-DoDamage-Funktion des Eltern-Skripts ausführen
		PoisonPlant(); // persönliche PoisonPlant-Funktion durchführen
		gameObject.SetActive(false); //Setze große Gegner inaktiv für Object-Pooling
	}
	protected override void PoisonPlant() //Basis PoisonPlant-Funktion des Elternskripts wird hier überschrieben
	{
		mainPlantScript.mainPlantState = MainPlantState.poisened; //Zustand der Main Plant auf "poisened" setzen
	}

	protected override int GetDropChance()
	{
		int dropChance = GameManager.Instance.GetDropChanceResource1BigEnemy();
		return dropChance;
	}
}
