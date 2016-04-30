using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour, IGravityBody
{
    [Range(1f, 10f)]
	public float mouseSensitivityX = 1;

    [Range(1f, 10f)]
    public float mouseSensitivityY = 1;

	public float moveSpeed = 6;
	public float jumpForce = 22000;

    public float maxJumpHeight;
    public float minJumpHeight;
    public float timeToApex;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;

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

    public GravityAttractor planet;

    void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cameraTransform = Camera.main.transform;
		rigidbody = GetComponent<Rigidbody> ();

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

        if(planet != null && !flying)
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
            upwardForceTotal += planet.Attract(rigidbody);

            if (grounded)
            {
                planet.Align(rigidbody);
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
            Vector3 targetMoveAmount = moveDir * moveSpeed;
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
            Ray ray = new Ray(transform.position, planet.gravityUp * -1);
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
            Vector3 targetMoveAmount = moveDir * moveSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        }
    }

    void FixedUpdate() {
		// Apply movement to rigidbody
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
		rigidbody.MovePosition(rigidbody.position + localMove);
    }

    private void CalculatePhysics()
    {
        gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);

        maxJumpVelocity = Mathf.Abs(gravity) * timeToApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Planet" && planet == null)
        {
            planet = col.gameObject.GetComponentInParent<GravityAttractor>();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Planet" && planet == null)
        {
            planet = col.gameObject.GetComponentInParent<GravityAttractor>();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.parent.gameObject == planet.gameObject)
        {
            planet = null;
        }
    }
}
