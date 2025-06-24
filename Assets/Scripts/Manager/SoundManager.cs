using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

	private AudioSource[] audioSource;
	public AudioClip hitMarkerSound;

	private bool isSFXPlaying;

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

	public void PlaySFX(float rate, Vector3 placeToPlay, AudioClip audioClip)
    {
		StartCoroutine(PlaySFXCoroutine(rate, placeToPlay, audioClip));
    }
    private IEnumerator PlaySFXCoroutine(float rate, Vector3 placeToPlay, AudioClip audioClip)
	{
		if (!isSFXPlaying)
		{
			isSFXPlaying = true;
			for (int i = 0; i < audioSource.Length; i++)
			{
				if (!audioSource[i].isPlaying)
				{
					audioSource[i].clip = hitMarkerSound;
					audioSource[i].transform.position = placeToPlay;
					//audioSource[i].volume = Mathf.Lerp(0.55f,0.01f,distanceToCollisionPoint);
					audioSource[i].Play();
					yield return new WaitForSeconds(rate);
					isSFXPlaying = false;
					break;
				}
			}
		}
	}
}
