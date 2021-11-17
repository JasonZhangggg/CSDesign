using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrosshairTarget : MonoBehaviour
{
    Camera cam;
    Ray ray;
    RaycastHit hitInfo;
    public GameObject crosshair1;
    public GameObject crosshair2;
    public GameObject crosshair3;
    public GameObject crosshair4;

    public Color32 crosshairColor;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("/Player/CameraRotation/CameraRecoil/PlayerCamera").GetComponent<Camera>();
        crosshair1 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair1");
        crosshair2 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair2");
        crosshair3 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair3");
        crosshair4 = GameObject.Find("/HUD/Crosshair/Canvas/crosshair4");
        crosshairColor = Color.white;

    }

    // Update is called once per frame
    void Update()
    {

        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
        if (hitInfo.transform.gameObject.tag.Contains("Enemy"))
        {
            Debug.Log("Enemy");
            crosshairColor = new Color32(255, 0, 0, 255);
        }
        else {
            crosshairColor = Color.white;

        }
        crosshair1.GetComponent<Image>().color = crosshairColor;
        crosshair2.GetComponent<Image>().color = crosshairColor;
        crosshair3.GetComponent<Image>().color = crosshairColor;
        crosshair4.GetComponent<Image>().color = crosshairColor;


    }
}
