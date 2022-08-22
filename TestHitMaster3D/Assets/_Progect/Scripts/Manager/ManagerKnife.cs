using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Tools;
using Cysharp.Threading.Tasks;

public class ManagerKnife : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    [SerializeField] private GameObject _knife;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var level = ServiceLocator.GetService<LevelManager>();
            level.StartLevel();
            //Debug.Log("Good");
            var camera = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camera, out RaycastHit hit))
            {
                //Debug.LogError(hit.point);
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<Enemy>())
                        Dead(hit);
                    else
                        OnClick(hit, hit.collider.gameObject).Forget();

                    level.NextMoving();
                }
            }
        }
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
    private void AddForge(RaycastHit hit)
    {
        hit.collider.GetComponent<Enemy>().Died().Forget();
        hit.rigidbody.AddForce(-hit.normal * 200);
    }
    private void Dead(RaycastHit hit)
    {
        var enemy = hit.collider.GetComponent<Enemy>();
        enemy.Rigidbody.isKinematic = true;
        enemy.Died().Forget(); 
    }
}
