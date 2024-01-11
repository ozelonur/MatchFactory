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

        private int _lastDestroyedItemsBoardIndex = -1;
        private List<ItemBoard> _sameItems = new();

        private bool _isFailed;

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.CheckMatch, CheckMatch);
                Register(CustomEvents.Sort, Sort);
                Register(CustomEvents.DestroyDestroyableItems, DestroyEvent);
                Register(CustomEvents.CheckHowManyBoardsEmpty, CheckHowManyBoardsEmpty);
                Register(CustomEvents.LaunchComplete, LaunchComplete);
            }

            else
            {
                Unregister(CustomEvents.CheckMatch, CheckMatch);
                Unregister(CustomEvents.Sort, Sort);
                Unregister(CustomEvents.DestroyDestroyableItems, DestroyEvent);
                Unregister(CustomEvents.CheckHowManyBoardsEmpty, CheckHowManyBoardsEmpty);
                Unregister(CustomEvents.LaunchComplete, LaunchComplete);
            }
        }

        private void LaunchComplete(object[] arguments)
        {
            if (!_isFailed)
            {
                return;
            }
            Roar(GameEvents.OnGameComplete, false);
        }

        private void CheckHowManyBoardsEmpty(object[] arguments)
        {
            int emptySlotCount = itemBoards.Count(x => x.isEmpty);

            if (emptySlotCount <= 1)
            {
                itemBoards[^1].LaunchHighlight();
            }

            else
            {
                itemBoards[^1].AbortHighlight();
            }
        }

        private void DestroyEvent(object[] arguments)
        {
            DestroyDestroyableItems();
        }

        private void Sort(object[] arguments)
        {
            for (int i = 0; i < itemBoards.Length; i++)
            {
                if (_lastDestroyedItemsBoardIndex > i)
                {
                    continue;
                }

                if (!itemBoards[i].isEmpty)
                {
                    itemBoards[i].currentItem.UpdateBoard(itemBoards.FirstOrDefault(x => x.isEmpty), i, false);
                    itemBoards[i].currentItem = null;
                    itemBoards[i].isEmpty = true;
                }
            }
        }

        private void CheckMatch(object[] arguments)
        {
            int itemId = ((Item)arguments[0]).id;

            List<ItemBoard> nonEmptyBoardList = itemBoards.Where(x => !x.isEmpty).ToList();

            _sameItems.Clear();

            _sameItems = nonEmptyBoardList.Where(x => x.currentItem.id == itemId).ToList();

            if (_sameItems.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    _sameItems[i].currentItem.willDestroy = true;
                }
            }

            else
            {
                _sameItems.Clear();

                int emptyBoardCount = itemBoards.Count(x => x.isEmpty);

                if (emptyBoardCount <= 0)
                {
                    _isFailed = true;
                    Roar(CustomEvents.DisableInput);
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

            ItemBoard itemBoard =
                nonEmptyBoards.LastOrDefault(x => x.currentItem.id == item.id && !x.currentItem.willDestroy);

            if (itemBoard == null)
            {
                return null;
            }

            int index = itemBoards.ToList().IndexOf(itemBoard);

            SwipeFromIndex(index);
            return itemBoards[index + 1];
        }

        public void DestroyDestroyableItems()
        {
            List<ItemBoard> nonEmptyItems = itemBoards.Where(x => !x.isEmpty).ToList();

            foreach (ItemBoard board in nonEmptyItems)
            {
                if (board.currentItem.isFlying)
                {
                    return;
                }
            }

            if (_sameItems is not { Count: > 0 })
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                if (i != 2)
                {
                    _sameItems[i].currentItem.DestroyItem(_sameItems[i]);
                }

                else
                {
                    _lastDestroyedItemsBoardIndex = itemBoards.ToList().IndexOf(_sameItems[i]);
                    _sameItems[i].currentItem.DestroyItem(_sameItems[i], i);
                }
            }

            _sameItems.Clear();
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