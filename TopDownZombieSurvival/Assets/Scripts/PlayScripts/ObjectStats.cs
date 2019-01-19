using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectStats : MonoBehaviour
    {
        public float Damage;
        public float AttackCoolDownTimer;
        public float AttackCoolDown;
        public float AttackRange;

        public int Gold;
        public int Wood;
        public int Stone;

        public int AggroRank; //0 being the highest and 3 being the lowest
    }
}
