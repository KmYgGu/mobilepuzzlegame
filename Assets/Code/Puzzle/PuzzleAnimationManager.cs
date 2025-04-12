using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleAnimationManager : MonoBehaviour
{
    // ���� ������ �̵���Ű�� �޼���
    public IEnumerator MovePiece(PuzzlePiece puzzlePiece, int targetX, int targetY, float fillTime, GridLayoutGroup gridLayoutGroup)
    {
        RectTransform pieceRectTransform = puzzlePiece.GetComponent<RectTransform>();
        Vector2 startPosition = pieceRectTransform.anchoredPosition;

        // ��ǥ ��ġ�� ����� �� GridLayoutGroup�� ����
        Vector2 targetPosition = GetPosition(targetX, targetY, gridLayoutGroup);

        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            pieceRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / fillTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ������ ��ġ�� ��Ȯ�� ����
        pieceRectTransform.anchoredPosition = targetPosition;
    }

    // ������ �׸��� ��ǥ�� ȭ���� ��Ŀ ��ǥ�� ��ȯ
    public Vector2 GetPosition(int x, int y, GridLayoutGroup gridLayoutGroup)
    {
        return new Vector2(
            (x - 2.5f) * gridLayoutGroup.cellSize.x, // X�� ��ġ
            -(y + 1 - 3.5f) * gridLayoutGroup.cellSize.y  // Y�� ��ġ
        );
    }

    // ������ ������ ������ �����Ǵ� ȿ�� (PuzzleManager�κ��� ȣ��)
    /*public IEnumerator FillPuzzleWithFallEffect(PuzzleManager puzzleManager, GridLayoutGroup gridLayoutGroup, List<PuzzleSpriteManager.PuzzleColor> selectedColors)
    {
        int gridSize = puzzleManager.gridSize;
        PuzzlePiece[,] puzzlePieces = puzzleManager.puzzlePieces;

        for (int y = gridSize - 1; y >= 0; y--)
        {
            for (int x = gridSize - 1; x >= 0; x--)
            {
                GameObject piece = Instantiate(puzzleManager.puzzlePiecePrefab, gridLayoutGroup.transform);
                PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();

                // ���� ������ ���� ���� ����
                PuzzleSpriteManager.PuzzleColor randomColor = selectedColors[Random.Range(0, selectedColors.Count)];
                //puzzlePiece.SetPiece(x, y, puzzleManager.puzzleSpriteManager.GetColor(randomColor));
                //puzzlePiece.Initialize(puzzleManager, x, y); // �ʱ�ȭ (x, y ��ǥ �Ҵ�)

                RectTransform pieceRectTransform = piece.GetComponent<RectTransform>();
                pieceRectTransform.sizeDelta = new Vector2(120, 120);

                Vector2 startPosition = new Vector2(
                    gridLayoutGroup.GetComponent<RectTransform>().rect.width - (gridSize - x + 2.5f) * gridLayoutGroup.cellSize.x,
                    gridLayoutGroup.GetComponent<RectTransform>().rect.height + (gridSize - y) * gridLayoutGroup.cellSize.y
                );
                pieceRectTransform.anchoredPosition = startPosition;

                puzzlePieces[x, y] = puzzlePiece;

                // MovePiece ȣ�� �� GridLayoutGroup ����
                StartCoroutine(MovePiece(puzzlePiece, x, y, 1f, gridLayoutGroup));

                yield return new WaitForSeconds(0.05f);
            }
        }
    }*/

}
