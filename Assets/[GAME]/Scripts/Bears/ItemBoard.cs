using OrangeBear.EventSystem;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class ItemBoard : Bear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Transform itemPlacementTransform;

        #endregion

        #region Public Variables

        public bool isEmpty;
        public Item currentItem;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            isEmpty = true;
        }

        #endregion

        #region Public Methods

        public void SetItem(Item item)
        {
            item.transform.parent = itemPlacementTransform;
            currentItem = item;
            isEmpty = false;
        }

        public void RemoveItem()
        {
            currentItem = null;
            isEmpty = true;
        }

        #endregion
    }
}