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

        if (validDiagonalPositions == null) // Only calculate if not already calculated
        {
            validDiagonalPositions = CalculateValidDiagonalPositions();
           
        }
        HighLightSquares(validDiagonalPositions);

    }
    public Vector2[] CalculateValidDiagonalPositions()
    {
        validDiagonalPositions = new Vector2[4]; 

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

         
        // Calculate forward-left diagonals (two squares)
        validDiagonalPositions[0] = new Vector2(currentPosition.x - 1, currentPosition.y + 1);
        validDiagonalPositions[1] = new Vector2(currentPosition.x - 1 * 2, currentPosition.y + 1 * 2);

        // Calculate forward-right diagonals (two squares)
        validDiagonalPositions[2] = new Vector2(currentPosition.x + 1, currentPosition.y + 1);
        validDiagonalPositions[3] = new Vector2(currentPosition.x + 1 * 2, currentPosition.y + 1 * 2);
       
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
        Vector2[] diagonals = CalculateValidDiagonalPositions();
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
        bool isValidMove = false;

       
        foreach (Vector2 validPos in validDiagonalPositions)
        {
            if (clickedPosition == validPos)
            {
                isValidMove = true;
                break;
            }
        }

        if (isValidMove)
        {
            transform.position = clickedPosition;
            ResetSquareColors();
            isSelected = false;
        }
       
        else
        {
            Debug.Log("Invalid move!");
           
        }
    }
}
