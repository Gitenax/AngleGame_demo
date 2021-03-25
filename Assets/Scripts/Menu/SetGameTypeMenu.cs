using MovingRules;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SetGameTypeMenu : MonoBehaviour
    {
        #pragma warning disable CS0649
        [Header("Переключатели")] 
        [SerializeField] private Toggle   _optionOneToggle;
        [SerializeField] private Toggle   _optionTwoToggle;
        [SerializeField] private Toggle   _optionThreeToggle;
        [SerializeField] private Button   _okButton;
        
        [Header("Описание")]
        [SerializeField] private Text     _optionOneLabel;
        [SerializeField] private Text     _optionOneDescription;
        [SerializeField] private Text     _optionTwoLabel;
        [SerializeField] private Text     _optionTwoDescription;
        [SerializeField] private Text     _optionThreeLabel;
        [SerializeField] private Text     _optionThreeDescription;

        [Header("Режим игры")] 
        [SerializeField] private MovingRuleType SelectedRuleType = MovingRuleType.HorizontalAndVertical;

        [Header("Слудующее меню")] 
        [SerializeField] private SetPlayersMenu _nextMenu;
        #pragma warning restore CS0649


        private void Awake()
        {
            _optionOneToggle.onValueChanged.AddListener(SetDescriptionOne);
            _optionTwoToggle.onValueChanged.AddListener(SetDescriptionTwo);
            _optionThreeToggle.onValueChanged.AddListener(SetDescriptionThree);
            _okButton.onClick.AddListener(ButtonClick);
        }

        private void SetDescriptionOne(bool state)
        {
            if(state)
            {
                _optionOneLabel.gameObject.SetActive(true);
                _optionOneDescription.gameObject.SetActive(true);
                SelectedRuleType = MovingRuleType.HorizontalAndVertical;
            }
            else
            {
                _optionOneLabel.gameObject.SetActive(false);
                _optionOneDescription.gameObject.SetActive(false);
            }
        }
        
        private void SetDescriptionTwo(bool state)
        {
            if(state)
            {
                _optionTwoLabel.gameObject.SetActive(true);
                _optionTwoDescription.gameObject.SetActive(true);
                SelectedRuleType = MovingRuleType.Diagonal;
            }
            else
            {
                _optionTwoLabel.gameObject.SetActive(false);
                _optionTwoDescription.gameObject.SetActive(false);
            }
        }
        
        private void SetDescriptionThree(bool state)
        {
            if(state)
            {
                _optionThreeLabel.gameObject.SetActive(true);
                _optionThreeDescription.gameObject.SetActive(true);
                SelectedRuleType = MovingRuleType.Free;
            }
            else
            {
                _optionThreeLabel.gameObject.SetActive(false);
                _optionThreeDescription.gameObject.SetActive(false);
            }
        }

        private void ButtonClick()
        {
            PlayerPrefs.SetInt("GAME_TYPE", (int)SelectedRuleType);
            _nextMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
