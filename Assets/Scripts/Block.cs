using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
	public int life;
	// GameControllerにアクセスする変数
	GameController gameController;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		life--;
		if (life <= 0)
		{
			gameController.OnDestroyBlock(this);
			Destroy(this.gameObject);
		}
	}
}
