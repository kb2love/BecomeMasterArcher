using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private float dragRadius = 50f;
    private RectTransform orignTouchPadTr;
    private Vector3 dragStartPos = Vector3.zero;
    private int touchIdx = -1;
    public bool buttonPress = false;
    [SerializeField] private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        orignTouchPadTr = GetComponent<RectTransform>();
        dragStartPos = orignTouchPadTr.position;
    }
    public void ButtonDown()
    {
        buttonPress = true;
    }
    public void ButtonUp()
    {
        buttonPress = false;
        HandleInput(dragStartPos);
    }


    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            HandleTouchInput();
        }
        else
        {
            HandleInput(Input.mousePosition);
        }
    }
    private void HandleTouchInput()
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                i++;
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x <= (dragStartPos.x + dragRadius))
                    {
                        touchIdx = i;
                    }
                    if (touch.position.y <= (dragStartPos.y + dragRadius))
                    {
                        touchIdx = i;
                    }
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touchIdx == i)
                        HandleInput(touchPos);
                }
                if (touch.phase == TouchPhase.Ended || !buttonPress)
                {
                    if (touchIdx == i)
                        touchIdx = -1;
                }
            }
        }
    }
    private void HandleInput(Vector3 input)
    {
        if (buttonPress)
        {
            Vector3 diferVector = input - dragStartPos;
            if(diferVector.sqrMagnitude > dragRadius * dragRadius)
            {
                diferVector.Normalize();
                orignTouchPadTr.position = dragStartPos + diferVector * dragRadius;
            }
            else
            {
                orignTouchPadTr.position = input;
            }
        }
        else
        {
            orignTouchPadTr.position = dragStartPos;
        }
        Vector3 diff = orignTouchPadTr.position - dragStartPos;
        Vector3 normalDiff = new Vector3(diff.x / dragRadius,0, diff.y / dragRadius);
        playerMovement.OnStickPos(normalDiff, diff);
    }
    public void HandleOriginPos()
    {
        orignTouchPadTr.position = dragStartPos;
    }
}
