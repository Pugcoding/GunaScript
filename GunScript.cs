using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript: MonoBehaviour

{

  public float damage = 10f;

  public float range = 100f;

  public float impactForce = 60f;

  public float fireRate = 55f;

  public bool semiauto = true;

  public int maxAmmo = 30;

  private int currentAmmo;

  public float Reloadtime = 1f;
  private bool IsReloading = false;

  public Camera fpsCam;

  public ParticleSystem muzzleFlash;

  public GameObject impactEffect;
  public Animator animator;
  public AudioClip gunshot;
  public AudioClip reloading;
  public AudioClip outtaammo;
  Text AmmoCounter;

  private float nextTimeToFire;

  void Start()
  {
   currentAmmo = maxAmmo;
   AmmoCounter = GetComponent<Text>();
  }

  public void Update() {
    
    if (IsReloading)
      return;
    if (currentAmmo <= 0)
    {
      StartCoroutine(Reload());
      return;
    }

    if (semiauto == true)

    {

      if ((Input.GetButtonDown("Fire1")) && Time.time >= nextTimeToFire)

      {

        nextTimeToFire = Time.time + 1f / fireRate;

        Shoot();

      }

    } else

    {

      if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)

      {

        nextTimeToFire = Time.time + 1f /fireRate;

        Shoot();

      }

    }
    AmmoCounter.text = currentAmmo.ToString();

  }

  public void Shoot()

  {
    currentAmmo --;
    GetComponent<AudioSource>().PlayOneShot(gunshot);

    RaycastHit hit;

    muzzleFlash.Play();

    if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))

    {

      Debug.Log(hit.transform.name);

      Target target = hit.transform.GetComponent < Target > ();

      if (target != null)

      {

        target.TakeDamage(damage);

      }

      if (hit.rigidbody != null)

      {

        hit.rigidbody.AddForce(-hit.normal * impactForce);

      }

      GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

      Destroy(impactGO, 2f);

    }
  }

  IEnumerator Reload() {
    Animator anim;
    GetComponent<AudioSource>().PlayOneShot(reloading);
    IsReloading = true;
    Debug.Log("Reloading");
    animator.SetBool("Reloading",true);
    yield return new WaitForSeconds(Reloadtime);
    animator.SetBool("Reloading",false);
    currentAmmo = maxAmmo;
    IsReloading = false;
  }

}