using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerManager : MonoBehaviour
{
    
    [SerializeField]private Checker[] allCheckers;
    [SerializeField] private GridManager gridManager;
    private bool isPlayer1Turn = true;

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
            SwitchTurns(); 
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
    private bool CheckWinConditions()
    {
        int player1Checkers = 0;
        int player2Checkers = 0;

        foreach (Checker checker in allCheckers)
        {
            if (checker.gameObject.activeSelf)
            {
                if (checker.CompareTag("BlackChecker"))
                {
                    player1Checkers++;
                }
                else if (checker.CompareTag("RedChecker"))
                {
                    player2Checkers++;
                }
            }
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
