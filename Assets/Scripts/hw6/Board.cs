using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The manager class for the 9 boxes
/// </summary>
public class Board : MonoBehaviour
{
    public UnityAction<Mark, Color> OnWinAction;
    public Mark[] marks;
    private Camera arCamera;
    private Mark currentMark;
    private int marksCount = 0;
    [SerializeField] private LineRenderer lineRenderer;

    /// <summary>
    /// If this is false, then someone already won
    /// </summary>
    private bool canPlay;

    private Vector3 startLinePos, endLinePos;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
        // DrawLine(new Vector3(3.87f, 0, -3.21f), new Vector3(-2.66f, 0, 3.92f));
        arCamera = Camera.main;
        currentMark = Mark.APPLE;

        marks = new Mark[9];

        // Ensure raycasts hit triggers.
        Physics.queriesHitTriggers = true;
        canPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine whether the left mouse button or touch input were pressed this frame.
        if (canPlay && Input.GetMouseButtonDown(0))
        {
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
            // If a box was hit
            // ensure the raycast only works on the seventh layer, which is named "Box" by me
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (1 << 7)))
            {
                HitBox(hit.transform.GetComponent<Box>());
            }
        }
    }

    private void HitBox(Box box)
    {
        if (box.isMarked) return;

        marks[box.index] = currentMark;
        string markerPrefabName = currentMark == Mark.APPLE ? "Apple" : "Orange";
        GameObject mark = Resources.Load<GameObject>(markerPrefabName);
        mark = Instantiate(mark, new Vector3(0, 0, 0), Quaternion.identity);
        box.setAsMarked(mark.transform, currentMark);
        ++marksCount;


        // check if someone wins
        if (CheckIfWin())
        {
            OnWinAction?.Invoke(currentMark, (currentMark == Mark.APPLE) ? Color.red : new Color(255, 165, 0));
            DrawLine(startLinePos, endLinePos);

            print($"{currentMark.ToString()} wins!");
            canPlay = false;
            return;
        }

        if (marksCount == 9)
        {
            OnWinAction?.Invoke(Mark.NONE, Color.black);
            print("Draw");
            canPlay = false;
            return;
        }

        SwitchPlayer();

    }

    private bool CheckIfWin()
    {
        if (AreBoxesMatched(0, 1, 2))
        {
            startLinePos = new Vector3(-2.65f, 0, -3.21f);
            endLinePos = new Vector3(-2.65f, 0, 3.47f);
            return true;
        }
        if (AreBoxesMatched(3, 4, 5))
        {
            startLinePos = new Vector3(0.5f, 0, -3.21f);
            endLinePos = new Vector3(0.5f, 0, 3.47f);
            return true;
        }
        if (AreBoxesMatched(6, 7, 8))
        {
            startLinePos = new Vector3(3.5f, 0, -3.21f);
            endLinePos = new Vector3(3.5f, 0, 3.47f);
            return true;
        }
        if (AreBoxesMatched(0, 3, 6))
        {
            startLinePos = new Vector3(-2.5f, 0, -3.21f);
            endLinePos = new Vector3(3.5f, 0, -3.25f);
            return true;
        }
        if (AreBoxesMatched(1, 4, 7))
        {
            startLinePos = new Vector3(-2.5f, 0, 0.25f);
            endLinePos = new Vector3(3.5f, 0, 0.25f);
            return true;
        }
        if (AreBoxesMatched(2, 5, 8))
        {
            startLinePos = new Vector3(-2.5f, 0, 3.75f);
            endLinePos = new Vector3(3.5f, 0, 3.75f);
            return true;
        }
        if (AreBoxesMatched(0, 4, 8))
        {
            startLinePos = new Vector3(-2.5f, 0, -3.21f);
            endLinePos = new Vector3(4, 0, 3.92f);
            return true;
        }
        if (AreBoxesMatched(2, 4, 6))
        {
            startLinePos = new Vector3(3.87f, 0, -3.21f);
            endLinePos = new Vector3(-2.66f, 0, 3.92f);
            return true;
        }


        return false;
    }

    private bool AreBoxesMatched(int i, int j, int k)
    {
        Mark m = currentMark;
        bool matched = (marks[i] == m && marks[j] == m && marks[k] == m);
        return matched;
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        Color color = Color.blue;
        color.a = .3f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.enabled = true;
    }

    // void OnDrawGizmos()
    // {
    //     for (var i = 0; i < 9; i++)
    //     {
    //         Gizmos.DrawCube(transform.GetChild(i).position, new Vector3(1, 1, 1));
    //     }
    // }


    private void SwitchPlayer()
    {
        currentMark = (currentMark == Mark.APPLE) ? Mark.ORANGE : Mark.APPLE;
    }
}
