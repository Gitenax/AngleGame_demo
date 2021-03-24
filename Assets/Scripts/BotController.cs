using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BotController : MonoBehaviour
{
                     private System.Random   _random;
    [SerializeField] private List<PlayerBot> _bots;
    [SerializeField] private AngleGame       _game;
    [SerializeField] private GameBoard       _gameBoard;
    [SerializeField] private FadedPanel      _fadedPanel;
                     private PlayerBot       _leadingBot;
    
    public void AddBot(PlayerBot bot)
    {
        _bots.Add(bot);
    }
    
    
    private void Awake()
    {
        _random     = new System.Random();
        _bots       = new List<PlayerBot>();
        _game       = FindObjectOfType<AngleGame>();
        _gameBoard  = FindObjectOfType<GameBoard>();
        _fadedPanel = FindObjectOfType<FadedPanel>();
        
        if(_game != null)
            _game.LeadingPlayerSelected += OnLeadingPlayerSelected;
    }

    private void OnLeadingPlayerSelected(Player obj)
    {
        if (obj is PlayerBot bot)
        {
            SetLeadingBot(bot);
            StartCoroutine(nameof(MakeMove));
        }
    }

    private void SetLeadingBot(PlayerBot bot)
    {
        _leadingBot = _bots.Find(x => x.Equals(bot));
    }
    
    private IEnumerator MakeMove()
    {
        var botFigures = GetBotFigures();
        var selectedFigure = GetRandomAvailableFigure(botFigures);
        var possiblePositions = _gameBoard.CheckVerticalAndHorizontalSpace(selectedFigure.PointPosition);
        
        // Переместить фигуру в случайную позицию
        bool awaitBotMove = true;
        while (awaitBotMove)
        {
            yield return new WaitForSeconds(2f);
            _gameBoard.MoveFigureTo(selectedFigure, possiblePositions[_random.Next(0, possiblePositions.Length)]);
            awaitBotMove = false;
        }
    }
    
    private Figure[] GetBotFigures()
    {
        return FindObjectsOfType<Figure>()
            .Where(figure => figure.Owner.Equals(_leadingBot))
            .ToArray();
    }

    private Figure GetRandomAvailableFigure(Figure[] figures)
    {
        // Поиск фигур которыми можно ходить
        var availableFigures = new List<Figure>();
        foreach (var figure in figures)
        {
            if(_gameBoard.VerifyMovableFigure(figure.PointPosition))
                availableFigures.Add(figure);
        }
        
        // Выбор случайной фигуры из возможных
        return availableFigures[_random.Next(0, availableFigures.Count)];
    }
}
