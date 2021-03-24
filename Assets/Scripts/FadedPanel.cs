using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FadedPanel : MonoBehaviour, IPointerClickHandler
{
    #pragma warning disable CS0649
    [SerializeField] private SelectableTile       _selectableTilePrefab;
    [SerializeField] private GameBoard            _gameBoard;
                     private List<SelectableTile> _tiles;
                     private bool                 _possibleMultiJump;
                     private Point                _currentFigurePosition;
                     private List<Point>          _exceptPositions;
    #pragma warning restore CS0649
    
    
    public event Action        ClickedOnPanel;
    public event Action<Point> NewPositionSelected;
    public event Action<Point> OneOfNewPositionsSelected;
    public event Action        Cancelled;
    public event Action        MultiJumpCancelled;


    public SelectableTile[] Tiles => _tiles.ToArray();
    

    public void PutSelectableTilesOnBoard(Point currentFigurePosition)
    {
        _currentFigurePosition = currentFigurePosition;
        _exceptPositions.Add(currentFigurePosition);
        
        CheckVerticalAndHorizontalSpace(currentFigurePosition);
        // CheckDiagonalSpace(currentFigurePosition);
    }

   public void RemoveSelectableTilesFromBoard()
    {
        foreach (var tile in _tiles)
        {
            tile.TileSelected -= OnSelectableTileSelected;
            Destroy(tile.gameObject);
        }
        _tiles.Clear();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(_possibleMultiJump)
            MultiJumpCancelled?.Invoke();
        else
            Cancelled?.Invoke();
    }

    
    private void Awake()
    {
        _tiles = new List<SelectableTile>();
        _exceptPositions = new List<Point>();
    }

    private void OnDisable()
    {
        RemoveSelectableTilesFromBoard();
        _exceptPositions.Clear();
    }

    private void RePutSelectableTilesOnBoard(Point[] availablePositions)
    {
        foreach (Point point in availablePositions)
            SetTileAfterFigure(point);
    }

    private void CheckVerticalAndHorizontalSpace(Point currentFigurePosition)
    {
        foreach (Point direction in Point.Directions)
        {
            var nextToPosition = currentFigurePosition + direction;
            CheckAndInstantiateTiles(nextToPosition, direction);
        }
    }

    private void CheckDiagonalSpace(Point currentFigurePosition)
    {
        foreach (Point diagonal in Point.Diagonals)
        {
            var nextToPosition = currentFigurePosition + diagonal;
            CheckAndInstantiateTiles(nextToPosition, diagonal);
        }
    }

    private void CheckAndInstantiateTiles(Point position, Point direction)
    {
        if (SetTileBeforeFigure(position)) return;
        
        SetTileAfterFigure(position + direction);
    }


    /// <returns>true - если плитка установлена после выбранной фишки</returns>
    private bool SetTileBeforeFigure(Point position)
    {
        if (CheckingPointWithin(position) && VerifyNextTileForEmpty(position))
        {
            _possibleMultiJump = false;
            InstantiateTile(position);
            return true;
        }
        return false;
    }

    /// <returns>true - если плитка установлена через другую фишку</returns>
    private bool SetTileAfterFigure(Point position)
    {
        if (CheckingPointWithin(position) && VerifyNextTileForEmpty(position))
        {
            _possibleMultiJump = true;
            InstantiateTile(position);
            return true;
        }
        return false;
    }
    
    
    /// <summary>
    /// Проверка, что данная точка вписывается в размеры "игровой доски"
    /// </summary>
    private bool CheckingPointWithin(Point point)
    {
        return ((point.X >= 0 && point.X < _gameBoard.Width) 
                && (point.Y >= 0 && point.Y < _gameBoard.Height));
    }

    private bool VerifyNextTileForEmpty(Point tilePosition)
    {
        if (_gameBoard.Figures[tilePosition] == null)
            return true;

        return false;
    }

    private void InstantiateTile(Point tilePosition)
    {
        var tile = Instantiate(_selectableTilePrefab, transform);
        tile.InitializeFields(tilePosition);
        tile.IsMultijumpRoot = _possibleMultiJump;
        tile.TileSelected += OnSelectableTileSelected;
        _tiles.Add(tile);
    }

    private void OnSelectableTileSelected(SelectableTile selectedTile)
    {
        if (selectedTile.IsMultijumpRoot)
        {
            _exceptPositions.Add(selectedTile.PointPosition);
            
            // Смещение одной из фишек
            OneOfNewPositionsSelected?.Invoke(selectedTile.PointPosition);
            RemoveSelectableTilesFromBoard();
            
            // Проверка на возможность перепрыгнуть соседние фишки
            // возвращает false если нет ходов, соответственно завершая ход игроком
            if (CheckingPossibleMoves(selectedTile.PointPosition, out Point[] available))
                RePutSelectableTilesOnBoard(available);
            else
                MultiJumpCancelled?.Invoke();
        }
        else
        {
            NewPositionSelected?.Invoke(selectedTile.PointPosition);
        }
    }

    private bool CheckingPossibleMoves(Point position, out Point[] available)
    {
        var availableTilePos = new HashSet<Point>();

        foreach (Point direction in Point.Directions)
        {
            var nextToPosition = position + direction;

            // Если в текущем направлении пустая клетка, то пропускаем ее
            if (CheckingPointWithin(nextToPosition) && VerifyNextTileForEmpty(nextToPosition))
                continue;
            
            // Если ячейка в следующем направлении не пуста(т.е. есть фишка)
            if (CheckingPointWithin(nextToPosition) && VerifyNextTileForEmpty(nextToPosition) == false)
            {
                nextToPosition += direction;
                
                // Если за фишкой место не занято
                if (CheckingPointWithin(nextToPosition) && VerifyNextTileForEmpty(nextToPosition))
                {
                    availableTilePos.Add(nextToPosition);
                }
            }
        }
        // Удалить предыдущую позицию(т.е. убрать ход назад)
        availableTilePos.ExceptWith(_exceptPositions);
        available = new Point[availableTilePos.Count];
        availableTilePos.CopyTo(available);
        
        return available.Length > 0;
    }
}
