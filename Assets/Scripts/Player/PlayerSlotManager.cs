using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerSlotManager : MonoBehaviour
{
    public int maxSlots = 3;
    public List<Transform> slotPositions = new List<Transform>();

    Queue<Item> items = new Queue<Item>();
    int availableSlots;

    private void Start()
    {
        PlayerInteraction.onTakeObject -= TakeSlot;
        PlayerInteraction.onTakeObject += TakeSlot;
        PlayerInteraction.onReleaseObject -= ReleaseSlot;
        PlayerInteraction.onReleaseObject += ReleaseSlot;
        availableSlots = maxSlots;
    }

    public void TakeSlot(Item item)
    {
        if (item.Picked)
        {
            Debug.Log("Oggetto già preso");
            return;
        }

        if (availableSlots - item.size >= 0)
        {
            availableSlots -= (int)item.size;
            item.Picked = true;
            items.Enqueue(item);
            MoveToSlot(item);
            Debug.Log("Taken " + item.size + " slots. Available: " + availableSlots);
        }
        else
        {
            Debug.Log("Pila piena");
        }
    }

    public void ReleaseSlot()
    {
        if (items.Count > 0)
        {
            Item entity = items.Dequeue();
            availableSlots += (int)entity.size;
            entity.Picked = false;
            Debug.Log("Released " + entity.size + " slots. Available: " + availableSlots);
        }
    }

    public int ReleaseAllItems(Vector3 releasePosition)
    {
        int points = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items.Dequeue();
            points += item.Value;
            item.transform.DOMove(releasePosition, .5f).OnComplete(() =>
            {
                item.transform.DOScale(Vector3.zero, .2f).OnComplete(() => Destroy(item));
            });
        }

        availableSlots = maxSlots;
        return points;
    }

    void MoveToSlot(Item item)
    {
        int index = (maxSlots - 1) - availableSlots;
        Transform slot = slotPositions[index];
        item.transform.DOMove(slot.position, .4f).OnComplete(() => { item.transform.parent = slot; });
    }
}