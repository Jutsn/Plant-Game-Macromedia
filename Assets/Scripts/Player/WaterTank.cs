using UnityEngine;

public class WaterTank : MonoBehaviour
{
    public static WaterTank Instance;

    public int playerTankMaxWaterLevel;
	public int playerTankWaterLevel;

	void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
        
    }
	void OnEnable()
	{
		StatsManager.OnStatsChanged += RefreshStats;
	}

	void OnDisable()
	{
		StatsManager.OnStatsChanged += RefreshStats;
	}

	private void Start()
	{
		playerTankMaxWaterLevel = StatsManager.Instance.stats.playerTankMaxWaterLevel;
		playerTankWaterLevel = StatsManager.Instance.stats.playerTankWaterLevel;
	}

	void RefreshStats(StatsManager stats)
	{
		playerTankMaxWaterLevel = StatsManager.Instance.stats.playerTankMaxWaterLevel;
		playerTankWaterLevel = StatsManager.Instance.stats.playerTankWaterLevel;
		
	}
	private void Update()
	{
		if (playerTankWaterLevel < 0)
            playerTankWaterLevel = 0;
        if (playerTankWaterLevel > playerTankMaxWaterLevel)
            playerTankWaterLevel = playerTankMaxWaterLevel;
		
        UIManager.Instance.UpdateWaterTankBar(playerTankWaterLevel, playerTankMaxWaterLevel); // gibt wasserstand an UI Manager durch
	}

    public void FillTank(int tankFillRate)
    {
        playerTankWaterLevel += tankFillRate;
		StatsManager.Instance.SetPlayerTankWater(playerTankWaterLevel);
	}

	public void UseTankWater (int waterAmount)
	{
		playerTankWaterLevel -= waterAmount;
		StatsManager.Instance.SetPlayerTankWater(playerTankWaterLevel);
	}
}
