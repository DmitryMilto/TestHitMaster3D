using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int Level = 1;

    [SerializeField] private bool _start = true;

    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private int _positionMoves = 0;
    [SerializeField] private PointMove _pointMove;

    private void Start()
    {
        _enemyManager = ServiceLocator.GetService<EnemyManager>();
        _playerManager = ServiceLocator.GetService<PlayerManager>();
    }
    [Button]
    public void StartLevel()
    {
        if (_start == false)
        {
            _enemyManager.MoveEnemy();
            _start = false;
        }
    }

    public void Add(PointMoving point)
    {
        if (!_pointMove.ContainsKey(point.name))
        {
            _pointMove.Add(point.name, point);
        }
        else
        {
            _pointMove[point.name] = point;
        }
    }

    public async UniTask NextMoving()
    {
        if (!_playerManager.player.namePlatform.Contains("Final") && _enemyManager.EveryoneDiedOnPlatform())
        {
            await _playerManager.player.Move(_pointMove[_playerManager.namePlatform].Points);
            _enemyManager.MoveEnemy();
        }

        if(_playerManager.player.namePlatform.Contains("Final"))
        {

        }
    }
}

[Serializable]
public class PointMove: UnitySerializedDictionary<string, PointMoving> { }
