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
    public TMP_Text powerPoints;
    


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
        if(!GameManager.Instance.isMainMenu)
        {
            UpdateAbilityPoints(0);
            UpdateBranchPoints(0);
        }

        if (GameManager.Instance.isMainMenu)
        {
            UpdatePowerPoints(0);
        }
    }

    private void Update()
    {
        if(!GameManager.Instance.isMainMenu)
        {
            UpdateAbilityPoints(0);
            UpdateBranchPoints(0);
        }

    }
    //wenn genug skillpunkte verfügbar sind wird auf das SkillSlot script verwiesen 
    // und geschaut ob für das gewuenschte Upgrade die vorraussetzungen erfuellt sind
    private void CheckAvailablePoints(SkillSlot slot)
    {
        if (GameManager.Instance.isMainMenu)
        {
            if (resources.resource3 >= slot.skillSO.powerCost)
            {
                slot.TryUpgradeSkill();
            }
        }
        else if (!GameManager.Instance.isMainMenu)
        {
            if (resources.resource1 >= slot.skillSO.upgradeCost && resources.resource2 >= slot.skillSO.unlockBranchCost)
            {
                slot.TryUpgradeSkill();
            }
        }


    }

    private void HandleAbilityPointsSpent(SkillSlot skillSlot)
    {
        if (GameManager.Instance.isMainMenu)
        {
            if (resources.resource3 > skillSlot.skillSO.powerCost)
            {
                UpdatePowerPoints(-skillSlot.skillSO.powerCost);
            }
        }
        else if (!GameManager.Instance.isMainMenu)
        {
            if (resources.resource1 > skillSlot.skillSO.upgradeCost && resources.resource2 > skillSlot.skillSO.unlockBranchCost)
                {
                    UpdateAbilityPoints(-skillSlot.skillSO.upgradeCost);
                    UpdateBranchPoints(-skillSlot.skillSO.unlockBranchCost);
                }
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

    public void UpdatePowerPoints(int amount)
    {
        resources.resource3 += amount;
        powerPoints.text = "Power Points: " + resources.resource3;
    }
}
