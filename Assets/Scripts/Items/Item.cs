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

    private Rigidbody rBody;
    private Collider[] colliders;
    AudioSource m_AudioSource;

    protected virtual void Start()
    {
       Value = Random.Range(minValue, maxValue);
       colliders = gameObject.GetComponentsInChildren<Collider>();
       rBody = GetComponent<Rigidbody>();
       m_AudioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        foreach (var coll in colliders)
        {
            coll.enabled = false;
        }

        rBody.useGravity = false;
        rBody.isKinematic = true;
        m_AudioSource.Play();
    }
}
