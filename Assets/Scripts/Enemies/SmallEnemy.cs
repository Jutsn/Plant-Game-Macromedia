using Unity.VisualScripting;
using UnityEngine;

public class SmallEnemy : EnemyBehaviour
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
		base.DoDamage(); //Basis-Funktion des Eltern-Skripts ausführen
		gameObject.SetActive(false); ////Setze kleine Gegner inaktiv für Object-Pooling
	}

	protected override int GetDropChance()
	{
		int dropChance = GameManager.Instance.GetDropChanceResource1SmallEnemy();
		return dropChance;
	}
}
