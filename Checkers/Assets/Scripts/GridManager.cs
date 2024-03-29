using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject darkSquarePrefab;
    public GameObject lightSquarePrefab;
    public GameObject redCheckerPrefab;
    public GameObject whiteCheckerPrefab;
    public int rows;
    public int columns;
    public float squareSize = 1.0f;
    private List<GameObject> squares = new List<GameObject>();

    public float bottomLastRow;
    public float topLastRow;
    void Start()
    {
        GenerateCheckerboard();

    }

    void GenerateCheckerboard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject squarePrefab = (row + col) % 2 == 0 ? darkSquarePrefab : lightSquarePrefab;

                Vector2 spawnPosition = new Vector2(col , row );
                GameObject square = Instantiate(squarePrefab, spawnPosition, Quaternion.identity);
                square.transform.SetParent(transform);
                squares.Add(square);
            }
        }


    }


    public GameObject[] GetGridSquares()
    {
        return squares.ToArray();
    }
    public void PopulateCheckers()
    {
        int rowsToFill = 3;

        for (int row = 0; row < rowsToFill; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if ((row % 2 == 0 && col % 2 == 0) || (row % 2 != 0 && col % 2 != 0))
                {
                    // Place black checkers
                    Vector3 blackCheckerPosition = new Vector3(col * squareSize, row * squareSize, -0.16f);
                    Instantiate(whiteCheckerPrefab, blackCheckerPosition, Quaternion.identity);
                }
                else
                {
                    // Place red checkers
                    Vector3 redCheckerPosition = new Vector3(col * squareSize, (rows - 1 - row) * squareSize, -0.16f);
                    Instantiate(redCheckerPrefab, redCheckerPosition, Quaternion.identity);
                }
            }
        }
    }

    public Checker GetCheckerAtPosition(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.2f);

        foreach (Collider2D col in colliders)
        {
            Checker checker = col.GetComponent<Checker>();
            if (checker != null)
            {
                return checker;
            }
        }

        return null;
    }

    public float GetBottomLastRow()
    {
        bottomLastRow = 0f;

        return bottomLastRow;
    }

    public float GetTopLastRow()
    {
        topLastRow = (rows - 1) * squareSize;
       
        return topLastRow;
    }

    public bool IsWithinGridBounds(Vector2 position)
    {
        return position.x >= 0 && position.x <= 7 && position.y >= 0 && position.y <= 7;
    }
}


