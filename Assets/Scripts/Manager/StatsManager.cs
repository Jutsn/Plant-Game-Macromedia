using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public StatsSO stats;
    public TMP_Text healthText;
    [Header("Movement Stats")]
    public int jumpForce;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);   
    }

    // Update is called once per frame
    public void UpdateSpeedStat(int amount)
    {
        stats.moveSpeed += amount;
    }
}
