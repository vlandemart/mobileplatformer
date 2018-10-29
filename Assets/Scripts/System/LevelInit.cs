using UnityEngine;

public class LevelInit : MonoBehaviour {

    public static PlayerMovement player;
    
    public static LevelSettings settings;

    [SerializeField]
    private LevelSettings _settings;


	void Awake () {
        Application.targetFrameRate = 60;
        if (_settings != null) //if settings is null, then it's not a level scene (e.g. menu)
        {
            settings = _settings;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            Instantiate(Resources.Load("Main Camera"));
        }
	}
	
}
