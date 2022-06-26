using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Space(10)]
    [Header("Variables here are for getting info on the current box (total of 9 boxes")]
    public int index;
    public Mark mark;
    public bool isMarked;

    private string objMarker = null;

    // Start is called before the first frame update
    void Start()
    {
        this.objMarker = gameObject.tag;

        // which box is this one out of the 9
        this.index = transform.GetSiblingIndex();
        this.mark = Mark.NONE;
        this.isMarked = false;
    }

    public void setAsMarked(Transform marker, Mark mark)
    {
        isMarked = true;
        this.mark = mark;
        marker.transform.position = transform.position;

        GetComponent<BoxCollider>().enabled = false;
    }
}
