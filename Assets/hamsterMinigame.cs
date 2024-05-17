using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class hamsterMinigame : MonoBehaviour,IDragHandler
{
    [SerializeField] Transform startPos;
    [SerializeField] float timer = 1;
    [SerializeField] GameObject finish;
    [SerializeField] TaskBase task;
    bool drag;
    public void OnDrag(PointerEventData eventData)
    {
        if(drag)
        {
            transform.position = eventData.position;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        drag = true;
    }
    public void Reset()
    {
        drag = false;
        transform.position = startPos.position;
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!drag)
        {
            timer -= Time.deltaTime;
        }
        if(timer < 0)
        {
            drag = true;
            timer = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == finish)
        {
            task.CompleteTask();
        }
    }
}
