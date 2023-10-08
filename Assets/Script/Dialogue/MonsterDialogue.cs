using UnityEngine;

public class MonsterDialogue : MonoBehaviour
{

    public Timer timer;
    public ItemDialogue itemDialogue;
    private bool hasTriggered = false;
    private void Update()
    {
        //Debug.Log(timer.getIsTimerRunning());
        if (timer.getIsTimerRunning()&& !hasTriggered)
        {
            itemDialogue.setActive();
            hasTriggered = true;
        }

    }
}