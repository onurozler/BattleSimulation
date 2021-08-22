using System;

namespace Core.View.Player
{
    public interface IPlayerView
    {
        event Action OnPlayPressed;
        event Action<int> OnShufflePressed;

        void SetShuffleButtonsId(int[] buttonIds);
        void Hide();
    }
}