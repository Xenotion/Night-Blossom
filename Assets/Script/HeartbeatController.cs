using UnityEngine;

public class HeartbeatController : MonoBehaviour
{
    public Transform player;
    public Transform monster;
    public float maxDistance = 50f; // The maximum distance at which the heartbeat is at its slowest

    private AudioSource heartbeatAudioSource;

    void Start()
    {
        heartbeatAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distanceToMonster = Vector3.Distance(player.position, monster.position);
        Debug.Log("Distance to monster: " + distanceToMonster); // For debugging

        // Ensure the distance is within the range you want to start affecting the heartbeat
        if (distanceToMonster < maxDistance)
        {
            // Calculate a pitch value based on the distance to the monster.
            // As the monster gets closer, the pitch should increase, causing the heartbeat to speed up.
            // The closer the monster, the higher the pitch, up to a maximum of 2, for example.
            float pitch = Mathf.Lerp(1f, 2f, 1f - (distanceToMonster / maxDistance));
            heartbeatAudioSource.pitch = pitch;

            // Optionally, you could also increase the volume as the monster gets closer
            heartbeatAudioSource.volume = Mathf.Lerp(0.5f, 1f, 1f - (distanceToMonster / maxDistance));

            if (!heartbeatAudioSource.isPlaying)
            {
                heartbeatAudioSource.Play();
            }
        }
        else if (heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
        }
    }
}
