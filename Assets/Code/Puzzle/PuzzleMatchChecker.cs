using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMatchChecker : MonoBehaviour
{
    private PuzzleManager puzzleManager;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>(); // PuzzleManager ����
    }

    // ���� ������ x, y ��ǥ�� �޾Ƽ� �ֺ��� ���� �� ������� Ž��
    public void CheckForMatches(int startX, int startY)
    {
        // Ž�� ���� ����: y�� 6 �̻� Ž��
        if (startY < 6)
            return;

        PuzzlePiece startPiece = puzzleManager.puzzlePieces[startX, startY];
        if (startPiece == null || !startPiece.pieceImage.enabled)
            return;

        List<PuzzlePiece> matchingPieces = new List<PuzzlePiece>();
        matchingPieces.Add(startPiece);

        // �����¿� ���� Ž��
        CheckAdjacentPieces(startX, startY, matchingPieces, startPiece.pieceImage.color);

        // �� 4�� �̻��� ���� ������ �ִٸ� ��Ȱ��ȭ ó��
        if (matchingPieces.Count >= 4)
        {
            foreach (var piece in matchingPieces)
            {
                piece.pieceImage.enabled = false;
            }
        }
    }

    // �����¿�� ���� �� ������ ��������� Ž��
    private void CheckAdjacentPieces(int x, int y, List<PuzzlePiece> matchingPieces, Color targetColor)
    {
        // �����¿� ��ǥ
        int[] xOffset = { 0, 0, -1, 1 };
        int[] yOffset = { -1, 1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + xOffset[i];
            int newY = y + yOffset[i];

            // ���� ������ ������� üũ + y ������ 6 �̻����� ����
            if (newX >= 0 && newX < puzzleManager.gridWidth && newY >= 6 && newY < puzzleManager.gridHeight)
            {
                PuzzlePiece adjacentPiece = puzzleManager.puzzlePieces[newX, newY];

                // ���� ���� ���� �������� üũ�ϰ�, ���� Ž������ ���� ���� ó��
                if (adjacentPiece != null && adjacentPiece.pieceImage.enabled && adjacentPiece.pieceImage.color == targetColor && !matchingPieces.Contains(adjacentPiece))
                {
                    matchingPieces.Add(adjacentPiece);
                    CheckAdjacentPieces(newX, newY, matchingPieces, targetColor); // ��� ȣ��
                }
            }
        }
    }
}
