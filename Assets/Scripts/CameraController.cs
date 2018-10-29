using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    private Vector2 velocity;

    [SerializeField]
    private float xOffSet = 0f;
    [SerializeField]
    private float yOffSet = 0f;
    [SerializeField]
    private float smoothTimeY = 0.05f;
    [SerializeField]
    private float smoothTimeX = 0.05f;

    void Start()
    {

    }

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX + xOffSet, posY + yOffSet, transform.position.z);
    }
}
