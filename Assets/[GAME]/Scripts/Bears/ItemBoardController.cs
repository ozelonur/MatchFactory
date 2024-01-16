using System.Collections;
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

        public ItemBoard GetEmptyBoard(int itemId)
        {
            List<ItemBoard> boards = itemBoards.Where(x => x.currentItem != null && x.currentItem.id == itemId)
                .ToList();

            if (boards.Count > 0)
            {
                int index = itemBoards.ToList().IndexOf(boards.LastOrDefault());

                StartCoroutine(Swipe(index));

                return itemBoards[index + 1];
            }

            ItemBoard board = itemBoards.FirstOrDefault(x => x.isEmpty);

            return board;
        }

        #endregion

        #region Private Methods

        private IEnumerator Swipe(int index)
        {
            for (int i = itemBoards.Length - 1; i >= 0; i--)
            {
                if (itemBoards[i].isEmpty)
                {
                    continue;
                }

                if (itemBoards.ToList().IndexOf(itemBoards[i]) > index)
                {
                    itemBoards[i + 1].SetItem(itemBoards[i].currentItem);
                    itemBoards[i].RemoveItem();
                }
            }

            for (int i = itemBoards.Length - 1; i >= 0; i--)
            {
                if (itemBoards[i].isEmpty)
                {
                    continue;
                }
                
                if (itemBoards.ToList().IndexOf(itemBoards[i]) > index)
                {
                    itemBoards[i].currentItem.Jump();
                }

                yield return null;  
            }
        }

        #endregion
    }
}