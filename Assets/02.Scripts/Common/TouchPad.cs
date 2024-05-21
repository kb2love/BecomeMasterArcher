using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private float dragRadius = 50f; // �巡�� ������ ������
    private RectTransform originTouchPadTr; // ��ġ�е��� RectTransform
    private Vector3 dragStartPos = Vector3.zero; // �巡�� ���� ��ġ
    private int touchIdx = -1; // ��ġ �ε���
    public bool buttonPress = false; // ��ư�� ���ȴ��� ����
    [SerializeField] private PlayerMovement playerMovement; // �÷��̾� ������ ��ũ��Ʈ

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>(); // PlayerMovement ������Ʈ ��������
        originTouchPadTr = GetComponent<RectTransform>(); // ��ġ�е��� RectTransform ��������
        dragStartPos = originTouchPadTr.position; // �巡�� ���� ��ġ �ʱ�ȭ
    }

    public void ButtonDown()
    {
        buttonPress = true; // ��ư�� �������� ǥ��
    }

    public void ButtonUp()
    {
        buttonPress = false; // ��ư�� ������ ������ ǥ��
        HandleInput(dragStartPos); // �巡�� ���� ��ġ�� �Է� ó��
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) // �ȵ���̵� �÷����̸�
        {
            HandleTouchInput(); // ��ġ �Է� ó��
        }
        else // �ٸ� �÷����̸�
        {
            HandleInput(Input.mousePosition); // ���콺 �Է� ó��
        }
    }

    private void HandleTouchInput()
    {
        int i = 0; // ��ġ �ε��� �ʱ�ȭ
        if (Input.touchCount > 0) // ��ġ�� �����ϸ�
        {
            foreach (Touch touch in Input.touches) // ��� ��ġ�� ����
            {
                i++;
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y); // ��ġ ��ġ ����
                if (touch.phase == TouchPhase.Began) // ��ġ ���� ��
                {
                    if (touch.position.x <= (dragStartPos.x + dragRadius) && touch.position.y <= (dragStartPos.y + dragRadius)) // ��ġ ��ġ�� ��ġ�е� ���� �ȿ� ������
                    {
                        touchIdx = i; // ��ġ �ε��� ����
                    }
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) // ��ġ�� �̵��ϰų� ������ ���� ��
                {
                    if (touchIdx == i)
                        HandleInput(touchPos); // �Է� ó��
                }
                if (touch.phase == TouchPhase.Ended || !buttonPress) // ��ġ�� �����ų� ��ư�� ������ �ʾ��� ��
                {
                    if (touchIdx == i)
                        touchIdx = -1; // ��ġ �ε��� �ʱ�ȭ
                }
            }
        }
    }

    private void HandleInput(Vector3 input)
    {
        if (buttonPress) // ��ư�� ������ ��
        {
            Vector3 diffVector = input - dragStartPos; // �Է� ��ġ�� �巡�� ���� ��ġ�� ���� ���
            if (diffVector.sqrMagnitude > dragRadius * dragRadius) // �巡�� �ݰ��� �Ѿ��
            {
                diffVector.Normalize(); // ���� ����ȭ
                originTouchPadTr.position = dragStartPos + diffVector * dragRadius; // ��ġ�е� ��ġ ����
            }
            else
            {
                originTouchPadTr.position = input; // ��ġ�е� ��ġ�� �Է� ��ġ�� ����
            }
        }
        else
        {
            originTouchPadTr.position = dragStartPos; // ��ư�� ������ �ʾ����� ��ġ�е� ��ġ �ʱ�ȭ
        }

        Vector3 diff = originTouchPadTr.position - dragStartPos; // ��ġ�е� ��ġ�� �巡�� ���� ��ġ�� ���� ���
        Vector3 normalDiff = new Vector3(diff.x / dragRadius, 0, diff.y / dragRadius); // ����ȭ�� ���� ���� ���
        playerMovement.OnStickPos(normalDiff, diff); // �÷��̾� ������ ó��
    }

    public void HandleOriginPos()
    {
        originTouchPadTr.position = dragStartPos; // ��ġ�е� ��ġ �ʱ�ȭ
    }
}
