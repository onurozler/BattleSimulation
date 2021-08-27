using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Core.View.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] 
        private Button playButton;

        [SerializeField] 
        private Button[] shuffleButtons;

        [SerializeField] 
        private Dropdown[] formationDropDowns;
                
        public event Action OnPlayPressed;
        public event Action<int> OnShufflePressed;
        public event Action<int,int> OnFormationPressed; 

        private void Awake()
        {
            playButton.onClick.AddListener(()=>OnPlayPressed?.Invoke());
        }

        public void SetShuffleButton(int buttonId, int armyId)
        {
            var shuffleButton = shuffleButtons[buttonId];
            shuffleButton.onClick.RemoveAllListeners();
            shuffleButton.onClick.AddListener(()=>OnShufflePressed?.Invoke(armyId));
        }

        public void SetDropDownButton(int index, List<string> options)
        {
            var formationDropDown = formationDropDowns[index];
            
            formationDropDown.onValueChanged.RemoveAllListeners();
            formationDropDown.onValueChanged.AddListener((formationId)=> OnFormationPressed?.Invoke(index,formationId));
            formationDropDown.ClearOptions();
            formationDropDown.AddOptions(options);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}