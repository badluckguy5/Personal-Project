using UnityEngine;

public class Level3Manager : LevelManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        globalAlerted = false;

        levelIndex = 4;
        nextLevelIndex = 5;
        levelCompleted = true;

    }

    void Update()
    {


    }

}
