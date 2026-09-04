using UnityEngine;

public class SceneBootStrap : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverScreen;
    public FloatingMessageSpawner messageSpawner;

    //Used by Level Manager
    void Start()
    {
        GameManager.Instance.BindSceneReferences(
            player, gameOverScreen, messageSpawner );

        GameManager.Instance.TryAllUpgradesOnLoad();
    }

}
