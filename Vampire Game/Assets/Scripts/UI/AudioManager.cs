﻿using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;


    void Awake()
    {
        if (instance == null) //don't have an audiomanager in the scene
        {
            instance = this;
        }
        else //do already have an audiomanager in the scene
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject); 

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        foreach( Sound red in sounds)
        {
            if (red.clip != null)
            {
                if (red.clip.name == s.clip.name && IsPlaying(red.name))
                {
                    return;
                }
            }
        }
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.Play();
    }


    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll(string name = "")
    {
        try
        {
            Sound q = Array.Find(sounds, sound => sound.name == name);
            string omit_clipname;
            if (q != null)
                omit_clipname = q.source.clip.name;
            else
                omit_clipname = "";
            foreach (Sound s in sounds)
            {
                if (s.source.clip.name != omit_clipname)
                    s.source.Stop();
            }
        }
        catch (Exception e)
        {
            print("Something Happened");
        }
    }


    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }
}


/*
 * To play sounds (like player death or something like that):
 * Add this line of code WHEN it's supposed to play (on a collision, etc.):
 * 
 * FindObjectOfType<AudioManager>().Play("NAMEOFSOUND");
 * 
 * 
 * to stop:
 * 
 * FindObjectOfType<AudioManager>().Stop("NAMEOFSOUND");
 * 
*/