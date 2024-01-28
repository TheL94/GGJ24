using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSlotManager : MonoBehaviour
{
    public const int MAX_SLOTS = 3;
    public int availableSlots = 3;

    public Queue<Item> pickables = new Queue<Item>();

    private void Start()
    {
        PlayerInteraction.onTakeObject -= TakeSlot;
        PlayerInteraction.onTakeObject += TakeSlot;
        PlayerInteraction.onReleaseObject -= ReleaseSlot;
        PlayerInteraction.onReleaseObject += ReleaseSlot;
    }

    public void TakeSlot(Item entity)
    {
        if (entity.Picked)
        {
            Debug.Log("Oggetto già preso");
            return;
        }

        if (availableSlots - entity.size >= 0)
        {
            availableSlots -= (int)entity.size;
            entity.Picked = true;
            pickables.Enqueue(entity);
            Debug.Log("Taken " + entity.size + " slots. Available: " + availableSlots);
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
            Item entity = pickables.Dequeue();
            availableSlots += (int)entity.size;
            entity.Picked = false;
            Debug.Log("Released " + entity.size + " slots. Available: " + availableSlots);
        }
    }
}
