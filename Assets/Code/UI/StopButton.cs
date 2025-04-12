using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StopButton : MonoBehaviour, IPointerDownHandler
{
    public RectTransform CanvasRect;//캔퍼스의 Rect값을 가져옴
    public RectTransform ButtonRect;

    void start()
    {
        AdjustRectSize();
    }
    public void AdjustRectSize()
    {
        // 화면 크기 가져오기
        float screenWidth = CanvasRect.rect.width;

        // 퍼즐 부모 RectTransform의 크기를 화면 비율에 맞춰 조정
        ButtonRect.sizeDelta = new Vector2(screenWidth * ButtonRect.rect.width, screenWidth * ButtonRect.rect.width); //화면에 맞게 크기 조정

        // 아래쪽에 5% 여백이 생기도록 위치 조정
        float verticalOffset = screenWidth * 0.05f; // 5% 여백
        ButtonRect.anchoredPosition = new Vector2(0, verticalOffset); // Y축으로 5%만큼 위로 이동

        

    }
    // 터치나 드래그가 끝나면 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Time.timeScale == 0)
        {
            Debug.Log("버튼 정지해제");
            Time.timeScale = 1;
        }
        else
        {
            Debug.Log("버튼 정지");
            Time.timeScale = 0;
        }

    }
}
