using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public List<Vector2> TargetPositions = new List<Vector2>();
    public float Speed;
    public int StartPosIndex;

    private Vector2 nextPos {  get { return TargetPositions[nextPosIndex]; } }
    private int nextPosIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (nextPosIndex < 0)
        {
            transform.position = TargetPositions[StartPosIndex];
            nextPosIndex = StartPosIndex + 1;
            if (nextPosIndex >= TargetPositions.Count) nextPosIndex = 0;
        }

        if (Vector3.Distance(transform.position, nextPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * Speed);
        }
        else
        {
            transform.position = nextPos;
            nextPosIndex++;
            if (nextPosIndex >= TargetPositions.Count) nextPosIndex = 0;
        }
    }
}
