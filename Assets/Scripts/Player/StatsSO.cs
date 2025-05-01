using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class StatsSO : ScriptableObject
{
    [Header("Plant Stats")]
    public int plantMaxHealth = 50;
    public int currentPlantHealth = 50;
    public float plantMaxWater  = 50;
    public float currentPlantWater = 590;
    public float waterLoss = 2;
    public float waterLossRate = 1;
    public int noWaterDamage = 1; 
    public int poisonDamage = 1; 
	public float passiveHealthLossRate = 1;
    public int healthRegen = 1; 
    public float healthRegenRate = 3;

    [Header("PlayerStats")]
    public float moveSpeed = 9;
    public float groundDrag = 5;
    public float jumpForce = 12;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    public bool readyToJump = true;

    [Header("WaterStats")]
    public int standingInWaterTankFillAmount;
    public float generalTankFillRate;
}
