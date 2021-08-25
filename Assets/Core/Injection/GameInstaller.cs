using Core.Command;
using Core.Controller;
using Core.Model;
using Core.Model.Config.Game;
using Core.Model.Config.Unit;
using Core.Util.Timing;
using Core.View.Player;
using Core.View.Unit;
using UnityEngine;
using Zenject;

namespace Core.Injection
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Configs")] 
        [SerializeField] private GameConfigData gameConfigData;
        [SerializeField] private UnitConfigData unitConfigData;
        
        [Header("Views")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private UnitView unitView;

        [Header("Helpers")] 
        [SerializeField] private CoroutineTimingManager coroutineTimingManager;
        
        public override void InstallBindings()
        {
            Container.Bind<ITimingManager>().FromInstance(coroutineTimingManager);
            
            Container.Bind<IPlayerView>().FromInstance(playerView);
            Container.Bind<PlayerData>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PlayerController>().AsSingle().NonLazy();

            Container.BindMemoryPool<UnitController, UnitController.Pool>();
            Container.BindMemoryPool<UnitView, UnitView.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(unitView)
                .UnderTransform(new GameObject("UnitPool").transform);

            Container.BindInstance(gameConfigData);
            Container.BindInstance(unitConfigData);

            Container.Bind<ArmiesData>().AsSingle().NonLazy();
            Container.Bind<CreateArmyCommand>().AsSingle().NonLazy();
            Container.Bind<ShuffleArmyCommand>().AsSingle().NonLazy();
            Container.Bind<SetArmyPositionCommand>().AsSingle().NonLazy();
            Container.Bind<KillUnitCommand>().AsSingle().NonLazy();
        }
    }
}