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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetStatsToBase();
        }
        Debug.Log(stats.health);
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

    #endregion 

    public void ResetStatsToBase()
    {
        ResetStats(baseStats, stats);
    }

    #region reset stats
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
        target.groundDrag = source.groundDrag;
        target.jumpForce = source.jumpForce;
        target.jumpCooldown = source.jumpCooldown;
        target.airMultiplier = source.airMultiplier;
        target.readyToJump = source.readyToJump;

        // Water Stats
        target.standingInWaterTankFillAmount = source.standingInWaterTankFillAmount;
        target.generalTankFillRate = source.generalTankFillRate;
    }
    #endregion stats
}
