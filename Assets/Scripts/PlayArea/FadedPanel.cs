using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gitenax.AngleCheckers.PlayArea
{
    public sealed class FadedPanel : MonoBehaviour, IPointerClickHandler
    {
#pragma warning disable CS0649
        [SerializeField] private SelectableTile _selectableTilePrefab;
        [SerializeField] private GameBoard _gameBoard;
        private List<SelectableTile> _tiles;
        private List<Point> _exceptPositions;
        private Point _currentFigurePosition;
        private bool _possibleMultiJump;
#pragma warning restore CS0649
        
        public event Action<Point> NewPositionSelected;
        public event Action<Point> OneOfNewPositionsSelected;
        public event Action Cancelled;
        public event Action MultiJumpCancelled;

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

        public void PutSelectableTilesOnBoard(Point[] positions, Point currentFigurePosition)
        {
            _currentFigurePosition = currentFigurePosition;
            _exceptPositions.Add(currentFigurePosition);
            _possibleMultiJump = false;

            foreach (Point position in positions)
                InstantiateTile(position);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_possibleMultiJump)
                MultiJumpCancelled?.Invoke();
            else
                Cancelled?.Invoke();
        }

        private void RePutSelectableTilesOnBoard(Point[] availablePositions)
        {
            foreach (Point point in availablePositions)
                InstantiateTile(point);
        }
    
        private void RemoveSelectableTilesFromBoard()
        {
            foreach (SelectableTile tile in _tiles)
            {
                tile.TileSelected -= OnSelectableTileSelected;
                Destroy(tile.gameObject);
            }
            _tiles.Clear();
        }
    
        private void InstantiateTile(Point tilePosition)
        {
            SelectableTile tile = Instantiate(_selectableTilePrefab, transform);
            tile.InitializeFields(tilePosition);
            tile.IsMultijumpRoot = tilePosition.Node;
            tile.TileSelected += OnSelectableTileSelected;
            _tiles.Add(tile);
        }

        private void OnSelectableTileSelected(SelectableTile selectedTile)
        {
            if (selectedTile.IsMultijumpRoot)
            {
                // Смещение одной из фишек
                _currentFigurePosition = selectedTile.PointPosition;
                OneOfNewPositionsSelected?.Invoke(selectedTile.PointPosition);
                RemoveSelectableTilesFromBoard();
            
                // Проверка на возможность перепрыгнуть соседние фишки
                // возвращает false если нет ходов, соответственно завершая ход игроком
                if (CheckingPossibleMoves(_currentFigurePosition, out Point[] available))
                {
                    _possibleMultiJump = true;
                    RePutSelectableTilesOnBoard(available);
                }
                else
                {
                    MultiJumpCancelled?.Invoke();
                }
                return;
            }

            NewPositionSelected?.Invoke(selectedTile.PointPosition);
        }

        private bool CheckingPossibleMoves(Point position, out Point[] available)
        {
            var availableTilePos = new HashSet<Point>();
            Point[] allPositions =  _gameBoard.MovingRule.GetJumpPoints(position);
            _exceptPositions.Add(position);
        
            foreach (Point point in allPositions)
                availableTilePos.Add(point);
        
            // Удалить предыдущую позицию(т.е. убрать ход назад)
            availableTilePos.ExceptWith(_exceptPositions);
            available = new Point[availableTilePos.Count];
            availableTilePos.CopyTo(available);
        
            return available.Length > 0;
        }
    }
}