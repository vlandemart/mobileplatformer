using UnityEngine;

public class AmmoBar : MonoBehaviour {

    [SerializeField]
    private PlayerMovement player = null;
    [SerializeField]
    private GameObject ammo = null;

    private int maxAmmo;
    private int ammoCount;

    private void Awake()
    {
        player = LevelInit.player;
    }

    void Start()
    {
        player.OnAmmoChange += UpdateAmmo;
        maxAmmo= player.MaxAmmo;
        ammoCount = maxAmmo;
        for (int i = 0; i < maxAmmo; i++)
        {
            var tmp = Instantiate(ammo);
            tmp.transform.SetParent(transform);
            tmp.transform.localScale = transform.localScale;
            tmp.transform.position = transform.position;
        }
    }

    void UpdateAmmo()
    {
        if (ammoCount > player.CurrentAmmo)
        {
            RemoveAmmo();
        }
        else if (ammoCount < player.CurrentAmmo)
        {
			while(ammoCount < player.CurrentAmmo)
            	AddAmmo();
        }
    }

    private void RemoveAmmo()
    {
        Destroy(transform.GetChild(0).gameObject);
        ammoCount--;
    }
    private void AddAmmo()
    {
        var tmp = Instantiate(ammo);
        tmp.transform.SetParent(transform);
        tmp.transform.localScale = transform.localScale;
        tmp.transform.position = transform.position;
        ammoCount++;
    }
}
