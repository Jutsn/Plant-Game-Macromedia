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

    private void HandleAbilityPointSpent(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;
        Debug.Log("skill gefunden");

        switch (skillName)
        {
            case "Speed Up":
                StatsManager.Instance.UpdateSpeedStat(2);
                break;

                default:
                    Debug.LogWarning("Unknown Skill: " + skillName);
                    break;
        }

    }
}
