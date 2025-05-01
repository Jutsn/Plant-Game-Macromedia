using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    
    public Slider plantHealthBar;
    public Slider plantWaterBar;
    public Slider waterTankBar;
    public Image plantFill;
    public Image hitMarker;
    public TextMeshProUGUI waterTankPercentage;
    
    public GameObject upgradeUIPanel;

    private bool isHitmarkerShown;


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

    private void Start()
    {
        plantFill = plantHealthBar.fillRect.GetComponent<Image>();
		hitMarker.gameObject.SetActive(false);
		isHitmarkerShown = false;
        plantHealthBar.maxValue = StatsManager.Instance.stats.plantMaxHealth;
        plantHealthBar.value = StatsManager.Instance.stats.health;
        plantWaterBar.maxValue = StatsManager.Instance.stats.maxPlantWater;
        plantWaterBar.value = StatsManager.Instance.stats.plantWater;
	}

            #region UpgradeUI
        public void ShowUpgradeUI()
        {
            upgradeUIPanel.SetActive(true);
        }

        public void HideUpgradeUI()
        {
            upgradeUIPanel.SetActive(false);
        }


            #endregion
    

    public void UpdatePlantHealthBar(int health) // funktion zum verändern des Sliders, bekommt werte von anderem Script
    {
        plantHealthBar.value = health;
    }

    public void UpdateWaterTankBar(int waterLevel, int maxWaterLevel)
    {
        waterTankBar.value = waterLevel;
        waterTankPercentage.text = ((float)waterLevel/maxWaterLevel) * 100 +"%";
    }

    public void UpdatePlantWaterBar(float plantWater)
    {
        plantWaterBar.value = plantWater;
    }

    public void ChangeHealthBarColor(Color color)
    {
        plantFill.color = color;   
    }

    public void ShowHitmarker(float rate)
    {
        StartCoroutine(ShowHitmarkerCoroutine(rate));
    }

    public IEnumerator ShowHitmarkerCoroutine(float rate)
    {
        
        if (isHitmarkerShown == false)
        {
			isHitmarkerShown = true;
			hitMarker.gameObject.SetActive(true);
			yield return new WaitForSeconds(rate);
            hitMarker.gameObject.SetActive(false);
		}
        isHitmarkerShown = false;
        
	}
}
