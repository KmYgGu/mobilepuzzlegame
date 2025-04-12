using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustRectTransform : MonoBehaviour
{
    public RectTransform ParentRect; // ������ ��� �ִ� �θ� RectTransform
    public RectTransform CanvasRect;

    void Start()
    {
        AdjustRectSize();
    }

    public void AdjustRectSize()
    {
        // ȭ�� ũ�� ��������
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;

        float screenWidth = CanvasRect.rect.width;
        float screenHeight = CanvasRect.rect.height;

        // ���� �θ� RectTransform�� ũ�⸦ ȭ�� ������ ���� ����
        ParentRect.sizeDelta = new Vector2(screenWidth * 0.9f, screenWidth * 0.9f); //ȭ���� 90% ũ�� ���
        //ParentRect.sizeDelta = new Vector2(screenWidth, screenWidth);

        // �Ʒ��ʿ� 5% ������ ���⵵�� ��ġ ����
        // Pivot�� �߾ӿ� ���߰� ������ ����� �� ��ġ ����
        float verticalOffset = screenWidth * 0.05f; // 5% ����
        ParentRect.anchoredPosition = new Vector2(0, verticalOffset); // Y������ 5%��ŭ ���� �̵�

        // Pivot ���� (Pivot�� (0.5, 0)�� �Ͽ� �Ʒ��� �߽ɿ� ����)
        //ParentRect.pivot = new Vector2(0.5f, 0); // �߽��� �Ʒ������� �����Ͽ� ��ġ ����
    }
}
