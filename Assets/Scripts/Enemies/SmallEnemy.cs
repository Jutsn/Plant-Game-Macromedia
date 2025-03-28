using Unity.VisualScripting;
using UnityEngine;

public class SmallEnemy : EnemyBehaviour
{
	[SerializeField] private int damage; //persönlichen Damage des Kindes im Inspector festlegen
	protected override void DoDamage() //Hier kann man die Basis Do-Damage-Funktion des Eltern-Skripts überschreiben
	{
		damageMade = damage;
		base.DoDamage(); //Basis-Funktion des Eltern-Skripts ausführen
		gameObject.SetActive(false); ////Setze kleine Gegner inaktiv für Object-Pooling
	}
}
