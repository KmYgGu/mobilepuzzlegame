using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image pieceImage; // ���� ������ �̹���
    private int pieceId;     // ���� ������ ID
    public int x, y; // ���� ������ �׸��� ��ġ
    private PuzzleManager puzzleManager;

    private Vector2 originalPosition; // ���� ������ ���� ��ġ
    public GameObject followObject; // �հ����� ���� �� ���� ������Ʈ
    private Image followImage; // �� ���� ������Ʈ�� �̹��� ������Ʈ
    private bool isDragging = false;
    private FollowObjectManager followObjectManager;



    // �ʵ�: enum �� ����
    private PuzzleSpriteManager.PuzzleColor pieceColorEnum;

    private void Awake()
    {
        if (pieceImage == null)
        {
            pieceImage = GetComponent<Image>();
        }

        // FollowObjectManager�� ã�� ����
        followObjectManager = FindObjectOfType<FollowObjectManager>();

    }

    // ���� ������ ID�� ���� ����
    public void SetPiece(Color color)
    {
        if (pieceImage != null)
        {
            //this.pieceColorEnum = puzzleColorEnum; // enum �� ����
            pieceImage.color = color; // ���� ����
        }
    }

    // ���� ������ ID ��ȯ (�ʿ� ��)
    public int GetPieceId()
    {
        return pieceId;
    }

    // ���� ������ �ʱ�ȭ�ϴ� �޼���
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

    // ���� ������ �׸��� ��ǥ�� ��ȿ�� ���� ���� �ִ��� Ȯ��
    private bool IsWithinValidRange()
    {
        return x >= 0 && x < 6 && y >= 0 && y < 6;//puzzleManager.gridSize
    }

    public void SetImageTrue()
    {
        pieceImage.enabled = true;
    }

    // ���� ������ ������ ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            Debug.Log("�������� �� ��ġ �Ұ�");
            return; // ������ �������� ���̸� ��ġ ����
        }
        // ���� ��ġ ����
        originalPosition = transform.position;

        Vector3 worldPosition = ScreenToWorldPosition(eventData);

        // PuzzleManager���� �� ���� ������Ʈ ������
        followObject = followObjectManager.GetFollowObject();
        followImage = followObject.GetComponent<Image>();

        // �� ���� ������Ʈ Ȱ��ȭ �� �̹��� ����
        followObject.transform.position = worldPosition;
        followImage.sprite = pieceImage.sprite;
        followImage.color = pieceImage.color;// ���� ����

        // ���� ������ ũ�⸦ ������ �� ���� ������Ʈ�� ũ�� ����
        RectTransform pieceRectTransform = GetComponent<RectTransform>();
        RectTransform followRectTransform = followObject.GetComponent<RectTransform>();
        followRectTransform.sizeDelta = pieceRectTransform.sizeDelta;

        followObject.SetActive(true);

        // ���� ���� ������ �̹����� ��Ȱ��ȭ
        pieceImage.enabled = false;

        isDragging = true;
    }

    // �巡�� ���� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            return; // ������ �������� ���̸� ��ġ ����
        }
        if (isDragging)
        {

            // �հ����� ��ġ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = ScreenToWorldPosition(eventData);

            // followObject�� ��ġ�� �հ��� ��ġ�� ����
            followObject.transform.position = worldPosition;
        }
    }

    // ��ġ�� �巡�װ� ������ ȣ��
    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsWithinValidRange())
        {
            return;
        }
        if (puzzleManager.isPuzzleFalling)
        {
            return; // ������ �������� ���̸� ��ġ ����
        }


        //����ִ� ���� ������ �ִ��� Ȯ��
        //LogIfImageInactive();

        //����� ���� ã�� ����
        closestPieceSet();

        // followObject ��Ȱ��ȭ
        followObject.SetActive(false);

        // �巡�� �� ���� ����
        isDragging = false;
    }

    // �̹����� ��Ȱ�� ���¶�� ����� �α��� �ϴ� �Լ�
    public void LogIfImageInactive()
    {
        if (pieceImage != null && !pieceImage.enabled)
        {
            puzzleManager.CheckPieceImageStatus(x, y);
        }
    }

    public void closestPieceSet()
    {
        // followObject�� ���� ��ǥ ��������
        Vector3 releaseWorldPosition = followObject.transform.position;

        // GridLayoutGroup���� ��� ���� ���� ��������
        PuzzlePiece closestPiece = null;
        float minDistance = float.MaxValue;

        // ���� �������� �ݺ��ϸ鼭 ���� ����� ���� ã��
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

        // ����� ���� ������ ������ �� ���� ������ ���� followObject�� ������ ����
        if (closestPiece != null)
        {
            // followObject�� ���� ���� �������� (PuzzlePiece Ŭ������ color�� sprite ������ �ִٰ� ����)
            Color followColor = followObject.GetComponent<Image>().color;

            // ���� ����� ���� ������ ���� ����
            closestPiece.GetComponent<Image>().color = followColor;

            //�� ���� ���� ��ġ ������
            puzzleManager.CheckPieceImageStatus(x, y);


            //���� ���ߴ� ��ġ ������
            puzzleManager.CheckForMatches(closestPiece.x, closestPiece.y);


            //Debug.Log("���� �ٲ��, ���� ���� �ֺ����� Ž���մϴ�.");
        }
        else
        {
            Debug.Log("No closest piece found.");
        }
    }

    private Vector3 ScreenToWorldPosition(PointerEventData eventData)
    {
        //���� ��ǥ�� �ٲٱ�
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
