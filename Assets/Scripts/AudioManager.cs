using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource m_audioSource;

    [SerializeField] AudioClip[] m_sounds;

    // Start is called before the first frame update
    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(int soundID)
    {
        m_audioSource.clip = m_sounds[soundID];
        m_audioSource.Play();
    }
}
