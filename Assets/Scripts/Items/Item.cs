using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{ 
    public enum ItemSize
    {
        Small,
        Medium,
        Big
    }
    public ItemSize size;
    private Collider coll;
    [HideInInspector]
    public int value;
    public int maxValue;
    public int minValue;
    // Start is called before the first frame update
    void Start()
    {
       value = Random.Range(minValue, maxValue);
       coll = gameObject.GetComponentInChildren<Collider>();
    }


    public void DestroyItem()
    {
        Debug.Log("destroyeed:" + gameObject.name);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
