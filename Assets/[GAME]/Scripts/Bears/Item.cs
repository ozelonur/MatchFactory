using UnityEngine;

namespace OrangeBear.Bears
{
    public class Item : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Material[] colorMaterials;

        [SerializeField] private Renderer renderer;

        #endregion

        #region Private Variables

        private Transform _transform;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _transform = transform;
        }

        #endregion

        #region Public Methods

        public void InitItem()
        {
            _transform.position = new Vector3(Random.Range(-.4f, .4f), 4.5f, Random.Range(-.4f, .4f));

            _transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            renderer.material = colorMaterials[Random.Range(0, colorMaterials.Length)];
        }

        #endregion
    }
}