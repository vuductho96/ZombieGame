using UnityEngine;

public class Jetpack : MonoBehaviour
{
    public float maxFuel = 4f;
    public float thurstForce = 10f;
    public float fuelConsumptionRate = 1f;
    public Transform groundCheck;
    public ParticleSystem jetpackParticles;

    private float currentFuel;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.1f);

        if (Input.GetKeyDown(KeyCode.B) && currentFuel > 0f && !isGrounded)
        {
            StartUsingJetpack();
        }
        else if (Input.GetKeyUp(KeyCode.B) || currentFuel <= 0f || isGrounded)
        {
            StopUsingJetpack();
        }
    }

    private void StartUsingJetpack()
    {
        rb.AddForce(transform.up * thurstForce, ForceMode.Force);
        jetpackParticles.Play();
        currentFuel -= Time.deltaTime * fuelConsumptionRate;
    }

    private void StopUsingJetpack()
    {
        jetpackParticles.Stop();
    }
}
