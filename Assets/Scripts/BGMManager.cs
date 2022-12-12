using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        if(m_audioSource.clip != null)
        {
            m_audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
