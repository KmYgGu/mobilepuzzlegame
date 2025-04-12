using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUi : MonoBehaviour
{
    public RectTransform CanvasRect;
    public RectTransform PuzzleFieldRect;
    //public GridLayoutGroup gridLayoutGroup;
    private RectTransform ThisRect;

    PuzzleManager puzzleManager;

    public int Rectno = 0;
    void Start()
    {
        ThisRect = GetComponent<RectTransform>();
        AdjustRectSize();
    }

    public void AdjustRectSize()
    {
        // ȭ�� ũ�� ��������
        float screenWidth = CanvasRect.rect.width;
        float screenHeight = CanvasRect.rect.height;

        //float PuzzleFieldHeight = PuzzleFieldRect.rect.height;

        // ���� �θ� RectTransform�� ũ�⸦ ȭ�� ������ ���� ����
        ThisRect.sizeDelta = new Vector2(screenWidth * 0.5f, screenWidth * 0.5f); //ȭ���� 90% ũ�� ���

        // �Ʒ��ʿ� 5% ������ ���⵵�� ��ġ ����
        // Pivot�� �߾ӿ� ���߰� ������ ����� �� ��ġ ����
        //float verticalOffset = screenWidth * 0.05f; // 5% ����

        switch (Rectno)
        {
            case 1: ThisRect.anchoredPosition = new Vector2(0, -(screenHeight / 2));
                break;

            case 2: ThisRect.anchoredPosition = new Vector2(-screenWidth / 2, -(screenHeight / 2));
                break;

            case 3: ThisRect.anchoredPosition = new Vector2(-screenWidth / 2, PuzzleFieldRect.rect.height + screenWidth * 0.05f);//screenWidth * 0.05f
                break;

            case 4:
                ThisRect.anchoredPosition = new Vector2(0, -(screenHeight / 2));
                break;

            case 5:
                ThisRect.anchoredPosition = new Vector2(screenWidth / 2, -(screenHeight / 2));
                break;

            case 6:
                ThisRect.anchoredPosition = new Vector2(screenWidth / 2, PuzzleFieldRect.rect.height+ screenWidth * 0.05f);
                break;
        }

    }
}
