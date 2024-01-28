using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSlotManager : MonoBehaviour
{
    public const int MAX_SLOTS = 3;
    public int availableSlots = 3;

    public Queue<Pickable> pickables = new Queue<Pickable>();

    private void Start()
    {
        PlayerInteraction.onTakeObject -= TakeSlot;
        PlayerInteraction.onTakeObject += TakeSlot;
        PlayerInteraction.onReleaseObject -= ReleaseSlot;
        PlayerInteraction.onReleaseObject += ReleaseSlot;
    }

    public void TakeSlot(Pickable entity)
    {
        if (entity.picked)
        {
            Debug.Log("Oggetto già preso");
            return;
        }

        if (availableSlots - entity.weight >= 0)
        {
            availableSlots -= entity.weight;
            entity.picked = true;
            pickables.Enqueue(entity);
            Debug.Log("Taken " + entity.weight + " slots. Available: " + availableSlots);
        }
        else
        {
            Debug.Log("Pila piena");
        }
    }

    public void ReleaseSlot()
    {
        if (pickables.Count > 0)
        {
            Pickable entity = pickables.Dequeue();
            availableSlots += entity.weight;
            entity.picked = false;
            Debug.Log("Released " + entity.weight + " slots. Available: " + availableSlots);
        }
    }
}
