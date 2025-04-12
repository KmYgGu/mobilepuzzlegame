using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StopButton : MonoBehaviour, IPointerDownHandler
{
    public RectTransform CanvasRect;//ĵ�۽��� Rect���� ������
    public RectTransform ButtonRect;

    void start()
    {
        AdjustRectSize();
    }
    public void AdjustRectSize()
    {
        // ȭ�� ũ�� ��������
        float screenWidth = CanvasRect.rect.width;

        // ���� �θ� RectTransform�� ũ�⸦ ȭ�� ������ ���� ����
        ButtonRect.sizeDelta = new Vector2(screenWidth * ButtonRect.rect.width, screenWidth * ButtonRect.rect.width); //ȭ�鿡 �°� ũ�� ����

        // �Ʒ��ʿ� 5% ������ ���⵵�� ��ġ ����
        float verticalOffset = screenWidth * 0.05f; // 5% ����
        ButtonRect.anchoredPosition = new Vector2(0, verticalOffset); // Y������ 5%��ŭ ���� �̵�

        

    }
    // ��ġ�� �巡�װ� ������ ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Time.timeScale == 0)
        {
            Debug.Log("��ư ��������");
            Time.timeScale = 1;
        }
        else
        {
            Debug.Log("��ư ����");
            Time.timeScale = 0;
        }

    }
}
