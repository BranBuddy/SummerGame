using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor.ShaderGraph.Internal;
using Unity.Cinemachine;

// Script Made By: Istvan Wallace
// Last Edited: change controls - 5/27/25
public class NorthernHemisphericCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera CinemachineCamera;

    [Header("Camera Controls")]
    [SerializeField] float moveSpeed = 50f; // Controls how fast camera moves
    [SerializeField] float rotateSpeed = 100f; // Controls how fast camera rotates
    private bool rightClickDown = false; // Controls right-click rotations

    [Header("Drag Pan Controls")]
    public float dragPanSpeed = 2f;

    [SerializeField] private bool useDragPan = false; // Enable or disable drag pan
    private bool dragPanMoveActive = false; // Determines if click and drag pan is active
    private Vector2 lastMousePosition; // Stores last mouse position

    [Header("Edge Scrolling")]
    [SerializeField] bool useEdgeScrolling; // Enable or disable edge scrolling
    [SerializeField] int edgeScrollSize = 20; // Controls the distance away from screen edge to activate edge scroll

    [Header("Zoom")]
    public float zoomSpeed = 10f;
    private float targetFieldOfView = 50f;
    [SerializeField] private float fieldOfViewMax = 50;
    [SerializeField] private float fieldOfViewMin = 10;

    private void Update()
    {
        HandleCameraMovement(); // Call Camera Movement function

        if (useEdgeScrolling)
        {
            HandleCameraMovementEdgeScrolling();
        }
        if (useDragPan) // If useEdgeScrolling is true apply
        {
            HandleCameraMovementDragPan();
        }
        HandleCameraRotation(); // Call Camera Rotation function
        HandleCameraZoom(); // Call Camera Zoom function
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        // Key based movements:
        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f; // Controls positive z axis movements
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f; // Controls negative z axis movements
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f; // Controls negative x axis movements
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f; // Controls positive x axis movements

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x; // Enables proper movement based on the camera view instead of global orientation
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        // Edge scroll based movements
        if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f; // Controls negative x axis movements
        if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f; // Controls negative z axis movements
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f; // Controls positive x axis movements
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1f; // Controls positive z axis movements

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x; // Enables proper movement based on the camera view instead of global orientation
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovementDragPan()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(0)) // On left click press start pan
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
            Cursor.visible = false; // Hides the cursor
        }
        if (Input.GetMouseButtonUp(0)) // On left click release stop pan
        {
            dragPanMoveActive = false;
            Cursor.visible = true; // Shows the cursor again
        }
        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition; // Subtracts last mouse position from current mouse position

            inputDir.x = -mouseMovementDelta.x * dragPanSpeed; // Controls the drag pan on the x axis
            inputDir.z = -mouseMovementDelta.y * dragPanSpeed; // Controls the drag pan on the y axis

            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x; // Enables proper movement based on the camera view instead of global orientation
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation()
    {
        float rotateDir = 0f; // sets value of rotate direction
        // Key Rotations:
        // if (Input.GetKey(KeyCode.Q)) rotateDir = +1f; // Controls clockwise camera rotations 
        // if (Input.GetKey(KeyCode.E)) rotateDir = -1f; // Control counter-clockwise camera rotations

        // Mouse Rotations
        if (Input.GetMouseButtonDown(2)) // On mouse right-click down
        {
            rightClickDown = true;
            // Debug.Log("Right-Click pressed!");
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
            Cursor.visible = false; // Hides the cursor
        }
        if (Input.GetMouseButtonUp(2)) // On mouse right-click release
        {
            rightClickDown = false;
            Cursor.lockState = CursorLockMode.None; // Frees the cursor
            Cursor.visible = true; // Shows the cursor again
        }
        if (rightClickDown) // If true execute:
        {
            float mouseX = Input.GetAxis("Mouse X"); // Gets mouse 'x' value
            transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime); // Roatate based on mouse.x value
        }
            


        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0); // Transforms camera rotation
    }

    private void HandleCameraZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += 5f;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax); // Clamps targetFieldOfView between min and max values

        CinemachineCamera.Lens.FieldOfView = // Sets the CinemachineCamera FOV to the target FOV
            Mathf.Lerp(CinemachineCamera.Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed); // Adds smoothing to zoom

    }
}
