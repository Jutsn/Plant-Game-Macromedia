using UnityEngine;

public class WaterTank : MonoBehaviour
{
    public static WaterTank Instance;

    public int waterLevel;
    void Awake()
    {
        Instance = this;
    }
	private void Update()
	{
		if (waterLevel < 0)
            waterLevel = 0;
	}
}
