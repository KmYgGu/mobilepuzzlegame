using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularTimer : MonoBehaviour
{
    private float totalTime = 60f; // 총 시간 (초 단위)
    [SerializeField] private float remainingTime;
    private Image circularImage;

    void Start()
    {
        circularImage = GetComponentInChildren<Image>();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        remainingTime = totalTime;
        while(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            circularImage.fillAmount = remainingTime / totalTime;
            yield return null;
        }
    }

    void UpdateTimer()
    {
        // 남은 시간을 deltaTime으로 감소
        remainingTime -= Time.deltaTime;
        //remainingTime = Mathf.Clamp(remainingTime, 0, totalTime);

        // 남은 시간에 따라 채움 비율 계산
        float fillAmount = remainingTime / totalTime;

        // 원형 채움 비율 업데이트
        circularImage.fillAmount = fillAmount;

        // 남은 시간이 없을 때 fillAmount가 0이 되도록
        if (remainingTime <= 0)
        {
            circularImage.fillAmount = 0; // fillAmount를 0으로 설정
        }
    }
}
