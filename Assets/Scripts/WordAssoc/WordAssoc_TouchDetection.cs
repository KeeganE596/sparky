﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAssoc_TouchDetection : MonoBehaviour
{
    public GameObject touch_blob;

    bool hitObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);
        //Raycast from camera
        if (hit && hit.collider.gameObject.tag == "TouchBlob")  {
            hitObj = true;
        }

        if(hitObj && (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))) {
                Vector2 toPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touch_blob.transform.position = Vector2.MoveTowards(new Vector2(touch_blob.transform.position.x, touch_blob.transform.position.y), toPos, (20 * Time.deltaTime));
            }
        else{
            touch_blob.transform.position = Vector2.MoveTowards(new Vector2(touch_blob.transform.position.x, touch_blob.transform.position.y), new Vector2(0,0), (15 * Time.deltaTime));
        }

        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) {
            hitObj = false;
        }
    }
}