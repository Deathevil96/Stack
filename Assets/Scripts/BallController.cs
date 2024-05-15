using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public float bounceForce = 15f;  // High bounce force for quick response
    public float fallSpeed = 5f;  // Adjusted fall speed
    private bool isHolding = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, bounceForce, 0);  // Initial bounce
    }

    void Update()
    {
        isHolding = Input.GetMouseButton(0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned.");
        }
        else
        {
            rb.velocity = new Vector3(0, bounceForce, 0);  // General bounce behavior
        }

        if (collision.gameObject.CompareTag("Safe") && isHolding)
        {
            int levelIndex = Mathf.FloorToInt(collision.transform.position.y / TowerGenerator.Instance.verticalSpacing);
            TowerGenerator.Instance.DestroyLevel(levelIndex);  // Destroy all platforms at this level
            rb.velocity = new Vector3(0, -fallSpeed, 0);  // Apply downward force to simulate falling
                                                          // DestroyPlatformAndCircle(collision.transform.position.y);  // Existing method to destroy platforms
                                                          //ApplyExplosionEffect(collision.transform.position);  // New method to apply explosive force
        }
        else if (collision.gameObject.CompareTag("Danger") && isHolding)
        {
            Debug.Log("Game Over");
            //GameManager.Instance.GameOver();  // Game over if holding on a black tile
        }
    }
    /*void ApplyExplosionEffect(Vector3 position)
    {
        // Find all platforms at the same height
        GameObject[] platforms = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject platform in platforms)
        {
            if (Mathf.Abs(platform.transform.position.y - position.y) < 0.1)  // Use a small threshold to ensure precision
            {
                if (platform.GetComponent<Rigidbody>() == null)
                {
                    platform.AddComponent<Rigidbody>();  // Add Rigidbody dynamically if not already added
                }
                Rigidbody rb = platform.GetComponent<Rigidbody>();
                rb.useGravity = true;  // Make sure gravity affects the platform
                rb.AddExplosionForce(500, platform.transform.position - new Vector3(0, 0.5f, 0), 5);  // Apply an explosion force
                Destroy(platform, 2f);  // Destroy the platform after 2 seconds to allow it to clear the scene
            }
        }
    }
    void DestroyPlatformAndCircle(float platformHeight)
    {
        // Find all platforms at the same height
        GameObject[] platforms = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject platform in platforms)
        {
            if (Mathf.Abs(platform.transform.position.y - platformHeight) < 0.1)  // Use a small threshold to ensure precision
            {
                if (platform.GetComponent<Rigidbody>() == null)
                {
                    platform.AddComponent<Rigidbody>();  // Add Rigidbody dynamically if not already added
                }
                Rigidbody rb = platform.GetComponent<Rigidbody>();
                rb.useGravity = true;  // Make sure gravity affects the platform
                rb.AddExplosionForce(500, platform.transform.position - new Vector3(0, 0.5f, 0), 5);  // Apply an explosion force
                Destroy(platform, 2f);  // Destroy the platform after 2 seconds to allow it to clear the scene
            }
        }
    }*/
}


