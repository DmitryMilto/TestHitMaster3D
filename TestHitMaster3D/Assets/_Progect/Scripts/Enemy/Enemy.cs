using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;

public class Enemy : MonoBehaviour, IMoving, IPaycastDown
{
    [SerializeField] private Player _player;
    [SerializeField] private float _duration = 20f;

    public void Move(Vector3 vector)
    {
        this.transform.DOMove(vector, _duration);
    }
    
    private void MoveToPlayer()
    {
        Move(_player.transform.position);
    }

    [SerializeField] private EnemyManager _enemyManager;

    public void Move()
    {
        if (!isDead)
        {
            _animator.SetTrigger("Move");
            MoveToPlayer();
        }
    }
   
    public void Attact()
    {
        this.transform?.DOKill();
        _animator.SetTrigger("Attack1");
    }
    public void Idle()
    {
        _animator.SetTrigger("Idle");
    }
    public async UniTask Hint()
    {
        _animator.SetTrigger("Hint");
        await UniTask.Delay(2);
        MoveToPlayer();
    }

    public async UniTask Dead()
    {
        _enemyManager.EnemyDead += 1;
        this.transform.DOKill();
        isDead = true;
        _animator.SetTrigger("Die");
        DeadCollider();
        await UniTask.Delay(2);
        _meshRenderer.material = _materialDead;
    }

    #region New Enemy

    [SerializeField] private Animator _animator;
    public string NamePlatform;

    [SerializeField] private Material _materialDead;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;

    [SerializeField] private int _xp = 2;
    [SerializeField] private Collider[] colliders;

    [SerializeField] private bool isDead = false;
    public bool IsDead => isDead;

    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody == null ? _rigidbody = this.GetComponent<Rigidbody>() : _rigidbody;


    public void Ray()
    {
        Ray ray = new Ray(this.transform.position, Vector3.down); // если "вниз" в мире
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            NamePlatform = hit.collider.name;
        }
    }
    private void Awake()
    {
        Ray();

        _enemyManager = ServiceLocator.GetService<EnemyManager>();
        _enemyManager.Add(this);

        _player = ServiceLocator.GetService<PlayerManager>().player;
    }

    public void SetDamage(string nameTag)
    {
        foreach(Collider collider in colliders)
        {
            if(collider.name == nameTag)
            {
                Tag(collider.name);
            }
        }
    }

    private void Tag(string tag)
    {
        switch (tag)
        {
            case "Head": Dead().Forget(); break;
            case "Body": BodyDamage(); break;
        }
    }

    private void BodyDamage()
    {
        if(_xp == 0)
        {
            Dead().Forget();
        }
        else
        {
            Hint().Forget();
            _xp -= 2;
        }
    }

    private void DeadCollider()
    {
        colliders.ForEach(x => x.enabled = false);
        Rigidbody.useGravity = false;
    }
    #endregion

    [SerializeField] private float pushPower = 2;
    public void PushAway(Vector3 pushFrom)
    {
        // Если нет прикреплённого Rigidbody2D, то выйдем из функции
        if (Rigidbody == null || pushPower == 0)
        {
            return;
        }

        // Определяем в каком направлении должен отлететь объект
        // А также нормализуем этот вектор, чтобы можно было точно указать силу "отскока"
        var vector = pushFrom - this.transform.position;
        var pushDirection = vector.normalized;

        // Толкаем объект в нужном направлении с силой pushPower
        Rigidbody.AddForce(pushDirection * pushPower);
    }
}
