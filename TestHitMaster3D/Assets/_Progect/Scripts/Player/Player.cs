using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using System;

public class Player : MonoBehaviour
{
    public string namePlatform;
    public int speed;

    [SerializeField] private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public string Ray()
    {
        Ray ray = new Ray(this.transform.position, Vector3.down); // если "вниз" в мире
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
           return hit.collider.name;
        }
        return null;
    }
    public string NamePlatform() => namePlatform = Ray();

    public void MovingPoint(List<Transform> points)
    {
        Move(points).Forget();
    }

    public async UniTask Move(List<Transform> points, float time = 1f)
    {
        _rigidbody.useGravity = false;
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        foreach (var point in points)
        {
            await transform.DOMove(point.position, time).AsyncWaitForCompletion();
        }
        _rigidbody.useGravity = true;
    }
}
