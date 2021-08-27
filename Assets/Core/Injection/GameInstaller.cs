using Core.Command;
using Core.Controller;
using Core.Model;
using Core.Model.Config;
using Core.Model.Config.Formation;
using Core.Model.Config.Game;
using Core.Model.Config.Unit;
using Core.Pool;
using Core.Util.Timing;
using Core.View.Camera;
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
        [SerializeField] private CameraView cameraView;

        [Header("Helpers")] 
        [SerializeField] private CoroutineTimingManager coroutineTimingManager;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<FormationConfigData>().FromResources(Constants.FormationResourcesPath);
            Container.Bind<ITimingManager>().FromInstance(coroutineTimingManager);

            Container.Bind<ICameraView>().FromInstance(cameraView);
            Container.Bind<CameraData>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CameraController>().AsSingle().NonLazy();
            
            Container.Bind<IPlayerView>().FromInstance(playerView);
            Container.Bind<PlayerData>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PlayerController>().AsSingle().NonLazy();

            Container.BindMemoryPool<UnitController, UnitControllerPool>();
            Container.BindMemoryPool<UnitView, UnitViewPool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(unitView)
                .UnderTransform(new GameObject("UnitPool").transform);

            Container.BindInstance(gameConfigData);
            Container.BindInstance(unitConfigData);

            Container.Bind<ArmiesData>().AsSingle().NonLazy();
            
            CreateSignalCommand<CreateArmySignal,CreateArmyCommand>();
            CreateSignalCommand<ShuffleArmySignal,ShuffleArmyCommand>();
            CreateSignalCommand<SetArmyPositionSignal,SetArmyPositionCommand>();
            CreateSignalCommand<KillUnitSignal,KillUnitCommand>();
        }
        
        private void CreateSignalCommand<TSignal,TCommand>() where TCommand : ICommand<TSignal>
        {
            Container.DeclareSignal<TSignal>();
            Container.Bind<TCommand>().AsSingle().NonLazy();
            Container.BindSignal<TSignal>().ToMethod<TCommand>(x=> x.Execute).FromResolve();
        }
    }
}