using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool gameOver;
    public bool IsPaused { get; private set; }
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
        if (Input.GetKeyDown(KeyCode.Tab))
    {
        if (!IsPaused)
        {
            OpenUpgradeUI();
        }
        else
        {
            ResumeGame();
            UIManager.Instance.HideUpgradeUI();
        }

    }
    }
    public void GameOver()
    {
        Debug.Log("GameOver"); //Hier Game-Over Bildschirm
    }
    #region pause game
    public void PauseGame()
    {
        Time.timeScale = 0f;
        IsPaused = true;
		Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 
    #endregion    

    public void OpenUpgradeUI()
    {
        PauseGame();
        UIManager.Instance.ShowUpgradeUI();
    }
}
