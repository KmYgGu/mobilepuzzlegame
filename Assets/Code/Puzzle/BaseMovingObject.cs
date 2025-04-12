using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseMovingObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image pieceImage; // 해당 오브젝트의 이미지
    public int x, y; // 퍼즐 조각의 그리드 위치
    private PuzzleManager puzzleManager;

    //private Vector2 originalPosition; // 퍼즐 조각의 원래 위치
    public GameObject followObject; // 손가락을 따라갈 빈 게임 오브젝트
    private Image followImage; // 빈 게임 오브젝트의 이미지 컴포넌트
    private bool isDragging = false;
    private FollowObjectManager followObjectManager;

    private void Awake()
    {
        if (pieceImage == null)
        {
            pieceImage = GetComponent<Image>();
        }
        // FollowObjectManager를 찾아 참조
        followObjectManager = FindObjectOfType<FollowObjectManager>();

    }

    //클릭하면 들고 있기
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            //Debug.Log("떨어지는 중 터치 불가");
            return; // 퍼즐이 떨어지는 중이면 터치 무시
        }
        // 원래 위치 저장
        //originalPosition = transform.position;

        Vector3 worldPosition = ScreenToWorldPosition(eventData);

        // PuzzleManager에서 빈 게임 오브젝트 가져옴
        followObject = followObjectManager.GetFollowObject();
        followImage = followObject.GetComponent<Image>();

        // 빈 게임 오브젝트 활성화 및 이미지 설정
        followObject.transform.position = worldPosition;
        //followImage.sprite = pieceImage.sprite;
        //followImage.color = pieceImage.color;// 색상 설정

        // 퍼즐 조각의 크기를 가져와 빈 게임 오브젝트의 크기 설정
        RectTransform pieceRectTransform = GetComponent<RectTransform>();
        RectTransform followRectTransform = followObject.GetComponent<RectTransform>();
        followRectTransform.sizeDelta = pieceRectTransform.sizeDelta;

        followObject.SetActive(true);

        // 원래 퍼즐 조각의 이미지를 비활성화
        pieceImage.enabled = false;

        isDragging = true;
    }

    //드래그 했을때
    public void OnDrag(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            return; // 퍼즐이 떨어지는 중이면 터치 무시
        }
        if (isDragging)
        {

            // 손가락의 위치를 월드 좌표로 변환
            Vector3 worldPosition = ScreenToWorldPosition(eventData);

            // followObject의 위치를 손가락 위치로 설정
            followObject.transform.position = worldPosition;
        }
    }

    //마우스를 뗐을 때,
    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            return; // 퍼즐이 떨어지는 중이면 터치 무시
        }

        // followObject 비활성화
        followObject.SetActive(false);

        // 드래그 중 상태 해제
        isDragging = false;
    }

    // 퍼즐 조각의 그리드 좌표가 유효한 범위 내에 있는지 확인
    private bool IsWithinValidRange()
    {
        return x >= 0 && x < 6 && y >= 0 && y < 6;//puzzleManager.gridSize
    }

    private Vector3 ScreenToWorldPosition(PointerEventData eventData)
    {
        //월드 좌표로 바꾸기
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
