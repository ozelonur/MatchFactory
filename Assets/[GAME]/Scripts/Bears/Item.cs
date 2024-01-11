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

        private ItemBoard _board;
        private int _index;

        private bool _isMouseDown;
        private bool _isInputDisabled;

        #endregion

        #region Public Variables

        public int id;
        public bool willDestroy;
        public bool isFlying;

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
            if (_isInputDisabled)
            {
                return;
            }
            
            if (_isMouseDown)
            {
                return;
            }

            _isMouseDown = true;
            modelRenderer.material = outlineMaterial;
            GoToBoard();
        }

        private void OnMouseUp()
        {
            if (_isInputDisabled)
            {
                return;
            }
            modelRenderer.material = colorMaterial;
        }

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.DisableInput, DisableInput);
            }

            else
            {
                Unregister(CustomEvents.DisableInput, DisableInput);   
            }
        }

        private void DisableInput(object[] arguments)
        {
            _isInputDisabled = true;
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

            float delay = .5f * .75f;

            if (reversed)
            {
                delay /= index;
            }
            else
            {
                delay = (.25f * .75f) * (index - 2);
            }

            if (delay <= 0)
            {
                delay = .05f * .75f;
            }

            board.SetItem(this);
            _transform.DOLocalJump(Vector3.zero, .2f, 1, .75f * .75f).SetEase(Ease.OutBack).SetDelay(delay)
                .SetLink(gameObject);
        }

        public void DestroyItem(ItemBoard board, int index = -1)
        {
            // willDestroy = true;
            _board = board;
            _index = index;

            DestroyItem();
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


            isFlying = true;
            board.SetItem(this);

            Roar(CustomEvents.CheckMatch, this);

            _transform.DOLocalJump(Vector3.zero, 1, 1, .75f * .75f).OnComplete(() =>
            {
                isFlying = false;
                Roar(CustomEvents.DestroyDestroyableItems);
                Roar(CustomEvents.CheckHowManyBoardsEmpty);
                Roar(CustomEvents.LaunchComplete);
            }).SetLink(gameObject);

            _transform.DOLocalRotate(Vector3.zero, .25f * .75f).OnComplete(() =>
            {
                _rigidbody.isKinematic = true;
                _collider.enabled = false;
            }).SetLink(gameObject);
        }

        private void DestroyItem()
        {
            _board.RemoveItem();
            _transform.DOLocalMove(Vector3.up * .1f, .05f * .75f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(Vector3.zero, .1f * .75f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        if (_index != -1)
                        {
                            Roar(CustomEvents.Sort);
                        }

                        Destroy(gameObject, .1f * .75f);
                    })
                    .SetLink(gameObject);
            }).SetDelay(.1f * .75f).SetLink(gameObject);
        }

        #endregion
    }
}