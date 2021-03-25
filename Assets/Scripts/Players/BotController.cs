using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayArea;
using UnityEngine;

namespace Players
{
    /// <summary>
    /// Реалищация примитивного AI
    /// </summary>
    /// <remarks>
    /// В данной реализации AI не имеет четкой цели, лишь делает случайный ход любой свободной фишкой которая
    /// может совершить ход
    /// </remarks>
    public class BotController : MonoBehaviour
    {
                         private System.Random    _random;
        [SerializeField] private List<PlayerBot>  _bots = new List<PlayerBot>();
        [SerializeField] private AngleGame        _game;
        [SerializeField] private GameBoard        _gameBoard;
                         private PlayerBot        _leadingBot;
    
                     
        public void AddBot(PlayerBot bot)
        {
            _bots.Add(bot);
        }
    
    
        private void Awake()
        {
            _random     = new System.Random();
            _game       = FindObjectOfType<AngleGame>();
            _gameBoard  = FindObjectOfType<GameBoard>();

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
            var possiblePositions = _gameBoard.MovingRule.GetAllAvailablePositions(selectedFigure.PointPosition);
        
            // Переместить фигуру в случайную позицию
            bool awaitBotMove = true;
            while (awaitBotMove)
            {
                yield return new WaitForSeconds(Random.Range(0.3f, 2f));
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
                var currentFigureMoves = _gameBoard.MovingRule.GetAllAvailablePositions(figure.PointPosition);
                if (currentFigureMoves.Length > 0)
                { 
                    availableFigures.Add(figure);
                }
            }

            return availableFigures[_random.Next(0, availableFigures.Count)];
        }
    }
}
