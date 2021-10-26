using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float gravity = 9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float speed = 12f;

    Vector3 impact = Vector3.zero;
    float mass = 3.0F; // defines the character mass

    private float dashTime = 0f;
    private bool isDashing = false;
    public float dashLength = 0.15f;
    public float dashSpeed = 75f;
    public float dashTimer = 0f;
    public float dashCooldown = 1f;

    public static int hasDashed = 0;
    public GameController gameController;

    // Update is called once per frame
    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (impact.magnitude > 0.2F) controller.Move(impact * Time.deltaTime);

        impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) == true && dashTime < dashLength && controller.isGrounded && !isDashing && dashTimer>dashCooldown)
        {
            dashTimer = 0;
            isDashing = true;
            if(gameController.winPart == 2) hasDashed = 1;
        }
 
        if (isDashing == true && dashTime < dashLength){
             controller.Move(move * dashSpeed * Time.deltaTime);
             dashTime += Time.deltaTime;
         }
        if (dashTime >= dashLength){
            isDashing = false;
            dashTime = 0f;
            controller.Move(move * speed * Time.deltaTime);
        }
        if(!isDashing && dashTimer< dashCooldown+0.1){
            dashTimer += Time.deltaTime;
        } 
    }

    public void AddImpact(Vector3 dir, float force){
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

}
