using UnityEngine;
using System.Collections.Generic;

public class EnemyKillTracker
{
    private readonly Dictionary<EnemyType, int> killCounts = new Dictionary<EnemyType, int>();

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

}
