using UnityEngine;

public class SmallEnemy : EnemyBehaviour
{
	[SerializeField] private int damage; //persönlichen Damage des Kindes im Inspector festlegen
	protected override void DoDamage(int damage) //Hier kann man die Basis Do-Damage-Funktion des Eltern-Skripts überschreiben
	{
		base.DoDamage(damage); //Basis-Funktion des Eltern-Skripts ausführen
		gameObject.SetActive(false); ////Setze kleine Gegner inaktiv für Object-Pooling
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!GameManager.Instance.gameOver)
		{
			if (collision.gameObject == mainPlant) //mainPlant wurde bereits im Eltern-Skript Enemy Behaviour befüllt
			{
				DoDamage(damage); //DoDamage-Funktion des Kindes mit persönlichem Damage-Wert des Kindes ausführen
			}
		}
	}

}
