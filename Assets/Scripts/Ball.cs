using UnityEngine;

public class Ball : MonoBehaviour
{
	public float speed = 1.0f;
	// Rigidbodyにアクセスする変数
	Rigidbody rb;
	// GameControllerにアクセスする変数
	GameController gameController;
	// 一時停止前に保管する速度
	Vector3 preVelocity;
	Vector3 preAngularVelocity;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();

		rb = GetComponent<Rigidbody>();
		rb.velocity += new Vector3(5f, 0f, 5f);
	}

	private void Update()
	{
		if (gameController.status != GameStatus.Playing)
		{
			if (!rb.isKinematic)
			{
				preVelocity = rb.velocity;
				preAngularVelocity = rb.angularVelocity;
				rb.isKinematic = true;
			}
		}
		else
		{
			if (rb.isKinematic)
			{
				rb.isKinematic = false;
				rb.velocity = preVelocity;
				rb.angularVelocity = preAngularVelocity;
			}

			// 一定の速度にする
			rb.velocity = rb.velocity.normalized * speed;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		// 下の壁に接触した場合、ゲーム失敗の処理を行う
		if (collision.gameObject.CompareTag("WallBottom"))
		{
			gameController.OnGameFailed();
		}
		else
		{
			// ボールが水平方向にしか跳ね返らなくなってしまう対策
			float limit = 5f;
			if (Mathf.Abs(rb.velocity.z) < limit)
			{
				float vec = rb.velocity.z > 0 ? 5 : -5;
				rb.velocity += new Vector3(0f, 0f, vec);
			}
			// ボールが垂直方向にしか跳ね返らなくなってしまう対策
			if (Mathf.Abs(rb.velocity.x) < limit)
			{
				float vec = rb.velocity.x > 0 ? 5 : -5;
				rb.velocity += new Vector3(vec, 0f, 0f);
			}
		}
	}
}
