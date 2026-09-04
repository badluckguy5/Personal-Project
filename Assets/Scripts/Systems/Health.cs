using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image healthSlider;
    public float maxHealth;

    public Transform target; // The character this bar follows
    private Vector3 offset = new Vector3(0, 10f, 0); // Position offset above head

    private Camera cam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        offset = new Vector3(-0.5f, 3f, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Offset in world space only (no camera-relative math needed)
            transform.position = target.position + offset;

            // Billboard toward camera
            transform.forward = cam.transform.forward;
        }
    }

    public void SetHealth(float normalizedHealth)
    {
        healthSlider.fillAmount = Mathf.Clamp01(normalizedHealth);
    }

}
