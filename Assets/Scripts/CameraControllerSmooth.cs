using UnityEngine;
using Player;
using System;

public class CameraControllerSmooth : MonoBehaviour
{
    public Transform target;
    public float smoothDampTime = 0.2f; //Lower values equals lower camera offset
    [HideInInspector]
    public new Transform transform;
    public Vector3 cameraOffset;
    public bool useFixedUpdate = false;

    private CharacterController2D _playerController;
    private Vector3 _smoothDampVelocity;

    private Vector2 minCameraPos;
    private Vector2 maxCameraPos;

    [SerializeField]
	private bool hasParallax;

    void Awake()
    {
        minCameraPos = LevelInit.settings.MinCameraPos;
        maxCameraPos = LevelInit.settings.MaxCameraPos;
        HasParallax = LevelInit.settings.HasParallax;

        target = LevelInit.player.GetComponent<Transform>();
        transform = gameObject.transform;
        _playerController = target.GetComponent<CharacterController2D>();
        transform.position = target.transform.position;

        CatchSky();
    }

    private void CatchSky()
    {
        var sky = GameObject.FindGameObjectWithTag("Sky");
        sky.transform.SetParent(transform);
        sky.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
    }

    void LateUpdate()
    {
        if (!useFixedUpdate)
            UpdateCameraPosition();
    }


    void FixedUpdate()
    {
        if (useFixedUpdate)
            UpdateCameraPosition();
    }


    void UpdateCameraPosition()
    {
        if (_playerController.velocity.x > 0)
        {
			float camX = Mathf.Clamp(Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime).x, minCameraPos.x, maxCameraPos.x);
			float camY = Mathf.Clamp(Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime).y, minCameraPos.y, maxCameraPos.y);

			transform.position = new Vector3 (camX, camY, -5);
            //transform.position = Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
        }
        else
        {
            var leftOffset = cameraOffset;
            leftOffset.x *= -1;

			float camX = Mathf.Clamp(Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime).x, minCameraPos.x, maxCameraPos.x);
			float camY = Mathf.Clamp(Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime).y, minCameraPos.y, maxCameraPos.y);

			transform.position = new Vector3 (camX, camY, -5);

            //transform.position = Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime);
        }
    }


    public bool HasParallax
    {
        get
        {
            return hasParallax;
        }

        set
        {
            hasParallax = value;
        }
    }


}
