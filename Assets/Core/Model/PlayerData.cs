using Core.Util;

namespace Core.Model
{
    public class PlayerData
    {
        public ObservableProperty<SimulationState> State { get; }

        public PlayerData()
        {
            State = new ObservableProperty<SimulationState>(SimulationState.GameSettings);
        }
    }

    public enum SimulationState
    {
        GameSettings,
        Playing
    }
}