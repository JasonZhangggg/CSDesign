using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

    private GameObject crosshair;

    private Vector3 desiredPosition;
    private Vector3 target;


    public GameController gameController;

    public float fireRate = 0.12f;

    public int clipSize = 24;
    private float reloadTime = 0f;
    private bool isReloading = false;
    private bool soundPlayed = false;
    public bool fullAuto;

    private bool canShoot = true;
    private int currentAmmoInClip;

    private Camera cam;
    private GameObject player;

    private Recoil recoil;

    private AudioSource playerAudioSource;

    private Text ammoText;

    public LineRenderer bulletTrail;
    private GameObject barrelLoc;

    public Transform raycastDest;
    Ray ray;
    RaycastHit hitInfo;

    public TrailRenderer tracerEffect;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;

    //public playerMovement movement;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        currentAmmoInClip = clipSize;
        playerAudioSource = GetComponent<AudioSource>();
        crosshair = GameObject.Find("/HUD/Crosshair");
        ammoText = GameObject.Find("/HUD/Ammo").GetComponent<Text>();
        player = GameObject.Find("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        barrelLoc = transform.Find("BulletLoc").gameObject;

        recoil = GameObject.Find("CameraRecoil").GetComponent<Recoil>();

    }

    // Update is called once per frame
    void Update()
    {
        DetermineAim();
        DetermineRotation();
        ammoText.text = currentAmmoInClip.ToString() + "/" + clipSize;

        if (Input.GetMouseButton(0) && canShoot && currentAmmoInClip > 0 && Time.timeScale == 1 && fullAuto)
        {
            StartCoroutine(ShootGun());
        }
        else if (Input.GetButtonDown("Fire1") && currentAmmoInClip > 0 && Time.timeScale == 1 && !fullAuto) shoot();
        if (Input.GetMouseButton(0) && fullAuto && currentAmmoInClip > 0) {
            playerMovement.speed = 4;
        }
        else
        {
            playerMovement.speed = 10;
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
                transform.Rotate(new Vector3(-90, 0, 0));
                gameController.playAudio(playerAudioSource, "Reload1");
                soundPlayed = true;
            }
            reloadTime += Time.deltaTime;
        }
        if (reloadTime >= 1)
        {
            transform.Rotate(new Vector3(90, 0, 0));
            gameController.playAudio(playerAudioSource, "Reload2");
            reloadTime = 0;
            isReloading = false;
            soundPlayed = false;
            currentAmmoInClip = clipSize;
            
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

    private void DetermineRotation()
    {

    }
    
    private void RaycastForEnemy()
    {
        var tracer = Instantiate(tracerEffect, barrelLoc.transform.position, Quaternion.identity);
        tracer.AddPosition(barrelLoc.transform.position);
        ray.origin = cam.transform.position;
        ray.direction = raycastDest.position - cam.transform.position;

        if (Physics.Raycast(ray, out hitInfo))
        {
            tracer.transform.position = hitInfo.point;
            GameObject hitObj = hitInfo.transform.gameObject;

            if (hitObj.tag == "Enemy1")
            {
                hitObj.GetComponent<Enemy1>().doDamage();
            }
            else if (hitObj.tag == "Enemy2")
            {
                hitObj.GetComponent<Enemy2>().doDamage();
            }
            else if (hitObj.tag == "Enemy3")
            {
                hitObj.GetComponent<Enemy3>().doDamage();
            }
            else if (hitObj.tag == "EnemyTarget")
            {
                Destroy(hitObj);
                gameController.addKill();
                GameObject.Find("Targets").GetComponent<PracticeTargets>().hit();
            }
            else if(hitObj.tag == "Boss")
            {
                hitObj.GetComponent<Boss>().takeDamage(10);
            }
            else if (hitObj.tag == "Key")
            {
                //do nothing so there isn't a floating decal
            }
            else {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
                //Debug.Log(hitObj.tag);
            }
        }
        else
        {
            tracer.transform.position = ray.origin + ray.direction * 50;

        }
    }
    private IEnumerator ShootGun()
    {
        canShoot = false;
        shoot();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    private void shoot()
    {
        gameController.playAudio(playerAudioSource, "Gun Shot", 0.5f);
        muzzleFlash.Emit(1);
        currentAmmoInClip--;
        RaycastForEnemy();
        kickBack();
        recoil.RecoilFire();
    }

    private void kickBack()
    {
        transform.localPosition -= Vector3.forward * 0.1f;
    }
    public void spawBulletTrail(Vector3 hitPos)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, barrelLoc.transform.position, Quaternion.identity);
        LineRenderer LineR = bulletTrailEffect.GetComponent<LineRenderer>();
        LineR.SetPosition(0, barrelLoc.transform.position);
        LineR.SetPosition(1, hitPos);
        Destroy(bulletTrailEffect, 0.018f);
    }
    //-5, 3, 17
}
