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

        [SerializeField] private Renderer renderer;

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
        }

        private void OnMouseDown()
        {
            Debug.Log("On Mouse Down");
            renderer.material = outlineMaterial;
            GoToBoard();
        }

        private void OnMouseUp()
        {
            renderer.material = colorMaterial;
        }

        #endregion

        #region Event Methods

        protected override void CheckRoarings(bool status)
        {
            if (status)
            {
                Register(CustomEvents.GetItemBoardController, GetItemBoardController);
            }

            else
            {
                Unregister(CustomEvents.GetItemBoardController, GetItemBoardController);
            }
        }

        private void GetItemBoardController(object[] arguments)
        {
            // _itemBoardController = (ItemBoardController)arguments[0];
        }

        #endregion

        #region Public Methods

        public void InitItem()
        {
            _transform.localPosition = new Vector3(Random.Range(-.4f, .4f), 4.5f, Random.Range(-.4f, .4f));

            _transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            renderer.material = colorMaterial;

            _itemBoardController = FindObjectOfType<ItemBoardController>();
        }

        public void UpdateBoard(ItemBoard board, int index)
        {
            if (board == null) return;

            board.SetItem(this);
            _transform.DOLocalJump(Vector3.zero, .2f, 1, .75f).SetEase(Ease.OutBack).SetDelay((float)(.75f / index));
        }

        #endregion

        #region Private Methods

        private void GoToBoard()
        {
            ItemBoard board = _itemBoardController.CheckSameItem(this);

            if (board == null)
            {
                Debug.Log("There is no same item!");
                board = _itemBoardController.GetFirstEmptyItem();

                // _collider.enabled = false;
                // _rigidbody.isKinematic = true;
            }

            if (board == null)
            {
                Debug.Log("There is no board!");
                return;
            }


            board.SetItem(this);
            _transform.DOLocalJump(Vector3.zero, 1, 1, .75f);

            _transform.DOLocalRotate(Vector3.zero, .25f).OnComplete(() =>
            {
                _rigidbody.isKinematic = true;
                _collider.enabled = false;
            });
        }

        #endregion
    }
}