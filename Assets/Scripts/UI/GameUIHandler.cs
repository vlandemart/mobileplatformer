using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour {
	[SerializeField]
	private GameObject gameHUD = null;
	[SerializeField]
	private GameObject deathScreen = null;
	[SerializeField]
	private GameObject pauseMenu = null;
    [SerializeField]
    private GameObject finishedLevelScreen = null;
    [SerializeField]
    private PlayerMovement player = null;

    [Header("Time setting")]
    [SerializeField]
    private Text finishedLevelTime = null;

    [SerializeField]
    private Color successColor;
    [SerializeField]
    private Color failedColor;

    [Header("Hit setting")]
    [SerializeField]
    private Text hitText;

    [Header("Results")]
    [SerializeField]
    private Text resultText;

    private int coins;

    private void Awake()
    {
        player = LevelInit.player;
        player.GameUIHandler = this;
        GameObject.FindGameObjectWithTag("Level Exit").GetComponent<LevelExit>().UI = this;
        PrepareHUD();
    }
    private void Start(){
		Time.timeScale = 1f;
		gameHUD.SetActive (true);
		deathScreen.SetActive (false);
		pauseMenu.SetActive (false);
	}

	#region Death screen

	public void DeathScreen(){
		gameHUD.SetActive (false);
		deathScreen.SetActive (true);
		Time.timeScale = 0.1f;
	}

	public void _Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void _BackToMenu(){
        Time.timeScale = 1f;
		SceneManager.LoadScene (0);
	}
	#endregion

	#region Pause screen

	public void _Pause()
	{
		pauseMenu.SetActive (true);
		gameHUD.SetActive (false);
		Time.timeScale = 0f;
	}

	public void _Resume(){
		gameHUD.SetActive (true);
		pauseMenu.SetActive (false);
		Time.timeScale = 1f;
	}
    #endregion

    #region Finish screen

    public void _Finish()
    {
        finishedLevelScreen.SetActive(true);
        player.levelStats.CalculateLevelScore(LevelInit.settings.PerfectTime);

        var finishedTime = player.levelStats.Time;

        string finishedTimeString = string.Format("{0}:{1:00}", (int)finishedTime / 60, (int)finishedTime % 60);
        string perfectTimeString = string.Format("{0}:{1:00}", (int)LevelInit.settings.PerfectTime / 60, (int)LevelInit.settings.PerfectTime % 60);


        finishedLevelTime.text = finishedTimeString + "/" + perfectTimeString;
        finishedLevelTime.color = finishedTime <= LevelInit.settings.PerfectTime ? successColor : failedColor;

        hitText.text = player.levelStats.DamageCount + "/" + PlayerMovement.Stats.MaxDamageCounts;
        hitText.color = player.levelStats.DamageCount <= PlayerMovement.Stats.MaxDamageCounts ? successColor : failedColor;

        resultText.text = player.levelStats.LevelScore.ToString();

        gameHUD.SetActive(false);
        player.gameObject.SetActive(false);
        //finishedLevelScreen.GetComponent<Animator>().SetTrigger("Finished");
    }

	public void _Continue(){
		if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene ().buildIndex + 1) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		} else {
			_BackToMenu ();
		}
	}

    #endregion

    #region Preparing HUD

    //Выставляем ивенты вручную, так как камера теперь загружается в сцену и не имеет ссылок на игрока
    void PrepareHUD()
    {
        var ctrl = GameObject.FindGameObjectWithTag("ControlUI").transform;   
        LeftButton(ctrl);
        RightButton(ctrl);
        Jump(ctrl);
        MeleeAttack(ctrl);
        RangedAttack(ctrl);
    }

    void LeftButton(Transform ctrl) 
    {
        var left = ctrl.transform.Find("ArrowPanel/Left");
        EventTrigger LeftTrigger = left.GetComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };

        enter.callback.AddListener((data) => { player.LeftArrow = true; });
        LeftTrigger.triggers.Add(enter);

        EventTrigger.Entry exit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };

        exit.callback.AddListener((data) => { player.LeftArrow = false; });
        LeftTrigger.triggers.Add(exit);
    }

    void RightButton(Transform ctrl)
    {
        var right = ctrl.transform.Find("ArrowPanel/Right");
        EventTrigger RightTrigger = right.GetComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };

        enter.callback.AddListener((data) => { player.RightArrow = true; });
        RightTrigger.triggers.Add(enter);

        EventTrigger.Entry exit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };

        exit.callback.AddListener((data) => { player.RightArrow = false; });
        RightTrigger.triggers.Add(exit);
    }

    void Jump(Transform ctrl)
    {
        var jump = ctrl.transform.Find("ButtonsPanel/Jump");
        EventTrigger JumpTrigger = jump.GetComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };

        enter.callback.AddListener((data) => { player.Jumping = true; });
        JumpTrigger.triggers.Add(enter);
    }

    void MeleeAttack(Transform ctrl)
    {
        var meleeAttack = ctrl.transform.Find("ButtonsPanel/Attack");
        EventTrigger AttackTrigger = meleeAttack.GetComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };

        enter.callback.AddListener((data) => { player.MeleeAttack = true; });
        AttackTrigger.triggers.Add(enter);
    }

    void RangedAttack(Transform ctrl)
    {
        var rangedAttack = ctrl.transform.Find("ButtonsPanel/RangedAttack");
        rangedAttack.GetComponent<Button>().onClick.AddListener(() => player.RangedAttack = true);
    }
    #endregion 
}
