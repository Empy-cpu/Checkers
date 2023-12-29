using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public enum CheckerState
{
    Regular,
    King
}
public class Checker : MonoBehaviour
{
    public Color selectedTint;
    private Color originalColor;
    public Color defaultTileColor;
    private SpriteRenderer checkerColor;

    private CheckerManager checkerManager;
    private GridManager gridManager;
    [SerializeField] public bool isSelected = false;
    private Vector2[] validDiagonalPositions;

    public CheckerState state = CheckerState.Regular;
    public Color kingTint;

   
    void Start()
    {
        checkerColor=GetComponent<SpriteRenderer>();
        originalColor = checkerColor.color;
        checkerManager = FindObjectOfType<CheckerManager>();
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
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
    void OnMouseDown(){
        if (!isSelected){
            checkerManager.SelectChecker(this);         
        }    
    }

    public void DeselectChecker()
    {
        isSelected = false;
        if (state == CheckerState.Regular)
        {
            checkerColor.color = originalColor;
        }
        else if (state == CheckerState.King)
        {
            checkerColor.color = kingTint;
        }
        ResetSquareColors();
    }

    public void SelectChecker()
    {
        isSelected = true;
        checkerColor.color = selectedTint;

        if (validDiagonalPositions == null) 
        {
            
            validDiagonalPositions = CalculateValidDiagonalPositions();
           
        }
        HighLightSquares(validDiagonalPositions);

    }
    public Vector2[] CalculateValidDiagonalPositions()
    {
        if (state == CheckerState.Regular)
        {
            validDiagonalPositions = new Vector2[4];

            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            int forward;
            if (this.CompareTag("BlackChecker"))
            {
                forward = 1;
            }
            else
            {
                forward = -1;
            }

           
            validDiagonalPositions[0] = new Vector2(currentPosition.x - forward, currentPosition.y + forward);           
            validDiagonalPositions[2] = new Vector2(currentPosition.x + forward, currentPosition.y + forward);
          

        }
        else
        {
            validDiagonalPositions = new Vector2[8]; 

           
            validDiagonalPositions[0] = new Vector2(transform.position.x - 1, transform.position.y + 1);
            validDiagonalPositions[1] = new Vector2(transform.position.x - 2, transform.position.y + 2);

         
            validDiagonalPositions[2] = new Vector2(transform.position.x + 1, transform.position.y + 1);
            validDiagonalPositions[3] = new Vector2(transform.position.x + 2, transform.position.y + 2);

          
            validDiagonalPositions[4] = new Vector2(transform.position.x - 1, transform.position.y - 1);
            validDiagonalPositions[5] = new Vector2(transform.position.x - 2, transform.position.y - 2);

          
            validDiagonalPositions[6] = new Vector2(transform.position.x + 1, transform.position.y - 1);
            validDiagonalPositions[7] = new Vector2(transform.position.x + 2, transform.position.y - 2);
        }

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
            checkerManager.SwitchTurns();
        }
       
    }
    public void MoveToValidDiagonal(Vector2 clickedPosition)
    {
        bool isValidMove = false;
        Checker opponentChecker = null;

        foreach (Vector2 validPos in validDiagonalPositions)
        {
            if (clickedPosition == validPos)
            {
                isValidMove = true;

               
                Vector2 midPosition = (clickedPosition + (Vector2)transform.position) / 2;
                opponentChecker = gridManager.GetCheckerAtPosition(midPosition);

                Checker currentCheckerAtMid = gridManager.GetCheckerAtPosition(midPosition);
                if (currentCheckerAtMid != null && currentCheckerAtMid.CompareTag(gameObject.tag))
                {
                    opponentChecker = null;
                    break;
                }
                break;
            }
        }

        if (isValidMove)
        {
            Vector3 newPosition = new Vector3(clickedPosition.x, clickedPosition.y, transform.position.z - 0.16f);
            transform.position = newPosition;
            ResetSquareColors();
            isSelected = false;
            if (state == CheckerState.Regular && ((transform.position.y <= gridManager.GetBottomLastRows()) || (transform.position.y >= gridManager.GetTopLastRows())))
            {
                PromoteToKing();
            }


            if (opponentChecker != null)
            {
                opponentChecker.gameObject.SetActive(false);
                Debug.Log("capture");
                checkerManager.CheckWinConditions(opponentChecker);
               
            }

        }
        else
        {
            Debug.Log("Invalid move!");
        }
    }

    public void PromoteToKing()
    {
        state = CheckerState.King;
        checkerColor.color = kingTint;
        Debug.Log("Promoted to King");
    }

}
