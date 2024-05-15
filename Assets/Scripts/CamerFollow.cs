using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float followSpeed = 10f; // Speed at which the camera follows the player

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
