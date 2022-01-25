using Gitenax.AngleCheckers.Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gitenax.AngleCheckers.Menu
{
    public sealed class SetPlayersMenu : MonoBehaviour
    {
#pragma warning disable CS0649
        [Header("Параметры игроков")] 
        [SerializeField] private string _firstPlayerName;
        [SerializeField] private PlayerType _firstPlayerType;
        [SerializeField] private string _secondPlayerName;
        [SerializeField] private PlayerType _secondPlayerType;

        [Header("Элементы управления")] 
        [SerializeField] private InputField _firstPlayerInputField;
        [SerializeField] private Dropdown _firstPlayerDropdown;
        [SerializeField] private InputField _secondPlayerInputField;
        [SerializeField] private Dropdown _secondPlayerDropdown;
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private SetGameTypeMenu _previousMenu;
#pragma warning restore CS0649

        private void Awake()
        {
            _okButton.onClick.AddListener(VerifyData);
            _backButton.onClick.AddListener(ShowPreviousMenu);
        }
        
        private void VerifyData()
        {
            SetDataFromFields();

            if (!CheckCorrectNames())
                return;
            
            SaveDataToPrefs();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        private void ShowPreviousMenu()
        {
            _previousMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        
        private void SetDataFromFields()
        {
            _firstPlayerName = _firstPlayerInputField.text;
            _secondPlayerName = _secondPlayerInputField.text;

            _firstPlayerType = _firstPlayerDropdown.value == 0 ? PlayerType.Human : PlayerType.Bot;
            _secondPlayerType = _secondPlayerDropdown.value == 0 ? PlayerType.Human : PlayerType.Bot;
        }
        
        private bool CheckCorrectNames()
        {
            string logMessage = (_firstPlayerName, _secondPlayerName) switch
            {
                var (f, s) when f.Trim().Length == 0 || s.Trim().Length == 0 => "<color=red>Одно из имен не заполнено!</color>",
                var (f, s) when f.CompareTo(s) == 0 => "<color=red>Игроки не могут иметь одинаковые имена</color>",
                _ => null
            };

            if (string.IsNullOrEmpty(logMessage))
                return true;
            
            Debug.Log(logMessage);
            return false;
        }

        private void SaveDataToPrefs()
        {
            PlayerPrefs.SetString("PLAYER1_NAME", _firstPlayerName);
            PlayerPrefs.SetString("PLAYER2_NAME", _secondPlayerName);
            PlayerPrefs.SetInt("PLAYER1_TYPE", (int)_firstPlayerType);
            PlayerPrefs.SetInt("PLAYER2_TYPE", (int)_secondPlayerType);
        }
    }
}
