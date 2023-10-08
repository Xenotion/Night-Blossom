using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource firstSoundtrack;
    public AudioSource secondSoundtrack;
    public Timer timer;

    private bool hasTriggered = false;

    private void Update()
    {
        //Debug.Log(timer.getIsTimerRunning());
        if (timer.getIsTimerRunning())
        {
            if (!hasTriggered)
            {
                // Stop the first soundtrack and play the second soundtrack
                firstSoundtrack.Stop();
                secondSoundtrack.Play();

                hasTriggered = true;
            }
        }
    }
}