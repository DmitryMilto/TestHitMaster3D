using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;

    public string namePlatform => player.NamePlatform();
    
}
