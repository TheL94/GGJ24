using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerSlotManager playerSlots;

    private InputAction interactAction;
    private InputAction releaseAction;

    private IInteractable latestInteract;

    public static UnityAction<Pickable> onTakeObject;
    public static UnityAction onReleaseObject;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerSlots = GetComponentInParent<PlayerSlotManager>();

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
        latestInteract.Interact();

        if (latestInteract is Pickable pickable)
            onTakeObject.Invoke(pickable);
    }

    private void Release(InputAction.CallbackContext ctx)
    {
        onReleaseObject.Invoke();
    }
}