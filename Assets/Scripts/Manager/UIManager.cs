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
    public Image plantHealthFill;
    public Image plantWaterFill;
	public Image waterTankFill;
	public Image hitMarker;
    public TextMeshProUGUI waterTankPercentage;
    public GameObject pauseUIPanel;
    
    public GameObject upgradeUIPanel;
    public GameObject gameOverUIPanel;

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
        plantHealthFill = plantHealthBar.fillRect.GetComponent<Image>();
		waterTankFill = waterTankBar.fillRect.GetComponent<Image>();
		hitMarker.gameObject.SetActive(false);
		isHitmarkerShown = false;
        plantHealthBar.maxValue = StatsManager.Instance.stats.plantMaxHealth;
        plantHealthBar.value = StatsManager.Instance.stats.health;
        plantWaterBar.maxValue = StatsManager.Instance.stats.plantMaxWater;
        plantWaterBar.value = StatsManager.Instance.stats.plantWater;
        waterTankBar.maxValue = StatsManager.Instance.stats.playerTankMaxWaterLevel;
        waterTankBar.value = StatsManager.Instance.stats.playerTankWaterLevel;
	}

            
    public void ShowUpgradeUI()
    {
        upgradeUIPanel.SetActive(true);
    }

    public void HideUpgradeUI()
    {
        upgradeUIPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseUIPanel.SetActive(true);
    }

    public void HidePauseMenu()
    {
		pauseUIPanel.SetActive(false);
		
	}

    public void ShowGameOverMenu()
    {
        gameOverUIPanel.SetActive(true);
    }

    public void HideGameOverMenu()
    {
        gameOverUIPanel.SetActive(false);
    }

            
    

    

    public void UpdateWaterTankBar(int waterLevel, int maxWaterLevel)
    {
        waterTankBar.value = waterLevel;
        waterTankPercentage.text = ((float)waterLevel/maxWaterLevel) * 100 +"%";
    }

	public void ChangeWaterTankBarColor(Color color)
	{
		waterTankFill.color = color;
	}

	public void UpdatePlantWaterBar(float plantWater)
	{
		plantWaterBar.value = plantWater;
	}
	public void ChangePlantWaterBarColor(Color color)
	{
		plantWaterFill.color = color;
	}
	public void UpdatePlantHealthBar(int health) // funktion zum verändern des Sliders, bekommt werte von anderem Script
	{
		plantHealthBar.value = health;
	}

    public void ChangeHealthBarColor(Color color)
    {
        plantHealthFill.color = color;   
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
