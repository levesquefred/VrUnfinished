using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandController : MonoBehaviour {
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    HashSet<InteractableItem> objectHoveringOver = new HashSet<InteractableItem>();
    private InteractableItem closestItem;
    private InteractableItem interactingItem;

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if (controller == null)
        {
            Debug.Log("Controller not Initialized");
            return;
        }

        if(controller.GetPressDown(triggerButton))
        {
            float minDistance = float.MaxValue;

            float distance;
            //vector difference between the item and the wand and square mag it
            foreach(InteractableItem item in objectHoveringOver)
            {
                distance = (item.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            interactingItem = closestItem;
            //passing it to other hand ends interaction with one and begins with the other
            if (interactingItem)
            {
                if (interactingItem.IsInteracting())
                {
                    interactingItem.EndInteraction(this);
                }

                interactingItem.BeginInteraction(this);

            }
        }

        if (controller.GetPressUp(triggerButton) && interactingItem != null)
        {
            interactingItem.EndInteraction(this);
        }
    }

   //If our hand is colliding with a collidable object
    private void OnTriggerEnter(Collider collider)
    {
        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            objectHoveringOver.Add(collidedItem);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            objectHoveringOver.Remove(collidedItem);
        }
    }
}
