using UnityEngine;

public class WaterTank : MonoBehaviour
{
    public static WaterTank Instance;

    public int waterLevel;
    private int maxWaterLevel;
    
    void Awake()
    {
        Instance = this;
        maxWaterLevel = waterLevel;
    }
	private void Update()
	{
		if (waterLevel < 0)
            waterLevel = 0;
        if (waterLevel > maxWaterLevel)
            waterLevel = maxWaterLevel;

        UIManager.Instance.UpdateWaterTankBar(waterLevel, maxWaterLevel); // gibt wasserstand an UI Manager durch
	}

    public void FillTank(int tankFillRate)
    {
        waterLevel += tankFillRate;
    }
}
