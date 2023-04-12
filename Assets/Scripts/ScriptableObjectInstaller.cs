using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableObjectInstaller", menuName = "Installers/ScriptableObjectInstaller")]
public class ScriptableObjectInstaller : ScriptableObjectInstaller<ScriptableObjectInstaller>
{
	[SerializeField] private LevelDifficulty levelDifficulty;
	
	public override void InstallBindings()
	{
		Container.Bind<LevelDifficulty>().FromScriptableObject(levelDifficulty).AsSingle().NonLazy();
	}
}