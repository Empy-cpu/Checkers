using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    [SerializeField] private CheckerManager checkerManager;
    [SerializeField] private GridManager gridManager;
    List<Checker> redCheckers;


    [ContextMenu("testAIFunction")]

    public void AIMove()
    {
        List<(Vector2, Vector2)> possibleMoves1 = GeneratePossibleMoves();
        int bestScore = int.MinValue;
        (Vector2, Vector2) bestMove = (Vector2.zero, Vector2.zero);

        foreach ((Vector2 start, Vector2 end) in possibleMoves1)
        {

            int score = CalculateMinimax(false, 3, possibleMoves1); // Depth is set to 3, you can adjust this
            Debug.Log(possibleMoves1);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = (start, end);
               
            }
        }

        PerformBestMove(bestMove);
    }

    public int EvaluateBoardState()
    {
        int aiScore = 0;
        int playerScore = 0;


        aiScore += CountPiecesAndState(CheckerState.King, "RedChecker");
        aiScore += 2 * CountPiecesAndState(CheckerState.Regular, "RedChecker");

        playerScore += CountPiecesAndState(CheckerState.King, "BlackChecker");
        playerScore += 2 * CountPiecesAndState(CheckerState.Regular, "BlackChecker");

        // 2. Piece mobility
        aiScore += CalculateMobility("RedChecker");
        playerScore += CalculateMobility("BlackChecker");



        int finalScore = aiScore - playerScore;

       

        return finalScore;
    }


    private int CountPiecesAndState(CheckerState state, string tag)
    {
        Checker[] checkers = FindObjectsOfType<Checker>();
        int count = 0;
        foreach (Checker checker in checkers)
        {
            if (checker.CompareTag(tag) && checker.state == state)
            {
                count++;
            }
        }
        return count;
    }


    private int CalculateMobility(string tag)
    {
        Checker[] checkers = FindObjectsOfType<Checker>();
        int mobility = 0;
        foreach (Checker checker in checkers)
        {
            if (checker.CompareTag(tag))
            {
                mobility += checker.CalculateValidDiagonalPositions(checker.transform.position).Length;
            }
        }
        return mobility;
    }

    public List<(Vector2, Vector2)> GeneratePossibleMoves()
    {
        List<(Vector2, Vector2)> possibleMoves = new List<(Vector2, Vector2)>();
        redCheckers = checkerManager.GetRedCheckers();
       // Debug.Log("Found " + redCheckers.Count + " RedCheckers");

        foreach (Checker redCheckerObject in redCheckers)
        {
            Checker redChecker = redCheckerObject.GetComponent<Checker>();

            if (redChecker != null)
            {
                Vector2[] validPositions = redChecker.CalculateValidDiagonalPositions(redChecker.transform.position);

                foreach (Vector2 position in validPositions)
                {
                    
                    if (IsWithinGridBounds(position) && gridManager.GetCheckerAtPosition(position) == null)
                    {
                        possibleMoves.Add((redChecker.transform.position, position));
                       // Debug.Log("moves are " + possibleMoves.Count + " RedCheckers");
                    }
                }
            }
            else
            {
                Debug.LogError("RedChecker object is missing the Checker component!");
            }
        }

        return possibleMoves;
    }

    private bool IsWithinGridBounds(Vector2 position)
    {
        return position.x >= 0 && position.x <= 7 && position.y >= 0 && position.y <= 7;
    }

    private int CalculateMinimax(bool maximizingPlayer, int depth, List<(Vector2, Vector2)> possibleMoves)
    {
        if (depth == 0)
        {
            return EvaluateBoardState();
        }

        if (maximizingPlayer)
        {
            int maxScore = int.MinValue;


            foreach ((Vector2 start, Vector2 end) in possibleMoves)
            {

                int score = CalculateMinimax(false, depth - 1, possibleMoves);

                maxScore = Mathf.Max(maxScore, score);
            }
            return maxScore;
        }
        else
        {
            return 0;
        }
    }

    private void PerformBestMove((Vector2, Vector2) bestMove)
    {
       
        // Extract initial and final positions from bestMove tuple
        Vector2 initialPosition = bestMove.Item1;
        Vector2 finalPosition = bestMove.Item2;

        // Finding the Red checker at the initial position
        redCheckers = checkerManager.GetRedCheckers();
        Checker redCheckerToMove = null;

        foreach (Checker redChecker in redCheckers)
        {
            Vector2 checkerPos = new Vector2(redChecker.transform.position.x, redChecker.transform.position.y);

            if (checkerPos == initialPosition)
            {
                redCheckerToMove = redChecker;
                break;
            }
        }

        // Moving the Red checker to the final position
        if (redCheckerToMove != null)
        {
            redCheckerToMove.MoveToValidDiagonal(finalPosition);
            Checker opponentChecker = null;

            Vector2 midPosition = (finalPosition + initialPosition) / 2;

            opponentChecker = gridManager.GetCheckerAtPosition(midPosition);

            if (opponentChecker != null && opponentChecker.CompareTag(gameObject.tag))
            {
                opponentChecker.gameObject.SetActive(false);
                Debug.Log("capture");
                checkerManager.CheckWinConditions(opponentChecker);
               

            }

            

        }
    }
}
