using System.Collections;
using UnityEngine;

public class AntitoxinSpawner : MonoBehaviour
{
    GameObject antidot;
    [SerializeField]private float respawnTime = 3;

	void Start()
    {
        StartCoroutine(RespawnTimer());
    }

    void SpawnAntitoxin()
    {
		antidot = AntitoxinPool.Instance.GetPooledObject();
		antidot.transform.position = transform.position;
		antidot.transform.rotation = Quaternion.identity;
		antidot.SetActive(true);
		antidot.GetComponent<Antitoxin>().GetSpawner(gameObject);

	}
    public void StartWaitingForRespawn()
    {
		StartCoroutine(RespawnTimer());
	}

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnAntitoxin();

    }
}
