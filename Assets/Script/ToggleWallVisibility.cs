using UnityEngine;

public class ToggleWallVisibility : MonoBehaviour
{
    public GameObject wallA;
    public GameObject wallB;
    public Timer timer;
    
    private bool hasSwapped = false;

    private void Start()
    {
        SetInitialState();
    }

    private void Update()
    {
        SwapBasedOnTimer();
    }

    private void SetInitialState()
    {
        wallA.SetActive(true);
        wallB.SetActive(false);
    }

    private void SwapBasedOnTimer()
    {
        if (timer.getIsTimerRunning() && !hasSwapped)
        {
            wallA.SetActive(!wallA.activeSelf);
            wallB.SetActive(!wallB.activeSelf);
            hasSwapped = true;
        }
    }
}
