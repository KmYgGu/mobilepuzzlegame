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
        // 화면 크기 가져오기
        float screenWidth = CanvasRect.rect.width;
        float screenHeight = CanvasRect.rect.height;

        //float PuzzleFieldHeight = PuzzleFieldRect.rect.height;

        // 퍼즐 부모 RectTransform의 크기를 화면 비율에 맞춰 조정
        ThisRect.sizeDelta = new Vector2(screenWidth * 0.5f, screenWidth * 0.5f); //화면의 90% 크기 사용

        // 아래쪽에 5% 여백이 생기도록 위치 조정
        // Pivot을 중앙에 맞추고 여백을 계산한 후 위치 설정
        //float verticalOffset = screenWidth * 0.05f; // 5% 여백

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
