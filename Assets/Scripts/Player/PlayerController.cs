using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 10f;

    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 10f;

    private int currentLane = 1;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleLaneInput();

        Vector3 targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneDistance;

        float horizontalMovement =
            (targetPosition.x - transform.position.x) * laneChangeSpeed;

        Vector3 movement = new Vector3(
            horizontalMovement,
            0f,
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
}