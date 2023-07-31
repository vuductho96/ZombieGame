using UnityEngine;

public class PickUpjetPack : MonoBehaviour
{
    private Jetpack jetpackScript; // Reference to the Jetpack script
    private bool isGrounded;
    public GameObject JetpackObject;
    public GameObject TargetJetpack;
    public GameObject PickUpText;
    private string jetpackTag = "JetPack";
    private GameObject currentJetpack;
    private bool hasPickedUpJetpack = false;
    private bool jetpackInRange;
    private bool hasJetpackObject = false;
    private CharacterController characterController;
    private bool isUsingJetpack = false;
    public float jetpackForce = 10f; // The force applied to the player when using the jetpack
    public float moveSpeed = 5f; // The horizontal movement speed while in the air

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        PickUpText.SetActive(false);
        TargetJetpack.SetActive(false);
        currentJetpack = JetpackObject;

        // Get the reference to the Jetpack script
        jetpackScript = GetComponent<Jetpack>();
        jetpackScript.enabled = false; // Disable the Jetpack script initially
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, characterController.height * 0.5f + 0.1f);
        if (Input.GetKeyDown(KeyCode.E) && !hasPickedUpJetpack && jetpackInRange)
        {
            PickUpText.SetActive(false);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2.0f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(jetpackTag))
                {
                    PickUp(collider.gameObject);
                    hasPickedUpJetpack = true;
                    hasJetpackObject = true;
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && hasPickedUpJetpack)
        {
            Drop();
            hasPickedUpJetpack = false;
            hasJetpackObject = false;
        }

        // Check if there are any colliders with the jetpackTag in range
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, 2.0f);
        bool anyJetpacksInRange = false;
        foreach (Collider collider in collidersInRange)
        {
            if (collider.CompareTag(jetpackTag))
            {
                anyJetpacksInRange = true;
                break;
            }
        }

        if (!anyJetpacksInRange)
        {
            jetpackInRange = false;
            PickUpText.SetActive(false);
        }

        // Check if the player is holding down the "Jump" button (usually Space key) to use the jetpack
        if (hasPickedUpJetpack)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                StartUsingJetpack();
            }
            else
            {
                StopUsingJetpack();
            }
        }

        // Apply gravity to the player even if the jetpack is not used
        ApplyGravity();

        // Move the player based on input (horizontal and vertical axes)
        MovePlayer();
    }

    private void PickUp(GameObject jetpack)
    {
        if (!jetpack.CompareTag(jetpackTag))
        {
            return;
        }

        JetpackObject.SetActive(false);
        currentJetpack.SetActive(false);
        currentJetpack = jetpack;
        TargetJetpack.SetActive(true);

        // Attach the jetpack to the player as a child
        currentJetpack.transform.SetParent(transform, false);
    }

    private void Drop()
    {
        TargetJetpack.SetActive(false);
        currentJetpack.SetActive(true);
        PickUpText.SetActive(false);

        // Detach the jetpack from the player
        currentJetpack.transform.SetParent(null);
    }

    private void StartUsingJetpack()
    {
        isUsingJetpack = true;
        characterController.Move(Vector3.up * jetpackForce * Time.deltaTime);
     
       

        // Move the player horizontally based on input (horizontal and vertical axes)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection *= Time.deltaTime * moveSpeed;
        characterController.Move(moveDirection);

        Debug.Log("Jetpack is ON!");
    }

    private void StopUsingJetpack()
    {
        isUsingJetpack = false;

        if (!isGrounded)
        {
            // If the jetpack is not in use, cancel the previous jetpackForce by applying a force in the opposite direction
            characterController.Move(Vector3.down * jetpackForce * Time.deltaTime);
        }

        Debug.Log("Jetpack is OFF!");
    }


    private void ApplyGravity()
    {
        // Check if the player is grounded using a raycast
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, characterController.height * 0.5f + 0.1f);

        if (!isGrounded && !isUsingJetpack)
        {
            // Apply custom gravity only if the player is not grounded and the jetpack is not being used
            characterController.Move(Physics.gravity * Time.deltaTime);
        }
    }


    private void MovePlayer()
    {
        if (!characterController.isGrounded && !isUsingJetpack)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            moveDirection *= Time.deltaTime * moveSpeed;
            characterController.Move(moveDirection);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(jetpackTag) && !hasPickedUpJetpack)
        {
            PickUpText.SetActive(true);
            jetpackInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(jetpackTag))
        {
            PickUpText.SetActive(false);
        }
    }
}
