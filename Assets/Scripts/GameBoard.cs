using System;
using System.Collections.Generic;
using UnityEngine;
using Data;
using MovingRules;

public partial class GameBoard : MonoBehaviour
{
    #pragma warning disable CS0649
                     private AngleGame        _game;
    [SerializeField] private Transform        _boardTransform;
    [SerializeField] private Transform        _fadeBoardTransform;
    [SerializeField] private Figure           _figurePrefab;
                     private FadedPanel       _fadedPanel;
                     private Figure           _selectedFigure;
                     private MovingRule       _rule;
    [SerializeField] private FigureCollection _figureCollection;
    #pragma warning restore CS0649


    public event Action FigureMoved;
    public event Action FigureMoving;
    
    
    public FigureCollection Figures => _figureCollection;

    public int Width => _game.BoardWidth;
    
    public int Height => _game.BoardHeight;

    public MovingRule MovingRule => _rule;

    
    public void InitializeBoard(AngleGame gameCore)
    {
        _game                                  = gameCore;
        _rule                                  = _game.Moving;
        _figureCollection                      = new FigureCollection(Width, Height);
        _fadedPanel                            = _fadeBoardTransform.GetComponent<FadedPanel>();
        _fadedPanel.NewPositionSelected       += OnFadedPanelNewPositionSelected;
        _fadedPanel.OneOfNewPositionsSelected += OnFadedPanelOneOfNewPositionsSelected;
        _fadedPanel.Cancelled                 += OnFadedPanelCancelled;
        _fadedPanel.MultiJumpCancelled        += OnFadedPanelMultiJumpCancelled;
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
        var figure = Instantiate(_figurePrefab, _boardTransform);
        figure.InitializeFields(position, figureColor, player);
        figure.FigureSelected += OnFigureSelected;
        
        _figureCollection.Add(figure);
    }
    
    private void SetFadedPanelOn()
    {
        _fadedPanel.gameObject.SetActive(true);
        
        var tilePositions = _rule.GetAllAvailablePositions(_selectedFigure.PointPosition);
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
        if (_selectedFigure.IsSelected)
        {
            Deselect();
            return true;
        }

        return false;
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
    
    private void OnFadedPanelNewPositionSelected(Point newPointPosition)
    {
        _figureCollection.Swap(_selectedFigure.PointPosition, newPointPosition);
        OnClickedFadedPanel();
        FigureMoved?.Invoke();
    }

    private void OnFadedPanelOneOfNewPositionsSelected(Point newPointPosition)
    {
        _figureCollection.Swap(_selectedFigure.PointPosition, newPointPosition);
        FigureMoving?.Invoke();
    }
    
    private void OnFadedPanelCancelled()
    {
        OnClickedFadedPanel();
    }
    
    private void OnFadedPanelMultiJumpCancelled()
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