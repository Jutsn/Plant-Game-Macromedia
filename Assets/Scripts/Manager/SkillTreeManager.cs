using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;
    public int availablePoints;

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
        if(availablePoints > 0)
        {
            slot.TryUpgradeSkill();
        }
    }

    private void HandleAbilityPointsSpent(SkillSlot skillSlot)
    {
        if(availablePoints > 0)
        {
            UpdateAbilityPoints(-1);
        }
    }
    //wird aufgerufen, wenn skill maximiert, schaltet den nächsten skill frei, wenn vorrausetzungen erfuellt
    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        foreach (SkillSlot slot in skillSlots)
        {
            if(!slot.isUnlocked && slot.CanUnlockSkill())
            {
                slot.Unlocked();
            }
            
        }
    }
    //updatet anzeige von skill points
    public void UpdateAbilityPoints(int amount)
    {
        availablePoints += amount;
        pointsText.text = "Points " + availablePoints;
    }
}
