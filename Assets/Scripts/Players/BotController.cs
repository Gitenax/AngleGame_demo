using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayArea;
using UnityEngine;
using Random = System.Random;

namespace Players
{
    /// <summary>
    /// Реалищация примитивного AI
    /// </summary>
    /// <remarks>
    /// В данной реализации AI не имеет четкой цели, лишь делает случайный ход любой свободной фишкой которая
    /// может совершить ход
    /// </remarks>
    public sealed class BotController : MonoBehaviour
    {
        [SerializeField] private List<PlayerBot> _bots = new List<PlayerBot>();
        [SerializeField] private AngleGame _game;
        [SerializeField] private GameBoard _gameBoard;
        private Random _random;
        private PlayerBot _leadingBot;

        private void Awake()
        {
            _random = new Random();
            _game = FindObjectOfType<AngleGame>();
            _gameBoard = FindObjectOfType<GameBoard>();

            if(_game != null)
                _game.LeadingPlayerSelected += OnLeadingPlayerSelected;
        }

        public void AddBot(PlayerBot bot)
        {
            _bots.Add(bot);
        }

        private void OnLeadingPlayerSelected(Player player)
        {
            if (!(player is PlayerBot bot))
                return;
            
            SetLeadingBot(bot);
            StartCoroutine(nameof(MakeMove));
        }

        private void SetLeadingBot(PlayerBot bot)
        {
            _leadingBot = _bots.Find(x => x.Equals(bot));
        }
    
        private IEnumerator MakeMove()
        {
            Figure[] botFigures = GetBotFigures();
            Figure selectedFigure = GetRandomAvailableFigure(botFigures);
            Point[] possiblePositions = _gameBoard.MovingRule.GetAllAvailablePositions(selectedFigure.PointPosition);
        
            // Переместить фигуру в случайную позицию
            bool awaitBotMove = true;
            while (awaitBotMove)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 2f));
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
                // Если данная фигура имеет ходы
                Point[] currentFigureMoves = _gameBoard.MovingRule.GetAllAvailablePositions(figure.PointPosition);
                if (currentFigureMoves.Length > 0)
                    availableFigures.Add(figure);
            }

            return availableFigures[_random.Next(0, availableFigures.Count)];
        }
    }
}