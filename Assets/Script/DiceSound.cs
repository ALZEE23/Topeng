using UnityEngine;

public class DiceSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shuffleClip;

    public void PlayShuffle()
    {
        if (audioSource == null || shuffleClip == null) return;

        audioSource.clip = shuffleClip;
        audioSource.Play();
    }
}
