using System;
using UnityEngine;

namespace Gitenax.AngleCheckers.PlayArea
{
    [Serializable]
    public sealed class FigureCollection
    {
        [SerializeField] private LogicArrayLayout _occupied; // Логический массив занятых ячеек
        private Figure[,] _figures; // Непосредственно сам массив игровых фигур
        private int _width; // Ширина массива т.е. шириина игровой доски
        private int _height; // Высота массива т.е. высота игровой доски
        
        public FigureCollection(int width, int height)
        {
            _figures = new Figure[width, height];
            _occupied = new LogicArrayLayout(width, height);
            _width = width;
            _height = height;
        }
        
        public Figure this[int x, int y]
        {
            get => this[new Point(x, y)];
            set => this[new Point(x, y)] = value;
        }
        
        public Figure this[Point point]
        {
            get
            {
                var (x, y) = (point.X, point.Y);
                if ((x >= 0 && x < _width) && (y >= 0 && y < _height))
                    return _figures[x, y];

                return default;
            }
            set
            {
                var (x, y) = (point.X, point.Y);
                if ((x < 0 || x >= _width) || (y < 0 || y >= _height))
                    return;
                
                _figures[x, y] = value;
                _occupied.Rows[y].Columns[x] = value != null;
            }
        }

        public void Add(Figure figure)
        {
            this[figure.PointPosition] = figure;
        }
        
        public void Swap(Point figureOldPoint, Point figureNewPoint)
        {
            var temp = this[figureOldPoint];

            // Если в логичееской матрице ячейка является пустой
            if (this[figureNewPoint] == null)
            {
                this[figureNewPoint] = temp;
                this[figureOldPoint] = default;

                UpdateFigurePosition(this[figureNewPoint], figureNewPoint);
                return;
            }

            this[figureOldPoint] = this[figureNewPoint];
            this[figureNewPoint] = temp;
            
            UpdateFigurePosition(this[figureOldPoint], figureOldPoint);
            UpdateFigurePosition(this[figureNewPoint], figureNewPoint);
        }
        
        private void UpdateFigurePosition(Figure figure, Point newPosition)
        {
            figure.PointPosition = newPosition;
            figure.UpdatePosition();
        }
    }
}