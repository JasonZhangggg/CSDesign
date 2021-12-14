using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public CharacterController controller;
    private float gravity = -20f;
    private float jumpHeight = 3f;

    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    public static float speed = 10f;

    private Vector3 impact = Vector3.zero;
    private float mass = 3.0F; // defines the character mass

    private float dashTime = 0f;
    private bool isDashing = false;
    private float dashLength = 0.15f;
    private float dashSpeed = 75f;
    private float dashTimer = 0f;
    private float dashCooldown = 1f;

    public static int hasDashed = 0;
    public GameController gameController;

    private int selectedWeapon = 0;
    private int totalWeapons = 2;
    private GameObject[] weapons;



    // Update is called once per frame
    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        weapons = GameObject.FindGameObjectsWithTag("Weapon");

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


        //swap weapons
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= totalWeapons-1) selectedWeapon = 0;
            else selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0) selectedWeapon = totalWeapons - 1;
            else selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedWeapon = 1;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == selectedWeapon) weapons[i].SetActive(true);
            else weapons[i].SetActive(false);
        }
    }

    public void AddImpact(Vector3 dir, float force){
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
    public bool inRange(Vector3 point, float dist) {
        if (Vector3.Distance(transform.position, point) <= dist) return true;
        return false;
    }

    
}
