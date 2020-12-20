using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 1.0f;
	// 爆発パーティクル
	public GameObject explosionParticlePrefab;
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
		if (gameController.status == GameStatus.Playing)
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

	// プレイヤーを爆発させる
	public void Explosion()
	{
		// プレイヤーの表示を消す
		this.gameObject.SetActive(false);
		// 爆発パーティクルのオブジェクト生成
		GameObject explosionObject = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
		// 爆発パーティクルを1秒後に消す
		Destroy(explosionObject, 1f);
	}
}
