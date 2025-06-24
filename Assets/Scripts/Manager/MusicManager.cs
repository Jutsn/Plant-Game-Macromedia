using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance {  get; private set; }

	public AudioClip songA;
	public AudioClip songB;
	public float fadeDuration = 2f;

	public AudioSource sourceA;
	public AudioSource sourceB;
	private bool isPlayingA = true;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		sourceA.loop = true;
		sourceB.loop = true;

		sourceA.volume = 1f;
		sourceB.volume = 0f;
	}

	void Start()
	{
		// Starte ersten Song
		sourceA.clip = songA;
		sourceA.Play();
	}

	public void CrossfadeToB()
	{
		if (isPlayingA)
		{
			sourceB.clip = songB;
			sourceB.Play();
			StartCoroutine(Crossfade(sourceA, sourceB));
			isPlayingA = false;
		}
	}

	public void CrossfadeToA()
	{
		if (!isPlayingA)
		{
			sourceA.clip = songA;
			sourceA.Play();
			StartCoroutine(Crossfade(sourceB, sourceA));
			isPlayingA = true;
		}
	}

	private IEnumerator Crossfade(AudioSource from, AudioSource to)
	{
		float time = 0f;

		while (time < fadeDuration)
		{
			float t = time / fadeDuration;
			from.volume = Mathf.Lerp(1f, 0f, t);
			to.volume = Mathf.Lerp(0f, 1f, t);

			time += Time.deltaTime;
			yield return null;
		}

		from.volume = 0f;
		from.Stop();
		to.volume = 1f;
	}
}