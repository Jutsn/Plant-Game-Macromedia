using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class StatsSO : ScriptableObject
{
    [Header("Plant Stats")]
    public int plantMaxHealth = 50;
    public int  health = 50;
    public float plantMaxWater  = 50;
    public float plantWater = 50;
    public float waterLoss = 2;
    public float waterLossRate = 1;
    public int noWaterDamage = 1; 
    public int poisonDamage = 1; 
	public float passiveHealthLossRate = 1;
    public int healthRegen = 1; 
    public float healthRegenRate = 3;

    [Header("PlayerStats")]
    public float moveSpeed = 9;
	public float sprintMultiplier = 1.3f;
	public float groundDrag = 5;
    public float normalJumpForce = 12;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;

	[Header("IceSprint")]
	public bool iceSprintIsUnlocked;
	public float iceMoveSpeed;
	public int waterConsumptionIceSprint = 2;
	public float waterConsumptionIceSprintIntervallInSeconds = 0.5f;

	[Header("Dash")]
	public bool dashIsUnlocked;
	public int maxDashCount = 3;
	public int dashCount = 1;
	public float dashCountResetIntervallInSeconds;
	public float dashForce = 18;
	public float timeUntilSpeedControlGetsActivatedAgain = 0.5f;
	public int waterConsumptionDash = 2;
	

	[Header("MultiJumpStats")]
	public int jumpCount = 1;
	public float extraJumpForce = 18;
	public int waterConsumptionExtraJump = 2;

	[Header("FlyingStats")]
	//public float jetpackFlyTimeMax;
	//public float FlyTimeLeft;
	public bool flyingIsUnlocked = false;
	public float timeUntilFlyingAfterJump = 1.5f;
	public float jetpackFlyForce = 6;
	public int waterConsumptionFlying = 2;
	public float waterConsumptionFlyingIntervallInSeconds = 0.5f;

	[Header("Antitoxin-Interaction")]
	public float interactionRange = 10;

	[Header("WaterStats")]
	public int playerTankMaxWaterLevel = 50;
	public int playerTankWaterLevel;
	public int standingInWaterTankFillAmount;
    public float tankFillRateInSeconds;
	public float fireRate;
	public int damage = 1;

    [Header("JetpackStats")]
    public bool hasIceSkating;
	public bool hasDash;
}
