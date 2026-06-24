using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 10f;

    [Header("Lane Movement")]
    [SerializeField] private float laneDistance = 4f;
    [SerializeField] private float laneChangeSpeed = 12f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;

    [Header("Slide Settings")]
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private float slideHeight = 1f;

    private bool isSliding;
    private float slideTimer;

    private float originalHeight;
    private Vector3 originalCenter;

    private CharacterController controller;

    private int currentLane = 1;

    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver())
        {
            return;
        }

        HandleLaneInput();
        HandleJump();
        HandleSlide();

        Vector3 targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneDistance;

        float horizontalMovement =
            (targetPosition.x - transform.position.x) * laneChangeSpeed;

        Vector3 movement = new Vector3(
            horizontalMovement,
            verticalVelocity,
            forwardSpeed
        );

        controller.Move(movement * Time.deltaTime);
    }

    private void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane--;
        }

        if (Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane++;
        }

        currentLane = Mathf.Clamp(currentLane, 0, 2);
    }

    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleSlide()
    {
        if (!isSliding)
        {
            if (Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartSlide();
            }
        }
        else
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        controller.height = slideHeight;

        controller.center = new Vector3(
            originalCenter.x,
            slideHeight / 2f,
            originalCenter.z
        );
    }

    private void StopSlide()
    {
        isSliding = false;

        controller.height = originalHeight;
        controller.center = originalCenter;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}