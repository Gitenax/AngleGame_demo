using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class AngleGame : MonoBehaviour
{
    [SerializeField] private List<Player>     _players;
    [SerializeField] private BotController    _botController;
    [SerializeField] private List<PlayerArea> _playerAreas;
    [SerializeField] private GameOptions      _gameOptions;
    [SerializeField] private GameBoard        _gameBoard;
    [SerializeField] private int              _gameFormat;    // Формат игры 2х2, 3х3 и т.д.
                     private Player           _leadingPlayer; // Игрок который в текущий момент ходит
                     private int              _leadingPlayerIndex;


    public event Action<Player> LeadingPlayerSelected;

    public int GameFormat => _gameFormat;

    public int BoardWidth => _gameOptions.BoardWidth;

    public int BoardHeight => _gameOptions.BoardHeight;

    public Player[] Players => _players.ToArray();
    
    public Player LeadingPlayer => _leadingPlayer;

    
    private void Awake()
    {
        InitializeFields();
        
        CreatePlayer("Красный", Color.red, new Point(2, 0));
        CreateBot("BOT", Color.blue, new Point(0, 0));
    }

    private void Start()
    {
        SetPlayersFiguresOnBoard();
        SetFirstMoveToPlayer();
    }

    private void InitializeFields()
    {
        _players     = new List<Player>();
        _playerAreas = new List<PlayerArea>();
        _gameFormat  = (int) _gameOptions.Format;
        _gameBoard.InitializeBoard(this);
        _gameBoard.FigureMoved += OnGameBoardFigureMoved;
        _gameBoard.FigureMoving += OnGameBoardFigureMoving;
    }

    private void SetPlayersFiguresOnBoard()
    {
        foreach (var player in _players)
        {
            var playerArea = _playerAreas.FirstOrDefault(x => x.Owner.Equals(player));
            _gameBoard.InitializeFiguresForPlayer(player, playerArea);
        }
    }

    private void CreatePlayer(string playerName, Color figuresColor, Point areaOffset)
    {
        CreatePlayer<Player>(playerName, figuresColor, areaOffset);
    }

    private void CreateBot(string playerName, Color figuresColor, Point areaOffset)
    {
        var bot = CreatePlayer<PlayerBot>(playerName, figuresColor, areaOffset);
        _botController.AddBot(bot);
    }

    private T CreatePlayer<T>(string playerName, Color figuresColor, Point areaOffset) where T : Player
    {
        T player = (T)Activator.CreateInstance(typeof(T), playerName, figuresColor);
        player.GotOpportunityToMove += OnPlayerGotMove;
        
        var playerArea = new PlayerArea(player, _gameBoard, _gameFormat, _gameFormat, areaOffset);
        _players.Add(player);
        _playerAreas.Add(playerArea);

        return player;
    }

    private void OnPlayerGotMove(Player obj)
    {
        _leadingPlayer = obj;
    }

    private Point SecondPlayerOffset()
    {
        return new Point(
            _gameOptions.BoardWidth - _gameFormat,
            _gameOptions.BoardHeight - _gameFormat);
    }

    private void OnGameBoardFigureMoved()
    {
        CheckWinCondition();
        GiveMoveToNextPlayer();
    }

    private void OnGameBoardFigureMoving()
    {
        CheckWinCondition();
    }
    
    private void SetFirstMoveToPlayer()
    {
        var random = new System.Random(364268844);
        _leadingPlayerIndex = random.Next(0, _players.Count);

        while (_players[_leadingPlayerIndex] is PlayerBot)
        {
            _leadingPlayerIndex = random.Next(0, _players.Count);
        }
        
        _players[_leadingPlayerIndex].IsMakeMove = true;
        LeadingPlayerSelected?.Invoke(_players[_leadingPlayerIndex]);
    }
    
    private void GiveMoveToNextPlayer()
    {
        _players[_leadingPlayerIndex].IsMakeMove = false;
        
        if (_leadingPlayerIndex + 1 >= _players.Count)
            _leadingPlayerIndex = 0;
        else
            _leadingPlayerIndex++;
        
        _players[_leadingPlayerIndex].IsMakeMove = true;
        LeadingPlayerSelected?.Invoke(_players[_leadingPlayerIndex]);
    }
    
    private void CheckWinCondition()
    {
        foreach (var player in _players)
        {
            var isWinCondition = false;
            var enemyAreas =
                _playerAreas
                    .Where(area => !area.Owner.Equals(player))
                    .ToArray();

            
       
            for (int i = 0; i < enemyAreas.Length; i++)
            {
                var currentArea = enemyAreas[i];
                var currentAreaPositions = ConvertToSingleDimentionalArray(currentArea.Positions);


                foreach (var position in currentAreaPositions)
                {
                    var figureToBeingInspected = _gameBoard.GetFigureAtPoint(position);
                    
                    // Если в зоне игрока нет фигуры, то сразу прерываем проверку
                    if (figureToBeingInspected == null)
                    {
                        isWinCondition = false;
                        break;
                    }
                    
                    // Если фигура в поле игрока принадлежит другому игроку
                    if (figureToBeingInspected.Owner.Equals(player)) 
                        isWinCondition = true;
                    else
                    {
                        isWinCondition = false;
                        break;
                    }
                }

                if (isWinCondition)
                {
                    Debug.Log($"<color=green><b>ИГРОК - {currentArea.Owner.Name} - ПРОИГРАЛ!</b></color>");
                }
            }
        }
    }

    private T[] ConvertToSingleDimentionalArray<T>(T[,] array)
   {
       T[] newArray = new T[array.Length];
       int index = 0;
       
       for (int i = 0; i < array.GetLength(0); i++)
       {
           for (int j = 0; j < array.GetLength(1); j++)
           {
               newArray[index] = array[i, j];
               index++;
           }
       }

       return newArray;
   }
}
    
