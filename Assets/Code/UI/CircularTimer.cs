using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularTimer : MonoBehaviour
{
    private float totalTime = 60f; // �� �ð� (�� ����)
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
        // ���� �ð��� deltaTime���� ����
        remainingTime -= Time.deltaTime;
        //remainingTime = Mathf.Clamp(remainingTime, 0, totalTime);

        // ���� �ð��� ���� ä�� ���� ���
        float fillAmount = remainingTime / totalTime;

        // ���� ä�� ���� ������Ʈ
        circularImage.fillAmount = fillAmount;

        // ���� �ð��� ���� �� fillAmount�� 0�� �ǵ���
        if (remainingTime <= 0)
        {
            circularImage.fillAmount = 0; // fillAmount�� 0���� ����
        }
    }
}
