using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001983 RID: 6531
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x0600906E RID: 36974 RVA: 0x0029A180 File Offset: 0x00298380
		public static RenderTexture Get(Pawn pawn, Vector2 size, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f, bool supersample = true, bool compensateForUIScale = true)
		{
			if (supersample)
			{
				size *= 1.25f;
			}
			if (compensateForUIScale)
			{
				size *= Prefs.UIScale;
			}
			Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.GetOrCreateCachedPortraitsWithParams(size, cameraOffset, cameraZoom).CachedPortraits;
			PortraitsCache.CachedPortrait cachedPortrait;
			if (dictionary.TryGetValue(pawn, out cachedPortrait))
			{
				if (!cachedPortrait.RenderTexture.IsCreated())
				{
					cachedPortrait.RenderTexture.Create();
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom);
				}
				else if (cachedPortrait.Dirty)
				{
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom);
				}
				dictionary.Remove(pawn);
				dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				return cachedPortrait.RenderTexture;
			}
			RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
			PortraitsCache.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
			dictionary.Add(pawn, new PortraitsCache.CachedPortrait(renderTexture, false, Time.time));
			return renderTexture;
		}

		// Token: 0x0600906F RID: 36975 RVA: 0x0029A25C File Offset: 0x0029845C
		public static void SetDirty(Pawn pawn)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.CachedPortrait cachedPortrait;
				if (dictionary.TryGetValue(pawn, out cachedPortrait) && !cachedPortrait.Dirty)
				{
					dictionary.Remove(pawn);
					dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
			}
		}

		// Token: 0x06009070 RID: 36976 RVA: 0x00060E3D File Offset: 0x0005F03D
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x06009071 RID: 36977 RVA: 0x0029A2CC File Offset: 0x002984CC
		public static void Clear()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in PortraitsCache.cachedPortraits[i].CachedPortraits)
				{
					PortraitsCache.DestroyRenderTexture(keyValuePair.Value.RenderTexture);
				}
			}
			PortraitsCache.cachedPortraits.Clear();
			for (int j = 0; j < PortraitsCache.renderTexturesPool.Count; j++)
			{
				PortraitsCache.DestroyRenderTexture(PortraitsCache.renderTexturesPool[j]);
			}
			PortraitsCache.renderTexturesPool.Clear();
		}

		// Token: 0x06009072 RID: 36978 RVA: 0x0029A390 File Offset: 0x00298590
		private static PortraitsCache.CachedPortraitsWithParams GetOrCreateCachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				if (PortraitsCache.cachedPortraits[i].Size == size && PortraitsCache.cachedPortraits[i].CameraOffset == cameraOffset && PortraitsCache.cachedPortraits[i].CameraZoom == cameraZoom)
				{
					return PortraitsCache.cachedPortraits[i];
				}
			}
			PortraitsCache.CachedPortraitsWithParams cachedPortraitsWithParams = new PortraitsCache.CachedPortraitsWithParams(size, cameraOffset, cameraZoom);
			PortraitsCache.cachedPortraits.Add(cachedPortraitsWithParams);
			return cachedPortraitsWithParams;
		}

		// Token: 0x06009073 RID: 36979 RVA: 0x00060E49 File Offset: 0x0005F049
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x06009074 RID: 36980 RVA: 0x0029A420 File Offset: 0x00298620
		private static void RemoveExpiredCachedPortraits()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toRemove.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in dictionary)
				{
					if (keyValuePair.Value.Expired)
					{
						PortraitsCache.toRemove.Add(keyValuePair.Key);
						PortraitsCache.renderTexturesPool.Add(keyValuePair.Value.RenderTexture);
					}
				}
				for (int j = 0; j < PortraitsCache.toRemove.Count; j++)
				{
					dictionary.Remove(PortraitsCache.toRemove[j]);
				}
				PortraitsCache.toRemove.Clear();
			}
		}

		// Token: 0x06009075 RID: 36981 RVA: 0x0029A514 File Offset: 0x00298714
		private static void SetAnimatedPortraitsDirty()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toSetDirty.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in dictionary)
				{
					if (PortraitsCache.IsAnimated(keyValuePair.Key) && !keyValuePair.Value.Dirty)
					{
						PortraitsCache.toSetDirty.Add(keyValuePair.Key);
					}
				}
				for (int j = 0; j < PortraitsCache.toSetDirty.Count; j++)
				{
					PortraitsCache.CachedPortrait cachedPortrait = dictionary[PortraitsCache.toSetDirty[j]];
					dictionary.Remove(PortraitsCache.toSetDirty[j]);
					dictionary.Add(PortraitsCache.toSetDirty[j], new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
				PortraitsCache.toSetDirty.Clear();
			}
		}

		// Token: 0x06009076 RID: 36982 RVA: 0x0029A638 File Offset: 0x00298838
		private static RenderTexture NewRenderTexture(Vector2 size)
		{
			int num = PortraitsCache.renderTexturesPool.FindLastIndex((RenderTexture x) => x.width == (int)size.x && x.height == (int)size.y);
			if (num != -1)
			{
				RenderTexture result = PortraitsCache.renderTexturesPool[num];
				PortraitsCache.renderTexturesPool.RemoveAt(num);
				return result;
			}
			return new RenderTexture((int)size.x, (int)size.y, 24)
			{
				filterMode = FilterMode.Bilinear
			};
		}

		// Token: 0x06009077 RID: 36983 RVA: 0x00060E57 File Offset: 0x0005F057
		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		// Token: 0x06009078 RID: 36984 RVA: 0x00060E67 File Offset: 0x0005F067
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Token: 0x04005BEC RID: 23532
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04005BED RID: 23533
		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		// Token: 0x04005BEE RID: 23534
		private const float SupersampleScale = 1.25f;

		// Token: 0x04005BEF RID: 23535
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x04005BF0 RID: 23536
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x02001984 RID: 6532
		private struct CachedPortrait
		{
			// Token: 0x170016D7 RID: 5847
			// (get) Token: 0x0600907A RID: 36986 RVA: 0x00060EBA File Offset: 0x0005F0BA
			// (set) Token: 0x0600907B RID: 36987 RVA: 0x00060EC2 File Offset: 0x0005F0C2
			public RenderTexture RenderTexture { get; private set; }

			// Token: 0x170016D8 RID: 5848
			// (get) Token: 0x0600907C RID: 36988 RVA: 0x00060ECB File Offset: 0x0005F0CB
			// (set) Token: 0x0600907D RID: 36989 RVA: 0x00060ED3 File Offset: 0x0005F0D3
			public bool Dirty { get; private set; }

			// Token: 0x170016D9 RID: 5849
			// (get) Token: 0x0600907E RID: 36990 RVA: 0x00060EDC File Offset: 0x0005F0DC
			// (set) Token: 0x0600907F RID: 36991 RVA: 0x00060EE4 File Offset: 0x0005F0E4
			public float LastUseTime { get; private set; }

			// Token: 0x170016DA RID: 5850
			// (get) Token: 0x06009080 RID: 36992 RVA: 0x00060EED File Offset: 0x0005F0ED
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}

			// Token: 0x06009081 RID: 36993 RVA: 0x00060F02 File Offset: 0x0005F102
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x04005BF4 RID: 23540
			private const float CacheDuration = 1f;
		}

		// Token: 0x02001985 RID: 6533
		private struct CachedPortraitsWithParams
		{
			// Token: 0x170016DB RID: 5851
			// (get) Token: 0x06009082 RID: 36994 RVA: 0x00060F20 File Offset: 0x0005F120
			// (set) Token: 0x06009083 RID: 36995 RVA: 0x00060F28 File Offset: 0x0005F128
			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits { get; private set; }

			// Token: 0x170016DC RID: 5852
			// (get) Token: 0x06009084 RID: 36996 RVA: 0x00060F31 File Offset: 0x0005F131
			// (set) Token: 0x06009085 RID: 36997 RVA: 0x00060F39 File Offset: 0x0005F139
			public Vector2 Size { get; private set; }

			// Token: 0x170016DD RID: 5853
			// (get) Token: 0x06009086 RID: 36998 RVA: 0x00060F42 File Offset: 0x0005F142
			// (set) Token: 0x06009087 RID: 36999 RVA: 0x00060F4A File Offset: 0x0005F14A
			public Vector3 CameraOffset { get; private set; }

			// Token: 0x170016DE RID: 5854
			// (get) Token: 0x06009088 RID: 37000 RVA: 0x00060F53 File Offset: 0x0005F153
			// (set) Token: 0x06009089 RID: 37001 RVA: 0x00060F5B File Offset: 0x0005F15B
			public float CameraZoom { get; private set; }

			// Token: 0x0600908A RID: 37002 RVA: 0x00060F64 File Offset: 0x0005F164
			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}
		}
	}
}
