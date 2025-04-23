using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;

    [Header("Jump & Gravity")]
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Crouch Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;

    [Header("Animation")]
    public Animator animator;  // Перетягни сюди свій Animator

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Блокуємо курсор та ховаємо
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // перевірка, чи на землі
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && moveDirection.y < 0)
            moveDirection.y = -2f;  // невеликий натиск вниз, щоб не "зависати"

        // рух вперед/вбік
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right   = transform.TransformDirection(Vector3.right);
        bool isRunning  = Input.GetKey(KeyCode.LeftShift);
        float speed     = isRunning ? runSpeed : walkSpeed;

        float curSpeedX = speed * Input.GetAxis("Vertical");
        float curSpeedY = speed * Input.GetAxis("Horizontal");

        float verticalVel = moveDirection.y;
        moveDirection = forward * curSpeedX + right * curSpeedY;

        // стрибок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = verticalVel;
        }

        // гравітація
        moveDirection.y -= gravity * Time.deltaTime;

        // присідання (R)
        if (Input.GetKey(KeyCode.R))
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        // виконуємо рух
        characterController.Move(moveDirection * Time.deltaTime);

        // горизонтальна швидкість для Blend Tree або переходів
        Vector3 horizontalVel = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        animator.SetFloat("Speed", horizontalVel.magnitude);

        // стрибок — true, коли не на землі
        animator.SetBool("IsJumping", !isGrounded);
    }
}
