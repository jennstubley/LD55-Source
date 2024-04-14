using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStartPosition : MonoBehaviour
{
    public List<Vector2> StartPositions = new List<Vector2>();
    public int StartPosIndex;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = StartPositions[StartPosIndex];
    }

    // Update is called once per frame
    void Update()
    {
    }

}
