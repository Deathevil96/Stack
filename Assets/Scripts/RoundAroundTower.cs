using UnityEngine;

public class RoundAroundTower : MonoBehaviour
{
    public GameObject towerCenter; // Assign the central tower GameObject in the inspector
    public float rotationSpeed = 50f; // Speed at which the platform rotates around the tower

    // Start is called before the first frame update
    void Start()
    {
        towerCenter = GameObject.FindWithTag("TowerCenterTag");
    }

    // Update is called once per frame
    void Update()
    {
        if (towerCenter != null)
        {
            // Rotate around the tower's position at a constant speed
            transform.RotateAround(towerCenter.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
