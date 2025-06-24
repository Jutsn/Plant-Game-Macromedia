using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
    }

    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;
    }

    //case für jeden Skill der erhalten werden kann --> updatet skill im StatsManager
    private void HandleAbilityPointSpent(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;
        Debug.Log("skill gefunden");

        switch (skillName)
        {
            case "Speed Up Lv1":
                StatsManager.Instance.UpdateSpeedStat(2);
                break;
            case "Speed Up Lv2":
                StatsManager.Instance.UpdateSpeedStat(4);
                break;
            case "Speed Up Lv3":
                StatsManager.Instance.UpdateSpeedStat(4);
                break;
            case "Water Loss Down Lv1":
                StatsManager.Instance.UpdateWaterLossStat(0.25f);
                break;
            case "Water Loss Down Lv2":
                StatsManager.Instance.UpdateWaterLossStat(0.5f);
                break;
            case "Water Loss Down Lv3":
                StatsManager.Instance.UpdateWaterLossStat(0.75f);
                break;
            case "Plant Health Boost Lv1":
                StatsManager.Instance.UpdateMaxHealthStat(25);
                break;
            case "Plant Health Boost Lv2":
                StatsManager.Instance.UpdateMaxHealthStat(35);
                break;
            case "Plant Health Boost Lv3":
                StatsManager.Instance.UpdateMaxHealthStat(50);
                break;
            case "Fire Rate Up Lv1":
                StatsManager.Instance.UpdateFireRateStat(0.1f);
                break;
            case "Fire Rate Up Lv2":
                StatsManager.Instance.UpdateFireRateStat(0.2f);
                break;
            case "Fire Rate Up Lv3":
                StatsManager.Instance.UpdateFireRateStat(0.3f);
                break;
            case "Damage Up Lv1":
                StatsManager.Instance.UpdateDamageStat(1);
                break;
            case "Damage Up Lv2":
                StatsManager.Instance.UpdateDamageStat(2);
                break;
            case "Damage Up Lv3":
                StatsManager.Instance.UpdateDamageStat(3);
                break;
            case "Water Tank Up Lv1":
                StatsManager.Instance.UpdateWaterTankStat(10);
                break;
            case "Water Tank Up Lv2":
                StatsManager.Instance.UpdateWaterTankStat(20);
                break;
            case "Water Tank Up Lv3":
                StatsManager.Instance.UpdateWaterTankStat(30);
                break;
            case "Refill Up Lv1":
                StatsManager.Instance.UpdateWaterTankFillRate(0.15f);
                break;
            case "Refill Up Lv2":
                StatsManager.Instance.UpdateWaterTankFillRate(0.2f);
                break;
            case "Refill Up Lv3":
                StatsManager.Instance.UpdateWaterTankFillRate(0.3f);
                break;
            case "Poision Resistance Lv1":
                //StatsManager.Instance.UpdatePoisonResistance(0.25);
                break;
            case "Poison Resistance Lv2":
                //StatsManager.Instance.UpdatePoisonResistance(0.35);
                break;
            case "Poison Resistance Lv3":
                //StatsManager.Instance.UpdatePoisonResistance(0.5);
                break;
            case "Poison Immnunity":
                //StatsManager.Instance.UpdatePoisonImmunity(true);
                break;
            case "JetPackJumpLv1":
                StatsManager.Instance.baseStats.jumpCount += 1;
                break;
            case "JetPackFlight":
                StatsManager.Instance.baseStats.flyingIsUnlocked = true;
                break;
            case "JetPackWaterLoss":
                StatsManager.Instance.baseStats.waterConsumptionFlying = 1;
                break;

            default:
                // wenn ein skill geupgradet wird aber nicht existiert kommt diese Warnung
                    Debug.LogWarning("Unknown Skill: " + skillName);
                    break;
        }

    }
}
