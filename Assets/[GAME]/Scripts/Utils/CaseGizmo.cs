using UnityEngine;

namespace _GAME_.Scripts.Utils
{
    public class CaseGizmo : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Configurations")] [SerializeField]
        private Color gizmoColor = Color.red;

        [SerializeField] private float gizmoScaleMultiplier = 1f;

        #endregion

        #region MonoBehaviour Methods

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Transform transform1 = transform;
            Gizmos.DrawWireCube(transform1.position, transform1.localScale * gizmoScaleMultiplier);
        }

        #endregion
    }
}