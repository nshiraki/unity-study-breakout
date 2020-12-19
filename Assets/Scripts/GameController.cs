using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	// ブロックのprefab
	public GameObject blockPrefab = null;
	// 一時停止中かどうか
	public bool isPause = false;
	// MessageTextにアクセスする変数
	public GameObject messageText;
	// PlayerLeftTextにアクセスする変数
	public Text playerLeftText;
	// Playerにアクセスする変数
	public GameObject player;
	// Ballにアクセスする変数
	public GameObject ball;
	// ブロック数
	public int blockCount = 0;
	// 残りのプレイヤー数
	public int playerLeft;
	// 現在のステージ数
	public int stage = 1;
	// 現在のステータスを格納
	public GameStatus status;
	// 次のステータスを格納
	public GameStatus nextStatus = GameStatus.Title;
	// 前のステータスを格納
	public GameStatus preStatus;
	// 最大ステージ数
	private const int MAX_STAGE_NUM = 5;
	// ステップ数（GameStatusを更新すると0クリアされる）
	private int step;
	// 経過時間（GameStatusを更新すると0クリアされる）
	private float countTime;
	// 表示用メッセージ
	private StringBuilder message = new StringBuilder();

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Quit();
		}

		if (status != nextStatus)
		{
			preStatus = status;
			status = nextStatus;
			countTime = 0f;
			step = 0;
		}

		countTime += Time.deltaTime;

		switch (status)
		{
			case GameStatus.Title:
				if (step == 0)
				{
					// タイトル表示
					message.Clear();
					message.AppendLine("breakout");
					message.AppendLine();
					message.AppendLine();
					message.AppendLine("press start space");
					ShowMessageText(message.ToString());
					//残り機数初期化
					playerLeft = 2;
					// ステージ数
					stage = 1;
					// プレイヤー非表示
					player.SetActive(false);
					// ボール非表示
					ball.SetActive(false);
					// 次のステップへ
					step++;
				}
				else if (step == 1)
				{
					if (Input.GetKeyDown(KeyCode.Space))
					{
						nextStatus = GameStatus.InitStage;
					}
				}
				break;
			case GameStatus.InitStage:
				// ステージデータ読み込み
				LoadStageData(stage);
				// プレイヤー表示
				player.SetActive(true);
				// ボール表示
				ball.SetActive(true);
				InitPlayerPosition();
				InitBallPosition();
				nextStatus = GameStatus.StageStart;
				break;
			case GameStatus.StageStart:
				if (step == 0)
				{
					message.Clear();
					message.AppendLine($"STAGE {stage}");
					if (countTime > 1f)
					{
						// 一定時間経過したらメッセージ追加
						message.AppendLine();
						message.AppendLine($"START");
						// 経過時間リセット
						countTime = 0;
						// 次のステップへ
						step++;
					}
					ShowMessageText(message.ToString());
				}
				else if (step == 1)
				{
					if (countTime > 1f)
					{
						HideMessageText();
						nextStatus = GameStatus.Playing;
					}
				}
				break;
			case GameStatus.Playing:
				if (blockCount <= 0)
				{
					nextStatus = GameStatus.StageClear;
				}
				break;
			case GameStatus.GameFailed:
				ShowMessageText("Failed!");
				// 一定時間経過
				if (countTime > 3f)
				{
					// 残り機数がある場合
					if (playerLeft > 0)
					{
						// 残り機数を減らす
						playerLeft--;
						InitPlayerPosition();
						InitBallPosition();
						// ステージを再開する
						nextStatus = GameStatus.StageStart;
					}
					// 残り機数がない場合
					else
					{
						// ステージに残っているすべてのブロックを削除
						RemoveAllBlocks();
						// ゲームオーバーへ
						nextStatus = GameStatus.GameOver;
					}
				}
				break;
			case GameStatus.StageClear:
				ShowMessageText($"STAGE {stage} CREAR");
				if (countTime > 3f)
				{
					if (stage < MAX_STAGE_NUM)
					{
						stage++;
						nextStatus = GameStatus.InitStage;
					}
					else
					{
						// 最後のステージの場合は暫定でゲームオーバーにする
						nextStatus = GameStatus.GameOver;
					}
				}
				break;
			case GameStatus.GameOver:
				if (step == 0)
				{
					// メッセージ表示
					ShowMessageText("GAME OVER");
					// プレイヤー非表示
					player.SetActive(false);
					// ボール非表示
					ball.SetActive(false);
					// 経過時間リセット
					countTime = 0;
					// 次のステップへ
					step++;
				}
				else if (step == 1)
				{
					// 一定時間経過
					if (countTime > 3f)
					{
						nextStatus = GameStatus.Title;
					}
				}
				break;
			default:
				break;
		}

		playerLeftText.text = "LEFT=" + playerLeft;
	}
	// プレイヤーを初期位置に配置する
	private void InitPlayerPosition()
	{
		player.transform.position = new Vector3(0f, 1f, -6.5f);
	}
	// ボールを初期位置に配置する
	private void InitBallPosition()
	{
		ball.transform.position = new Vector3(0f, 1f, -5.5f);
	}
	// すべてのブロックを削除する
	private void RemoveAllBlocks()
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Block");
		foreach (GameObject gameObject in gameObjects)
		{
			blockCount--;
			Destroy(gameObject);
		}
	}

	// ステージデータの読み込み
	private void LoadStageData(int stage)
	{
		float left = -4;
		float top = 7.5f;
		float width = 2f;
		float height = 1f;
		blockCount = 0;

		int[,] data = StageData.GetStageData(stage);

		for (int j = 0; j < data.GetLength(0); j++)
		{
			for (int i = 0; i < data.GetLength(1); i++)
			{
				if (data[j, i] > 0 && blockPrefab != null)
				{
					GameObject block = Instantiate(blockPrefab);
					float x = left + i * width;
					float z = top - j * height;
					block.transform.position = new Vector3(x, 1f, z);
					blockCount++;
				}
			}
		}
	}

	// メッセージを表示する
	private void ShowMessageText(string message)
	{
		messageText.GetComponent<Text>().text = message;
		messageText.SetActive(true);
	}
	// メッセージを非表示にする
	private void HideMessageText()
	{
		messageText.SetActive(false);
	}

	private void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
				UnityEngine.Application.Quit();
#endif
	}
	// 返球失敗時に外部から呼ばれるコールバック関数
	public void OnGameFailed()
	{
		// ゲーム失敗の処理に移行する
		nextStatus = GameStatus.GameFailed;
	}

	// ブロック破壊時に外部から呼ばれるコールバック関数
	public void OnDestroyBlock(Block block)
	{
		blockCount--;
	}
}
