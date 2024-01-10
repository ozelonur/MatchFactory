using UnityEngine;

namespace OrangeBear.Bears
{
    public class Item : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Components")] [SerializeField]
        private Material[] colorMaterials;

        [SerializeField] private Material[] outlineMaterials;

        [SerializeField] private Renderer renderer;

        #endregion

        #region Private Variables

        private Transform _transform;
        private int _materialIndex;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _transform = transform;
        }

        private void OnMouseDown()
        {
            Debug.Log("On Mouse Down");
            renderer.material = outlineMaterials[_materialIndex];
        }

        private void OnMouseUp()
        {
            renderer.material = colorMaterials[_materialIndex];
        }

        #endregion

        #region Public Methods

        public void InitItem()
        {
            _transform.localPosition = new Vector3(Random.Range(-.4f, .4f), 4.5f, Random.Range(-.4f, .4f));

            _transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            _materialIndex = Random.Range(0, colorMaterials.Length);
            
            renderer.material = colorMaterials[_materialIndex];
        }

        #endregion
    }
}