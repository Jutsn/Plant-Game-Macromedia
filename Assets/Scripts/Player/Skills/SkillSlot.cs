using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO; 
    public int currentLevel;
    public bool isUnlocked;
    public Image skillIcon; 
    public TMP_Text skillLevelText;
    // Funktion runs every Time a Variable in the Script gets changed
    private void OnValidate()
    {
        if(skillSO != null && skillIcon != null)
        {
            Debug.Log("hi");
            UpdateUI();
        }
    }   

    private void UpdateUI()
    {
        skillIcon.sprite = skillSO.skillIcon;    
        if(isUnlocked)
        {
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            skillIcon.color = Color.white;
        }
        else{
           skillIcon.color = Color.grey;
           skillLevelText.text = "LOCKED";
        }
    }
}
