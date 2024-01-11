using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts.Models;
using _GAME_.Scripts.Utils;
using OrangeBear.EventSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class ItemSpawner : Bear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Item[] itemPrefabs;

        [SerializeField] private Transform spawnParent;

        [Header("Configurations")] [SerializeField]
        private SpawnData[] spawnDataArray;
        
        [SerializeField] private Item[] spawnedItems;

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(GameEvents.OnGameStart, OnGameStart);
            }
            else
            {
                Unregister(GameEvents.OnGameStart, OnGameStart);
            }
        }

        private void OnGameStart(object[] arguments)
        {
            StartCoroutine(SpawnItems());
        }

        #endregion

        #region Private Methods

        private List<int> PrepareSpawn()
        {
            List<int> idList = new();

            foreach (SpawnData data in spawnDataArray)
            {
                for (int j = 0; j < data.count; j++)
                {
                    idList.Add(data.id);
                }
            }

            idList.Shuffle();

            return idList;
        }

        private IEnumerator SpawnItems()
        {
            foreach (Item item in spawnedItems)
            {
                item.gameObject.SetActive(true);
                item.InitItem();
                yield return null;
            }
        }

        [Button("Spawn Level")]
        private void SpawnLevel()
        {
            List<int> idList = PrepareSpawn();

            spawnedItems = new Item[idList.Count];

            for (int i = 0; i < idList.Count; i++)
            {
                Item item = Instantiate(itemPrefabs.FirstOrDefault(x => x.id == idList[i]), spawnParent);
                spawnedItems[i] = item;
                item.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}