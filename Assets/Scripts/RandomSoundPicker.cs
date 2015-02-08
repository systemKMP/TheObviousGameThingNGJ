using UnityEngine;
using System.Collections;

public class RandomSoundPicker : MonoBehaviour
{

    public AudioSource source;
    public AudioClip[] clips = new AudioClip[0];

    public void Start()
    {
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
        Destroy(this);
    }
}
