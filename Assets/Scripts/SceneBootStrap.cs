using UnityEngine;

public class SceneBootStrap : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverScreen;
    public FloatingMessageSpawner messageSpawner;


    void Start()
    {
        GameManager.Instance.BindSceneReferences(
            player, gameOverScreen, messageSpawner );
    }

}
