using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private float dragRadius = 50f; // 드래그 가능한 반지름
    private RectTransform originTouchPadTr; // 터치패드의 RectTransform
    private Vector3 dragStartPos = Vector3.zero; // 드래그 시작 위치
    private int touchIdx = -1; // 터치 인덱스
    public bool buttonPress = false; // 버튼이 눌렸는지 여부
    [SerializeField] private PlayerMovement playerMovement; // 플레이어 움직임 스크립트

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>(); // PlayerMovement 컴포넌트 가져오기
        originTouchPadTr = GetComponent<RectTransform>(); // 터치패드의 RectTransform 가져오기
        dragStartPos = originTouchPadTr.position; // 드래그 시작 위치 초기화
    }

    public void ButtonDown()
    {
        buttonPress = true; // 버튼이 눌렸음을 표시
    }

    public void ButtonUp()
    {
        buttonPress = false; // 버튼이 눌리지 않음을 표시
        HandleInput(dragStartPos); // 드래그 시작 위치로 입력 처리
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) // 안드로이드 플랫폼이면
        {
            HandleTouchInput(); // 터치 입력 처리
        }
        else // 다른 플랫폼이면
        {
            HandleInput(Input.mousePosition); // 마우스 입력 처리
        }
    }

    private void HandleTouchInput()
    {
        int i = 0; // 터치 인덱스 초기화
        if (Input.touchCount > 0) // 터치가 존재하면
        {
            foreach (Touch touch in Input.touches) // 모든 터치에 대해
            {
                i++;
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y); // 터치 위치 설정
                if (touch.phase == TouchPhase.Began) // 터치 시작 시
                {
                    if (touch.position.x <= (dragStartPos.x + dragRadius) && touch.position.y <= (dragStartPos.y + dragRadius)) // 터치 위치가 터치패드 범위 안에 있으면
                    {
                        touchIdx = i; // 터치 인덱스 설정
                    }
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) // 터치가 이동하거나 정지해 있을 때
                {
                    if (touchIdx == i)
                        HandleInput(touchPos); // 입력 처리
                }
                if (touch.phase == TouchPhase.Ended || !buttonPress) // 터치가 끝났거나 버튼이 눌리지 않았을 때
                {
                    if (touchIdx == i)
                        touchIdx = -1; // 터치 인덱스 초기화
                }
            }
        }
    }

    private void HandleInput(Vector3 input)
    {
        if (buttonPress) // 버튼이 눌렸을 때
        {
            Vector3 diffVector = input - dragStartPos; // 입력 위치와 드래그 시작 위치의 차이 계산
            if (diffVector.sqrMagnitude > dragRadius * dragRadius) // 드래그 반경을 넘어가면
            {
                diffVector.Normalize(); // 벡터 정규화
                originTouchPadTr.position = dragStartPos + diffVector * dragRadius; // 터치패드 위치 설정
            }
            else
            {
                originTouchPadTr.position = input; // 터치패드 위치를 입력 위치로 설정
            }
        }
        else
        {
            originTouchPadTr.position = dragStartPos; // 버튼이 눌리지 않았으면 터치패드 위치 초기화
        }

        Vector3 diff = originTouchPadTr.position - dragStartPos; // 터치패드 위치와 드래그 시작 위치의 차이 계산
        Vector3 normalDiff = new Vector3(diff.x / dragRadius, 0, diff.y / dragRadius); // 정규화된 차이 벡터 계산
        playerMovement.OnStickPos(normalDiff, diff); // 플레이어 움직임 처리
    }

    public void HandleOriginPos()
    {
        originTouchPadTr.position = dragStartPos; // 터치패드 위치 초기화
    }
}
