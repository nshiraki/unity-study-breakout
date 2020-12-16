using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 1.0f;
	// GameControllerにアクセスする変数
	GameController gameController;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update()
	{
		if(gameController.status == GameController.GameStatus.Playing)
		{
			Vector3 pos = transform.position;
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				pos.x -= speed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				pos.x += speed * Time.deltaTime;
			}
			float limit = 3.75f;
			pos.x = Mathf.Clamp(pos.x, -limit, limit);
			transform.position = pos;

		}
	}
}
