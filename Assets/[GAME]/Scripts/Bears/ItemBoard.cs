using DG.Tweening;
using OrangeBear.EventSystem;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class ItemBoard : Bear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Transform itemPlacementTransform;

        [SerializeField] private Renderer boardRenderer;

        [Header("Configurations")] [SerializeField]
        private Color highLightColor;

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

        public void LaunchHighlight()
        {
            boardRenderer.material.DOColor(highLightColor, 1f).SetLoops(-1, LoopType.Yoyo).SetId("BoardRendererId");
        }

        public void AbortHighlight()
        {
            DOTween.Kill("BoardRendererId");
            boardRenderer.material.color = Color.white;
        }

        #endregion
    }
}