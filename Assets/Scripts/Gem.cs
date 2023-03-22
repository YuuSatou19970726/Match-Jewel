using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //[HideInInspector]
    public Vector2Int pos;
    //[HideInInspector]
    public Board board;

    Vector3 firstPosition;
    Vector3 lastPosition;

    bool mousePressesd;

    float swipeAngle = 0f;

    Gem otherGem;

    public enum GemType { blue, green, yellow, red, purple}

    public GemType gemType;
    public bool isMatched;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, pos) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, pos, board.gameSpeed * Time.deltaTime);
        } else
        {
            transform.position = new Vector2(pos.x, pos.y);
            board.allGems[pos.x, pos.y] = this;
        }

        if (mousePressesd && Input.GetMouseButtonUp(0))
        {
            lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressesd = false;
            CalculateAngle();
        }
    }

    public void SetupGem(Vector2Int pos, Board board)
    {
        this.pos = pos;
        this.board = board;
    }

    private void OnMouseDown()
    {
        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressesd = true;
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(lastPosition.y - firstPosition.y, lastPosition.x - firstPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;

        Debug.Log(Vector3.Distance(lastPosition, firstPosition));
        if (Vector3.Distance(lastPosition, firstPosition) > 0.5f)
        {
            MoveGem();
        }
    }

    void MoveGem()
    {
        if(swipeAngle > -45 && swipeAngle < 45 && pos.x < board.width)
        {
            otherGem = board.allGems[pos.x + 1, pos.y];
            pos.x++;
            otherGem.pos.x--;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && pos.y < board.height)
        {
            otherGem = board.allGems[pos.x, pos.y + 1];
            pos.y++;
            otherGem.pos.y--;
        }
        else if (swipeAngle > -135 && swipeAngle <= -45 && pos.y > 0)
        {
            otherGem = board.allGems[pos.x, pos.y - 1];
            pos.y--;
            otherGem.pos.y++;
        }
        else if ((swipeAngle > 135 || swipeAngle <= 135) && pos.x > 0)
        {
            otherGem = board.allGems[pos.x - 1, pos.y];
            pos.x--;
            otherGem.pos.x++;
        }

        board.allGems[pos.x, pos.y] = this;
        board.allGems[otherGem.pos.x, otherGem.pos.y] = otherGem;
    }
}
