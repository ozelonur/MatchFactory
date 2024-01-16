using DG.Tweening;
using OrangeBear.EventSystem;
using UnityEngine;

namespace OrangeBear.Bears
{
    public class Item : Bear
    {
        #region Private Variables

        private Transform _transform;
        private ItemBoardController _itemBoardController;

        private Rigidbody _rigidbody;
        private Collider _collider;

        private bool _clicked;

        #endregion

        #region Public Variables

        public int id;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _transform = transform;

            _itemBoardController = transform.root.GetComponent<ItemBoardController>();

            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void OnMouseDown()
        {
            if (_clicked)
            {
                return;
            }

            _clicked = true;

            ItemBoard board = _itemBoardController.GetEmptyBoard(id);

            if (board != null)
            {
                board.SetItem(this);

                Jump();

                DisablePhysics();
            }
        }

        #endregion

        #region Public Methods

        public void InitItem()
        {
            _transform.localEulerAngles =
                new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
            _transform.localPosition = new Vector3(Random.Range(-.45f, .45f), 4.5f, Random.Range(-.45f, .45f));
        }

        public void Jump()
        {
            _transform.DOLocalJump(Vector3.zero, 1, 1, .3f).SetEase(Ease.Linear).SetLink(gameObject);

            _transform.DOLocalRotate(Vector3.zero, .3f).SetEase(Ease.Linear);
        }

        #endregion

        #region Private Methods

        private void DisablePhysics()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        #endregion
    }
}