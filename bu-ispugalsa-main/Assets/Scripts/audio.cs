using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;

    private bool activeAudio = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (!activeAudio &&  other.GetComponent<PlayerMovement>())
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips. Length)];
            audioSource.Play();
            StartCoroutine(routine: Timer ());
        }
    }

    private IEnumerator Timer()
    {
        activeAudio = true;
        yield return new WaitForSeconds(10);
        activeAudio = false;
    }
}
