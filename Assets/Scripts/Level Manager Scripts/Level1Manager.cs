using UnityEngine;

public class Level1Manager : LevelManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        globalAlerted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
