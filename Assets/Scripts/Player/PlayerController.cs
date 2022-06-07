using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Test 1, 2, 3.
    [Header("Main Settings")]
    [Space]

    [SerializeField] private float MouseSensitivity = 3.5f;
    [SerializeField] private float WalkSpeed = 6f;
    [SerializeField] private float Gravity = -9.80665f;
    [SerializeField] private Transform Cam = null;

    [Header("Smooth Times")]
    [Space]

    [SerializeField][Range(0.0f, 0.5f)] private float MoveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.05f)] private float MouseSmoothTime = 0.03f;

    float CameraPitch = 0.0f;
    float VelocityY = 0.0f;
    CharacterController Controller = null;


    Vector2 CurrentDirection = Vector2.zero;
    Vector2 CurrentDirectionVelocity = Vector2.zero;

    Vector2 CurrentMouseDelta = Vector2.zero;
    Vector2 CurrentMouseDeltaVelocity = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        // Locking the Cursor To The Center Of the Screen.
        Cursor.lockState = CursorLockMode.Locked;

        // Hiding The Cursor.
        Cursor.visible = false;

        // Setting The "Controller" Variable To The Character Controller On The Player.
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateMouseLook();
        UpdateMovement();
        Sprint();
    }


    void UpdateMouseLook()
    {
        // Defining Both Our Mouse X And Mouse Y In A Vector 2 (Up And Down) Variable.
        Vector2 TargetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Smoothing Our Camera Down.
        CurrentMouseDelta = Vector2.SmoothDamp(CurrentMouseDelta, TargetMouseDelta, ref CurrentMouseDeltaVelocity, MouseSmoothTime);

        // Setting Our Camera Pitch To Be Eqaul To Our Mouse Y Axis.
        CameraPitch -= TargetMouseDelta.y * MouseSensitivity;

        // Clamping The Camera Angle So We Can't Look Behind Us.
        CameraPitch = Mathf.Clamp(CameraPitch, -90.0f, 90.0f);

        // Rotating Our Camera Only On The Y Axis According To the Camera Pitch.
        Cam.localEulerAngles = Vector3.right * CameraPitch;

        // Rotating The Player According To Our Mouse X Axis.
        transform.Rotate(Vector3.up * TargetMouseDelta.x * MouseSensitivity);
    }

    void UpdateMovement()
    {
        // Defining Both Our Forward And Back Input (Horizontal), And Left And Right Input (Vertical).
        Vector2 TargetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalizing The Input To A Maxinum Of 1.
        TargetDirection.Normalize();

        // Smoothing Down the Movement.
        CurrentDirection = Vector2.SmoothDamp(CurrentDirection, TargetDirection, ref CurrentDirectionVelocity, MoveSmoothTime);

        // If We Are Touching The Ground, Then The Velocity Of Our Y Axis Would Be 0.
        if (Controller.isGrounded)
        {
            VelocityY = 0.0f;
        }

        // Increase The Velocity Of Our Character According To Gravity;
        VelocityY += Gravity * Time.deltaTime;

        // Defining the Velocity Of Our Character Depending On The Direction And Speed
        Vector3 Velocity = (transform.forward * CurrentDirection.y + transform.right * CurrentDirection.x) * WalkSpeed + Vector3.up * VelocityY;

        // Moving Our Character Depending On The Velocity And Multiply That Over Time.
        Controller.Move(Velocity * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            WalkSpeed = 10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            WalkSpeed = 6f;
        }
    }
}
