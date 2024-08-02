using UnityEngine.Audio;
using System;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    
    public Sounds[] sounds;
    AudioSource[] sources;

    private void Awake()
    {
        foreach (Sounds s in sounds)
        { 
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
           

            s.source.volume = s.volume;
            s.source.volume = s.pitch;
            s.source.loop = s.loop;
        }
        sources = GetComponents<AudioSource>();
    }
    private void Start()
    {
        Play("Main Theme");
        

    }
    public  void ButtonPressed()
    {
        sources[2].Stop();
        //s.source.Stop();
        Play("Dungeon Theme");



    }
    public void Play (string name)
    {
        Sounds s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null)
            return;
        s.source.Play();
        Debug.Log("sss");
    }


    
}
