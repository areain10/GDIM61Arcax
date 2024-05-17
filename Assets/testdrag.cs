using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testdrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
