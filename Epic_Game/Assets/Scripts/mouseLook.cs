using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{

    public GameObject player;
    private float mouseSensitivity = 1;
    private Vector2 currentRotation;
    public float recoil = 0f;
    public Camera cam;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        updateMouseSensitivity();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseAxis *= mouseSensitivity / 100;
        currentRotation += mouseAxis;

        currentRotation.y = Mathf.Clamp(currentRotation.y, -90, 90);

        transform.GetChild(0).localPosition += (Vector3)mouseAxis * 10 / 1000;

        player.transform.localRotation = Quaternion.AngleAxis(currentRotation.x, Vector3.up);
        cam.transform.localRotation = Quaternion.AngleAxis(-currentRotation.y, Vector3.right);
        
    }

    public void addRecoil(float recoilAmount)
    {
        recoil = recoilAmount;
    }

    public void updateMouseSensitivity()
    {
        mouseSensitivity = gameController.mouseSensitivity;
    }
}
