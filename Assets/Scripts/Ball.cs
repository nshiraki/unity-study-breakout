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
	// サーブされたかどうか
	public bool isServed = false;

	// Start is called before the first frame update
	void Start()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();

		rb = GetComponent<Rigidbody>();
	}

	public void Serve()
	{
		if (!isServed)
		{
			rb.velocity += new Vector3(5f, 0f, 5f);
			isServed = true;
		}
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
		else if (collision.gameObject.CompareTag("Player"))
		{
			// ボールの中心がプレイヤーの中心からどのくらい離れているか
			Vector3 distance = transform.position - collision.transform.position;
			// プレイヤーの中心であれば、90度（真上）とする。
			// プレイヤーの中心から左に離れていれば、最大で90度＋80度とする。
			// プレイヤーの中心から右に離れていれば、最大で90度－80度とする。
			float playerWidth = collision.collider.bounds.size.x;
			float deg = 90f - (distance.x / (playerWidth / 2)) * 80f;
			print("deg=" + deg);
			float rad = deg * Mathf.Deg2Rad;
			rb.velocity += new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
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
