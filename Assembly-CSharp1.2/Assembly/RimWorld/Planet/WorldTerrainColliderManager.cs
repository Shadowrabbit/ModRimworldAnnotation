using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200206F RID: 8303
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x0600B01D RID: 45085 RVA: 0x000726FA File Offset: 0x000708FA
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x0600B01F RID: 45087 RVA: 0x0007270D File Offset: 0x0007090D
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}

		// Token: 0x04007924 RID: 31012
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();
	}
}
