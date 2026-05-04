using UnityEngine;

public class Level1Manager : LevelManager
{
    [SerializeField] private GameObject[] buildingsToDestroy;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        globalAlerted = false;

        levelIndex = 2;
        nextLevelIndex = 3;
        levelCompleted = false;

    }

    void Update()
    {
        if (levelCompleted) return;

        bool allDestroyed = true;

        foreach (GameObject buildings in buildingsToDestroy)
        {
            if (buildings != null)
            {
                allDestroyed = false;
                break;
            }

        }

        if (allDestroyed)
        {

        levelCompleted = true;
        }

    }

}
