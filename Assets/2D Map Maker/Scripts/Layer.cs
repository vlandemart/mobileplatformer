using UnityEngine;

public class Layer : MonoBehaviour {

	public Layer(int l,int i)
	{
		id = i;
		priority = l;

	}

	public int id;
	public int priority;
}
