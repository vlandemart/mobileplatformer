using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float speedX; //Чем ближе, тем выше скорость
    public float speedY; // Движение по горизонтали (на деревьях смотрится эффектно)
    public bool moveInOppositeDirection; //Почему бы и нет

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private bool previousMoveParallax;

    void Start()
    {
        GameObject gameCamera = Camera.main.gameObject;
        cameraTransform = gameCamera.transform;
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 distance = cameraTransform.position - previousCameraPosition;
        float direction = (moveInOppositeDirection) ? -1f : 1f;
        transform.position += Vector3.Scale(distance, new Vector3(speedX, speedY)) * direction;
        previousCameraPosition = cameraTransform.position;
    }
}


