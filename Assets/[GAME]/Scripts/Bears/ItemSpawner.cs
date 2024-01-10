using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts.Models;
using _GAME_.Scripts.Utils;
using OrangeBear.EventSystem;
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
            List<int> idList = PrepareSpawn();
            StartCoroutine(SpawnItems(idList));
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

        private IEnumerator SpawnItems(List<int> idList)
        {
            foreach (Item item in idList.Select(id =>
                         Instantiate(itemPrefabs.FirstOrDefault(x => x.id == id), spawnParent)))
            {
                item.InitItem();

                yield return null;
            }
        }

        #endregion
    }
}