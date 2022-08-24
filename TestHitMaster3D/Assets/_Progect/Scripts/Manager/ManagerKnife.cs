using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using System;

public class ManagerKnife : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    [SerializeField] private GameObject _knife;

    [SerializeField] private LevelManager level;
    [SerializeField] private bool isStart;
    [SerializeField] private bool isThrowKnife = true;

    [SerializeField] private GameObject particle;

    private void Start()
    {
        GameController.Instance.OnStateChange += UpdateKnife;
        level = ServiceLocator.GetService<LevelManager>();
    }

    private void UpdateKnife(GameController.GameState state)
    {
        isStart = state == GameController.GameState.Play || state == GameController.GameState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && isStart && isThrowKnife)
        {
            ThrowKnife();
        }
    }
    private async UniTask ThrowKnife()
    {
        isThrowKnife = false;
        level.StartLevel();
        //Debug.Log("Good");
        var camera = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camera, out RaycastHit hit))
        {
            //Debug.LogError(hit.point);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponentInParent<Enemy>())
                {
                    Dead(hit);
                    await CreateParticle(hit);
                }
                else
                    OnClick(hit, hit.collider.gameObject).Forget();
            }
        }
        await level.NextMoving();
        isThrowKnife = true;
    }

    [Button]
    private async UniTask OnClick(RaycastHit point, GameObject game)
    {
        //var objects = Instantiate(this._knife, _player.player.PositionStart.position, Quaternion.LookRotation(point.normal), game.transform);
        //if (!objects.TryGetComponent<Knife>(out Knife _knife))
        //{
        //    _knife = objects.AddComponent<Knife>();
        //}
        //await _knife.Move(point.point);
    }
    private async UniTask CreateParticle(RaycastHit point)
    {
        var objects = Instantiate(this.particle, point.point, Quaternion.LookRotation(point.normal));
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Destroy(objects);
    }
    private void AddForge(RaycastHit hit)
    {
        hit.collider.GetComponent<Enemy>().Dead().Forget();
        hit.rigidbody.AddForce(-hit.normal * 200);
    }
    private void Dead(RaycastHit hit)
    {
        var enemy = hit.collider.GetComponentInParent<Enemy>();
        enemy.SetDamage(hit.collider.name);
    }
}
