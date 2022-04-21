using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    public Sound[] audioClips;
    public bool muted;

    void Awake () {
        if (inst == null) {
            inst = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var s in audioClips) {
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip   = s.clip;
            s.src.volume = s.volume;
            s.src.loop   = s.loop;
        }
    }

    void Start() {
        Play(Constants.SOUND_THEME);
        Play(Constants.SOUND_SEA);
    }

    public void Play (string name) {
        if (muted) return;
        foreach (var s in audioClips) {
            if (s.name == name) {
                s.src.Play();
                return;
            }
        }
        Debug.LogWarning("No sound with name: " + name);
    }

    public void Pause (string name) {
        foreach (var s in audioClips) {
            if (s.name == name) {
                s.src.Pause();
                return;
            }
        }
        Debug.LogWarning("No sound with name: " + name);
    }

    public void Mute (bool val) {
        muted = val;
        if (muted) {
            Pause(Constants.SOUND_THEME);
            Pause(Constants.SOUND_SEA);
        } else {
            Play(Constants.SOUND_THEME);
            Play(Constants.SOUND_SEA);
        }
    }
}
