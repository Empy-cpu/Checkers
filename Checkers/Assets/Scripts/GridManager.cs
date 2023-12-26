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

                Vector2 spawnPosition = new Vector2(col * squareSize, row * squareSize);
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
                    Vector3 blackCheckerPosition = new Vector3(col * squareSize, row * squareSize, -0.19f);
                    Instantiate(whiteCheckerPrefab, blackCheckerPosition, Quaternion.identity);
                }
                else
                {
                    // Place red checkers
                    Vector3 redCheckerPosition = new Vector3(col * squareSize, (rows - 1 - row) * squareSize, -0.19f);
                    Instantiate(redCheckerPrefab, redCheckerPosition, Quaternion.identity);
                }
            }
        }
    }
}


