using System;
using MovingRules;
using Players;
using UnityEngine;

namespace PlayArea
{
    public sealed partial class GameBoard : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Transform _boardTransform;
        [SerializeField] private Transform _fadeBoardTransform;
        [SerializeField] private Figure _figurePrefab;
        [SerializeField] private FigureCollection _figureCollection;
        private AngleGame _game;
        private FadedPanel _fadedPanel;
        private Figure _selectedFigure;
        private MovingRule _rule;
#pragma warning restore CS0649
        
        public event Action FigureMoved;
        public event Action FigureMoving;
        
        public FigureCollection Figures => _figureCollection;
        public int Width => _game.BoardWidth;
        public int Height => _game.BoardHeight;
        public MovingRule MovingRule => _rule;

        private void OnDestroy()
        {
            _fadedPanel.NewPositionSelected -= OnFadedPanelNewPositionSelectedHandler;
            _fadedPanel.OneOfNewPositionsSelected -= OnFadedPanelOneOfNewPositionsSelectedHandler;
            _fadedPanel.Cancelled -= OnFadedPanelCancelledHandler;
            _fadedPanel.MultiJumpCancelled -= OnFadedPanelMultiJumpCancelledHandler;
        }
        
        public void InitializeBoard(AngleGame gameCore)
        {
            _game = gameCore;
            _rule = _game.Moving;
            _figureCollection = new FigureCollection(Width, Height);
            _fadedPanel = _fadeBoardTransform.GetComponent<FadedPanel>();
            _fadedPanel.NewPositionSelected += OnFadedPanelNewPositionSelectedHandler;
            _fadedPanel.OneOfNewPositionsSelected += OnFadedPanelOneOfNewPositionsSelectedHandler;
            _fadedPanel.Cancelled += OnFadedPanelCancelledHandler;
            _fadedPanel.MultiJumpCancelled += OnFadedPanelMultiJumpCancelledHandler;
        }
    
        public void InitializeFiguresForPlayer(Player player, PlayerArea area)
        {
            for (int x = 0; x < _game.GameFormat; x++)
            {
                for (int y = 0; y < _game.GameFormat; y++)
                {
                    InstantiateAndSubscribeToFigure(
                        area.Positions[x, y], 
                        player.PlayerColor,
                        player);
                }
            }
        }

        public void MoveFigureTo(Figure figure, Point destination)
        {
            _figureCollection.Swap(figure.PointPosition, destination);
            FigureMoved?.Invoke();
        }
        
        private void InstantiateAndSubscribeToFigure(Point position, Color figureColor, Player player)
        {
            Figure figure = Instantiate(_figurePrefab, _boardTransform);
            figure.InitializeFields(position, figureColor, player);
            figure.FigureSelected += OnFigureSelected;
        
            _figureCollection.Add(figure);
        }
    
        private void SetFadedPanelOn()
        {
            _fadedPanel.gameObject.SetActive(true);
        
            Point[] tilePositions = _rule.GetAllAvailablePositions(_selectedFigure.PointPosition);
            _fadedPanel.PutSelectableTilesOnBoard(tilePositions, _selectedFigure.PointPosition);
        }
    
        private void SetFadedPanelOff()
        {
            _fadedPanel.gameObject.SetActive(false);
        }
    
        private void OnFigureSelected(Figure selectedFigure)
        {
            // Если фишка не пренадлежит ведущему
            if(!selectedFigure.Owner.Equals(_game.LeadingPlayer))
                return;
        
            _selectedFigure = selectedFigure;
            if(DeselectSelectedFigure() == false) 
                Select();
        }

        private bool DeselectSelectedFigure()
        {
            if (!_selectedFigure.IsSelected)
                return false;
            
            Deselect();
            return true;
        }

        private void Deselect()
        {
            _selectedFigure.IsSelected = false;
            SetFadedPanelOff();
            SetSelectedFigureParent(_boardTransform);
        }

        private void Select()
        {
            _selectedFigure.IsSelected = true;
            SetFadedPanelOn();
            SetSelectedFigureParent(_fadeBoardTransform);
        }
    
        private void OnFadedPanelNewPositionSelectedHandler(Point newPointPosition)
        {
            _figureCollection.Swap(_selectedFigure.PointPosition, newPointPosition);
            OnClickedFadedPanel();
            FigureMoved?.Invoke();
        }

        private void OnFadedPanelOneOfNewPositionsSelectedHandler(Point newPointPosition)
        {
            _figureCollection.Swap(_selectedFigure.PointPosition, newPointPosition);
            FigureMoving?.Invoke();
        }
    
        private void OnFadedPanelCancelledHandler()
        {
            OnClickedFadedPanel();
        }
    
        private void OnFadedPanelMultiJumpCancelledHandler()
        {
            OnClickedFadedPanel();
            FigureMoved?.Invoke();
        }
    
        private void OnClickedFadedPanel()
        {
            Deselect();
        }
    
        private void SetSelectedFigureParent(Transform parent)
        {
            _selectedFigure.Parent = parent;
        }
    }
}