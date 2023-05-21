
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    void FixedUpdate()
    {
        this.transform.position = new Vector3(target.position.x, this.transform.position.y, this.transform.position.z);


    }
}
