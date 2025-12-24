using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FpsMovement : MonoBehaviour
{
    [SerializeField] private Camera headCam;

    public float speed = 6.0f;
    public float gravity = -9.8f;

    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float rotationVert = 0;
    private CharacterController charController;

    void Start()
    {
        charController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Auto-find camera if not assigned
        if (headCam == null)
        {
            headCam = GetComponentInChildren<Camera>();
            if (headCam == null)
            {
                headCam = FindObjectOfType<Camera>();
            }
            if (headCam == null)
            {
                Debug.LogWarning("HeadCam not assigned and no camera found in scene!");
            }
        }
    }

    void Update()
    {
        MoveCharacter();
        RotateCharacter();
        RotateCamera();

        // Escape key to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        charController.Move(movement);
    }

    private void RotateCharacter()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
    }

    private void RotateCamera()
    {
        if (headCam == null) return;

        rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
        rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);

        headCam.transform.localEulerAngles = new Vector3(rotationVert, 0, 0);
    }
}