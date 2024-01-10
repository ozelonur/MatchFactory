using System.Collections;
using OrangeBear.EventSystem;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class ItemSpawner : Bear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Item itemPrefab;

        [SerializeField] private Transform spawnParent;

        [Header("Configurations")] [SerializeField]
        private float spawnCount = 20f;

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(GameEvents.InitLevel, InitLevel);
            }
            else
            {
                Unregister(GameEvents.InitLevel, InitLevel);
            }
        }

        private void InitLevel(object[] arguments)
        {
            StartCoroutine(SpawnItems());
        }

        #endregion

        #region Private Methods

        private IEnumerator SpawnItems()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Item item = Instantiate(itemPrefab, spawnParent);
                
                item.InitItem();

                yield return null;
            }
        }

        #endregion
    }
}