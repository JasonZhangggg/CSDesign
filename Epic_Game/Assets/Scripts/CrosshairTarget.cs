using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrosshairTarget : MonoBehaviour
{
    Camera cam;
    Ray ray;
    RaycastHit hitInfo;
    private GameObject crosshair1;
    private GameObject crosshair2;
    private GameObject crosshair3;
    private GameObject crosshair4;

    private Color32 crosshairColor;

    public GameObject barrelLoc;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        crosshair1 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair1");
        crosshair2 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair2");
        crosshair3 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair3");
        crosshair4 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair4");
        crosshairColor = Color.white;
        //barrelLoc = transform.Find("/Player/CameraRotation/CamerRecoil/PlayerCamera/AK-47/BulletLoc").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
            if (hitInfo.transform.gameObject.tag.Contains("Enemy"))
            {
                Debug.Log("Enemy");
                crosshairColor = new Color32(255, 0, 0, 255);
            }
            else
            {
                crosshairColor = Color.white;

            }
        }
        else {
            transform.position = barrelLoc.transform.position + barrelLoc.transform.TransformDirection(new Vector3(0, 0, 50));
        }
        crosshair1.GetComponent<Image>().color = crosshairColor;
        crosshair2.GetComponent<Image>().color = crosshairColor;
        crosshair3.GetComponent<Image>().color = crosshairColor;
        crosshair4.GetComponent<Image>().color = crosshairColor;


    }
}
