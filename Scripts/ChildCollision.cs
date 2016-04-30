using UnityEngine;
using System.Collections;

public class ChildCollision : MonoBehaviour
{
    GravityAttractor parent;

	// Use this for initialization
	void Awake ()
    {
        parent = this.gameObject.GetComponentInParent<GravityAttractor>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
