using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;

    public string namePlatform => player.NamePlatform();

    public void StartPosition(Transform transform)
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.StartIdle();
    }
    
}
