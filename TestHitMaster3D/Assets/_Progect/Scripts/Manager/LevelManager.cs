using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private bool _start = true;

    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private int _positionMoves = 0;
    [SerializeField] private PointMove _pointMove;

    [SerializeField] private LoaderManagerScene _loader;

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
            GameController.Instance.State = GameController.GameState.Play;
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
        var namePlatform = _playerManager.namePlatform;
        var final = _pointMove[_playerManager.namePlatform].IsFinal;
        if (_enemyManager.EveryoneDiedOnPlatform())
        {
            await _playerManager.player.Move(_pointMove[namePlatform].Points);

            if (final)
            {
                GameController.Instance.State = GameController.GameState.Win;
                _enemyManager.ClearDictionaries();
                //_loader.NextLevel();
            }
            else
                _enemyManager.MoveEnemy();
        }
    }
    public void GameOver()
    {
        GameController.Instance.State = GameController.GameState.GameOver;
        _enemyManager.ClaerLevel();
    }
}

[Serializable]
public class PointMove: UnitySerializedDictionary<string, PointMoving> { }
