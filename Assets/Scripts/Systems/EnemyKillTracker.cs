using UnityEngine;
using System.Collections.Generic;

public class EnemyKillTracker
{
    private Dictionary<EnemyType, int> killCounts = new Dictionary<EnemyType, int>();

    public int IncrementKillCount(EnemyType type)
    {
        killCounts.TryGetValue(type, out int count);
        count++;
        killCounts[type] = count;
        return count;
    }

    public int GetKillCount(EnemyType type)
    {
        return killCounts.TryGetValue(type, out int count) ? count : 0;
    }

    //Save functions
    public Dictionary<EnemyType, int> GetKillCounts()
    {
        return killCounts;
    }

    public void SetKillCounts(Dictionary<EnemyType, int> loadedCounts)
    {
        killCounts = loadedCounts;
    }

}
