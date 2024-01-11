using _GAME_.Scripts.GlobalVariables;
using DG.Tweening;
using OrangeBear.EventSystem;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class Item : Bear
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Material colorMaterial;

        [SerializeField] private Material outlineMaterial;

        [SerializeField] private Renderer modelRenderer;

        #endregion

        #region Private Variables

        private Transform _transform;

        private ItemBoardController _itemBoardController;

        private Collider _collider;
        private Rigidbody _rigidbody;

        #endregion

        #region Public Variables

        public int id;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _transform = transform;
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _itemBoardController = transform.root.GetComponent<ItemBoardController>();
        }

        private void OnMouseDown()
        {
            modelRenderer.material = outlineMaterial;
            GoToBoard();
        }

        private void OnMouseUp()
        {
            modelRenderer.material = colorMaterial;
        }

        #endregion

        #region Public Methods

        public void InitItem()
        {
            _transform.localPosition = new Vector3(Random.Range(-.4f, .4f), 4.5f, Random.Range(-.4f, .4f));

            _transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            modelRenderer.material = colorMaterial;
        }

        public void UpdateBoard(ItemBoard board, int index, bool reversed = true)
        {
            if (board == null) return;

            float delay = .75f;

            if (reversed)
            {
                delay /= index;
            }
            else
            {
                delay = .25f * (index - 2);
            }
            
            board.SetItem(this);
            _transform.DOLocalJump(Vector3.zero, .2f, 1, .75f).SetEase(Ease.OutBack).SetDelay(delay);
        }

        public void DestroyItem(ItemBoard board, int index = -1)
        {
            Sequence destroySequence = DOTween.Sequence();

            destroySequence.Join(_transform.DOLocalMove(Vector3.up * .1f, .1f))
                .Join(_transform.DOScale(Vector3.zero, .1f)).OnComplete(() =>
                {
                    board.RemoveItem();

                    if (index != -1)
                    {
                        Roar(CustomEvents.Sort);
                    }
                    
                    Destroy(gameObject);
                })
                .SetDelay(.15f)
                .SetLink(gameObject);
        }

        #endregion

        #region Private Methods

        private void GoToBoard()
        {
            ItemBoard board = _itemBoardController.CheckSameItem(this);

            if (board == null)
            {
                board = _itemBoardController.GetFirstEmptyItem();
            }

            if (board == null)
            {
                return;
            }


            board.SetItem(this);
            _transform.DOLocalJump(Vector3.zero, 1, 1, .75f).OnComplete(() => { Roar(CustomEvents.CheckMatch, this); });

            _transform.DOLocalRotate(Vector3.zero, .25f).OnComplete(() =>
            {
                _rigidbody.isKinematic = true;
                _collider.enabled = false;
            });
        }

        #endregion
    }
}