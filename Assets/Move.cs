using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    public enum FacingDir { Down, Left, Right, Up };

    public float MoveSpeed;

    private Rigidbody2D body;
    private Animator anim;
    private FacingDir facingDir;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingDir = FacingDir.Down;
    }

    // Update is called once per frame
    void Update()
    {

        int deltaX = 0, deltaY = 0;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            deltaY = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            deltaY = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            deltaX = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            deltaX = 1;
        }

        if (deltaX != 0 || deltaY != 0)
        {
            ValidateAndSetPosition(deltaX * MoveSpeed, deltaY * MoveSpeed);
        }
        else
        {
            body.velocity = Vector2.zero;
        }

    }

    private void ValidateAndSetPosition(float x, float y)
    {
        body.velocity = new Vector2(x, y);
        anim.SetFloat("Dir", DirValue(x, y));
        SetFacingDir(x, y);
    }

    private void SetFacingDir(float x, float y)
    {
        if (y < 0) facingDir = FacingDir.Down;
        if (x < 0) facingDir = FacingDir.Left;
        if (x > 0) facingDir = FacingDir.Right;
        if (y > 0) facingDir = FacingDir.Up;
    }


    private float DirValue(float x, float y)
    {
        if (y < 0) return 0;
        if (x < 0) return 0.5f;
        if (x > 0) return 0.75f;
        if (y > 0) return 1f;
        return 0;
    }

    public FacingDir GetFacingDir()
    {
        return facingDir;
    }

}
