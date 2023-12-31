using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerManager : MonoBehaviour
{

    [SerializeField] private Checker[] allCheckers;
    [SerializeField] private GridManager gridManager;
    private bool isPlayer1Turn = true;

    int player1Checkers = 12;
    int player2Checkers = 12;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>(); // Find and assign the GridManager
        allCheckers = FindObjectsOfType<Checker>();
        gridManager.PopulateCheckers();
        GetAllCheckers();
    }
    public void GetAllCheckers()
    {
        allCheckers = FindObjectsOfType<Checker>();
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
        Debug.Log("Player " + (isPlayer1Turn ? "1" : "2") + "'s turn");
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
            Debug.Log("Red checker captured" + player2Checkers);
        }



        if (player1Checkers == 0)
        {
            Debug.Log("Player 2 wins!");
            return true;
        }
        else if (player2Checkers == 0)
        {
            Debug.Log("Player 1 wins!");
            return true;
        }

        return false;
    }

}
