using UnityEngine;

public class Level0Manager : LevelManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        globalAlerted = false;

        levelIndex = 1;
        nextLevelIndex = 2;
        levelCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
