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
            case "Speed Up":
                StatsManager.Instance.UpdateSpeedStat(2);
                break;
            case "Water Loss Down":
                StatsManager.Instance.UpdateWaterLossStat(0.25f);
                break;
            case "Plant Health Boost":
                StatsManager.Instance.UpdateMaxHealthStat(25);
                break;

                default:
                // wenn ein skill geupgradet wird aber nicht existiert kommt diese Warnung
                    Debug.LogWarning("Unknown Skill: " + skillName);
                    break;
        }

    }
}
