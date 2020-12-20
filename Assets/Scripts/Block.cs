using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
	public int life;
	// 爆発パーティクル
	public GameObject explosionParticlePrefab;
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
			// 爆発パーティクルのオブジェクト生成
			GameObject explosionObject = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
			// 爆発パーティクルを1秒後に消す
			Destroy(explosionObject, 1f);
			// GameControllerに通知する
			gameController.OnDestroyBlock(this);
			// ブロックを消す
			Destroy(this.gameObject);
		}
	}
}
