using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class SkillSlot : MonoBehaviour
{   
    public List<SkillSlot> prerequisiteSkillSlots;
    public SkillSO skillSO; 
    public int currentLevel;
    public bool isUnlocked;
    public Image skillIcon; 
    public Button skillButton;
    public TMP_Text skillLevelText;
    //events nutzen, um zu verhindern, dass scripts zu abhänging voneinander sind
    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;
    // Funktion runs every Time a Variable in the Script gets changed
    private void OnValidate()
    {
        if(skillSO != null && skillLevelText != null)
        {
            UpdateUI();
        }
    }   

    public void TryUpgradeSkill()
    {
        if(isUnlocked && currentLevel < skillSO.maxLevel)
        {
            currentLevel ++;
            //Nachricht von diesem SkillSlot wird an alle events die "subscribed" sind uns zuhören
            // Das fragezeichen versichert, dass das event existiert
            OnAbilityPointSpent?.Invoke(this);   
            if(currentLevel >= skillSO.maxLevel)    
            {
                OnSkillMaxed?.Invoke(this);
            }     
            UpdateUI();

        }
    }
    // überprüft für jeden Skill ob die Vorrausgesetzen Skills freigeschaltet sind
    public bool CanUnlockSkill()
    {
        foreach(SkillSlot slot in prerequisiteSkillSlots)
        {
            if(!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;
    }
    //wenn ein skill freigeschalten wird, dann wird die UI von dem Skill geupdatet 
    public void Unlocked()
    {
        isUnlocked = true;
        UpdateUI();
    }
    //Farbe vom skill Icon wird geupdatet eventuell in UIManager verschieben
    private void UpdateUI()
    {
        skillIcon.sprite = skillSO.skillIcon;    
        if(isUnlocked)
        {
            skillButton.interactable = true;
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            skillIcon.color = Color.white;
        }
        else{
            skillButton.interactable = false;
           skillIcon.color = Color.grey;
           skillLevelText.text = "LOCKED";
        }
    }
}
