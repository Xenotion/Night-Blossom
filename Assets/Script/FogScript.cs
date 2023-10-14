using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // follows the player
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
    }

    public void setActive() { 
        gameObject.SetActive(true);
    }
}
