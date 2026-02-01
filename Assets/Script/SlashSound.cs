using UnityEngine;

public class SlashSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip slashsoundClip;

    public void PlaySlash()
    {
        if (audioSource == null || slashsoundClip == null) return;
        audioSource.PlayOneShot(slashsoundClip);
    }
}
