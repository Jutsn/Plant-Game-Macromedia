using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool gameOver;

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
        
    }
    public void GameOver()
    {
        Debug.Log("GameOver"); //Hier Game-Over Bildschirm
    }
}
