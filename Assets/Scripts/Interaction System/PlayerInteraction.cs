using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction interactAction;

    private IInteractable latestInteract;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interaction");
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
    }
}