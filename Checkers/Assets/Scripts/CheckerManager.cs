using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerManager : MonoBehaviour
{
    [SerializeField]private Checker[] allCheckers;
    [SerializeField] private GridManager gridManager;
  

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
        DeselectAllCheckers();
        selectedChecker.SelectChecker();
        
    }


    public void ResetSquareColors()
    {
       
    }
}
