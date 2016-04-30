using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public float gravity = -9.8f;
    public Vector3 gravityUp;	
	
	public Vector3 Attract(Rigidbody body)
    {
		gravityUp = (body.position - transform.position).normalized;

        // Apply downwards gravity to body
        //body.AddForce(gravityUp * gravity);

        return gravityUp * gravity;
	}  

    public void Align(Rigidbody body)
    {
        Vector3 localUp = body.transform.up;

        // Allign bodies up axis with the centre of planet
        body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
    }
}
