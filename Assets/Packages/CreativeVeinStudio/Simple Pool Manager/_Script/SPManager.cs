using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AngryLab
{
    public class SPManager : MonoBehaviour
    {
        [System.Serializable]
        public class PoolCollectionInfo
        {
            public string _Name;
            public bool ActiveFirstItem;
            public int InstantiateQty;
            public Transform ParentTransform;
            public GameObject Item;
            public List<GameObject> items = new List<GameObject>();
            public bool toggleShowHide;
        }

        public static SPManager instance;
        public PoolCollectionInfo _poolCollectionInfo = new PoolCollectionInfo() { _Name = "Pool Name - Please change" };
        public string searchTxt = "";

        [SerializeField] public List<PoolCollectionInfo> _collection = new List<PoolCollectionInfo>();
        internal Dictionary<string, PoolCollectionInfo> _instantiatedList = new Dictionary<string, PoolCollectionInfo>();

        #region Unity Functions
        void Awake()
        {
            Singleton();
            InstantatePoolGameObjects();
        }
        #endregion

        //****************************
        // Initiations
        //****************************
        #region INIT Functions
        void InstantatePoolGameObjects()
        {
            var newlist = new List<GameObject>();

            foreach (PoolCollectionInfo poolCollection in _collection)
            {
                foreach (GameObject poolItem in poolCollection.items)
                {
                    for (int i = 0; i < poolCollection.InstantiateQty; i++)
                    {
                        GameObject instantiatedGo = poolCollection.ParentTransform != null ?
                            Instantiate(poolItem, transform.position, Quaternion.identity, poolCollection.ParentTransform) :
                            Instantiate(poolItem, transform.position, Quaternion.identity);
                        instantiatedGo.name = instantiatedGo.name.Replace("(Clone)", "");
                        instantiatedGo.SetActive(false);
                        instantiatedGo.transform.localPosition = Vector3.zero;
                        newlist.Add(instantiatedGo);
                    }
                }

                AddToInstantiatedList(poolCollection, newlist);
                newlist.Clear();
            }

            ActivateFirstItem();
        }

        void ActivateFirstItem()
        {
            foreach (PoolCollectionInfo poolCollection in _collection)
            {
                if (poolCollection.ActiveFirstItem)
                {
                    _instantiatedList[poolCollection._Name].items.FirstOrDefault().SetActive(true);
                }
            }
        }
        #endregion

        //****************************
        // Set Actions
        //****************************
        #region Set Functions
        internal void SetNewCollectionParent(string poolName, Transform newParent)
        {
            if (DoesPoolExist(poolName))
            {
                _instantiatedList[poolName].items.ForEach(x =>
                {
                    x.transform.parent = newParent;
                    x.transform.localPosition = Vector3.zero;
                    x.transform.rotation = newParent.rotation;
                });
            }
            else
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
            }
        }
        #endregion

        //****************************
        // Get Actions
        //****************************
        #region Get functions

        /// <summary>
        /// Allows you to retrieve a specific pool item from the given pool by an index
        /// </summary>
        /// <param name="poolName"></param><param name="index"></param>
        internal GameObject GetSpecificPoolItemInCollection(string poolName, int index)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }

            return _instantiatedList[poolName].items[index];
        }

        /// <summary>
        /// Allows you to retrieve a specific pool item from the given pool by an index and returns it through a callback 
        /// </summary>
        /// <param name="poolName"></param><param name="index"></param><param name="callback"></param>
        internal void GetSpecificPoolItemInCollection(string poolName, int index, Action<GameObject> callback)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return;
            }

            callback(_instantiatedList[poolName].items[index]);
        }

        /// <summary>
        /// Retrieves the specific item provided from the specified Pool. 
        /// </summary>
        /// <param name="poolName"></param><param name="item"></param>
        /// <returns>GameObject</returns>
        internal GameObject GetSpecificPoolItemInCollection(string poolName, GameObject item)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }

            GameObject go = _instantiatedList[poolName].items.FirstOrDefault(x => x.name.Equals(item.name));
            if (go == null)
            {
                Debug.Log(string.Format($"The item passed: {item.name}, does not exist in the pool: {poolName}"));
            }
            return go;
        }

        /// <summary>
        /// Retrieves the avilable pool item that is NOT active from the specified Pool
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns>GameObject</returns>
        internal GameObject GetNextAvailablePoolItem(string poolName)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }

            GameObject go = null;
            foreach (var item in _instantiatedList[poolName].items)
            {
                if (!item.activeSelf)
                {
                    go = item;
                    break;
                }
            } 

            if (go == null)
            {
                AddOverflowToCurrentPoolList(poolName);
                return GetNextAvailablePoolItem(poolName);
            }

            return go;
        }

        /// <summary>
        /// Retrieves a random pool object from the specified Pool
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns>GameObject</returns>
        internal GameObject GetRandomPoolItem(string poolName)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }

            GameObject go = _instantiatedList[poolName].items.Where(itm => !itm.activeSelf).ToList()[UnityEngine.Random.Range(0, _instantiatedList[poolName].items.Count - 1)];
            if (go == null)
            {
                AddOverflowToCurrentPoolList(poolName);
                go = _instantiatedList[poolName].items.Where(itm => !itm.activeSelf).ToList()[UnityEngine.Random.Range(0, _instantiatedList[poolName].items.Count - 1)];
            }

            go.SetActive(true);
            return go;
        }

        /// <summary>
        /// Retrieves all of the items in a specified Pool
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns>List<GameObject></returns>
        internal List<GameObject> GetAllItemInACategory(string poolName)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }
            return _instantiatedList[poolName].items;
        }

        /// <summary>
        /// Retrieves all ACTIVE items in a specified Pool (ONLY the items that are enabled)
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns>List<GameObject></returns>
        internal List<GameObject> GetAllActiveCategoryItem(string poolName)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }
            return _instantiatedList[poolName].items.Where(x => x.activeSelf).ToList();
        }

        /// <summary>
        /// Retrieves the parent's Transform for the specified pool
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns>Transform</returns>
        internal Transform GetPoolItemParentTransform(string poolName)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return null;
            }

            return _instantiatedList[poolName].ParentTransform;
        }

        internal bool DoesPoolExist(string poolName)
        {
            return _instantiatedList.ContainsKey(poolName);
        }

        internal bool DoesPoolItemExist(GameObject item)
        {
            bool itemFound = false;
            foreach (var list in _instantiatedList)
            {
                itemFound = list.Value.items.Contains(item);
            }

            return itemFound;
        }
        #endregion

        //****************************
        // Add to pool Actions
        //****************************
        #region Add to pool Actions
        /// <summary>
        /// Allows you to add a list of pool items through Script by creating a PoolCollectionInfo list and passing it in.
        /// </summary>
        /// <param name="newItemCol"></param>
        internal void AddNewItemToCollection(List<PoolCollectionInfo> newItemCol)
        {
            foreach (PoolCollectionInfo obj in newItemCol)
            {
                if (_collection.Any(x => x._Name == obj._Name))
                {
                    _collection.FirstOrDefault(x => x._Name == obj._Name).items.Add(obj.items.FirstOrDefault());
                }
                else
                {
                    _collection.Add(obj);
                }
            }

            InstantatePoolGameObjects();
        }

        void AddToInstantiatedList(PoolCollectionInfo poolObj, List<GameObject> items)
        {
            if (DoesPoolExist(name))
            {
                items.ForEach(_instantiatedList[poolObj._Name].items.Add);
            }
            else
            {
                _instantiatedList.Add(poolObj._Name, new PoolCollectionInfo() { ParentTransform = poolObj.ParentTransform, items = new List<GameObject>(items) }); ;
            }
        }

        void AddOverflowToCurrentPoolList(string poolName)
        {
            Debug.Log(string.Format("Added extra items to : {0}, considder increasing the Qty to create in the pool", poolName));

            var currentPoolDetails = _collection.FirstOrDefault(x => x._Name == poolName);
            int currentPoolQty = currentPoolDetails.InstantiateQty;
            currentPoolDetails.InstantiateQty += currentPoolQty;

            for (int i = 0; i < currentPoolQty; i++)
            {
                foreach (var item in currentPoolDetails.items)
                {
                    GameObject instantiatedGo = currentPoolDetails.ParentTransform != null ?
                        Instantiate(item, transform.position, Quaternion.identity, currentPoolDetails.ParentTransform) :
                        Instantiate(item, transform.position, Quaternion.identity);
                    instantiatedGo.SetActive(false);
                    instantiatedGo.transform.localPosition = Vector3.zero;

                    _instantiatedList[poolName].items.Add(instantiatedGo);
                }
            }

        }
        #endregion

        //****************************
        // Disable Actions
        //****************************
        #region Disable Actions

        /// <summary>
        /// Disables the Pool item that is located on the specified pool by its Transform
        /// </summary>
        /// <param name="poolObjectTransform"></param>
        /// <returns>bool</returns>
        internal void DisablePoolObject(string poolName, Transform poolObjectTransform)
        {
            int indx = _instantiatedList[poolName].items.IndexOf(poolObjectTransform.gameObject);
            if (indx < 0)
            {
                Debug.Log(string.Format("Object {0}. Doesnt exist in the specified Poool: {1}", poolObjectTransform.name, poolName));
            }
            _instantiatedList[poolName].items[indx].SetActive(false);
        }


        /// <summary>
        /// Disables the Pool item that is provided to this funciton as a Transform and returns wether it was disabled or not
        /// </summary>
        /// <param name="poolObjectTransform"></param>
        /// <returns>bool</returns>
        internal void DisablePoolObject(Transform poolObjectTransform)
        {
            foreach (var list in _instantiatedList)
            {
                int indx = list.Value.items.IndexOf(poolObjectTransform.gameObject);
                if (indx > 0)
                {
                    list.Value.items[indx].SetActive(false);
                }
            }
        }

        /// <summary>
        /// Disables the Pool item that is provided to this funciton as a GameObject and returns wether it was disabled or not
        /// </summary>
        /// <param name="poolObject"></param>
        /// <returns>bool</returns>
        internal bool DisablePoolObject(GameObject poolObject)
        {
            foreach (var list in _instantiatedList)
            {
                //int indx = list.Value.items.IndexOf(poolObject);
                var item = list.Value.items.Find(x => x.Equals(poolObject));
                if(item != null)
                {
                    item.SetActive(false);
                    item.transform.parent = list.Value.ParentTransform;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Disables ALL pool objects in the Pool Manager
        /// </summary>
        public void DisableAllExistingInPoolObjects()
        {
            foreach (var list in _instantiatedList)
            {
                try
                {
                    list.Value.items.FindAll(x => x.activeSelf).ForEach(x => x.SetActive(false));
                }
                catch (Exception ex)
                {
                    Debug.Log("Error: " + ex.Message);
                    Debug.Break();
                }
            }
        }

        /// <summary>
        /// Disables all the objects in a given Pool
        /// </summary>
        /// <param name="poolName"></param>
        public void DisableAllPoolObjectByCategory(string poolName)
        {
            if (!DoesPoolExist(poolName))
            {
                Debug.Log($"No {poolName} object in the pool list. Please make sure you have the right name or have created it");
                return;
            }
            _instantiatedList[poolName].items.FindAll(x => x.activeSelf).ForEach(x => x.SetActive(false));
        }
        #endregion


        #region Editor Functions
        // used for the Editor Script
        public void AddToCollection(string _name, bool activeFirstItem, int instQty, Transform parentTrans, GameObject _item)
        {
            _collection.Add(new PoolCollectionInfo() { ActiveFirstItem = activeFirstItem, InstantiateQty = instQty, items = new List<GameObject>() { _item }, Item = _item, ParentTransform = parentTrans, _Name = _name });
        }
        // used for the Editor Script
        public void AddToCollection(object poolInfo)
        {
            PoolCollectionInfo info = poolInfo as PoolCollectionInfo;
            _collection.Add(new PoolCollectionInfo()
            {
                ActiveFirstItem = info.ActiveFirstItem,
                InstantiateQty = info.InstantiateQty,
                items = new List<GameObject>() { info.Item },
                Item = info.Item,
                ParentTransform = info.ParentTransform,
                _Name = info._Name
            });
        }

        #endregion
        void Singleton()
        {
            if (instance == null || instance != this)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
        }
    }
}