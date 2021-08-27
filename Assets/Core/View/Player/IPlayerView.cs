using System;
using System.Collections.Generic;

namespace Core.View.Player
{
    public interface IPlayerView
    {
        event Action OnPlayPressed;
        event Action<int> OnShufflePressed;
        event Action<int,int> OnFormationPressed;

        void SetShuffleButton(int buttonId, int armyId);
        void SetDropDownButton(int index, List<string> options);
        void Show();
        void Hide();
    }
}