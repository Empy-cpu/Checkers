using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerManager : MonoBehaviour
{
    [SerializeField]private Minimax miniMax;
    [SerializeField] private Checker[] allCheckers;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MenuManager menuManager;

    private List<Checker> blackCheckers = new List<Checker>();
    private bool isPlayer1Turn = true;



    int player1Checkers = 12;
    int player2Checkers = 12;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>(); // Find and assign the GridManager
       // allCheckers = FindObjectsOfType<Checker>();
        gridManager.PopulateCheckers();
        GetAllCheckers();
        GetRedCheckers();
    }
    public Checker[] GetAllCheckers()
    {
        allCheckers = FindObjectsOfType<Checker>();
        return allCheckers;
    }

    public List<Checker> GetRedCheckers()
    {
        Checker[] allCheckers = GetAllCheckers(); 
        blackCheckers.Clear(); 

        foreach (Checker checker in allCheckers)
        {
            if (checker.CompareTag("RedChecker"))
            {
                blackCheckers.Add(checker);
               
            }
        }

        return blackCheckers;
    }
    public List<Checker> GetBlackCheckers()
    {
        Checker[] allCheckers = GetAllCheckers();
        blackCheckers.Clear();

        foreach (Checker checker in allCheckers)
        {
            if (checker.CompareTag("BlackChecker"))
            {
                blackCheckers.Add(checker);

            }
        }

        return blackCheckers;
    }


    public void DeselectAllCheckers()
    {
        foreach (Checker checker in allCheckers)
        {
            checker.DeselectChecker();
        }

    }

    public void SelectChecker(Checker selectedChecker)
    {
        if ((isPlayer1Turn && selectedChecker.CompareTag("BlackChecker")) ||
         (!isPlayer1Turn && selectedChecker.CompareTag("RedChecker")))
        {
            DeselectAllCheckers();
            selectedChecker.SelectChecker();

        }
        else
        {
            Debug.Log("It's not your turn!");
        }

    }

    public void SwitchTurns()
    {
        isPlayer1Turn = !isPlayer1Turn;
     
        if(!isPlayer1Turn)
        {
            miniMax.AIMove();
            Debug.Log("its computer turn ");
        }
        else
        {
            Debug.Log("Player " + (isPlayer1Turn ? "1" : "2") + "'s turn");
        }
    }
    public bool CheckWinConditions(Checker capturedChecker)
    {

        if (capturedChecker.CompareTag("BlackChecker"))
        {
            player1Checkers--;
            Debug.Log("blacker checker captured" + player1Checkers);
        }
        else if (capturedChecker.CompareTag("RedChecker"))
        {
            player2Checkers--;
            blackCheckers.Remove(capturedChecker);
            Debug.Log("Red checker captured" + player2Checkers);
        }


        List<(Vector2, Vector2)> possibleMoves = GeneratePossibleMoves();
        if (player1Checkers == 0)
        {
            Debug.Log("Player 2 wins!");
            menuManager.WinPanel(2);
            return true;
        }
        else if (player2Checkers == 0)
        {
            Debug.Log("Player 1 wins!");
            menuManager.WinPanel(1);

            return true;
        }
        else if(possibleMoves==null)
        {
            Debug.Log("game over no more moves left");
            menuManager.WinPanel(3);
        }

        return false;
    }

    public List<(Vector2, Vector2)> GeneratePossibleMoves()
    {
        List<(Vector2, Vector2)> possibleMoves = new List<(Vector2, Vector2)>();
        blackCheckers = GetBlackCheckers();
       // Debug.Log("Found " + blackCheckers.Count + " RedCheckers");

        foreach (Checker black in blackCheckers)
        {
            Checker blackChecker = black.GetComponent<Checker>();

            if (blackChecker != null)
            {
                Vector2[] validPositions = blackChecker.CalculateValidDiagonalPositions(blackChecker.transform.position);

                foreach (Vector2 position in validPositions)
                {
                   
                        possibleMoves.Add((blackChecker.transform.position, position));
                    
                }
            }
            else
            {
                Debug.LogError("BlackChecker object is missing the Checker component!");
            }
        }

        return possibleMoves;
    }
}
