using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{

    public float mouseSensitivity;

    public Transform playerBody;

    float xRotation = 0f;

    public float recoil = 0f;

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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime + recoil;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        if(recoil >= 0f)
        {
            recoil -= Time.deltaTime * 100;
        }
        
        if(recoil < 0f)
        {
            recoil = 0f;
        }
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
