﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gitenax.AngleCheckers.Data;
using Gitenax.AngleCheckers.Helpers;
using Gitenax.AngleCheckers.Menu;
using Gitenax.AngleCheckers.MovingRules;
using Gitenax.AngleCheckers.PlayArea;
using Gitenax.AngleCheckers.Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Gitenax.AngleCheckers
{
    public sealed class AngleGame : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private List<Player> _players;
        [SerializeField] private BotController _botController;
        [SerializeField] private List<PlayerArea> _playerAreas;
        [SerializeField] private GameOptions _gameOptions;
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private int _gameFormat; // Формат игры 2х2, 3х3 и т.д.
        [SerializeField] private MovingRuleType _movingType;
        private Random _random;
        private MovingRule _movingRule;
        private Player _leadingPlayer; // Игрок который в текущий момент ходит
        private int _leadingPlayerIndex;
#pragma warning restore CS0649
    
        public event Action<Player> LeadingPlayerSelected;
        public event Action<Player, Player> GameEnded;
    
        public int GameFormat => _gameFormat;
        public int BoardWidth => _gameOptions.BoardWidth;
        public int BoardHeight => _gameOptions.BoardHeight;
        public Player[] Players => _players.ToArray();
        public PlayerArea[] PlayerAreas => _playerAreas.ToArray();
        public Player LeadingPlayer => _leadingPlayer;
        public MovingRule Moving => _movingRule;

        private void Awake()
        {
            InitializeFields();
            InitializePlayers();
            // CreatePlayer("Синий", Color.blue, new Point(2, 2));
            // CreatePlayer("Красный", Color.red, new Point(4, 2));
        }

        private void Start()
        {
            SetPlayersFiguresOnBoard();
            StartCoroutine(nameof(SelectPlayerForFirstMove));
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator SelectPlayerForFirstMove()
        {
            bool delay = true;
            while (delay)
            {
                yield return new WaitForSeconds(0.2f);
                SetFirstMoveToPlayer();
                delay = false;
            }
        }

        private void InitializeFields()
        {
            _players = new List<Player>();
            _playerAreas = new List<PlayerArea>();
            _random = new Random();
            _gameFormat = (int) _gameOptions.Format;
            _movingType = (MovingRuleType) PlayerPrefs.GetInt("GAME_TYPE");
            _movingRule = SetMovingRuleForGame(_movingType);
            _gameBoard.InitializeBoard(this);
            _gameBoard.FigureMoved += OnGameBoardFigureMoved;
            _gameBoard.FigureMoving += OnGameBoardFigureMoving;
        }

        private void InitializePlayers()
        {
            string firstPlayerName = PlayerPrefs.GetString("PLAYER1_NAME");
            PlayerType firstPlayerType = (PlayerType)PlayerPrefs.GetInt("PLAYER1_TYPE");        
        
            string secondPlayerName = PlayerPrefs.GetString("PLAYER2_NAME");
            PlayerType secondPlayerType = (PlayerType)PlayerPrefs.GetInt("PLAYER2_TYPE");

            Point player1StartPosition = new Point(0, 0);
            Point player2StartPosition = new Point(_gameOptions.BoardWidth - _gameFormat, _gameOptions.BoardHeight - _gameFormat);;

            if(firstPlayerType == PlayerType.Human)
                CreatePlayer(firstPlayerName, Color.blue, player1StartPosition);
            else
                CreateBot(firstPlayerName, Color.blue, player1StartPosition);

            if(secondPlayerType == PlayerType.Human)
                CreatePlayer(secondPlayerName, Color.red, player2StartPosition);
            else
                CreateBot(secondPlayerName, Color.red, player2StartPosition);
        }
    
        private MovingRule SetMovingRuleForGame(MovingRuleType ruleType)
        {
            return ruleType switch
            {
                MovingRuleType.HorizontalAndVertical => new MovingOnCardinalPointsRule(_gameBoard),
                MovingRuleType.Diagonal => new DiagonalMovingRule(_gameBoard),
                MovingRuleType.Free => new FreeMoveRule(_gameBoard),
                _ => new FreeMoveRule(_gameBoard)
            };
        }
    
        private void SetPlayersFiguresOnBoard()
        {
            foreach (Player player in _players)
            {
                PlayerArea playerArea = _playerAreas.FirstOrDefault(x => x.Owner.Equals(player));
                _gameBoard.InitializeFiguresForPlayer(player, playerArea);
            }
        }

        private void CreatePlayer(string playerName, Color figuresColor, Point areaOffset)
        {
            CreatePlayer<Player>(playerName, figuresColor, areaOffset);
        }

        private void CreateBot(string playerName, Color figuresColor, Point areaOffset)
        {
            PlayerBot bot = CreatePlayer<PlayerBot>(playerName, figuresColor, areaOffset);
            _botController.AddBot(bot);
        }

        private T CreatePlayer<T>(string playerName, Color figuresColor, Point areaOffset) where T : Player
        {
            T player = (T)Activator.CreateInstance(typeof(T), playerName, figuresColor);
            player.GotOpportunityToMove += plyr => { _leadingPlayer = plyr; };
        
            var playerArea = new PlayerArea(player, _gameBoard, _gameFormat, _gameFormat, areaOffset);
            _players.Add(player);
            _playerAreas.Add(playerArea);

            return player;
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
            // выбор случайного игрока
            _leadingPlayerIndex = _random.Next(0, _players.Count);

            // Если все игроки являются ботами
            if (CheckAllPlayersForBot())
            {
                SelectPlayerAtIndex(_leadingPlayerIndex);
                return;
            }
        
            // Если есть среди игроков есть бот, то первый ход только игроку
            while (CheckPlayerForBot(_players[_leadingPlayerIndex]))
            {
                _leadingPlayerIndex = _random.Next(0, _players.Count);
            }
            SelectPlayerAtIndex(_leadingPlayerIndex);
        }

        private void SelectPlayerAtIndex(int index)
        {
            _players[index].IsMakeMove = true;
            LeadingPlayerSelected?.Invoke(_players[_leadingPlayerIndex]);
        }
    
        private bool CheckAllPlayersForBot()
        {
            foreach (Player player in _players)
            {
                if (!CheckPlayerForBot(player))
                    return false;
            }
            return true;
        }
    
        private bool CheckPlayerForBot(Player player)
        {
            return player is PlayerBot;
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
            foreach (Player player in _players)
            {
                var isWinCondition = false;
                PlayerArea[] enemyAreas = _playerAreas.Where(area => !area.Owner.Equals(player)).ToArray();
            
                for (int i = 0; i < enemyAreas.Length; i++)
                {
                    PlayerArea currentArea = enemyAreas[i];
                    Point[] currentAreaPositions = currentArea.Positions.ToOneDimensionArray();
                
                    foreach (Point position in currentAreaPositions)
                    {
                        Figure figureToBeingInspected = _gameBoard.GetFigureAtPoint(position);
                    
                        // Если в зоне игрока нет фигуры, то сразу прерываем проверку
                        if (figureToBeingInspected == null)
                        {
                            isWinCondition = false;
                            break;
                        }
                    
                        // Если фигура в поле игрока принадлежит другому игроку
                        if (figureToBeingInspected.Owner.Equals(player))
                        {
                            isWinCondition = true;
                        }
                        else
                        {
                            isWinCondition = false;
                            break;
                        }
                    }

                    if (!isWinCondition)
                        continue;
                
                    EndGameMenu endGamePanel = FindObjectOfType<EndGameMenu>(true);
                    endGamePanel.gameObject.SetActive(true);
                
                    // Победитель / проигравший
                    GameEnded?.Invoke(player, currentArea.Owner);
                    Debug.Log($"<color=green><b>ИГРОК - {currentArea.Owner.Name} - ПРОИГРАЛ!</b></color>");
                }
            }
        }
    }
}