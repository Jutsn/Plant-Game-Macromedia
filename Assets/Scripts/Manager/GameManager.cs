using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool gameOver;
    public bool IsPaused { get; private set; }
    public int missionTimer = 0;
    public int missionTimeMax = 1200;
	public int waveLength = 150;
	public int killedEnemies = 0;

    private SpawnManager spawnManagerScript;
    

    public ResourcesSO resources;

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

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
        spawnManagerScript = GameObject.FindAnyObjectByType<SpawnManager>();
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(MissionTimerCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //Upgrade-UI
        {
            if (!IsPaused)
            {
				PauseGame();
				UIManager.Instance.ShowUpgradeUI();
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
        gameOver = true;
        GetResource3();

        Debug.Log("GameOver"); //Hier Game-Over Bildschirm
    }

    IEnumerator MissionTimerCoroutine()
    {
        while (!gameOver)
        {
			if (missionTimer < missionTimeMax)
			{
				yield return new WaitForSeconds(1);
				missionTimer += 1;

                if (missionTimer % waveLength == 0)
                {
                    spawnManagerScript.SpawnResource2();
                }

			}
            else if (missionTimer >= missionTimeMax)
			{
				missionTimer = missionTimeMax;
				GameOver();
			}
		}
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

    public void GetResource1()
    {
        resources.resource1 += 1;
    }
    public void GetResource2()
    {
        resources.resource2 += 1;
    }
    public void GetResource3()
    {
		resources.resource3 += missionTimer + killedEnemies;
	} 
    public void GetAntitoxin()
    {
		resources.antitoxin += 1;
	}
    public void LooseAntitoxin()
    {
		resources.antitoxin -= 1;
	}

    public int GetDropChanceResource1SmallEnemy()
    {
        return resources.dropChanceResource1SmallEnemy;
    }
    public int GetDropChanceResource1BigEnemy()
    {
        return resources.dropChanceResource1BigEnemy;
    }

    public int GetMinDropAmountResource2Elite()
    {
        return resources.minDropAmountResource2Elite;
    }
    public int GetMaxDropAmountResource2Elite()
    {
        return resources.maxDropAmountResource2Elite;
    }
}
