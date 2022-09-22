using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraMovement : MonoBehaviour
{
    private InputControls controls;
    [SerializeField]
    private Transform player;

    private float cameraRotSpeed = 100f;
    private float xRot;
    private float yRot;
    private float yMinValue = -20f;
    private float yMaxValue = 90f;
    private float distance = 5f;
    private Vector2 rotateDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls = new InputControls();
        controls.CharacterControls.Camera.performed += RotateCamera;
        controls.CharacterControls.Camera.canceled += StopRotateCamera;
        controls.CharacterControls.Camera.Enable();

        xRot = transform.eulerAngles.x;
        yRot = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        xRot += rotateDir.x * cameraRotSpeed * Time.deltaTime;
        yRot -= rotateDir.y * cameraRotSpeed * Time.deltaTime;
        yRot = ClampAngle(yRot);

        Quaternion rotation = Quaternion.Euler(new Vector3(yRot, xRot, 0));
        Vector3 negDistance = new Vector3(0, 0, -distance);
        Vector3 pos = rotation * negDistance + player.position;

        transform.position = pos;
        transform.rotation = rotation;
    }

    float ClampAngle(float yRotation)
    {
        if (yRotation < -360f)
            yRotation += 360f;
        if (yRotation > 360f)
            yRotation -= 360f;
        return Mathf.Clamp(yRotation, yMinValue, yMaxValue);
    }

    void RotateCamera(CallbackContext ctx)
    {
        rotateDir = ctx.ReadValue<Vector2>();
    }

    void StopRotateCamera(CallbackContext ctx)
    {
        rotateDir = Vector2.zero;
    }
}
