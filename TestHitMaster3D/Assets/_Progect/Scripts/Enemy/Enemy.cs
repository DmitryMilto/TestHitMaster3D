using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Enemy : MonoBehaviour, IMoving, IPaycastDown
{
    public float force = 20f;
    
    [SerializeField] private Player _player;
    [SerializeField] private float _duration = 20f;

    public void Move(Vector3 vector)
    {
        this.transform.DOMove(vector, _duration);
    }
    [Button]
    private void Damage()
    {
        Rigidbody.AddForce(transform.forward * force);
    }
    [Button]
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
   
    public void Run()
    {
        _animator.SetTrigger("Run");
    }
    public void Idle()
    {
        _animator.SetTrigger("Idle");
        this.transform.DOKill();
    }
    public async UniTask Hint()
    {
        Idle();
        await UniTask.Delay(2);
        _animator.SetTrigger("Hint");
        await UniTask.Delay(2);
        MoveToPlayer();
    }

    public async UniTask Died()
    {
        this.transform.DOKill();
        isDead = true;
        _animator.SetTrigger("Die");
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
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
            Debug.Log(NamePlatform);
        }
    }
    private void Awake()
    {
        Ray();

        _enemyManager = ServiceLocator.GetService<EnemyManager>();
        _enemyManager.Add(this);

        _player = ServiceLocator.GetService<PlayerManager>().player;
    }
    #endregion
}
