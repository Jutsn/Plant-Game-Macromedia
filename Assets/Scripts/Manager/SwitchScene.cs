using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
	public void LoadMainMenu()
	{
		SceneManager.LoadScene(0);
	}
	public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
	}
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
	}
        
}
