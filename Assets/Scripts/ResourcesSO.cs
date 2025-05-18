using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "Scriptable Objects/Resources")]
public class ResourcesSO : ScriptableObject
{
    public int resource1 = 0;
    public int resource2 = 0;
    public int resource3 = 0;
	public int antitoxin = 0;
	public int dropChanceResource1SmallEnemy = 50;
	public int dropChanceResource1BigEnemy = 50;
	public int minDropAmountResource2Elite = 1;
	public int maxDropAmountResource2Elite = 4;
}
