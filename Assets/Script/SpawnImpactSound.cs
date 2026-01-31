using UnityEngine;

public class SpawnImpactSound : MonoBehaviour
{
    public AudioClip impactSound;

    void Start()
    {
        if (impactSound == null) return;

        AudioSource.PlayClipAtPoint(
            impactSound,
            transform.position,
            1f
        );
    }
}
