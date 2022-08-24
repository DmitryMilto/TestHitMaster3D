using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int enemyCountry = 0;
    [SerializeField] private int enemyDead = 0;

    public DictionariesEnemies enemies;

    public Action<int, int> OnStateChange;
    public int EnemyCountry
    {
        get => enemyCountry;
        set 
        {
            enemyCountry = value; 
        }
    }
    public int EnemyDead
    {
        get => enemyDead;
        set 
        { 
            enemyDead = value;
            OnStateChange?.Invoke(enemyCountry, enemyDead);
        }
    }

    [Button]
    public void MoveEnemy()
    {
        if (enemies.Count == 0) return;

        var name = ServiceLocator.GetService<PlayerManager>().namePlatform;

        foreach (Enemy enemy in enemies[name])
        {
            enemy.Move();
        }
    }

    public void ClaerLevel()
    {
        var name = ServiceLocator.GetService<PlayerManager>().namePlatform;

        foreach (Enemy enemy in enemies[name])
        {
            enemy.Attact();
        }
        ClearDictionaries();
    }
    public void ClearDictionaries()
    {
        EnemyDead = 0;
        EnemyCountry = 0;
        enemies.Clear();
    }

    public void Add(Enemy enemy)
    {
        if (!enemies.ContainsKey(enemy.NamePlatform))
        {
            enemies.Add(enemy.NamePlatform, new List<Enemy>());
            enemies[enemy.NamePlatform].Add(enemy);
            EnemyCountry++;
        }
        else
        {
            enemies[enemy.NamePlatform].Add(enemy);
            EnemyCountry++;
        }
    }
    public bool EveryoneDiedOnPlatform()
    {
        var name = ServiceLocator.GetService<PlayerManager>().namePlatform;

        foreach (Enemy enemy in enemies[name])
        {
            if (!enemy.IsDead)
                return false;
        }
        return true;
    }
}
[Serializable]
public class DictionariesEnemies: UnitySerializedDictionary<string, List<Enemy>> { }
