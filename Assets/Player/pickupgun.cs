using UnityEngine;

public class pickupgun : MonoBehaviour
{
    AMMODISPLAY ammo;
    public GameObject GunObject;
    public GameObject TargetGun;
    public GameObject PickUpText;
    public GameObject CrossHair;
    private string gunTag = "Gun";
    private GameObject currentWeapon;
    private bool hasPickedUpGun = false;
    private bool weaponInRange;
    private bool hasGunObject = false;
    private Animator Anim;
    private AudioSource audioSource;

    private void Start()
    {
        ammo = FindObjectOfType<AMMODISPLAY>();
        audioSource = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();

        // Deactivate components and objects at the start
        if (ammo != null)
        {
            ammo.SetAmmoUIActive(false);
        }
        PickUpText.SetActive(false);
        TargetGun.SetActive(false);
        currentWeapon = GunObject;
        CrossHair.SetActive(false);
    }

    private void Update()
    {
        HandlePickup();
        HandleDrop();
        CheckGunsInRange();
    }

    private void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hasPickedUpGun && weaponInRange)
        {
            // Deactivate old gun and activate picked up gun
            GunObject.SetActive(false);
            TargetGun.SetActive(true);
            ammo.SetAmmoUIActive(true);
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
    }

    private void HandleDrop()
    {
        if (Input.GetKeyDown(KeyCode.Q) && hasPickedUpGun)
        {
            Drop();
            hasPickedUpGun = false;
            hasGunObject = false;
        }
    }

    private void CheckGunsInRange()
    {
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

        weaponInRange = anyGunsInRange;
        PickUpText.SetActive(weaponInRange);
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
            weaponInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(gunTag))
        {
            weaponInRange = false;
            PickUpText.SetActive(false);
        }
    }
}
