using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    public static RefillManager m_instance { get; private set; }

    [System.Serializable]
    private struct RespawnRequest
    {
        public GameObject prefab;

        public Transform parent;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public int siblingIndex;

        // AmmoBox data
        public AmmoType ammoType;
        public int ammoAmount;

        // Health pack data
        public int healthAmount;

        // Throwable data
        public ThrowableType throwableType;
    }

    private readonly List<RespawnRequest> m_requests = new();

    private void Awake()
    {
        if (m_instance != null && m_instance != this) 
        { 
            Destroy(gameObject); return; 
        }
        m_instance = this;
    }
    // ---------- AMMO BOX ----------
    public void RegisterAmmoBox(GameObject prefab, Transform pickupRoot, AmmoBox ammoBox)
    {
        if (prefab == null || pickupRoot == null || ammoBox == null) return;

        m_requests.Add(new RespawnRequest
        {
            prefab = prefab,
            parent = pickupRoot.parent,
            localPosition = pickupRoot.localPosition,
            localRotation = pickupRoot.localRotation,
            localScale = pickupRoot.localScale,
            siblingIndex = pickupRoot.GetSiblingIndex(),

            ammoType = ammoBox.m_ammoType,
            ammoAmount = ammoBox.GetAmmoAmount()
        });
    }

    // ---------- HEALTH PACK ----------
    public void RegisterHealthPack(GameObject prefab, Transform pickupRoot, int healthAmount)
    {
        if (prefab == null || pickupRoot == null) return;

        m_requests.Add(new RespawnRequest
        {
            prefab = prefab,
            parent = pickupRoot.parent,
            localPosition = pickupRoot.localPosition,
            localRotation = pickupRoot.localRotation,
            localScale = pickupRoot.localScale,
            siblingIndex = pickupRoot.GetSiblingIndex(),

            healthAmount = healthAmount
        });
        Debug.Log("Registered Health Pack for refill: " + prefab.name);
    }

    // ---------- THROWABLE ----------
    public void RegisterThrowable(GameObject prefab, Transform pickupRoot, Throwable throwable)
    {
        if (prefab == null || pickupRoot == null || throwable == null) return;

        m_requests.Add(new RespawnRequest
        {
            prefab = prefab,
            parent = pickupRoot.parent,
            localPosition = pickupRoot.localPosition,
            localRotation = pickupRoot.localRotation,
            localScale = pickupRoot.localScale,
            siblingIndex = pickupRoot.GetSiblingIndex(),

            throwableType = throwable.m_type
        });
    }

    // ---------- REFILL ----------
    public void RefillAll()
    {
        Debug.Log("Refilling " + m_requests.Count + " items.");
        foreach (var r in m_requests)
        {
            GameObject obj = Instantiate(r.prefab, r.parent);

            obj.transform.localPosition = r.localPosition;
            obj.transform.localRotation = r.localRotation;
            obj.transform.localScale = r.localScale;

            if (r.parent != null)
                obj.transform.SetSiblingIndex(r.siblingIndex);

            // Restore AmmoBox
            AmmoBox ammoBox = obj.GetComponentInChildren<AmmoBox>(true);
            if (ammoBox != null && r.ammoAmount > 0)
            {
                ammoBox.m_ammoType = r.ammoType;
                ammoBox.SetAmmoAmount(r.ammoAmount);
            }

            // Restore Throwable
            Throwable throwable = obj.GetComponentInChildren<Throwable>(true);
            if (throwable != null && r.throwableType != ThrowableType.None)
            {
                throwable.m_type = r.throwableType;
            }

            // Restore HealthPack
            HealthPack health = obj.GetComponentInChildren<HealthPack>(true);
            if (health != null && r.healthAmount > 0)
            {
                health.SetHealthAmmount(r.healthAmount);
            }
        }

        m_requests.Clear();
    }
}
