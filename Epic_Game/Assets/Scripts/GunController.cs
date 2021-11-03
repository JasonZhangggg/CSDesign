﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

    public GameObject crosshair;

    private Vector3 desiredPosition;
    private Vector3 target;


    public GameController gameController;

    public float fireRate = 0.12f;

    private int clipSize = 24;
    public float reloadTime = 0f;
    public bool isReloading = false;
    bool soundPlayed = false;

    private bool canShoot = true;
    private int currentAmmoInClip;

    public Camera cam;
    public GameObject player;
    // Start is called before the first frame update

    private Recoil recoil;

    private AudioSource playerAudioSource;

    public Text ammoText;

    public LineRenderer bulletTrail;
    public GameObject barrelLoc;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        currentAmmoInClip = clipSize;
        playerAudioSource = GetComponent<AudioSource>();

        recoil = GameObject.Find("CameraRecoil").GetComponent<Recoil>();

    }

    // Update is called once per frame
    void Update()
    {
        DetermineAim();
        DetermineRotation();
        ammoText.text = currentAmmoInClip.ToString()+"/"+clipSize;

        if (Input.GetMouseButton(0) && canShoot && currentAmmoInClip > 0)
        {
            gameController.playAudio(playerAudioSource, "Gun Shot");
            currentAmmoInClip--;
            StartCoroutine(ShootGun());
        }
        if (Input.GetButtonDown("Reload") && !isReloading && clipSize > currentAmmoInClip)
        {
            //manually reloads
            currentAmmoInClip = 0;
            isReloading = true;
        }
        if (currentAmmoInClip == 0) isReloading = true;
        if (isReloading)
        {
            if (!soundPlayed)
            {
                gameController.playAudio(playerAudioSource, "Reload1");
                soundPlayed = true;
            }
            reloadTime += Time.deltaTime;
        }
        if (reloadTime >= 1)
        {
            gameController.playAudio(playerAudioSource, "Reload2");
            reloadTime = 0;
            isReloading = false;
            soundPlayed = false;
            currentAmmoInClip = 24;
        }


    }
    private void DetermineAim()
    {
        target = normalLocalPosition;
        if (Input.GetMouseButton(1))
        {
            crosshair.SetActive(false);
            target = aimingLocalPosition;
        }
        else crosshair.SetActive(true);
        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 30);
        transform.localPosition = desiredPosition;
    }

    private void DetermineRotation() {

    }
    private void RaycastForEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f))
        {
            spawBulletTrail(hit.point);
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.tag == "Enemy1")
            {
                hitObj.GetComponent<Enemy1>().doDamage();
            }
            else if (hitObj.tag == "Enemy2")
            {
                hitObj.GetComponent<Enemy2>().doDamage();
            }
            else if (hit.transform.tag == "Target")
            {
                Destroy(hitObj);
                gameController.addKill();
                GameObject.Find("Targets").GetComponent<PracticeTargets>().hit();
            }
        }
    }
    private IEnumerator ShootGun()
    {
        canShoot = false;
        RaycastForEnemy();
        kickBack();
        recoil.RecoilFire();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void kickBack() {
        transform.localPosition -= Vector3.forward * 0.1f; 
    }
    public void spawBulletTrail(Vector3 hitPos) {
        Debug.Log("Creating thing");
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, barrelLoc.transform.position, Quaternion.identity);
        LineRenderer LineR = bulletTrailEffect.GetComponent<LineRenderer>();
        LineR.SetPosition(0, barrelLoc.transform.position);
        LineR.SetPosition(1, hitPos);
        Destroy(bulletTrailEffect, 0.018f);
    }
    //-5, 3, 17
}