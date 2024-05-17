using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class splashart : MonoBehaviour
{
    [SerializeField] Image art;
    private float timer;
    private float anotherTimer;
    bool fading;
    // Start is called before the first frame update
    private void OnEnable()
    {
        anotherTimer = 2;
        fading = false;
        art.gameObject.SetActive(true);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(anotherTimer > 0)
        {
            anotherTimer -= Time.deltaTime;
        }
        if(anotherTimer < 0)
        {
            timer = 1;
            fading = true;
            anotherTimer = 0;
        }
        if(timer> 0 && fading)
        {
            art.color = new Color(255, 255, 255, timer);
            timer -= Time.deltaTime;
        }
        if(timer < 0)
        {
            art.gameObject.SetActive(false);
        }
    }
}
