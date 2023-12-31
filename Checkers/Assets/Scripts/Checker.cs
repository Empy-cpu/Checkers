using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public float moveSpeed = 5.0f;
    public bool moreJumpsAvailable = false;
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
            
            validDiagonalPositions = CalculateValidDiagonalPositions(this.transform.position);
           
        }
        HighLightSquares(validDiagonalPositions);

    }
    public Vector2[] CalculateValidDiagonalPositions(Vector2  checkerPosition)
    {
        if (state == CheckerState.Regular)
        {
            validDiagonalPositions = new Vector2[4];

            Vector2 currentPosition = new Vector2(checkerPosition.x, checkerPosition.y);

            int forward;
            if (this.CompareTag("BlackChecker"))
            {
                forward = 1;
            }
            else
            {
                forward = -1;
            }

            // Calculate forward-left diagonals (two squares)
            validDiagonalPositions[0] = new Vector2(currentPosition.x - forward, currentPosition.y + forward);
            // Calculate forward-right diagonals (two squares)
            validDiagonalPositions[1] = new Vector2(currentPosition.x + forward, currentPosition.y + forward);


          Vector2 checkerPos = validDiagonalPositions[0];

           Checker checkerMid1 = gridManager.GetCheckerAtPosition(checkerPos);
            if (checkerMid1 != null)
            {
                validDiagonalPositions[2] = new Vector2(currentPosition.x - forward * 2, currentPosition.y + forward * 2);
               
            }
           
            
           checkerPos = validDiagonalPositions[1];
              Checker checkerMid2 = gridManager.GetCheckerAtPosition(checkerPos);
                if (checkerMid2 != null)
                {
                   
                    validDiagonalPositions[3] = new Vector2(currentPosition.x + forward * 2, currentPosition.y + forward * 2);
                }
            

        }
        else//for king
        {
            validDiagonalPositions = new Vector2[8];

            validDiagonalPositions[0] = new Vector2(transform.position.x - 1, transform.position.y + 1);
            validDiagonalPositions[1] = new Vector2(transform.position.x + 1, transform.position.y + 1);
            validDiagonalPositions[2] = new Vector2(transform.position.x - 1, transform.position.y - 1);
            validDiagonalPositions[3] = new Vector2(transform.position.x + 1, transform.position.y - 1);

            for (int i = 0; i < 4; i += 2)
            {
                Vector2 checkerPos = validDiagonalPositions[i];

                Checker checkerMid = gridManager.GetCheckerAtPosition(checkerPos);
                if (checkerMid != null)
                {
                    validDiagonalPositions[4] = new Vector2(transform.position.x - 2, transform.position.y + 2);
                    validDiagonalPositions[5] = new Vector2(transform.position.x + 2, transform.position.y + 2);
                    validDiagonalPositions[6] = new Vector2(transform.position.x - 2, transform.position.y - 2);
                    validDiagonalPositions[7] = new Vector2(transform.position.x + 2, transform.position.y - 2);
                }
            }
            
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
                    Checker checkerAtPos = gridManager.GetCheckerAtPosition(diagonalPos);
                    if (checkerAtPos == null)
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
    }

    void ResetSquareColors()
    {
        Vector2[] diagonals = CalculateValidDiagonalPositions(this.transform.position);
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

    private IEnumerator MoveSmoothly(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, elapsedTime));
            yield return null;
        }

        transform.position = end; 
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

                if (opponentChecker != null && opponentChecker.CompareTag(gameObject.tag))
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
            StartCoroutine(MoveSmoothly(transform.position, newPosition));

           
            ResetSquareColors();
            isSelected = false;

            //Debug.Log("Checker Position Y: " + transform.position.y);
            //Debug.Log("Bottom Last Row: " + gridManager.GetBottomLastRow());
            //Debug.Log("Top Last Row: " + gridManager.GetTopLastRow());
            if (state == CheckerState.Regular && transform.position.y <= gridManager.GetBottomLastRow() || transform.position.y >= gridManager.GetTopLastRow())
            {              
                PromoteToKing();
            }

            if (opponentChecker != null)
            {
                
                opponentChecker.gameObject.SetActive(false);
                Debug.Log("capture");              
                checkerManager.CheckWinConditions(opponentChecker);

                if (CheckForMultipleJumps(newPosition))
                    {
                    Debug.Log("you can jump");
                }
            }

            
            else
            {
                checkerManager.SwitchTurns();
                Debug.Log("switch turns");
            }
          
        }
        else
        {
            Debug.Log("Invalid move!");
        }
    }

    private bool CheckForMultipleJumps(Vector2 newPosition)
    {
               
        Vector2[] newValidPositions = CalculateValidDiagonalPositions(newPosition);
        bool moreJumpsAvailable = false;      
     
         Checker furtherOpponentChecker1 = gridManager.GetCheckerAtPosition(validDiagonalPositions[1]);
          Checker furtherOpponentChecker2 = gridManager.GetCheckerAtPosition(validDiagonalPositions[2]);
          if (furtherOpponentChecker1 != null && furtherOpponentChecker2 ==null /*&& furtherOpponentChecker.CompareTag(gameObject.tag)*/)
          {
               moreJumpsAvailable = true;
                   
          }

            

        Debug.Log("go" + moreJumpsAvailable);
        return moreJumpsAvailable;
    }

    public void PromoteToKing()
    {
        state = CheckerState.King;
        checkerColor.color = kingTint;
        Debug.Log("promoted to king");
    }

}
