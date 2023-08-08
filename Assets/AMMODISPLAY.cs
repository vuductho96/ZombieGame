using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AMMODISPLAY : MonoBehaviour
{
    public Shooting shooting; // Reference to the Shooting2 script
    public TextMeshProUGUI CountAmmo;
    public TextMeshProUGUI TotalAmmo;
    public void SetAmmoUIActive(bool active)
    {
        CountAmmo.gameObject.SetActive(active);
        TotalAmmo.gameObject.SetActive(active);
    }

    private void Start()
    {
        // Assuming you've already established the reference between these scripts
        // via the Unity Inspector (drag the Shooting2 script onto the AMMODISPLAY script's slot)
        // Otherwise, you can do it programmatically using: shooting = GetComponent<Shooting2>();
    }

    private void Update()
    {
        if (shooting != null)
        {
            // Update the ammo UI text
            CountAmmo.text = shooting.CurrentAmmo.ToString("0/");
            TotalAmmo.text = shooting.MaxAmmo.ToString("0");
        }
    }

}
