using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public Color selectedTint;
    private Color originalColor;
    public Color defaultTileColor;
    private SpriteRenderer checkerColor;

    private CheckerManager checkerManager;
    private GridManager gridManager;
    [SerializeField] private bool isSelected = false;
    private Vector2[] validDiagonalPositions;

    private bool isPlayerChecker;
    void Start()
    {
        checkerColor=GetComponent<SpriteRenderer>();
        originalColor = checkerColor.color;
        checkerManager = FindObjectOfType<CheckerManager>();
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                Vector2 clickedPosition = hit.collider.transform.position;
                HandleTileClick(clickedPosition);
            }
        }
    }
    void OnMouseDown()
    {
        if (!isSelected)
        {
            checkerManager.SelectChecker(this);
            if (gameObject.CompareTag("BlackChecker"))
            {
                isPlayerChecker = true;
            }
            else if (gameObject.CompareTag("RedChecker"))
            {
                isPlayerChecker = false;
            }

          
        }
     
    }

    public void DeselectChecker()
    {
        isSelected = false;
        checkerColor.color = originalColor;
       ResetSquareColors();
    }

    public void SelectChecker()
    {
        isSelected = true;
        checkerColor.color = selectedTint;
        Vector2[] diagonals = CalculateValidDiagonalPositions(isPlayerChecker);
        HighLightSquares(diagonals);

    }
    public Vector2[] CalculateValidDiagonalPositions(bool isPlayerChecker)
    {
        validDiagonalPositions = new Vector2[4]; 

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

        int forward = isPlayerChecker ? 1 : -1; // Adjust the forward direction based on side

        // Calculate forward-left diagonals (two squares)
        validDiagonalPositions[0] = new Vector2(currentPosition.x - forward, currentPosition.y + forward);
        validDiagonalPositions[1] = new Vector2(currentPosition.x - forward * 2, currentPosition.y + forward * 2);

        // Calculate forward-right diagonals (two squares)
        validDiagonalPositions[2] = new Vector2(currentPosition.x + forward, currentPosition.y + forward);
        validDiagonalPositions[3] = new Vector2(currentPosition.x + forward * 2, currentPosition.y + forward * 2);

        return validDiagonalPositions;
    }

    void HighLightSquares(Vector2[] diagonalPositions)
    {
        GameObject[] squares = gridManager.GetGridSquares();
        foreach (Vector2 diagonalPos in diagonalPositions)
        {
            foreach (GameObject square in squares)
            {
                Vector2 squarePosition = new Vector2(square.transform.position.x, square.transform.position.y);
                if (squarePosition == diagonalPos)
                {
                    SpriteRenderer squareRenderer = square.GetComponent<SpriteRenderer>();
                    if (squareRenderer != null)
                    {
                        squareRenderer.color = selectedTint;
                    }
                }
            }
        }
    }

    void ResetSquareColors()
    {
        Vector2[] diagonals = CalculateValidDiagonalPositions(isPlayerChecker);
        GameObject[] squares = gridManager.GetGridSquares();

        foreach (Vector2 diagonalPos in diagonals)
        {
            foreach (GameObject square in squares)
            {
                Vector2 squarePosition = new Vector2(square.transform.position.x, square.transform.position.y);
                if (squarePosition == diagonalPos)
                {
                    SpriteRenderer squareRenderer = square.GetComponent<SpriteRenderer>();
                    if (squareRenderer != null)
                    {
                        squareRenderer.color = defaultTileColor;
                    }
                }
            }
        }
    }
    void HandleTileClick(Vector2 clickedPosition)
    {
        if (isSelected)
        {
            MoveToValidDiagonal(clickedPosition);
        }
       
    }
    public void MoveToValidDiagonal(Vector2 clickedPosition)
    {
        transform.position = clickedPosition;
        ResetSquareColors(); 
        isSelected = false;
    }
}
