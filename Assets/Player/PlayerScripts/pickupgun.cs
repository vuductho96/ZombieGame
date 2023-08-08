using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class pickupgun2 : MonoBehaviour
{
    public GameObject GunObject;
    public GameObject TargetGun;
    public GameObject PickUpText;
    private string gunTag = "Gun";
    private GameObject currentWeapon;
    private bool hasPickedUpGun = false;
    private bool weaponInRange;
    private bool hasGunObject = false;
    public GameObject CrossHair;
    private Animator Anim;
    private AudioSource audioSource;


    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
        PickUpText.SetActive(false);
        TargetGun.SetActive(false);
        currentWeapon = GunObject;
        CrossHair.SetActive(false);


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hasPickedUpGun && weaponInRange)
        {
            CrossHair.SetActive(true);
            PickUpText.SetActive(false);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2.0f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(gunTag))
                {
                    PickUp(collider.gameObject);
                    hasPickedUpGun = true;
                    hasGunObject = true;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && hasPickedUpGun)
        {
            Drop();
            hasPickedUpGun = false;
            hasGunObject = false;
        }

        // Check if there are any colliders with the gunTag in range
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, 2.0f);
        bool anyGunsInRange = false;
        foreach (Collider collider in collidersInRange)
        {
            if (collider.CompareTag(gunTag))
            {
                anyGunsInRange = true;
                break;
            }
        }

        if (!anyGunsInRange)
        {
            weaponInRange = false;
            PickUpText.SetActive(false);
        }
    }

    private void PickUp(GameObject gun)
    {
        if (!gun.CompareTag(gunTag))
        {

            return;
        }

        GunObject.SetActive(false);
        currentWeapon.SetActive(false);
        currentWeapon = gun;
        TargetGun.SetActive(true);


    }

    private float GetGroundHeight(Vector3 position)
    {
        RaycastHit hit;
        float groundHeight = 0f;
        if (Physics.Raycast(position, Vector3.down, out hit))
        {
            groundHeight = hit.point.y;
        }
        return groundHeight;
    }

    private void Drop()
    {
        TargetGun.SetActive(false);
        currentWeapon.SetActive(true);
        PickUpText.SetActive(false);
        CrossHair.SetActive(false);
        Vector3 dropPosition = transform.position + transform.forward * 2.0f;
        dropPosition.y = GetGroundHeight(dropPosition) + 0.5f;

        currentWeapon.transform.position = dropPosition;

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(gunTag) && !hasPickedUpGun)
        {
            PickUpText.SetActive(true);
            weaponInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(gunTag))
        {
            PickUpText.SetActive(false);
        }
    }
}