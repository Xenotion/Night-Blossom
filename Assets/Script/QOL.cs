using UnityEngine;

public class QOL : MonoBehaviour
{
    public Timer timer;

    public void Start()
    {
        if (gameObject.tag == "Disappear")
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (timer.getIsTimerRunning())
        {
            gameStart();
        }
    }

    private void gameStart()
    {
        // if (gameObject.tag == "Appear")
        // {
        //     Debug.Log("Objects turned off");
        //     gameObject.SetActive(false);
        // }
        
        if (gameObject.tag == "Disappear") 
        {
            Debug.Log("Objects turned on");
            gameObject.SetActive(true);
        }
    }
}
