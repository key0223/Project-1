using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
    //오브젝트 풀링 위해 미리 함수 만들어놓음.
    public static GameObject Instantiate(GameObject obj)
    {
        GameObject go = MonoBehaviour.Instantiate(obj);

        return go;
    }

    public static async Task<T> LoadResource<T>(string address)
    {
        return await LoadResourceAdress<T>(address);
    }

    public static async Task<T> LoadResourceAdress<T>(string address)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        await handle.Task;

        T rtnValue = handle.Result;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            rtnValue = handle.Result;
        }

        Addressables.Release(handle);

        return rtnValue;
    
    }

    public static async Task<IList<T>> LoadResources<T>(string address)
    {
        return await LoadResourcesAdress<T>(address);
    }

    public static async Task<IList<T>> LoadResourcesAdress<T>(string label)
    {
        
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
        await handle.Task;

        IList<T> rtnValue = handle.Result;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            rtnValue = handle.Result;
        }

        Addressables.Release(handle);

        return rtnValue;

    }

    public static T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public static TextAsset GetStringTable<T>(string path)
    {

        TextAsset textAsset = Load<TextAsset>(path);

        if(textAsset == null)
        {
            Debug.Log($"Failed to load textAsset : {path}");
            return null;

        }

        return Resources.Load<TextAsset>(path);
    }
}
