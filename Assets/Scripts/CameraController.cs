using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Camera Settings")]
    [SerializeField] private float cameraLookSpeed = 2f;
    [SerializeField] private float cameraLookXLimit = 80f;
    [SerializeField] private bool canMoveCamera = true;
    [SerializeField] private Transform playerCamera;

    private PlayerController playerController;
    private Vector3 originalPosition;
    private Vector3 crouchPosition;
    private float rotationX = 0;

    private void Start() {
        playerController = GetComponent<PlayerController>();

        originalPosition = playerCamera.localPosition;
        crouchPosition = new Vector3(originalPosition.x, originalPosition.y - 0.5f, originalPosition.z);
    }

    private void Update() {
        if (canMoveCamera) {
            CameraMovement();
        }
    }

    private void CameraMovement() {
        rotationX -= Input.GetAxis("Mouse Y") * cameraLookSpeed;
        rotationX = Mathf.Clamp(rotationX, -cameraLookXLimit, cameraLookXLimit);

        if (playerController.IsCrouching) {
            playerCamera.localPosition = crouchPosition;
        } else {
            playerCamera.localPosition = originalPosition;
        }

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * cameraLookSpeed, 0);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

}
