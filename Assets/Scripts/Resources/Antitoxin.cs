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

	void LateUpdate()
	{
		// Kamera finden (die Main Camera muss den Tag "MainCamera" haben)
		Transform cam = Camera.main.transform;

		// Das Objekt zur Kamera rotieren
		transform.LookAt(transform.position + cam.rotation * Vector3.forward,cam.rotation * Vector3.up);
	}
}

