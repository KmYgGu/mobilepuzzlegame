using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image pieceImage; // 퍼즐 조각의 이미지
    private int pieceId;     // 퍼즐 조각의 ID
    public int x, y; // 퍼즐 조각의 그리드 위치
    private PuzzleManager puzzleManager;

    private Vector2 originalPosition; // 퍼즐 조각의 원래 위치
    public GameObject followObject; // 손가락을 따라갈 빈 게임 오브젝트
    private Image followImage; // 빈 게임 오브젝트의 이미지 컴포넌트
    private bool isDragging = false;
    private FollowObjectManager followObjectManager;



    // 필드: enum 값 저장
    private PuzzleSpriteManager.PuzzleColor pieceColorEnum;

    private void Awake()
    {
        if (pieceImage == null)
        {
            pieceImage = GetComponent<Image>();
        }

        // FollowObjectManager를 찾아 참조
        followObjectManager = FindObjectOfType<FollowObjectManager>();

    }

    // 퍼즐 조각의 ID와 색상 설정
    public void SetPiece(Color color)
    {
        if (pieceImage != null)
        {
            //this.pieceColorEnum = puzzleColorEnum; // enum 값 저장
            pieceImage.color = color; // 색상 적용
        }
    }

    // 퍼즐 조각의 ID 반환 (필요 시)
    public int GetPieceId()
    {
        return pieceId;
    }

    // 퍼즐 조각을 초기화하는 메서드
    public void Initialize(PuzzleManager manager, int xPos, int yPos)
    {
        puzzleManager = manager;
        x = xPos;
        y = yPos;
    }

    public void SetGridxy(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }

    // 퍼즐 조각의 그리드 좌표가 유효한 범위 내에 있는지 확인
    private bool IsWithinValidRange()
    {
        return x >= 0 && x < 6 && y >= 0 && y < 6;//puzzleManager.gridSize
    }

    public void SetImageTrue()
    {
        pieceImage.enabled = true;
    }

    // 퍼즐 조각이 눌리면 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            Debug.Log("떨어지는 중 터치 불가");
            return; // 퍼즐이 떨어지는 중이면 터치 무시
        }
        // 원래 위치 저장
        originalPosition = transform.position;

        Vector3 worldPosition = ScreenToWorldPosition(eventData);

        // PuzzleManager에서 빈 게임 오브젝트 가져옴
        followObject = followObjectManager.GetFollowObject();
        followImage = followObject.GetComponent<Image>();

        // 빈 게임 오브젝트 활성화 및 이미지 설정
        followObject.transform.position = worldPosition;
        followImage.sprite = pieceImage.sprite;
        followImage.color = pieceImage.color;// 색상 설정

        // 퍼즐 조각의 크기를 가져와 빈 게임 오브젝트의 크기 설정
        RectTransform pieceRectTransform = GetComponent<RectTransform>();
        RectTransform followRectTransform = followObject.GetComponent<RectTransform>();
        followRectTransform.sizeDelta = pieceRectTransform.sizeDelta;

        followObject.SetActive(true);

        // 원래 퍼즐 조각의 이미지를 비활성화
        pieceImage.enabled = false;

        isDragging = true;
    }

    // 드래그 중일 때 호출
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

    // 터치나 드래그가 끝나면 호출
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


        //비어있는 퍼즐 조각이 있는지 확인
        //LogIfImageInactive();

        //가까운 퍼즐 찾고 놓기
        closestPieceSet();

        // followObject 비활성화
        followObject.SetActive(false);

        // 드래그 중 상태 해제
        isDragging = false;
    }

    // 이미지가 비활성 상태라면 디버그 로깅을 하는 함수
    public void LogIfImageInactive()
    {
        if (pieceImage != null && !pieceImage.enabled)
        {
            puzzleManager.CheckPieceImageStatus(x, y);
        }
    }

    public void closestPieceSet()
    {
        // followObject의 월드 좌표 가져오기
        Vector3 releaseWorldPosition = followObject.transform.position;

        // GridLayoutGroup에서 모든 퍼즐 조각 가져오기
        PuzzlePiece closestPiece = null;
        float minDistance = float.MaxValue;

        // 퍼즐 조각들을 반복하면서 가장 가까운 조각 찾기
        foreach (PuzzlePiece piece in puzzleManager.puzzlePieces)
        {
            Vector3 pieceWorldPosition = piece.GetComponent<RectTransform>().position;
            float distance = Vector3.Distance(releaseWorldPosition, pieceWorldPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestPiece = piece;
            }
        }

        // 가까운 퍼즐 조각이 있으면 그 퍼즐 조각의 색을 followObject의 색으로 변경
        if (closestPiece != null)
        {
            // followObject의 색상 정보 가져오기 (PuzzlePiece 클래스에 color나 sprite 정보가 있다고 가정)
            Color followColor = followObject.GetComponent<Image>().color;

            // 가장 가까운 퍼즐 조각의 색상 변경
            closestPiece.GetComponent<Image>().color = followColor;

            //빈 퍼즐 조각 위치 보내기
            puzzleManager.CheckPieceImageStatus(x, y);


            //퍼즐 맞추는 위치 보내기
            puzzleManager.CheckForMatches(closestPiece.x, closestPiece.y);


            //Debug.Log("색을 바꿨고, 놓은 퍼즐 주변으로 탐색합니다.");
        }
        else
        {
            Debug.Log("No closest piece found.");
        }
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
