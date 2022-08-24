using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class LoaderManagerScene : MonoBehaviour
{
    [SerializeField] private int level = 0;
    [SerializeField] private List<AssetReference> levelAsset;

    AsyncOperationHandle handle1;

    public System.Action<int> OnStateChange;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            OnStateChange?.Invoke(level);
        }
    }
    void Start()
    {
        NextLevel();
    }

    [Button]
    public async void NextLevel()
    {
        GameController.Instance.State = GameController.GameState.Load;
        await LoadLevel(level % 2);
        Level += 1;
        GameController.Instance.State = GameController.GameState.Play;
    }
    public async void StartLevel()
    {
        GameController.Instance.State = GameController.GameState.Load;
        await LoadLevel(0);
        Level = 1;
        GameController.Instance.State = GameController.GameState.Play;
    }
    [Button]
    private void OnDestroy()
    {
        Addressables.UnloadSceneAsync(handle1, true);
    }
    private async UniTask LoadLevel(int number)
    {
        if (handle1.IsValid()) await Addressables.UnloadSceneAsync(handle1, true);
        handle1 = Addressables.LoadSceneAsync(levelAsset[number], LoadSceneMode.Additive, true, 2);
        await handle1.Task;
    }
}
