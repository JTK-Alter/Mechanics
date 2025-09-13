using UnityEngine;

public class CameraOrientation : MonoBehaviour
{
    public Transform player;
    public Transform orientation;

    private void FixedUpdate()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
    }
}
