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
    //Update Funktionen für Stat Buffs von Variablen
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
    //Button Input für das Playtesten
    public void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ResetStatsToBase();
        }
        if(Input.GetKeyDown(KeyCode.K))
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
        target.groundDrag = source.groundDrag;
        target.jumpForce = source.jumpForce;
        target.jumpCooldown = source.jumpCooldown;
        target.airMultiplier = source.airMultiplier;
        target.readyToJump = source.readyToJump;

        // Water Stats
        target.standingInWaterTankFillAmount = source.standingInWaterTankFillAmount;
        target.generalTankFillRate = source.generalTankFillRate;
        OnStatsChanged.Invoke(this);
    }
    // health variable wird mit der MainPlant Health Variable synchronisiert
    public void SetHealth(int amount)
    {
        stats.health += amount;
        OnStatsChanged.Invoke(this);
    }
    // plantWater variable wird mit der MainPlant plantWater Variable synchronisiert
    public void SetPlantWater(float amount)
    {
        stats.plantWater += amount;
        OnStatsChanged.Invoke(this);

    }
    #endregion stats
}
