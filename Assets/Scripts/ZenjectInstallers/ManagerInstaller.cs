using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ManagerInstaller", menuName = "Installers/ManagerInstaller")]
public class ManagerInstaller : ScriptableObjectInstaller<ManagerInstaller>
{
    [SerializeField] private AudioManager _audioManagerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<AudioManager>().FromComponentInNewPrefab(_audioManagerPrefab).AsSingle();
    }
}