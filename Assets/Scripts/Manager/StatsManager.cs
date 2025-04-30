using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public TMP_Text healthText;
    [Header("Movement Stats")]
    public int movementSpeed;
    public int jumpForce;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);   
    }

    // Update is called once per frame
    void UpdateSpeedStat(int amount)
    {
        movementSpeed += amount;
    }
}
