using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02001753 RID: 5971
	public static class WorldCameraManager
	{
		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x060089D3 RID: 35283 RVA: 0x00318209 File Offset: 0x00316409
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x060089D4 RID: 35284 RVA: 0x00318210 File Offset: 0x00316410
		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x060089D5 RID: 35285 RVA: 0x00318217 File Offset: 0x00316417
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		// Token: 0x060089D6 RID: 35286 RVA: 0x00318220 File Offset: 0x00316420
		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		// Token: 0x060089D7 RID: 35287 RVA: 0x003182D0 File Offset: 0x003164D0
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

		// Token: 0x060089D8 RID: 35288 RVA: 0x00318368 File Offset: 0x00316568
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

		// Token: 0x04005796 RID: 22422
		private static Camera worldCameraInt;

		// Token: 0x04005797 RID: 22423
		private static Camera worldSkyboxCameraInt;

		// Token: 0x04005798 RID: 22424
		private static WorldCameraDriver worldCameraDriverInt;

		// Token: 0x04005799 RID: 22425
		public static readonly string WorldLayerName = "World";

		// Token: 0x0400579A RID: 22426
		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		// Token: 0x0400579B RID: 22427
		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		// Token: 0x0400579C RID: 22428
		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		// Token: 0x0400579D RID: 22429
		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		// Token: 0x0400579E RID: 22430
		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		// Token: 0x0400579F RID: 22431
		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.11764706f);
	}
}
