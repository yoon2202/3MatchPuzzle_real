using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float aspectRatio = 0.45f;   // 가로 세로 비율 <- 10/16
    public float padding = 2; // 양쪽 사이드 빈공간 비율
    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board != null)
        {
            RepositionCamera(board.width-1, board.height-1);
        }
    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new Vector3(x / 2, y / 2, cameraOffset);
        transform.position = tempPosition;
        if (board.width >= board.height)
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        else
            Camera.main.orthographicSize = board.height / 2 + padding;
    }

}
