using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ObjectAddressables : MonoBehaviour
{
    [SerializeField] private AssetReference mapReference;
    [SerializeField] private AssetLabelReference labelReference;
    [SerializeField] private AssetReferenceGameObject objectReference;
    [SerializeField] private AssetLabelReference spritesLabel;

    private GameObject map;
    [SerializeField] private Image loadedPicture;

    public string groupName = "SpritesAssets";

    private void Update()
    {

        // Loading a particular ASSETREFERENCEGAMEOBJECT type
        if (Input.GetKeyDown(KeyCode.W))
        {
            objectReference.InstantiateAsync().Completed += (asyncOperationHandle) =>
            {
                Debug.Log("Instantiated ASSETREFERENCEGAMEOBJECT completed");
                map = asyncOperationHandle.Result;
            };

        }

        //Load a folder content
        if (Input.GetKeyDown(KeyCode.D))
        {
            int i = 0;
            Addressables.LoadAssetsAsync<Sprite>(spritesLabel, (_sprite) =>
            {
                i++;
                Debug.Log(_sprite.name + " - " + i);
                
            }).Completed += (asyncOperationHandle) =>
            {
                loadedPicture.sprite = asyncOperationHandle.Result[1];
            };
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            objectReference.ReleaseInstance(map);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }

    private void ReleaseGroupAssets(string Groupname)
    {
        //Addressables.ResourceManager.rele
    }
}
