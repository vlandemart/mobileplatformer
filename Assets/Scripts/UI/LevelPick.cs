using UnityEngine;
using UnityEngine.UI;

public class LevelPick : MonoBehaviour {

    [SerializeField]
	private GameObject levelButton  = null;

	void Start () {
        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for(int i = 1; i < count; i++)
        {
            var tmp = Instantiate(levelButton);
            tmp.name = i + "";
            tmp.transform.SetParent(transform);
            tmp.transform.localScale = transform.localScale;
            tmp.transform.GetChild(0).GetComponent<Text>().text = tmp.name;
            tmp.GetComponent<Button>().onClick.AddListener(EventSystemManager.PickLevel);
        }
	}
}
