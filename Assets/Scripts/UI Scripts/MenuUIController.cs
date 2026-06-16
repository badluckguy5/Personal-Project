using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    private int startLevelIndex = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startLevelIndex);
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        startLevelIndex = GameManager.Instance.GetLevelIndex();
    }
}
