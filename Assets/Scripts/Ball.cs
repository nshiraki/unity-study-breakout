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
		// 初回だけAddForceを実行して指定したベクトルへボールを打ち出す
		// AddForceに与えているパラメータについて
		// Vector3.forward（上 Vector3(0, 0, 1) と同じ意味）
		// Vector3.right （右 Vector3(1, 0, 0) と同じ意味）
		// ForceMode.VelocityChange（質量を無視して瞬間的に力を加える）
		rb.AddForce((Vector3.forward + Vector3.right) * speed, ForceMode.VelocityChange);
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
			// 壁に当たるたびに一定の速度にする
			rb.velocity = rb.velocity.normalized * 10f;

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
