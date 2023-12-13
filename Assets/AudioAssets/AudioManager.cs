using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;


	void Awake()
	{
		instance= this;
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

		}
	}
    private void Start()
    {
		//Play("Ambience");
		Play("BG Music");
    }
    private void Update()
    {
  //      if(CheckIfPlaying("Planet Generate"))
		//{
		//	StartCoroutine(StartFade("BG Music", 1f, 0));
		//}
		//else
		//{
		//	if(!CheckIfPlaying("BG Music")) { StartCoroutine(StartFade("BG Music", 0.5f, 0.5f)); }
		//}
    }
	public IEnumerator StartFade(string sound, float fadeTime, float targetVol)
	{
		Sound s = FindSound(sound);
		float currentTime = 0;
		float startVol = s.volume;
		while (currentTime < fadeTime)
		{
			currentTime += Time.deltaTime;
			s.volume = Mathf.Lerp(startVol, targetVol, currentTime/fadeTime);
			yield return null;
		}
		//if(s.volume >= 0) { Stop(sound); }
		yield break;
	}
    private Sound FindSound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found");
		}
		return s;
	}
    public void Play(string sound)
	{
		Sound s = FindSound(sound);

		s.source.Play();
	}
	public bool CheckIfPlaying(string sound)
	{
        Sound s = FindSound(sound);

        if (!s.source.isPlaying)
		{
			return false;
		}
		return true;
	}
	public void Stop(string sound)
	{
        Sound s = FindSound(sound);

        s.source.Stop();
    }

}
