using UnityEngine;

public class LevelExit : MonoBehaviour {

    [SerializeField]
	private GameUIHandler uI = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() && collision is BoxCollider2D)
        {
            UI._Finish();
        }
    }

    public GameUIHandler UI
    {
        get
        {
            return uI;
        }

        set
        {
            uI = value;
        }
    }
}
