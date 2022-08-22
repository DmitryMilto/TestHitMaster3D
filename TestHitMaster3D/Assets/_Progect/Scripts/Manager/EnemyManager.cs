using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int enemyCountry = 0;

    public DictionariesEnemies enemies;

    [Button]
    public void MoveEnemy()
    {
        if (enemies.Count == 0) return;

        var name = ServiceLocator.GetService<PlayerManager>().namePlatform;

        Debug.LogError($"Enemy: {name}");

        foreach (Enemy enemy in enemies[name])
        {
            enemy.Move();
        }
    }
    //[Button]
    //private void RunEnemy()
    //{
    //    //if (enemis.Count == 0) return;
    //    //foreach (Enemy enemy in enemis)
    //    //{
    //    //    enemy.Run();
    //    //}
    //}
    //[Button]
    //private void HintEnemy()
    //{
    //    //if (enemis.Count == 0) return;
    //    //foreach (Enemy enemy in enemis)
    //    //{
    //    //    enemy.Hint().Forget();
    //    //}
    //}

    public void Add(Enemy enemy)
    {
        if (!enemies.ContainsKey(enemy.NamePlatform))
        {
            enemies.Add(enemy.NamePlatform, new List<Enemy>());
            enemies[enemy.NamePlatform].Add(enemy);
            enemyCountry++;
        }
        else
        {
            enemies[enemy.NamePlatform].Add(enemy);
            enemyCountry++;
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
