using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance { get; private set; }
    public GameObject upgradeUIPanel;

	private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

            
    public void ShowUpgradeUI()
    {
        upgradeUIPanel.SetActive(true);
    }

    public void HideUpgradeUI()
    {
        upgradeUIPanel.SetActive(false);
    }
}
