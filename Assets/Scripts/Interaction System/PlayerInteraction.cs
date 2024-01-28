using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction interactAction;
    private InputAction releaseAction;

    private IInteractable latestInteract;

    public static UnityAction<Item> onTakeObject;
    public static UnityAction onReleaseObject;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        interactAction = playerInput.actions.FindAction("Interact");
        releaseAction = playerInput.actions.FindAction("Release");

        releaseAction.performed -= Release;
        releaseAction.performed += Release;
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            Debug.Log("found " + other.name);
            latestInteract = interactable;
            interactAction.performed += Interact;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();

        if (interactable != null && latestInteract != null && latestInteract.Equals(latestInteract))
        {
            Debug.Log("ciao " + other.name);
            latestInteract = null;
            interactAction.performed -= Interact;
        }
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        interactAction.performed -= Interact;
        latestInteract.Interact();

        if (latestInteract is Item pickable)
            onTakeObject.Invoke(pickable);
    }

    private void Release(InputAction.CallbackContext ctx)
    {
        onReleaseObject.Invoke();
    }
}