using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{ 
    public enum ItemSize
    {
        Small = 1,
        Medium = 2, 
        Big = 3
    }

    public bool Picked { get; set; }
    public int Value{ get; private set; }
    
    public ItemSize size;
    public int maxValue;
    public int minValue;

    private Collider coll;
    
    void Start()
    {
       Value = Random.Range(minValue, maxValue);
       coll = gameObject.GetComponentInChildren<Collider>();
    }
    
    public void DestroyItem()
    {
        Debug.Log("destroyeed:" + gameObject.name);
        Destroy(this.gameObject);
    }

    public void Interact()
    {
        coll.enabled = false;
        
    }
}
