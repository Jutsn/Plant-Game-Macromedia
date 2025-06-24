using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void CallUnpause()
    {
        GameManager.Instance.UnpauseGame();
    }
}
