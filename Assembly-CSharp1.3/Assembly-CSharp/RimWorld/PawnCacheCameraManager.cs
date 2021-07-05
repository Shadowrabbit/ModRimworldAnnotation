using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD6 RID: 3542
	public static class PawnCacheCameraManager
	{
		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x0600523C RID: 21052 RVA: 0x001BC137 File Offset: 0x001BA337
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.pawnCacheCameraInt;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x001BC13E File Offset: 0x001BA33E
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.pawnCacheRendererInt;
			}
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x001BC160 File Offset: 0x001BA360
		private static Camera CreatePawnCacheCamera()
		{
			GameObject gameObject = new GameObject("PortraitCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<PawnCacheRenderer>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.position = new Vector3(0f, 15f, 0f);
			component.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			component.orthographic = true;
			component.cullingMask = 0;
			component.orthographicSize = 1f;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = new Color(0f, 0f, 0f, 0f);
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			Camera camera = Current.Camera;
			component.nearClipPlane = camera.nearClipPlane;
			component.farClipPlane = camera.farClipPlane;
			return component;
		}

		// Token: 0x04003097 RID: 12439
		private static Camera pawnCacheCameraInt = PawnCacheCameraManager.CreatePawnCacheCamera();

		// Token: 0x04003098 RID: 12440
		private static PawnCacheRenderer pawnCacheRendererInt = PawnCacheCameraManager.pawnCacheCameraInt.GetComponent<PawnCacheRenderer>();
	}
}
