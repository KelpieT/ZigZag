using UnityEngine;
using Zenject;

public class MainMonoInstaller : MonoInstaller
{
	[SerializeField] private MainEnterPoint mainEnterPoint;
	[SerializeField] private RoadTile roadTilePrefab;
	[SerializeField] private Level level;
	[SerializeField] private MainPlayer playerPrefab;
	[SerializeField] private Crystal crystalPrefab;
	[SerializeField] private MainMenuUI mainMenuUI;

	public override void InstallBindings()
	{
		Container.Bind<MainEnterPoint>().FromInstance(mainEnterPoint).AsSingle();

		Container.Bind<Counter>().FromNew().AsSingle();
		Container.Bind<MainMenu>().FromNew().AsSingle();
		Container.Bind<MainMenuUI>().FromInstance(mainMenuUI).AsSingle();

		Container.Bind<RoadTile>().FromInstance(roadTilePrefab).AsSingle();
		Container.Bind<Level>().FromInstance(level).AsSingle();
		Container.BindFactory<RoadBlock, RoadBlockFactory>().AsSingle();
		Container.BindFactory<Crystal, CrystalFactory>().FromComponentInNewPrefab(crystalPrefab);

		MainPlayer mainPlayer = Container.InstantiatePrefabForComponent<MainPlayer>(playerPrefab);
		Container.Bind<MainPlayer>().FromInstance(mainPlayer).AsSingle();
	}
}

