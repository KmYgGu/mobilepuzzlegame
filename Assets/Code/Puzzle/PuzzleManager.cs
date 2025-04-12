using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePiecePrefab; // ���� ���� ������
    public GridLayoutGroup gridLayoutGroup; // GridLayoutGroup ������Ʈ
    public PuzzleSpriteManager puzzleSpriteManager; // ��������Ʈ�� �����ϴ� Ŭ����

    //public int gridSize = 6; // 6x6 ���� ������
    public int gridWidth = 6; // 6x12 ������ ���� ũ��
    public int gridHeight = 12; // 6x12 ������ ���� ũ��

    public PuzzlePiece[,] puzzlePieces; // 2���� �迭�� ����
    private List<PuzzleSpriteManager.PuzzleColor> selectedColors; // ���õ� 5���� ����

    public RectTransform GridLayoutGroupRect; // ������ ��� �ִ� �θ� RectTransform
    public RectTransform CanvasRect;

    public bool isPuzzleFalling = true; // ������ �������� ������ Ȯ��

    // ��Ȱ��ȭ�� ���� ������ x, y ��ǥ�� ������ ť
    private Queue<Tuple<int, int>> inactivePiecesQueue = new Queue<Tuple<int, int>>();

    //private List<Tuple <int, int>> BeMatchedPieces = new List<Tuple<int, int>>();


    //���ŵ����� �𸣴� ���� ������ ����
    public List<Tuple<int, int>> matchingList = new List<Tuple<int, int>>();

    [SerializeField] private int Score = 0;

    //private List<PuzzlePiece> matchedPieces;

    private void Awake()
    {
        //ȭ�鿡 ���� ���� �θ� ũ�� ����
        AdjustRectSize();
    }

    void Start()
    {

        // ������ ��������� ����
        if (puzzlePieces == null || puzzlePieces.Length == 0)
        {
            StartCoroutine(FillPuzzleWithFallEffect());
        }
    }

    // ���� ������ ������ ������ �����Ǵ� �ð� ȿ��
    private IEnumerator FillPuzzleWithFallEffect()
    {
        puzzlePieces = new PuzzlePiece[gridWidth, gridHeight];
        selectedColors = puzzleSpriteManager.SelectRandomColors(6); // 6���� ���� ����


        // ���� ���� �� ȭ�� ���� ��ġ��Ŵ
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = gridWidth - 1; x >= 0; x--)
            {
                // ���� ���� ����
                GameObject piece = Instantiate(puzzlePiecePrefab, gridLayoutGroup.transform);
                PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();

                // ���� ������ ���� ���� ����
                SetRandomColor(puzzlePiece);

                // ���� ������ ������ 4�� �̻� ������� �ʵ��� �˻縦 ����
                /*while (CheckForMatch(puzzlePiece, x, y))
                {
                    SetRandomColor(puzzlePiece); // �ߺ��� ������ ������ �ٽ� ����
                }*/

                puzzlePiece.Initialize(this, x, y); // �ʱ�ȭ (x, y ��ǥ �Ҵ�)

                // ȭ�� ũ�⿡ ���� GridLayoutGroup ����
                AdjustGridSize();

                // ���� ������ ���� ��ġ�� ȭ�� ���� ����
                RectTransform pieceRectTransform = piece.GetComponent<RectTransform>();

                // ũ�⸦ 120x120���� ����
                pieceRectTransform.sizeDelta = new Vector2(120, 120);

                //��ġ����
                Vector2 startPosition = new Vector2(
                    GridLayoutGroupRect.rect.width - (gridWidth - x + 2.5f) * gridLayoutGroup.cellSize.x, // X�� ��ġ
                GridLayoutGroupRect.rect.height + (gridHeight - y) * gridLayoutGroup.cellSize.y // Y�� ��ġ (ȭ�� ��)
                );
                pieceRectTransform.anchoredPosition = startPosition;

                // ���� ������ �迭�� ����
                puzzlePieces[x, y] = puzzlePiece;

                // ���� ������ �Ʒ��� ����߸��� �ִϸ��̼�
                StartCoroutine(MovePiece(puzzlePiece, x, y, 0.25f));//1f

                yield return new WaitForSeconds(0.05f); // �� ���� ������ �������� ����
                
            }
        }
        isPuzzleFalling = false; // ������ �� �������� false�� ����

    }

    //���� ������ ����
    private void SetRandomColor(PuzzlePiece puzzlePiece)
    {
        PuzzleSpriteManager.PuzzleColor randomColor = selectedColors[UnityEngine.Random.Range(0, selectedColors.Count)];
        puzzlePiece.SetPiece(puzzleSpriteManager.GetColor(randomColor));
    }

    // ���� ������ 4�� �̻� ����Ǵ��� �˻��ϴ� �Լ�
    /*private bool CheckForMatch(PuzzlePiece puzzlePiece, int x, int y)
    {
        int matchCount = 1; // ���� ������ �����ϹǷ� 1�� ����

        // �����¿� ���� ���� �˻�
        if (x > 0 && puzzlePieces[x - 1, y] != null && puzzlePieces[x - 1, y].pieceImage.color == puzzlePiece.pieceImage.color)
        {
            matchCount++;
        }
        if (x < gridWidth - 1 && puzzlePieces[x + 1, y] != null && puzzlePieces[x + 1, y].pieceImage.color == puzzlePiece.pieceImage.color)
        {
            matchCount++;
        }
        if (y > 0 && puzzlePieces[x, y - 1] != null && puzzlePieces[x, y - 1].pieceImage.color == puzzlePiece.pieceImage.color)
        {
            matchCount++;
        }
        if (y < gridHeight - 1 && puzzlePieces[x, y + 1] != null && puzzlePieces[x, y + 1].pieceImage.color == puzzlePiece.pieceImage.color)
        {
            matchCount++;
        }

        // 4�� �̻��� ���� ������ ������ �ִ��� Ȯ��
        return matchCount >= 4;
    }*/

    // ���� ������ �̵���Ű�� �޼���
    private IEnumerator MovePiece(PuzzlePiece puzzlePiece, int targetX, int targetY, float fillTime)
    {
        RectTransform pieceRectTransform = puzzlePiece.GetComponent<RectTransform>();
        Vector2 startPosition = pieceRectTransform.anchoredPosition;
        Vector2 targetPosition = GetPosition(targetX, targetY); // ��ǥ ��ġ ���

        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            pieceRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / fillTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������ ��ġ�� ��Ȯ�� ����
        pieceRectTransform.anchoredPosition = targetPosition;

        //������ ������ �������� ������ Ȯ��
        //CheckForMatches(targetX, targetY);
    }


    // ������ �׸��� ��ǥ�� ȭ���� ��Ŀ ��ǥ�� ��ȯ
    private Vector2 GetPosition(int x, int y)
    {
        return new Vector2(
            (x - 2.5f) * gridLayoutGroup.cellSize.x, // X�� ��ġ
            -(y + 1 - 9.5f) * gridLayoutGroup.cellSize.y  // Y�� ��ġ
        );
    }

    // GridLayoutGroup�� �� ũ�⸦ ȭ�� ũ�⿡ �°� ����
    private void AdjustGridSize()
    {

        // ���� ���� ������ �°� �� ũ�� ���
        float width = GridLayoutGroupRect.rect.width / gridWidth;
        float height = GridLayoutGroupRect.rect.height / gridWidth;

        gridLayoutGroup.cellSize = new Vector2(width, height);

    }

    public void AdjustRectSize()
    {
        // ȭ�� ũ�� ��������
        float screenWidth = CanvasRect.rect.width;
        float screenHeight = CanvasRect.rect.height;

        // ���� �θ� RectTransform�� ũ�⸦ ȭ�� ������ ���� ����
        GridLayoutGroupRect.sizeDelta = new Vector2(screenWidth * 0.9f, screenWidth * 0.9f); //ȭ���� 90% ũ�� ���

        // �Ʒ��ʿ� 5% ������ ���⵵�� ��ġ ����
        // Pivot�� �߾ӿ� ���߰� ������ ����� �� ��ġ ����
        float verticalOffset = screenWidth * 0.05f; // 5% ����
        GridLayoutGroupRect.anchoredPosition = new Vector2(0, verticalOffset); // Y������ 5%��ŭ ���� �̵�

    }
    public void CheckPieceImageStatus(int x, int y)
    {
        // ���� x, y ���� ť�� �߰�
        inactivePiecesQueue.Enqueue(new Tuple<int, int>(x, y));

        //BeMatchedPieces.Add(new Tuple<int, int>(x, y));
    }

    //�������� ��ĭ ������
    public void CheckAndMovePiecesDown()
    {
        // ť�� �ִ� ��� ����ִ� ���� ������ x �� �������� �׷�ȭ
        Dictionary<int, List<int>> emptyPiecesByColumn = new Dictionary<int, List<int>>();
        PuzzlePiece piecesave;

        while (inactivePiecesQueue.Count > 0)
        {

            (int emptyX, int emptyY) = inactivePiecesQueue.Dequeue();

            if (!emptyPiecesByColumn.ContainsKey(emptyX))
            {
                emptyPiecesByColumn[emptyX] = new List<int>();
            }
            emptyPiecesByColumn[emptyX].Add(emptyY);

        }

        // �� ��(x ��) ���� �� ���� ������ ó��
        foreach (var column in emptyPiecesByColumn)
        {
            int x = column.Key;
            List<int> emptyYs = column.Value;

            // �� ĭ�� �ִ� ���� ���� y ���� �����Ͽ� ���������� �Ʒ��� ó��
            emptyYs.Sort();  // ���� y���� ū y���� ������� ������ ���� ����

            foreach (int emptyY in emptyYs)
            {
                piecesave = puzzlePieces[column.Key, emptyY];
                // �� ���� ���� ���� �ִ� ���� �������� �� ĭ�� ������
                for (int y = emptyY - 1; y >= 0; y--)
                {
                    PuzzlePiece pieceAbove = puzzlePieces[x, y];

                    if (pieceAbove != null && pieceAbove.pieceImage.enabled)
                    {
                        // ���� ������ ���ο� ��ġ�� 2���� �迭�� �ݿ�
                        puzzlePieces[x, y + 1] = pieceAbove;

                        // ���� ������ ���ο� y ��ǥ ������Ʈ
                        pieceAbove.y = y + 1;

                        // ���� ������ �Ʒ��� ����߸��� �ִϸ��̼�
                        StartCoroutine(MovePiece(pieceAbove, x, y + 1, 0.5f));//0.1f
                        //CheckForMatches(pieceAbove.x, pieceAbove.y);
                    }
                }

                // �� ���� ���� �ڸ��� ���ο� ���� ������ �����Ͽ� ä��
                puzzlePieces[x, 0] = piecesave;
                puzzlePieces[x, 0].y = 0;
                // �켱 �̹����� Ȱ��ȭ
                puzzlePieces[x, 0].SetImageTrue();

                // ���� ���� ������ ���� ���� ����
                SetRandomColor(puzzlePieces[x, 0]);

                // ��ǥ ��ġ�� ����ϰ� ����
                RectTransform emptyPieceRectTransform = puzzlePieces[x, 0].GetComponent<RectTransform>();
                Vector2 lowestPosition = GetPosition(x, 0);
                emptyPieceRectTransform.anchoredPosition = lowestPosition;
            }
        }
    }
    //public List<PuzzlePiece> matchingPieces2;

    // 4������ ��Ī ���� ������ ������ �������� ����
    private Stack<Tuple<PuzzlePiece, int>> upStack = new Stack<Tuple<PuzzlePiece, int>>();
    private Stack<Tuple<PuzzlePiece, int>> downStack = new Stack<Tuple<PuzzlePiece, int>>();
    private Stack<Tuple<PuzzlePiece, int>> leftStack = new Stack<Tuple<PuzzlePiece, int>>();
    private Stack<Tuple<PuzzlePiece, int>> rightStack = new Stack<Tuple<PuzzlePiece, int>>();

    // �̹� Ž���� ���� ������ ������ HashSet
    private HashSet<PuzzlePiece> visitedPieces = new HashSet<PuzzlePiece>();

    // ���� �� ���� Ž��
    public void CheckForMatches(int startX, int startY)
    {
        if (startY < 6) return;

        PuzzlePiece startPiece = puzzlePieces[startX, startY];
        if (startPiece == null || !startPiece.pieceImage.enabled) return;

        // ������ �� �� ���� �ʱ�ȭ
        upStack.Clear();
        downStack.Clear();
        leftStack.Clear();
        rightStack.Clear();
        visitedPieces.Clear();

        // ���� ���� ������ �湮�� ������ ǥ��
        visitedPieces.Add(startPiece);

        // �� ���⿡ ���� ��Ī�� ���� ť�� Ž��
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, upStack, 0, -1, 0);    // ����, ���� 0
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, downStack, 0, 1, 1);   // �Ʒ���, ���� 1
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, leftStack, -1, 0, 2);  // ����, ���� 2
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, rightStack, 1, 0, 3);  // ������, ���� 3

        if (visitedPieces.Count >= 4)
        {
            inactivePiecesQueue.Enqueue(new Tuple<int, int>(startX, startY));

            //BeMatchedPieces.Add(new Tuple<int, int>(startX, startY));

            AddListToQueue(matchingList);
            // ���� �� ������ ã�� �ִϸ��̼� ����
            ProcessLongestStack();
        }
        else
        {
            CheckAndMovePiecesDown();
        }
        matchingList.Clear();
    }

    // ���� �� ������ ã�� �ִϸ��̼� �����ϴ� �Լ�
    private void ProcessLongestStack()
    {
        Stack<Tuple<PuzzlePiece, int>> longestStack = FindLongestStack();

        if (longestStack.Count > 0)
        {
            //Debug.Log("���� ���� ��Ī�� ���� ���� ��: " + longestStack.Count);
            AnimateMatchingPieces(longestStack);
        }
        else
        {

            //matchedPieces.Add(puzzlePieces[1, 2]);
            /*foreach (var piece in matchedPieces)
            {
                // ���� ���� ������ �߰�
                Score += puzzleSpriteManager.GetScoreForColor(piece.pieceImage.color); // piece.Color�� ���� ������ ������ ��Ÿ��
            }

            Debug.Log("�� ����: " + Score);*/
            CheckAndMovePiecesDown();
            return;
        }
    }

    // �� ������ ��Ī ���� ť�� ã�� �Լ�
    private void CheckAdjacentPiecesWithStack(int x, int y, Color targetColor, Stack<Tuple<PuzzlePiece, int>> Stack, int xOffset, int yOffset, int direction)
    {
        int newX = x + xOffset;
        int newY = y + yOffset;

        if (newX >= 0 && newX < gridWidth && newY >= 6 && newY < gridHeight)
        {
            PuzzlePiece adjacentPiece = puzzlePieces[newX, newY];

            // �̹� �湮�� ������ �ǳʶ�
            if (adjacentPiece != null && adjacentPiece.pieceImage.enabled && adjacentPiece.pieceImage.color == targetColor && !visitedPieces.Contains(adjacentPiece))
            {
                Stack.Push(new Tuple<PuzzlePiece, int>(adjacentPiece, direction));  // ���� ������ �Բ� ť�� �߰�
                visitedPieces.Add(adjacentPiece);

                matchingList.Add(new Tuple<int, int>(newX, newY));

                // 4�������� ����� ������ ��� Ž��
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, 0, -1, 0); // ����
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, 0, 1, 1);  // �Ʒ���
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, -1, 0, 2); // ����
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, 1, 0, 3);  // ������
            }
        }
    }

    private void AnimateMatchingPieces(Stack<Tuple<PuzzlePiece, int>> matchingStack)
    {
        // ���������� ť�� �ִ� ������� ȸ����Ű�� ���� �ڷ�ƾ ����
        StartCoroutine(AnimatePiecesSequentially(matchingStack));
    }

    // ť�� �ִ� ������� �ϳ��� ȸ����Ű�� �ڷ�ƾ
    private IEnumerator AnimatePiecesSequentially(Stack<Tuple<PuzzlePiece, int>> matchingStack)
    {
        while (matchingStack.Count > 0)
        {
            // ť���� ���� ������ ����(int) �� ������
            var (piece, direction) = matchingStack.Pop();

            // ���� ������ RectTransform ��������
            RectTransform pieceRectTransform = piece.GetComponent<RectTransform>();

            // ���� ��ġ ����
            Vector3 oldPosition = pieceRectTransform.anchoredPosition;
            Vector3 oldPosition2 = pieceRectTransform.anchoredPosition;

            // ȸ���� ���� ���� ����
            Quaternion startRotation = pieceRectTransform.localRotation;
            Quaternion targetRotation = Quaternion.identity;

            // ���⿡ ���� pivot �� ������ ��ġ ����
            switch (direction)
            {
                case 0: // ���� (0.5, 1), x�� 180�� ȸ��
                    pieceRectTransform.pivot = new Vector2(0.5f, 0f);
                    oldPosition.y -= gridLayoutGroup.cellSize.y * 0.5f;

                    // x�� 180�� ȸ�� ��ǥ ����
                    targetRotation = Quaternion.Euler(180f, 0f, 0f);
                    break;

                case 1: // �Ʒ��� (0.5, 0), x�� 180�� ȸ��
                    pieceRectTransform.pivot = new Vector2(0.5f, 1f);
                    oldPosition.y += gridLayoutGroup.cellSize.y * 0.5f;

                    // x�� 180�� ȸ�� ��ǥ ����
                    targetRotation = Quaternion.Euler(180f, 0f, 0f);
                    break;

                case 2: // ���� (0, 0.5), y�� 180�� ȸ��
                    pieceRectTransform.pivot = new Vector2(1f, 0.5f);
                    oldPosition.x += gridLayoutGroup.cellSize.x * 0.5f;

                    // y�� 180�� ȸ�� ��ǥ ����
                    targetRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;

                case 3: // ������ (1, 0.5), y�� 180�� ȸ��
                    pieceRectTransform.pivot = new Vector2(0f, 0.5f);
                    oldPosition.x -= gridLayoutGroup.cellSize.x * 0.5f;

                    // y�� 180�� ȸ�� ��ǥ ����
                    targetRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
            }

            // pivot ���� �� ��ġ ����
            pieceRectTransform.anchoredPosition = oldPosition;

            // ȸ�� �ִϸ��̼��� ���� ������ ���
            yield return StartCoroutine(RotatePiece(pieceRectTransform, startRotation, targetRotation, 0.2f));

            //ȸ���� �������� �ش� ���� ���� �̹��� ��Ȱ��ȭ,�ٽ� ���� ������ �ǵ�����.
            piece.pieceImage.enabled = false;
            pieceRectTransform.pivot = new Vector2(0.5f, 0.5f);
            pieceRectTransform.localEulerAngles = new Vector2(0f, 0f);
            pieceRectTransform.anchoredPosition = oldPosition2;
        }

        // ��� ��Ī ���� ó�� �� �ٽ� ���� �� ������ ã�Ƽ� ó��
        ProcessLongestStack();
    }

    // ȸ�� �ִϸ��̼� �ڷ�ƾ
    private IEnumerator RotatePiece(RectTransform pieceRectTransform, Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Lerp�� ���� �ε巴�� ȸ��
            pieceRectTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        // ���������� ��ǥ ȸ���� �°� ����
        pieceRectTransform.localRotation = targetRotation;
    }

    // ���� �� ������ ã�� �Լ�
    private Stack<Tuple<PuzzlePiece, int>> FindLongestStack()
    {
        Stack<Tuple<PuzzlePiece, int>> longestStack = upStack;

        if (downStack.Count > longestStack.Count) longestStack = downStack;
        if (leftStack.Count > longestStack.Count) longestStack = leftStack;
        if (rightStack.Count > longestStack.Count) longestStack = rightStack;

        return longestStack;
    }

    public void AddListToQueue(List<Tuple<int, int>> list)
    {
        foreach (var tuple in list)
        {

            inactivePiecesQueue.Enqueue(tuple);          
        }

        //Tuple<int, int> array = inactivePiecesQueue.ToArray();
        Debug.Log($"����Ʈ�� ��� ���� ť�� �߰��Ǿ����ϴ�. ���� ť�� ũ��: {inactivePiecesQueue.Count}");
    }

    /*public IEnumerator AddBeMatchedPieces(List<Tuple<int, int>> list)
    {
        if (BeMatchedPieces != null)
        {
            foreach (var tuple in list)
            {
                BeMatchedPieces.Add(tuple);
            }
        }
        else
        {
            yield return new WaitForFixedUpdate();
            StartCoroutine(AddBeMatchedPieces(list));
        }
        
    }*/

}
