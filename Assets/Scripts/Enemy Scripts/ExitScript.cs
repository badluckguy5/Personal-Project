using UnityEngine;

public class ExitScript : MonoBehaviour
{

    private FortController fort;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fort = GetComponentInParent<FortController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fort.PlayerExitFort();

        }
    }
}
