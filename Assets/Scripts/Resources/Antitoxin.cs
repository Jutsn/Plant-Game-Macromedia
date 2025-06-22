using UnityEngine;

public class Antitoxin : MonoBehaviour
{
    private GameObject spawner;
    private AntitoxinSpawner spawnerScript;
    public void GetSpawner(GameObject antitoxinSpawner)
    {
        spawner = antitoxinSpawner;
        spawnerScript = spawner.GetComponent<AntitoxinSpawner>();
    }
    public void DeactivateAntitoxin()
    {
        spawnerScript.StartWaitingForRespawn();
        gameObject.SetActive(false);
    }
}
