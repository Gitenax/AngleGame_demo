using System;
using UnityEngine;

[Serializable]
public class PlayerArea
{
    [SerializeField] private Player           _owner;
    [SerializeField] private LogicArrayLayout _occupied;
                     private Point[,]         _area;
                     private int              _width;
                     private int              _height;
                     private int              _offsetX;
                     private int              _offsetY;
                     private GameBoard        _gameBoard;


    public PlayerArea(Player owner, GameBoard board, int width, int height, Point offset)
    {
        _area = new Point[height, width];
        _occupied = new LogicArrayLayout(board.Height, board.Width);
        _gameBoard = board;
        _owner = owner;
        _width = width;
        _height = height;
        _offsetX = offset.X;
        _offsetY = offset.Y;

        FillArray();
    }


    public Point[,] Positions => _area;

    public Player Owner => _owner;


    private void FillArray()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _area[y, x] = new Point(x + _offsetX, y + _offsetY);
                _occupied.Rows[y + _offsetY].Columns[x + _offsetX] = true;
            }
        }
    }
}
