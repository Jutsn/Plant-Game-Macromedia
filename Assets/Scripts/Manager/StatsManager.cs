using UnityEngine;
using TMPro;
using System;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public StatsSO baseStats;
    [SerializeField] public StatsSO stats;
     public static Action<StatsManager> OnStatsChanged;
    void Awake()
    {
        //erstellt Kopie von Base Stats SO, wenn keine eigenen Stats SO eingesetzt wurden (fuer das testen)
        if(Instance == null){
            Instance = this;
            if(stats == null)
            stats = Instantiate(baseStats);
        }   
        else{
            Destroy(gameObject);   
        }
    }

    void Update()
    {   
        GetInput();
    }

    #region Upgrade Stats
    public void UpdateSpeedStat(int amount)
    {
        stats.moveSpeed += amount;
        OnStatsChanged.Invoke(this);
    }   
     public void UpdateWaterLossStat(float amount)
    {
        stats.waterLoss -= amount;
        OnStatsChanged.Invoke(this);
    }
    public void UpdateMaxHealthStat(int amount)
    {
        stats.plantMaxHealth += amount;
        stats.health += amount;
        OnStatsChanged.Invoke(this);
        UIManager.Instance.plantHealthBar.maxValue = stats.plantMaxHealth;
        UIManager.Instance.UpdatePlantHealthBar(stats.health);     
    }
    public void UpdateFireRateStat(float amount)
    {
        stats.fireRate -= amount;
        OnStatsChanged.Invoke(this);
    }

    public void UpdateDamageStat(int amount)
    {
        stats.damage += amount;
        OnStatsChanged.Invoke(this);
    }

    public void UpdateWaterTankStat(int amount)
    {
        stats.playerTankMaxWaterLevel += amount;
        stats.playerTankWaterLevel += amount;
        OnStatsChanged.Invoke(this);
    }

    public void UpdateWaterTankFillRate(float amount)
    {
        stats.tankFillRateInSeconds -= amount;
        OnStatsChanged.Invoke(this);
    }

    #endregion
    //Button Input für das Playtesten
    public void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetStatsToBase();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnStatsChanged.Invoke(this);
        }
    }
    public void ResetStatsToBase()
    {
        ResetStats(baseStats, stats);
    }

    #region set stats
    //Stats SO wird zurückgesetzt auf BaseStats 
     private void ResetStats(StatsSO source, StatsSO target)
    {
        // Plant Stats
        target.plantMaxHealth = source.plantMaxHealth;
        target.health = source.health;
        target.plantMaxWater = source.plantMaxWater;
        target.plantWater = source.plantWater;  
        target.waterLoss = source.waterLoss;
        target.waterLossRate = source.waterLossRate;
        target.noWaterDamage = source.noWaterDamage;
        target.poisonDamage = source.poisonDamage;
        target.passiveHealthLossRate = source.passiveHealthLossRate;
        target.healthRegen = source.healthRegen;
        target.healthRegenRate = source.healthRegenRate;

        // Player Stats
        target.moveSpeed = source.moveSpeed;
        target.sprintMultiplier = source.sprintMultiplier;
        target.groundDrag = source.groundDrag;
        target.normalJumpForce = source.normalJumpForce;
        target.jumpCooldown = source.jumpCooldown;
        target.airMultiplier = source.airMultiplier;

		//Ice-Sprint
        target.iceSprintIsUnlocked = source.iceSprintIsUnlocked;
		target.iceMoveSpeed = source.iceMoveSpeed;
		target.waterConsumptionIceSprint = source.waterConsumptionIceSprint;
		target.waterConsumptionIceSprintIntervallInSeconds = source.waterConsumptionIceSprintIntervallInSeconds;

        //Dash
        target.dashIsUnlocked = source.dashIsUnlocked;
		target.maxDashCount = source.maxDashCount;
		target.dashCount = source.dashCount;
		target.dashCountResetIntervallInSeconds = source.dashCountResetIntervallInSeconds;
		target.dashForce = source.dashForce;
        target.timeUntilSpeedControlGetsActivatedAgain = source.timeUntilSpeedControlGetsActivatedAgain;
		target.waterConsumptionDash = source.waterConsumptionDash;

		//MultiJumpStats
		target.jumpCount = source.jumpCount;
	    target.extraJumpForce = source.extraJumpForce;
	    target.waterConsumptionExtraJump = source.waterConsumptionExtraJump;

		//FlyingStats
		target.flyingIsUnlocked = source.flyingIsUnlocked;
		target.timeUntilFlyingAfterJump = source.timeUntilFlyingAfterJump;
		target.jetpackFlyForce = source.jetpackFlyForce;
		target.waterConsumptionFlying = source.waterConsumptionFlying;
		target.waterConsumptionFlyingIntervallInSeconds = source.waterConsumptionFlyingIntervallInSeconds;

		//Antitoxin-Interaction
        target.interactionRange = source.interactionRange;

        // Water Stats
        target.playerTankMaxWaterLevel = source.playerTankMaxWaterLevel;
		target.playerTankWaterLevel = source.playerTankWaterLevel;
		target.standingInWaterTankFillAmount = source.standingInWaterTankFillAmount;
        target.tankFillRateInSeconds = source.tankFillRateInSeconds;
        OnStatsChanged.Invoke(this);
    }
    // health variable wird mit der MainPlant Health Variable synchronisiert
    public void SetHealth(int amount)
    {
        stats.health = amount;
        OnStatsChanged.Invoke(this);
    }
    // plantWater variable wird mit der MainPlant plantWater Variable synchronisiert
    public void SetPlantWater(float amount)
    {
        stats.plantWater = amount;
        OnStatsChanged.Invoke(this);
    }
    public void SetPlayerTankWater(int amount)
    {
        stats.playerTankWaterLevel = amount;
        OnStatsChanged.Invoke(this);
    }
    
    public void SetGroundDrag(float amount)
    {
		stats.groundDrag = amount;
		OnStatsChanged.Invoke(this);
	}
    public void SetSpeed(float amount)
    {
		stats.moveSpeed = amount;
		OnStatsChanged.Invoke(this);
	}
    public void SetDashCount(int amount)
    {
		stats.dashCount = amount;
		OnStatsChanged.Invoke(this);
	}

    #endregion stats
}
