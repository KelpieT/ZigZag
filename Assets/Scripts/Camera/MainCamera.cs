using Cinemachine;
using UnityEngine;
using Zenject;

public class MainCamera : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	private MainPlayer mainPlayer;

	[Inject]
	private void Construct(MainPlayer mainPlayer)
	{
		this.mainPlayer = mainPlayer;
		mainPlayer.OnAlive += SetTarget;
		mainPlayer.OnDead += UnsetTarget;
	}

	private void SetTarget()
	{
		virtualCamera.Follow = mainPlayer.transform;
		virtualCamera.LookAt = mainPlayer.transform;
	}

	private void UnsetTarget()
	{
		virtualCamera.Follow = null;
		virtualCamera.LookAt = null;
	}

	private void OnDestroy()
	{
		mainPlayer.OnAlive -= SetTarget;
		mainPlayer.OnDead -= UnsetTarget;
	}
}
