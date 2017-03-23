using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour {

    public Rigidbody rigidBody;
    private bool currentlyInteracting;
    private WandController attachedWand;
    private Transform interactionPoint;

    private Vector3 posDelta;
    private float velocityVector = 20000.0f;
    private Quaternion rotaionDelta;
    private float angle;
    private Vector3 axis;
    private float rotaionFactor = 20.0f;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityVector /= rigidBody.mass;
	
	}
	
	// Update is called once per frame
	void Update () {

        if(attachedWand && currentlyInteracting)
        {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            this.rigidBody.velocity = posDelta * velocityVector * Time.fixedDeltaTime;

            rotaionDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotaionDelta.ToAngleAxis(out angle, out axis);

            if(angle > 180)
            {
                angle -= 360;
            }

            this.rigidBody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotaionFactor;
        }
	
	}

    public void BeginInteraction(WandController wand)
    {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(WandController wand)
    {
        if(wand == attachedWand)
        {
            attachedWand = null;
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting()
    {
        return currentlyInteracting;
    }
}
