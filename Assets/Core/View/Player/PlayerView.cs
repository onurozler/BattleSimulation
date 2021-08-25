using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] 
        private Button playButton;

        [SerializeField] 
        private Button[] shuffleButtons;
                
        public event Action OnPlayPressed;
        public event Action<int> OnShufflePressed;

        private void Awake()
        {
            playButton.onClick.AddListener(()=>OnPlayPressed?.Invoke());
        }

        public void SetShuffleButtonsId(int[] buttonIds)
        {
            if (buttonIds.Length != shuffleButtons.Length)
            {
                throw new Exception("Shuffle Buttons and ids must be same lenght!");
            }
            
            for (int i = 0; i < shuffleButtons.Length; i++)
            {
                var shuffleButton = shuffleButtons[i];
                var buttonIndex = buttonIds[i];
                shuffleButton.onClick.AddListener(()=>OnShufflePressed?.Invoke(buttonIndex));
            }
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