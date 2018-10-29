using UnityEngine;

public class HealthBar : MonoBehaviour {

    [SerializeField]
	private PlayerMovement player = null;
    [SerializeField]
	private GameObject heart  = null;

    private int maxHearts;
    private int heartCount;

    private void Awake()
    {
        player = LevelInit.player;
    }

    void Start () {
        player.OnHealthChange += UpdateHearts;
        maxHearts = (int)player.MaxHP;
        heartCount = maxHearts;
        for(int i = 0; i < maxHearts; i++)
        {
            var tmp = Instantiate(heart);
            tmp.transform.SetParent(transform);
            tmp.transform.localScale = transform.localScale;
            tmp.transform.position = transform.position;
        }
    }

    void UpdateHearts()
    {
        if(heartCount > player.HP)
        {
            RemoveHeart();
        }else if(heartCount < player.HP)
        {
            AddHeart();
        }
    }

    private void RemoveHeart()
    {
        Destroy(transform.GetChild(0).gameObject);
        heartCount--;
    }
    private void AddHeart()
    {
        var tmp = Instantiate(heart);
        tmp.transform.SetParent(transform);
        tmp.transform.localScale = transform.localScale;
        tmp.transform.position = transform.position;
        heartCount++;
    }
}
