using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

	private AudioSource[] audioSource;
	public AudioClip hitMarkerSound;

	private bool isSplashPlaying;

	private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponents<AudioSource>();
	}

	public void PlaySplashSound(float rate, Vector3 collisionPoint)
    {
		StartCoroutine(PlaySplashSoundCoroutine(rate, collisionPoint));
    }
    private IEnumerator PlaySplashSoundCoroutine(float rate, Vector3 collisionPoint)
	{
		if (!isSplashPlaying)
		{
			isSplashPlaying = true;
			for (int i = 0; i < audioSource.Length; i++)
			{
				if (!audioSource[i].isPlaying)
				{
					audioSource[i].clip = hitMarkerSound;
					audioSource[i].transform.position = collisionPoint;
					//audioSource[i].volume = Mathf.Lerp(0.55f,0.01f,distanceToCollisionPoint);
					audioSource[i].Play();
					yield return new WaitForSeconds(rate);
					isSplashPlaying = false;
					break;
				}
			}
		}
	}
}
