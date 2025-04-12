using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseMovingObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image pieceImage; // �ش� ������Ʈ�� �̹���
    public int x, y; // ���� ������ �׸��� ��ġ
    private PuzzleManager puzzleManager;

    //private Vector2 originalPosition; // ���� ������ ���� ��ġ
    public GameObject followObject; // �հ����� ���� �� ���� ������Ʈ
    private Image followImage; // �� ���� ������Ʈ�� �̹��� ������Ʈ
    private bool isDragging = false;
    private FollowObjectManager followObjectManager;

    private void Awake()
    {
        if (pieceImage == null)
        {
            pieceImage = GetComponent<Image>();
        }
        // FollowObjectManager�� ã�� ����
        followObjectManager = FindObjectOfType<FollowObjectManager>();

    }

    //Ŭ���ϸ� ��� �ֱ�
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            //Debug.Log("�������� �� ��ġ �Ұ�");
            return; // ������ �������� ���̸� ��ġ ����
        }
        // ���� ��ġ ����
        //originalPosition = transform.position;

        Vector3 worldPosition = ScreenToWorldPosition(eventData);

        // PuzzleManager���� �� ���� ������Ʈ ������
        followObject = followObjectManager.GetFollowObject();
        followImage = followObject.GetComponent<Image>();

        // �� ���� ������Ʈ Ȱ��ȭ �� �̹��� ����
        followObject.transform.position = worldPosition;
        //followImage.sprite = pieceImage.sprite;
        //followImage.color = pieceImage.color;// ���� ����

        // ���� ������ ũ�⸦ ������ �� ���� ������Ʈ�� ũ�� ����
        RectTransform pieceRectTransform = GetComponent<RectTransform>();
        RectTransform followRectTransform = followObject.GetComponent<RectTransform>();
        followRectTransform.sizeDelta = pieceRectTransform.sizeDelta;

        followObject.SetActive(true);

        // ���� ���� ������ �̹����� ��Ȱ��ȭ
        pieceImage.enabled = false;

        isDragging = true;
    }

    //�巡�� ������
    public void OnDrag(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            return; // ������ �������� ���̸� ��ġ ����
        }
        if (isDragging)
        {

            // �հ����� ��ġ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = ScreenToWorldPosition(eventData);

            // followObject�� ��ġ�� �հ��� ��ġ�� ����
            followObject.transform.position = worldPosition;
        }
    }

    //���콺�� ���� ��,
    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            return; // ������ �������� ���̸� ��ġ ����
        }

        // followObject ��Ȱ��ȭ
        followObject.SetActive(false);

        // �巡�� �� ���� ����
        isDragging = false;
    }

    // ���� ������ �׸��� ��ǥ�� ��ȿ�� ���� ���� �ִ��� Ȯ��
    private bool IsWithinValidRange()
    {
        return x >= 0 && x < 6 && y >= 0 && y < 6;//puzzleManager.gridSize
    }

    private Vector3 ScreenToWorldPosition(PointerEventData eventData)
    {
        //���� ��ǥ�� �ٲٱ�
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out worldPosition
        );
        return worldPosition;
    }
}
