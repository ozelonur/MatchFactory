using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Tween Data", menuName = "Orange Bear/Data/Tween Data", order = 1)]
    public class TweenDataScriptableObject : ScriptableObject
    {
        #region Serialized Fields

        [BoxGroup("Configurations")] [SerializeField]
        private float updateBoardJumpDuration;

        [BoxGroup("Configurations")] [BoxGroup("Configurations")] [SerializeField]
        private float updateBoardDelay;

        [BoxGroup("Configurations")] [SerializeField]
        private float gotoBoardJumpDuration;

        [BoxGroup("Configurations")] [SerializeField]
        private float rotateNeutralizeDuration;

        [BoxGroup("Destroy Tween Durations")] [SerializeField]
        private float destroyingMoveUpDuration;

        [BoxGroup("Destroy Tween Durations")] [SerializeField]
        private float destroyingScaleDuration;

        [BoxGroup("Destroy Tween Durations")] [SerializeField]
        private float destroyingDelayDuration;

        #endregion

        #region Properties

        public float UpdateBoardDelay => updateBoardDelay;
        public float UpdateBoardJumpDuration => updateBoardJumpDuration;
        public float GotoBoardJumpDuration => gotoBoardJumpDuration;
        public float RotateNeutralizeDuration => rotateNeutralizeDuration;
        public float DestroyingMoveUpDuration => destroyingMoveUpDuration;
        public float DestroyingScaleDuration => destroyingScaleDuration;
        public float DestroyingDelayDuration => destroyingDelayDuration;

        #endregion
    }
}