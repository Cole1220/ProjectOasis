using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonControllerFlat : MonoBehaviour, IGravityBody
{

    // public vars
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 10;
    public LayerMask groundedMask;

    // System vars
    public bool grounded;
    public bool flying;
    public bool cameraBasedMovement;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    Rigidbody rigidbody;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();

        // Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        Vector3 upwardForceTotal = Vector3.zero;

        //========================================
        //Input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            flying = true;
            upwardForceTotal = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            flying = false;
        }
        //========================================

        if (!flying)
        {
            cameraBasedMovement = false;
        }
        else
        {
            cameraBasedMovement = true;
        }


        if (!cameraBasedMovement) //On Planet and not flying
        {
            //Gravity
            if (!grounded)
            {
                upwardForceTotal.y += -9.8f;
            }

            // Look rotation:
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
            verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
            cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

            // Calculate movement:
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
            Vector3 targetMoveAmount = moveDir * walkSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

            if (Input.GetKey(KeyCode.Space))
            {
                if (grounded)
                {
                    upwardForceTotal += transform.up * jumpForce;
                }
            }

            // =============================================================
            // Grounded check
            Ray ray = new Ray(transform.position, transform.up * -1);
            RaycastHit hit;

            //make ray from character to planet core
            if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
            // =============================================================

            rigidbody.AddForce(upwardForceTotal);
        }
        else
        {
            // Look rotation:
            cameraTransform.localEulerAngles = Vector3.zero; //TODO: do forward on camera's forward not gameobject's to not do this then slowly rotate model to correct position
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
            transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * mouseSensitivityY);

            // Calculate movement:
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
            Vector3 targetMoveAmount = moveDir * walkSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        }
    }

    void FixedUpdate()
    {
        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }

    void OnTriggerEnter(Collider col)
    {

    }

    void OnTriggerStay(Collider col)
    {

    }

    void OnTriggerExit(Collider col)
    {

    }
}
