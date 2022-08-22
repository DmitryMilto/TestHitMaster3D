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
    [SerializeField] private AssetReference level;
    [SerializeField] private AssetReference asset;

    AsyncOperationHandle handle1;
    AsyncOperationHandle handle2;
    // Start is called before the first frame update
    async void Start()
    {
        handle1 = Addressables.LoadSceneAsync(level, LoadSceneMode.Additive, true, 1);
        //handle2 = Addressables.LoadSceneAsync(asset, LoadSceneMode.Additive, true, 2);
        //handle1.Completed += (Obj) =>
        //{
        //    if (Obj.Status != AsyncOperationStatus.Succeeded) return;
            
        //    handle2.Completed += (Obj) =>
        //    {
        //        if (Obj.Status != AsyncOperationStatus.Succeeded) return;
        //        //ServiceLocator.GetService<PlayerManager>().player.Ray();
        //    };
        //};
        //handle2 = Addressables.LoadSceneAsync(asset, LoadSceneMode.Additive, true, 2);
    }
    [Button]
    private void OnDestroy()
    {
        Addressables.UnloadSceneAsync(handle1, true);
        Addressables.UnloadSceneAsync(handle2, true);
    }
}
