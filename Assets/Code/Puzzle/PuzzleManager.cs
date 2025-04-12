using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePiecePrefab; // 퍼즐 조각 프리팹
    public GridLayoutGroup gridLayoutGroup; // GridLayoutGroup 컴포넌트
    public PuzzleSpriteManager puzzleSpriteManager; // 스프라이트를 관리하는 클래스

    //public int gridSize = 6; // 6x6 퍼즐 사용안함
    public int gridWidth = 6; // 6x12 퍼즐의 가로 크기
    public int gridHeight = 12; // 6x12 퍼즐의 세로 크기

    public PuzzlePiece[,] puzzlePieces; // 2차원 배열로 변경
    private List<PuzzleSpriteManager.PuzzleColor> selectedColors; // 선택된 5가지 색상

    public RectTransform GridLayoutGroupRect; // 퍼즐을 담고 있는 부모 RectTransform
    public RectTransform CanvasRect;

    public bool isPuzzleFalling = true; // 퍼즐이 떨어지는 중인지 확인

    // 비활성화된 퍼즐 조각의 x, y 좌표를 저장할 큐
    private Queue<Tuple<int, int>> inactivePiecesQueue = new Queue<Tuple<int, int>>();

    //private List<Tuple <int, int>> BeMatchedPieces = new List<Tuple<int, int>>();


    //제거될지도 모르는 퍼즐 조각를 저장
    public List<Tuple<int, int>> matchingList = new List<Tuple<int, int>>();

    [SerializeField] private int Score = 0;

    //private List<PuzzlePiece> matchedPieces;

    private void Awake()
    {
        //화면에 따라 퍼즐 부모 크기 조절
        AdjustRectSize();
    }

    void Start()
    {

        // 퍼즐이 비어있으면 생성
        if (puzzlePieces == null || puzzlePieces.Length == 0)
        {
            StartCoroutine(FillPuzzleWithFallEffect());
        }
    }

    // 퍼즐 조각이 위에서 떨어져 생성되는 시각 효과
    private IEnumerator FillPuzzleWithFallEffect()
    {
        puzzlePieces = new PuzzlePiece[gridWidth, gridHeight];
        selectedColors = puzzleSpriteManager.SelectRandomColors(6); // 6가지 색상 선택


        // 퍼즐 생성 및 화면 위에 위치시킴
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = gridWidth - 1; x >= 0; x--)
            {
                // 퍼즐 조각 생성
                GameObject piece = Instantiate(puzzlePiecePrefab, gridLayoutGroup.transform);
                PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();

                // 퍼즐 조각에 랜덤 색상 설정
                SetRandomColor(puzzlePiece);

                // 퍼즐 조각의 색상이 4개 이상 연결되지 않도록 검사를 수행
                /*while (CheckForMatch(puzzlePiece, x, y))
                {
                    SetRandomColor(puzzlePiece); // 중복이 있으면 색상을 다시 설정
                }*/

                puzzlePiece.Initialize(this, x, y); // 초기화 (x, y 좌표 할당)

                // 화면 크기에 맞춰 GridLayoutGroup 조정
                AdjustGridSize();

                // 퍼즐 조각의 시작 위치를 화면 위에 설정
                RectTransform pieceRectTransform = piece.GetComponent<RectTransform>();

                // 크기를 120x120으로 설정
                pieceRectTransform.sizeDelta = new Vector2(120, 120);

                //위치보정
                Vector2 startPosition = new Vector2(
                    GridLayoutGroupRect.rect.width - (gridWidth - x + 2.5f) * gridLayoutGroup.cellSize.x, // X축 위치
                GridLayoutGroupRect.rect.height + (gridHeight - y) * gridLayoutGroup.cellSize.y // Y축 위치 (화면 위)
                );
                pieceRectTransform.anchoredPosition = startPosition;

                // 퍼즐 조각을 배열에 저장
                puzzlePieces[x, y] = puzzlePiece;

                // 퍼즐 조각을 아래로 떨어뜨리는 애니메이션
                StartCoroutine(MovePiece(puzzlePiece, x, y, 0.25f));//1f

                yield return new WaitForSeconds(0.05f); // 각 퍼즐 조각이 떨어지는 간격
                
            }
        }
        isPuzzleFalling = false; // 퍼즐이 다 떨어지면 false로 변경

    }

    //색상 무작위 설정
    private void SetRandomColor(PuzzlePiece puzzlePiece)
    {
        PuzzleSpriteManager.PuzzleColor randomColor = selectedColors[UnityEngine.Random.Range(0, selectedColors.Count)];
        puzzlePiece.SetPiece(puzzleSpriteManager.GetColor(randomColor));
    }

    // 퍼즐 조각이 4개 이상 연결되는지 검사하는 함수
    /*private bool CheckForMatch(PuzzlePiece puzzlePiece, int x, int y)
    {
        int matchCount = 1; // 현재 퍼즐을 포함하므로 1로 시작

        // 상하좌우 인접 퍼즐 검사
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

        // 4개 이상의 같은 색상이 인접해 있는지 확인
        return matchCount >= 4;
    }*/

    // 퍼즐 조각을 이동시키는 메서드
    private IEnumerator MovePiece(PuzzlePiece puzzlePiece, int targetX, int targetY, float fillTime)
    {
        RectTransform pieceRectTransform = puzzlePiece.GetComponent<RectTransform>();
        Vector2 startPosition = pieceRectTransform.anchoredPosition;
        Vector2 targetPosition = GetPosition(targetX, targetY); // 목표 위치 계산

        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            pieceRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / fillTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 위치를 정확히 설정
        pieceRectTransform.anchoredPosition = targetPosition;

        //떨어진 조각이 맞을수도 있으니 확인
        //CheckForMatches(targetX, targetY);
    }


    // 퍼즐의 그리드 좌표를 화면의 앵커 좌표로 변환
    private Vector2 GetPosition(int x, int y)
    {
        return new Vector2(
            (x - 2.5f) * gridLayoutGroup.cellSize.x, // X축 위치
            -(y + 1 - 9.5f) * gridLayoutGroup.cellSize.y  // Y축 위치
        );
    }

    // GridLayoutGroup의 셀 크기를 화면 크기에 맞게 조정
    private void AdjustGridSize()
    {

        // 가로 세로 비율에 맞게 셀 크기 계산
        float width = GridLayoutGroupRect.rect.width / gridWidth;
        float height = GridLayoutGroupRect.rect.height / gridWidth;

        gridLayoutGroup.cellSize = new Vector2(width, height);

    }

    public void AdjustRectSize()
    {
        // 화면 크기 가져오기
        float screenWidth = CanvasRect.rect.width;
        float screenHeight = CanvasRect.rect.height;

        // 퍼즐 부모 RectTransform의 크기를 화면 비율에 맞춰 조정
        GridLayoutGroupRect.sizeDelta = new Vector2(screenWidth * 0.9f, screenWidth * 0.9f); //화면의 90% 크기 사용

        // 아래쪽에 5% 여백이 생기도록 위치 조정
        // Pivot을 중앙에 맞추고 여백을 계산한 후 위치 설정
        float verticalOffset = screenWidth * 0.05f; // 5% 여백
        GridLayoutGroupRect.anchoredPosition = new Vector2(0, verticalOffset); // Y축으로 5%만큼 위로 이동

    }
    public void CheckPieceImageStatus(int x, int y)
    {
        // 받은 x, y 값을 큐에 추가
        inactivePiecesQueue.Enqueue(new Tuple<int, int>(x, y));

        //BeMatchedPieces.Add(new Tuple<int, int>(x, y));
    }

    //퍼즐조각 한칸 내리기
    public void CheckAndMovePiecesDown()
    {
        // 큐에 있는 모든 비어있는 퍼즐 조각을 x 값 기준으로 그룹화
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

        // 각 열(x 값) 별로 빈 퍼즐 조각을 처리
        foreach (var column in emptyPiecesByColumn)
        {
            int x = column.Key;
            List<int> emptyYs = column.Value;

            // 빈 칸이 있는 열에 대해 y 값을 정렬하여 위에서부터 아래로 처리
            emptyYs.Sort();  // 작은 y부터 큰 y까지 순서대로 내리기 위해 정렬

            foreach (int emptyY in emptyYs)
            {
                piecesave = puzzlePieces[column.Key, emptyY];
                // 빈 퍼즐 조각 위에 있는 퍼즐 조각들을 한 칸씩 내리기
                for (int y = emptyY - 1; y >= 0; y--)
                {
                    PuzzlePiece pieceAbove = puzzlePieces[x, y];

                    if (pieceAbove != null && pieceAbove.pieceImage.enabled)
                    {
                        // 퍼즐 조각의 새로운 위치를 2차원 배열에 반영
                        puzzlePieces[x, y + 1] = pieceAbove;

                        // 퍼즐 조각의 새로운 y 좌표 업데이트
                        pieceAbove.y = y + 1;

                        // 퍼즐 조각을 아래로 떨어뜨리는 애니메이션
                        StartCoroutine(MovePiece(pieceAbove, x, y + 1, 0.5f));//0.1f
                        //CheckForMatches(pieceAbove.x, pieceAbove.y);
                    }
                }

                // 맨 위의 퍼즐 자리에 새로운 퍼즐 조각을 생성하여 채움
                puzzlePieces[x, 0] = piecesave;
                puzzlePieces[x, 0].y = 0;
                // 우선 이미지를 활성화
                puzzlePieces[x, 0].SetImageTrue();

                // 새로 퍼즐 조각에 랜덤 색상 지정
                SetRandomColor(puzzlePieces[x, 0]);

                // 목표 위치를 계산하고 설정
                RectTransform emptyPieceRectTransform = puzzlePieces[x, 0].GetComponent<RectTransform>();
                Vector2 lowestPosition = GetPosition(x, 0);
                emptyPieceRectTransform.anchoredPosition = lowestPosition;
            }
        }
    }
    //public List<PuzzlePiece> matchingPieces2;

    // 4방향의 매칭 퍼즐 조각을 각각의 스택으로 저장
    private Stack<Tuple<PuzzlePiece, int>> upStack = new Stack<Tuple<PuzzlePiece, int>>();
    private Stack<Tuple<PuzzlePiece, int>> downStack = new Stack<Tuple<PuzzlePiece, int>>();
    private Stack<Tuple<PuzzlePiece, int>> leftStack = new Stack<Tuple<PuzzlePiece, int>>();
    private Stack<Tuple<PuzzlePiece, int>> rightStack = new Stack<Tuple<PuzzlePiece, int>>();

    // 이미 탐색된 퍼즐 조각을 저장할 HashSet
    private HashSet<PuzzlePiece> visitedPieces = new HashSet<PuzzlePiece>();

    // 같은 색 퍼즐 탐색
    public void CheckForMatches(int startX, int startY)
    {
        if (startY < 6) return;

        PuzzlePiece startPiece = puzzlePieces[startX, startY];
        if (startPiece == null || !startPiece.pieceImage.enabled) return;

        // 시작할 때 각 스택 초기화
        upStack.Clear();
        downStack.Clear();
        leftStack.Clear();
        rightStack.Clear();
        visitedPieces.Clear();

        // 현재 시작 퍼즐은 방문한 것으로 표시
        visitedPieces.Add(startPiece);

        // 각 방향에 대해 매칭된 퍼즐 큐를 탐색
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, upStack, 0, -1, 0);    // 위쪽, 방향 0
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, downStack, 0, 1, 1);   // 아래쪽, 방향 1
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, leftStack, -1, 0, 2);  // 왼쪽, 방향 2
        CheckAdjacentPiecesWithStack(startX, startY, startPiece.pieceImage.color, rightStack, 1, 0, 3);  // 오른쪽, 방향 3

        if (visitedPieces.Count >= 4)
        {
            inactivePiecesQueue.Enqueue(new Tuple<int, int>(startX, startY));

            //BeMatchedPieces.Add(new Tuple<int, int>(startX, startY));

            AddListToQueue(matchingList);
            // 가장 긴 스택을 찾아 애니메이션 실행
            ProcessLongestStack();
        }
        else
        {
            CheckAndMovePiecesDown();
        }
        matchingList.Clear();
    }

    // 가장 긴 스택을 찾아 애니메이션 실행하는 함수
    private void ProcessLongestStack()
    {
        Stack<Tuple<PuzzlePiece, int>> longestStack = FindLongestStack();

        if (longestStack.Count > 0)
        {
            //Debug.Log("가장 많은 매칭된 퍼즐 조각 수: " + longestStack.Count);
            AnimateMatchingPieces(longestStack);
        }
        else
        {

            //matchedPieces.Add(puzzlePieces[1, 2]);
            /*foreach (var piece in matchedPieces)
            {
                // 색상에 따른 점수를 추가
                Score += puzzleSpriteManager.GetScoreForColor(piece.pieceImage.color); // piece.Color는 퍼즐 조각의 색상을 나타냄
            }

            Debug.Log("총 점수: " + Score);*/
            CheckAndMovePiecesDown();
            return;
        }
    }

    // 각 방향의 매칭 퍼즐 큐를 찾는 함수
    private void CheckAdjacentPiecesWithStack(int x, int y, Color targetColor, Stack<Tuple<PuzzlePiece, int>> Stack, int xOffset, int yOffset, int direction)
    {
        int newX = x + xOffset;
        int newY = y + yOffset;

        if (newX >= 0 && newX < gridWidth && newY >= 6 && newY < gridHeight)
        {
            PuzzlePiece adjacentPiece = puzzlePieces[newX, newY];

            // 이미 방문한 퍼즐은 건너뜀
            if (adjacentPiece != null && adjacentPiece.pieceImage.enabled && adjacentPiece.pieceImage.color == targetColor && !visitedPieces.Contains(adjacentPiece))
            {
                Stack.Push(new Tuple<PuzzlePiece, int>(adjacentPiece, direction));  // 방향 정보와 함께 큐에 추가
                visitedPieces.Add(adjacentPiece);

                matchingList.Add(new Tuple<int, int>(newX, newY));

                // 4방향으로 연결된 퍼즐을 모두 탐색
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, 0, -1, 0); // 위쪽
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, 0, 1, 1);  // 아래쪽
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, -1, 0, 2); // 왼쪽
                CheckAdjacentPiecesWithStack(newX, newY, targetColor, Stack, 1, 0, 3);  // 오른쪽
            }
        }
    }

    private void AnimateMatchingPieces(Stack<Tuple<PuzzlePiece, int>> matchingStack)
    {
        // 순차적으로 큐에 있는 퍼즐들을 회전시키기 위한 코루틴 실행
        StartCoroutine(AnimatePiecesSequentially(matchingStack));
    }

    // 큐에 있는 퍼즐들을 하나씩 회전시키는 코루틴
    private IEnumerator AnimatePiecesSequentially(Stack<Tuple<PuzzlePiece, int>> matchingStack)
    {
        while (matchingStack.Count > 0)
        {
            // 큐에서 퍼즐 조각과 방향(int) 값 꺼내기
            var (piece, direction) = matchingStack.Pop();

            // 퍼즐 조각의 RectTransform 가져오기
            RectTransform pieceRectTransform = piece.GetComponent<RectTransform>();

            // 현재 위치 저장
            Vector3 oldPosition = pieceRectTransform.anchoredPosition;
            Vector3 oldPosition2 = pieceRectTransform.anchoredPosition;

            // 회전을 위한 변수 선언
            Quaternion startRotation = pieceRectTransform.localRotation;
            Quaternion targetRotation = Quaternion.identity;

            // 방향에 따른 pivot 값 설정과 위치 조정
            switch (direction)
            {
                case 0: // 위쪽 (0.5, 1), x축 180도 회전
                    pieceRectTransform.pivot = new Vector2(0.5f, 0f);
                    oldPosition.y -= gridLayoutGroup.cellSize.y * 0.5f;

                    // x축 180도 회전 목표 설정
                    targetRotation = Quaternion.Euler(180f, 0f, 0f);
                    break;

                case 1: // 아래쪽 (0.5, 0), x축 180도 회전
                    pieceRectTransform.pivot = new Vector2(0.5f, 1f);
                    oldPosition.y += gridLayoutGroup.cellSize.y * 0.5f;

                    // x축 180도 회전 목표 설정
                    targetRotation = Quaternion.Euler(180f, 0f, 0f);
                    break;

                case 2: // 왼쪽 (0, 0.5), y축 180도 회전
                    pieceRectTransform.pivot = new Vector2(1f, 0.5f);
                    oldPosition.x += gridLayoutGroup.cellSize.x * 0.5f;

                    // y축 180도 회전 목표 설정
                    targetRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;

                case 3: // 오른쪽 (1, 0.5), y축 180도 회전
                    pieceRectTransform.pivot = new Vector2(0f, 0.5f);
                    oldPosition.x -= gridLayoutGroup.cellSize.x * 0.5f;

                    // y축 180도 회전 목표 설정
                    targetRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
            }

            // pivot 변경 후 위치 조정
            pieceRectTransform.anchoredPosition = oldPosition;

            // 회전 애니메이션이 끝날 때까지 대기
            yield return StartCoroutine(RotatePiece(pieceRectTransform, startRotation, targetRotation, 0.2f));

            //회전이 끝났으면 해당 퍼즐 조각 이미지 비활성화,다시 원래 값으로 되돌리기.
            piece.pieceImage.enabled = false;
            pieceRectTransform.pivot = new Vector2(0.5f, 0.5f);
            pieceRectTransform.localEulerAngles = new Vector2(0f, 0f);
            pieceRectTransform.anchoredPosition = oldPosition2;
        }

        // 모든 매칭 스택 처리 후 다시 가장 긴 스택을 찾아서 처리
        ProcessLongestStack();
    }

    // 회전 애니메이션 코루틴
    private IEnumerator RotatePiece(RectTransform pieceRectTransform, Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Lerp를 통해 부드럽게 회전
            pieceRectTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        // 최종적으로 목표 회전에 맞게 설정
        pieceRectTransform.localRotation = targetRotation;
    }

    // 가장 긴 스택을 찾는 함수
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
        Debug.Log($"리스트의 모든 값이 큐에 추가되었습니다. 현재 큐의 크기: {inactivePiecesQueue.Count}");
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
