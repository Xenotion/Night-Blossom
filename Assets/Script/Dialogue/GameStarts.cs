using UnityEngine;

public class MonsterDialogue : MonoBehaviour
{

    public Timer timer;
    public ItemDialogue itemDialogue;
    private bool hasTriggered = false;
    public FogScript fog;
    private EnemyAI enemy;


    private void Start()
    {
        
        enemy = FindObjectOfType<EnemyAI>();
    }
    
    private void Update()
    {
        //Debug.Log(timer.getIsTimerRunning());
        if (timer.getIsTimerRunning()&& !hasTriggered)
        {
            itemDialogue.setActive();
            fog.setActive();
            enemy.Spawn();
            hasTriggered = true;
            
        }

    }
}