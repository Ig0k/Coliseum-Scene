using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList<T> : MonoBehaviour where T : BaseEnemy
{
    private static List<T> _enemiesList = new List<T>();

    private void Start()
    {
        _enemiesList.Clear();
    }   

    public static void AddEnemies(T enemy)
    {
        if (!_enemiesList.Contains(enemy))
        {
            _enemiesList.Add(enemy);
        }
       
        //Debug.Log("AddEnemies Methos Called");
    }

    public static void BurnEnemies()
    {
        foreach(BaseEnemy enemy in _enemiesList)
        {
            if(enemy != null) enemy.Burn();
        }
    }

}
