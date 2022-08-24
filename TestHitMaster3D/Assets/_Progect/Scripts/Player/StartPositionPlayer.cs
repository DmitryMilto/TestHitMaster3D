using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ServiceLocator.GetService<PlayerManager>().StartPosition(this.transform);
    }
}
