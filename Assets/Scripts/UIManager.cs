using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Slider plantHealthBar;
    public Slider plantWaterBar;
    public Slider waterTankBar;
    public Image plantFill;
    public TextMeshProUGUI waterTankPercentage;


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
		Application.targetFrameRate = 60;
	}

    private void Start()
    {
        
        plantFill = plantHealthBar.fillRect.GetComponent<Image>();
    }
    

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
}
