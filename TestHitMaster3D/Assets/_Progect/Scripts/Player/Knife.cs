using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public class Knife : MonoBehaviour { 
    [SerializeField] private int _damage = 1;
    [SerializeField] private float speed = 2f;
    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    [Button]
    public async UniTask Move(Vector3 endPosition)
    {
        float time = Vector3.Distance(endPosition, this.transform.position) / speed;
        this.transform.DORotate(new Vector3(0,720 , 0), time, RotateMode.FastBeyond360);
        this.transform.DOMove(endPosition, time);
        await UniTask.Delay(int.Parse(time.ToString()));
    }
}
