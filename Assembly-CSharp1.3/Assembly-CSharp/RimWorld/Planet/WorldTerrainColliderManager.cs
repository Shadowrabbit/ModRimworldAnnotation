using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001770 RID: 6000
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06008A6D RID: 35437 RVA: 0x0031AE5C File Offset: 0x0031905C
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x06008A6F RID: 35439 RVA: 0x0031AE6F File Offset: 0x0031906F
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}

		// Token: 0x04005811 RID: 22545
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();
	}
}
