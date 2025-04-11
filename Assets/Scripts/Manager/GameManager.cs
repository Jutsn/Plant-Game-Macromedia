using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool gameOver;
    public float missionTimer = 120f;

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
	void Start()
    {
        
    }

    void Update()
    {
        missionTimer -= Time.deltaTime;
    }
    public void GameOver()
    {
        Debug.Log("GameOver"); //Hier Game-Over Bildschirm
    }
}
