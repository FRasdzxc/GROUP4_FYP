using UnityEngine;

namespace PathOfHero.Items
{
    public class PointDropper : MonoBehaviour
    {
        [SerializeField]
        private PointPickup m_Prefab;

        [SerializeField]
        private float m_InitialForce;

        [SerializeField]
        private Drop[] m_Drops;

        public void DropPoints()
        {
            foreach (var drop in m_Drops)
            {
                var pickup = Instantiate(m_Prefab, transform.position, Quaternion.identity);
                pickup.name = $"{drop.type}Pickup";
                pickup.Type = drop.type;
                pickup.Amount = drop.amount;
                if (pickup.TryGetComponent<Rigidbody2D>(out var rigidbody2D))
                {
                    Vector2 force = new(
                        Random.Range(-m_InitialForce, m_InitialForce),
                        Random.Range(-m_InitialForce, m_InitialForce)
                    );
                    rigidbody2D.AddForce(force);
                }
            }
        }

        [System.Serializable]
        public class Drop
        {
            public PointPickup.PointType type;
            public float amount;
        }
    }
}
