using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts.GlobalVariables;
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

        #region Private Variables

        private int _lastDestroyedItemsBoardIndex;

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.CheckMatch, CheckMatch);
                Register(CustomEvents.Sort, Sort);
            }

            else
            {
                Unregister(CustomEvents.CheckMatch, CheckMatch);
                Unregister(CustomEvents.Sort, Sort);
            }
        }

        private void Sort(object[] arguments)
        {
            for (int i = 0; i < itemBoards.Length; i++)
            {
                if (i > _lastDestroyedItemsBoardIndex)
                {
                    if (!itemBoards[i].isEmpty)
                    {
                        itemBoards[i].currentItem.UpdateBoard(itemBoards[i - 3], i, false);
                        itemBoards[i].currentItem = null;
                        itemBoards[i].isEmpty = true;
                    }
                }
            }
        }

        private void CheckMatch(object[] arguments)
        {
            int itemId = ((Item)arguments[0]).id;

            List<ItemBoard> nonEmptyBoardList = itemBoards.Where(x => !x.isEmpty).ToList();

            List<ItemBoard> sameItems = nonEmptyBoardList.Where(x => x.currentItem.id == itemId).ToList();

            if (sameItems.Count < 3)
            {
                return;
            }

            foreach (ItemBoard board in sameItems)
            {
                board.currentItem.DestroyItem(board);
            }

            for (int i = 0; i < sameItems.Count; i++)
            {
                if (i != sameItems.Count - 1)
                {
                    sameItems[i].currentItem.DestroyItem(sameItems[i]);
                }

                else
                {
                    _lastDestroyedItemsBoardIndex = itemBoards.ToList().IndexOf(sameItems[i]);
                    sameItems[i].currentItem.DestroyItem(sameItems[i], i);
                }
            }
        }

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