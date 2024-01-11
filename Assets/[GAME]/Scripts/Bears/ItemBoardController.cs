using System.Collections.Generic;
using System.Linq;
using OrangeBear.EventSystem;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class ItemBoardController : Bear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private ItemBoard[] itemBoards;

        #endregion

        #region Public Methods

        public ItemBoard GetFirstEmptyItem()
        {
            ItemBoard board = itemBoards.FirstOrDefault(x => x.isEmpty);

            return board;
        }

        public ItemBoard CheckSameItem(Item item)
        {
            List<ItemBoard> nonEmptyBoards = itemBoards.Where(x => !x.isEmpty).ToList();

            if (nonEmptyBoards.Count <= 0)
            {
                return null;
            }

            ItemBoard itemBoard = nonEmptyBoards.LastOrDefault(x => x.currentItem.id == item.id);

            if (itemBoard == null)
            {
                return null;
            }

            int index = itemBoards.ToList().IndexOf(itemBoard);

            SwipeFromIndex(index);
            return itemBoards[index + 1];
        }

        #endregion

        #region Private Methods

        private void SwipeFromIndex(int index)
        {
            ItemBoard lastNonEmptyBoard = itemBoards.Last(x => !x.isEmpty);

            int lastNonEmptyBoardIndex = itemBoards.ToList().IndexOf(lastNonEmptyBoard);

            if (lastNonEmptyBoardIndex > index)
            {
                for (int i = lastNonEmptyBoardIndex; i > index; i--)
                {
                    Item item = itemBoards[i].currentItem;
                    item.UpdateBoard(itemBoards[i + 1], (i));

                    itemBoards[i].currentItem = null;
                    itemBoards[i].isEmpty = true;
                }
            }
        }

        #endregion
    }
}