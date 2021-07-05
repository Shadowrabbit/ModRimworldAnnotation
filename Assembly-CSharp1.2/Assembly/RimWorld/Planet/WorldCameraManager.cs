using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002040 RID: 8256
	public static class WorldCameraManager
	{
		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x0600AEFC RID: 44796 RVA: 0x00071F42 File Offset: 0x00070142
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x0600AEFD RID: 44797 RVA: 0x00071F49 File Offset: 0x00070149
		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x0600AEFE RID: 44798 RVA: 0x00071F50 File Offset: 0x00070150
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		// Token: 0x0600AEFF RID: 44799 RVA: 0x0032E07C File Offset: 0x0032C27C
		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		// Token: 0x0600AF00 RID: 44800 RVA: 0x0032E12C File Offset: 0x0032C32C
		private static Camera CreateWorldCamera()
		{
			GameObject gameObject = new GameObject("WorldCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<WorldCameraDriver>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldLayerMask;
			component.clearFlags = CameraClearFlags.Depth;
			component.useOcclusionCulling = true;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 1200f;
			component.fieldOfView = 20f;
			component.depth = 1f;
			return component;
		}

		// Token: 0x0600AF01 RID: 44801 RVA: 0x0032E1C4 File Offset: 0x0032C3C4
		private static Camera CreateWorldSkyboxCamera(Camera parent)
		{
			GameObject gameObject = new GameObject("WorldSkyboxCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(true);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.SetParent(parent.transform);
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldSkyboxLayerMask;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = WorldCameraManager.SkyColor;
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 1200f;
			component.fieldOfView = 60f;
			component.depth = 0f;
			return component;
		}

		// Token: 0x04007843 RID: 30787
		private static Camera worldCameraInt;

		// Token: 0x04007844 RID: 30788
		private static Camera worldSkyboxCameraInt;

		// Token: 0x04007845 RID: 30789
		private static WorldCameraDriver worldCameraDriverInt;

		// Token: 0x04007846 RID: 30790
		public static readonly string WorldLayerName = "World";

		// Token: 0x04007847 RID: 30791
		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		// Token: 0x04007848 RID: 30792
		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		// Token: 0x04007849 RID: 30793
		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		// Token: 0x0400784A RID: 30794
		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		// Token: 0x0400784B RID: 30795
		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		// Token: 0x0400784C RID: 30796
		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.11764706f);
	}
}
