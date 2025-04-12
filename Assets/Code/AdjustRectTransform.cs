using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustRectTransform : MonoBehaviour
{
    public RectTransform ParentRect; // 퍼즐을 담고 있는 부모 RectTransform
    public RectTransform CanvasRect;

    void Start()
    {
        AdjustRectSize();
    }

    public void AdjustRectSize()
    {
        // 화면 크기 가져오기
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;

        float screenWidth = CanvasRect.rect.width;
        float screenHeight = CanvasRect.rect.height;

        // 퍼즐 부모 RectTransform의 크기를 화면 비율에 맞춰 조정
        ParentRect.sizeDelta = new Vector2(screenWidth * 0.9f, screenWidth * 0.9f); //화면의 90% 크기 사용
        //ParentRect.sizeDelta = new Vector2(screenWidth, screenWidth);

        // 아래쪽에 5% 여백이 생기도록 위치 조정
        // Pivot을 중앙에 맞추고 여백을 계산한 후 위치 설정
        float verticalOffset = screenWidth * 0.05f; // 5% 여백
        ParentRect.anchoredPosition = new Vector2(0, verticalOffset); // Y축으로 5%만큼 위로 이동

        // Pivot 설정 (Pivot을 (0.5, 0)로 하여 아래쪽 중심에 맞춤)
        //ParentRect.pivot = new Vector2(0.5f, 0); // 중심을 아래쪽으로 설정하여 위치 보정
    }
}
