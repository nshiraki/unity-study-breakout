using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージごとのデータを管理するクラス
public sealed class StageData
{
	// ステージに応じたデータを取得
	public static int[,] GetStageData(int stage)
	{
		switch (stage)
		{
			case 1:
				return new int[5, 5] // [y, x]
				{
					{1,0,1,0,1 },
					{1,1,1,1,1 },
					{1,0,1,0,1 },
					{1,1,1,1,1 },
					{1,0,1,0,1 },
				};
			case 2:
				return new int[6, 5] // [y, x]
				{
					{1,0,1,0,1 },
					{1,0,1,0,1 },
					{1,0,1,0,1 },
					{1,0,1,0,1 },
					{1,0,1,0,1 },
					{1,0,1,0,1 },
				};
			case 3:
				return new int[6, 5] // [y, x]
				{
					{1,0,1,0,1 },
					{0,1,0,1,0 },
					{1,0,1,0,1 },
					{0,1,0,1,0 },
					{1,0,1,0,1 },
					{0,1,0,1,0 },
				};
			case 4:
				return new int[6, 5] // [y, x]
				{
					{1,1,1,1,1 },
					{1,0,0,0,1 },
					{1,0,0,0,1 },
					{1,0,0,0,1 },
					{1,0,0,0,1 },
					{1,1,1,1,1 },
				};
			case 5:
				return new int[6, 5] // [y, x]
				{
					{1,1,1,1,1 },
					{1,0,0,0,0 },
					{1,0,1,1,1 },
					{1,0,1,0,1 },
					{1,0,0,0,1 },
					{1,1,1,1,1 },
				};
			default:
				return new int[6, 5]
				{
					{1,1,1,1,1 },
					{1,1,1,1,1 },
					{1,1,1,1,1 },
					{1,1,1,1,1 },
					{1,1,1,1,1 },
					{1,1,1,1,1 },
				};
		}
	}
}
