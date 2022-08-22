using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointMoving : MonoBehaviour
{
    [SerializeField] private List<Transform> points;

    public List<Transform> Points => points;

    [Button]
    void AddPoint()
    {
        points = this.gameObject.GetComponentsInChildren<Transform>().ToList();
    }
    private void Awake()
    {
        ServiceLocator.GetService<LevelManager>().Add(this);
    }
}
