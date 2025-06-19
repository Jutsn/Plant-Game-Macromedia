using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public ResourcesSO resources;
    public TMP_Text pointsText;
    public TMP_Text branchPointsText;
    


    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointsSpent;
        SkillSlot.OnSkillMaxed += HandleSkillMaxed;
    }

    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointsSpent;
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
    }
    private void Start()
    {
        //für jeden SkillButton wird überprüft ob der Button geklickt wurde und ob genug Skillpunkte verfügbar sind
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot));
        }
        UpdateAbilityPoints(0);
    }
    //wenn genug skillpunkte verfügbar sind wird auf das SkillSlot script verwiesen 
    // und geschaut ob für das gewuenschte Upgrade die vorraussetzungen erfuellt sind
    private void CheckAvailablePoints(SkillSlot slot)
    {
        if(resources.resource1 >= slot.skillSO.upgradeCost && resources.resource2 >= slot.skillSO.unlockBranchCost)
        {
            slot.TryUpgradeSkill();
        }
    }

    private void HandleAbilityPointsSpent(SkillSlot skillSlot)
    {
        if(resources.resource1 > skillSlot.skillSO.upgradeCost)
        {
            UpdateAbilityPoints(-skillSlot.skillSO.upgradeCost);
        }
    }
    //wird aufgerufen, wenn skill maximiert, schaltet den nächsten skill frei, wenn vorrausetzungen erfuellt
    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        Debug.Log("HandleSkillMaxed called for: " + skillSlot.skillSO.skillName);
        foreach (SkillSlot slot in skillSlots)
        {
            if (!slot.isUnlocked && slot.CanUnlockSkill())
            {
                Debug.Log("Unlocking: " + slot.skillSO.skillName);
                slot.Unlocked();
            }
        }
    }
    //updatet anzeige von skill points
    public void UpdateAbilityPoints(int amount)
    {
        resources.resource1 += amount;
        pointsText.text = "Abillity Points: " + resources.resource1;
    }

    public void UpdateBranchPoints(int amount)
    {
        resources.resource2 += amount;
        branchPointsText.text = "Branch Points: " + resources.resource2;
    }
}
