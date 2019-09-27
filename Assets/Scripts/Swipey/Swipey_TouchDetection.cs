﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swipey_TouchDetection : MonoBehaviour
{
    public bool enabletouch = false;

    [SerializeField]
    public Score_manager score;

    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    [Range(0.05f, 1f)] //slider in inspector
    public float throwForce = 0.3f;

    GameObject gnattObject;
    public GameObject aoePrefab;
    GameObject aoeObject;
    AoeSwipe aoeScript;
    

    Vector3 mousePos;
    Vector2 aoeStartPos;
    Vector2 aoeEndPos;

    private void Start() {
        aoeObject = Instantiate(aoePrefab, new Vector2(0, 0), Quaternion.identity);
        aoeScript = aoeObject.GetComponent<AoeSwipe>();
        aoeObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);

            //Raycast from camera
            if (hit && hit.collider.gameObject.CompareTag("Spark")) {
                //if raycast hits gameobject with tag "Spark" Run this code.
                Spark spark = hit.collider.GetComponent<Spark>();
                spark.Activate();
            }

            //get touch position and mark time when screen is touched
            touchTimeStart = Time.time;
            startPos = Input.mousePosition;

            //Move aoeObject for detecting gnatts in swipe
            aoeObject.SetActive(true);
            aoeScript.clearGnatts();
            Vector2 aoeStartPos = Camera.main.ScreenToWorldPoint(startPos);
            //aoeObject = Instantiate(aoePrefab, aoeStartPos, Quaternion.identity);
            aoeObject.transform.position = aoeStartPos;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);

            //Raycast from camera
            if (hit && hit.collider.gameObject.CompareTag("Spark")) {
                //if raycast hits gameobject with tag "Spark" Run this code.
                Spark spark = hit.collider.GetComponent<Spark>();
                spark.Activate();
            }

            //get touch position and mark time when screen is touched
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;

            //Move aoeObject for detecting gnatts in swipe
            aoeObject.SetActive(true);
            aoeScript.clearGnatts();
            Vector2 aoeStartPos = Camera.main.ScreenToWorldPoint(startPos);
            //aoeObject = Instantiate(aoePrefab, aoeStartPos, Quaternion.identity);
            aoeObject.transform.position = aoeStartPos;
        }

        //when finger is released
        if (Input.GetMouseButtonUp(0)){
            //get time when released
            touchTimeFinish = Time.time;
            // calculate swipe time interval
            timeInterval = touchTimeFinish - touchTimeStart;
            //get release finger position
            endPos = Input.mousePosition;
            //calculate swipe direction in 2D space
            direction = startPos - endPos;

            //Flick gnatts away
            if(startPos != endPos && !aoeScript.isGnattsEmpty()) {
                aoeScript.flickGnatts(direction, timeInterval, throwForce);
                aoeScript.clearGnatts();
            }
            aoeObject.SetActive(false);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            //get time when released
            touchTimeFinish = Time.time;
            // calculate swipe time interval
            timeInterval = touchTimeFinish - touchTimeStart;
            //get release finger position
            endPos = Input.GetTouch(0).position;
            //calculate swipe direction in 2D space
            direction = startPos - endPos;

            //Flick gnatts away
            if(startPos != endPos && !aoeScript.isGnattsEmpty()) {
                aoeScript.flickGnatts(direction, timeInterval, throwForce);
                aoeScript.clearGnatts();
            }
            aoeObject.SetActive(false);
        }
    }
}
