using UnityEngine;

public class Level2Manager : LevelManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        globalAlerted = false;

        levelIndex = 3;
        nextLevelIndex = 4;
        levelCompleted = true;

    }

    void Update()
    {


    }

}
