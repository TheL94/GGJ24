using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionArea : MonoBehaviour
{

    LayerMask checklayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.GetComponentInParent<Item>())
        {
            var item = other.GetComponentInParent<Item>();
            GameManager.Instance.Points += item.value;
            item.DestroyItem();
            Debug.Log("the score is: " + GameManager.Instance.Points);
        }
    }
}
