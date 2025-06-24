using NUnit.Framework.Internal;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool gameOver;
    public bool wonGame;
    public bool IsPaused;
    public int missionTimer = 0;
    public int missionTimeMax = 1200;
    public int waveLength = 150;
    public int timeBetweenWaves = 1;
    public bool waveActive;
    public int killedEnemies = 0;
    public bool isMainMenu = false;

    private bool pauseMenu;
    private bool skillMenu;

    private SpawnManager spawnManagerScript;

    public static Action GameOverEvent;


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
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.HidePauseMenu();
        UIManager.Instance.HideGameOverMenu();
        UIManager.Instance.HideWinMenu();
        pauseMenu = false;
        skillMenu = false;
        wonGame = false;
        resources.resource1 = 0;
        resources.resource2 = 0;
        resources.antitoxin = 0;
        missionTimer = 0;
        killedEnemies = 0;

        if (scene.buildIndex == 0) //MainMenu
        {
            isMainMenu = true;
            gameOver = true;
        }

        if (scene.buildIndex == 1) //Level 1
        {
            isMainMenu = false;
            gameOver = false;
            waveActive = true;
            spawnManagerScript = FindAnyObjectByType<SpawnManager>();
            StartCoroutine(MissionTimerCoroutine());
        }


    }

    void Update()
    {
        CheckPauseInput();
    }
    public void GameOver()
    {
        gameOver = true;
        GameOverEvent.Invoke();
        if (wonGame)
        {
			UIManager.Instance.ShowWinMenu();
		}
        else if (!wonGame)
        {
			UIManager.Instance.ShowGameOverMenu();
		}   
        GetResource3();

        Debug.Log("GameOver"); //Hier Game-Over Bildschirm
    }

    IEnumerator MissionTimerCoroutine()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(1);
            if (missionTimer < missionTimeMax && waveActive)
            {
                missionTimer += 1;

                if (missionTimer % waveLength == 0) //Welle beenden
                {
                    spawnManagerScript.SpawnResource2();
                    spawnManagerScript.SpawnEliteEnemy(); //Spawn Elite
                    waveActive = false;
                    spawnManagerScript.newWave = true;
                }
                UIManager.Instance.RefreshGameTimer(missionTimer);
            }
            else if (missionTimer >= missionTimeMax)
            {
                missionTimer = missionTimeMax;
				UIManager.Instance.RefreshGameTimer(missionTimer);
				wonGame = true;
                GameOver();
            }
        }
    }

    public IEnumerator SetWaveActiveAgainCoroutine() //After Killed Elite
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        waveActive = true;
    }

    #region pause game

    public void CheckPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //Upgrade-UI
        {
            if (!IsPaused && !gameOver && !pauseMenu)
            {
                skillMenu = true;
                IsPaused = true;
                PauseGame();
                UIManager.Instance.ShowUpgradeUI();
            }
            else if (IsPaused && !gameOver && !pauseMenu)
            {
                ResumeGame();
                UIManager.Instance.HideUpgradeUI();
                skillMenu = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused && !gameOver && !skillMenu)
            {
                pauseMenu = true;
                IsPaused = true;
                PauseGame();
                UIManager.Instance.ShowPauseMenu();
            }
            else if (IsPaused && !gameOver && !skillMenu)
            {
                UnpauseGame();
            }
        }
    }
    public void UnpauseGame()
    {

        ResumeGame();
        UIManager.Instance.HidePauseMenu();
        pauseMenu = false;

    }
    public void PauseGame()
    {

        Time.timeScale = 0f;
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

    #region ResourceFunctions
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
        UIManager.Instance.RefreshAntidotDisplay(resources.antitoxin);
    }
    public void LooseAntitoxin()
    {
        resources.antitoxin -= 1;
		UIManager.Instance.RefreshAntidotDisplay(resources.antitoxin);
	}

    #endregion ResourceFunctions

    #region DropchanceFunctions
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
    #endregion DropchanceFunctions
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
