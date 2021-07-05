using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001298 RID: 4760
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x060071BE RID: 29118 RVA: 0x002605C8 File Offset: 0x0025E7C8
		public static RenderTexture Get(Pawn pawn, Vector2 size, Rot4 rotation, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f, bool supersample = true, bool compensateForUIScale = true, bool renderHeadgear = true, bool renderClothes = true, Dictionary<Apparel, Color> overrideApparelColors = null, bool stylingStation = false)
		{
			if (supersample)
			{
				size *= 1.25f;
			}
			if (compensateForUIScale)
			{
				size *= Prefs.UIScale;
			}
			Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.GetOrCreateCachedPortraitsWithParams(size, cameraOffset, cameraZoom, rotation, overrideApparelColors, stylingStation).CachedPortraits;
			PortraitsCache.CachedPortrait cachedPortrait;
			if (dictionary.TryGetValue(pawn, out cachedPortrait))
			{
				if (!cachedPortrait.RenderTexture.IsCreated())
				{
					cachedPortrait.RenderTexture.Create();
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom, rotation, renderHeadgear, renderClothes, overrideApparelColors, stylingStation);
				}
				else if (cachedPortrait.Dirty)
				{
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom, rotation, renderHeadgear, renderClothes, overrideApparelColors, stylingStation);
				}
				dictionary.Remove(pawn);
				dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				return cachedPortrait.RenderTexture;
			}
			RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
			PortraitsCache.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom, rotation, renderHeadgear, renderClothes, overrideApparelColors, stylingStation);
			dictionary.Add(pawn, new PortraitsCache.CachedPortrait(renderTexture, false, Time.time));
			return renderTexture;
		}

		// Token: 0x060071BF RID: 29119 RVA: 0x002606CC File Offset: 0x0025E8CC
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

		// Token: 0x060071C0 RID: 29120 RVA: 0x00260739 File Offset: 0x0025E939
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x060071C1 RID: 29121 RVA: 0x00260748 File Offset: 0x0025E948
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

		// Token: 0x060071C2 RID: 29122 RVA: 0x0026080C File Offset: 0x0025EA0C
		private static PortraitsCache.CachedPortraitsWithParams GetOrCreateCachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom, Rot4 rotation, Dictionary<Apparel, Color> overrideApparelColors = null, bool stylingStation = false)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				if (PortraitsCache.cachedPortraits[i].Size == size && PortraitsCache.cachedPortraits[i].CameraOffset == cameraOffset && PortraitsCache.cachedPortraits[i].CameraZoom == cameraZoom && PortraitsCache.cachedPortraits[i].Rotation == rotation && PortraitsCache.cachedPortraits[i].StylingStation == stylingStation && GenCollection.DictsEqual<Apparel, Color>(PortraitsCache.cachedPortraits[i].OverrideApparelColors, overrideApparelColors))
				{
					return PortraitsCache.cachedPortraits[i];
				}
			}
			PortraitsCache.CachedPortraitsWithParams cachedPortraitsWithParams = new PortraitsCache.CachedPortraitsWithParams(size, cameraOffset, cameraZoom, rotation, overrideApparelColors, stylingStation);
			PortraitsCache.cachedPortraits.Add(cachedPortraitsWithParams);
			return cachedPortraitsWithParams;
		}

		// Token: 0x060071C3 RID: 29123 RVA: 0x002608F8 File Offset: 0x0025EAF8
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x060071C4 RID: 29124 RVA: 0x00260908 File Offset: 0x0025EB08
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

		// Token: 0x060071C5 RID: 29125 RVA: 0x002609FC File Offset: 0x0025EBFC
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

		// Token: 0x060071C6 RID: 29126 RVA: 0x00260B20 File Offset: 0x0025ED20
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
				useMipMap = false,
				filterMode = FilterMode.Bilinear
			};
		}

		// Token: 0x060071C7 RID: 29127 RVA: 0x00260B9C File Offset: 0x0025ED9C
		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom, Rot4 rotation, bool renderHeadgear, bool renderClothes, Dictionary<Apparel, Color> overrideApparelColors = null, bool stylingStation = false)
		{
			float angle = 0f;
			Vector3 positionOffset = default(Vector3);
			if (pawn.Dead || pawn.Downed)
			{
				angle = 85f;
				positionOffset.x -= 0.18f;
				positionOffset.z -= 0.18f;
			}
			Find.PawnCacheRenderer.RenderPawn(pawn, renderTexture, cameraOffset, cameraZoom, angle, rotation, pawn.health.hediffSet.HasHead, true, renderHeadgear, renderClothes, true, positionOffset, overrideApparelColors, stylingStation);
		}

		// Token: 0x060071C8 RID: 29128 RVA: 0x00260C19 File Offset: 0x0025EE19
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Token: 0x04003EAB RID: 16043
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04003EAC RID: 16044
		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		// Token: 0x04003EAD RID: 16045
		private const float SupersampleScale = 1.25f;

		// Token: 0x04003EAE RID: 16046
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x04003EAF RID: 16047
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x02002609 RID: 9737
		private struct CachedPortrait
		{
			// Token: 0x17002099 RID: 8345
			// (get) Token: 0x0600D4E2 RID: 54498 RVA: 0x0040601C File Offset: 0x0040421C
			// (set) Token: 0x0600D4E3 RID: 54499 RVA: 0x00406024 File Offset: 0x00404224
			public RenderTexture RenderTexture { get; private set; }

			// Token: 0x1700209A RID: 8346
			// (get) Token: 0x0600D4E4 RID: 54500 RVA: 0x0040602D File Offset: 0x0040422D
			// (set) Token: 0x0600D4E5 RID: 54501 RVA: 0x00406035 File Offset: 0x00404235
			public bool Dirty { get; private set; }

			// Token: 0x1700209B RID: 8347
			// (get) Token: 0x0600D4E6 RID: 54502 RVA: 0x0040603E File Offset: 0x0040423E
			// (set) Token: 0x0600D4E7 RID: 54503 RVA: 0x00406046 File Offset: 0x00404246
			public float LastUseTime { get; private set; }

			// Token: 0x1700209C RID: 8348
			// (get) Token: 0x0600D4E8 RID: 54504 RVA: 0x0040604F File Offset: 0x0040424F
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}

			// Token: 0x0600D4E9 RID: 54505 RVA: 0x00406064 File Offset: 0x00404264
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x04009102 RID: 37122
			private const float CacheDuration = 1f;
		}

		// Token: 0x0200260A RID: 9738
		private struct CachedPortraitsWithParams
		{
			// Token: 0x1700209D RID: 8349
			// (get) Token: 0x0600D4EA RID: 54506 RVA: 0x00406082 File Offset: 0x00404282
			// (set) Token: 0x0600D4EB RID: 54507 RVA: 0x0040608A File Offset: 0x0040428A
			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits { get; private set; }

			// Token: 0x1700209E RID: 8350
			// (get) Token: 0x0600D4EC RID: 54508 RVA: 0x00406093 File Offset: 0x00404293
			// (set) Token: 0x0600D4ED RID: 54509 RVA: 0x0040609B File Offset: 0x0040429B
			public Dictionary<Apparel, Color> OverrideApparelColors { get; private set; }

			// Token: 0x1700209F RID: 8351
			// (get) Token: 0x0600D4EE RID: 54510 RVA: 0x004060A4 File Offset: 0x004042A4
			// (set) Token: 0x0600D4EF RID: 54511 RVA: 0x004060AC File Offset: 0x004042AC
			public Vector2 Size { get; private set; }

			// Token: 0x170020A0 RID: 8352
			// (get) Token: 0x0600D4F0 RID: 54512 RVA: 0x004060B5 File Offset: 0x004042B5
			// (set) Token: 0x0600D4F1 RID: 54513 RVA: 0x004060BD File Offset: 0x004042BD
			public Vector3 CameraOffset { get; private set; }

			// Token: 0x170020A1 RID: 8353
			// (get) Token: 0x0600D4F2 RID: 54514 RVA: 0x004060C6 File Offset: 0x004042C6
			// (set) Token: 0x0600D4F3 RID: 54515 RVA: 0x004060CE File Offset: 0x004042CE
			public float CameraZoom { get; private set; }

			// Token: 0x170020A2 RID: 8354
			// (get) Token: 0x0600D4F4 RID: 54516 RVA: 0x004060D7 File Offset: 0x004042D7
			// (set) Token: 0x0600D4F5 RID: 54517 RVA: 0x004060DF File Offset: 0x004042DF
			public Rot4 Rotation { get; private set; }

			// Token: 0x170020A3 RID: 8355
			// (get) Token: 0x0600D4F6 RID: 54518 RVA: 0x004060E8 File Offset: 0x004042E8
			// (set) Token: 0x0600D4F7 RID: 54519 RVA: 0x004060F0 File Offset: 0x004042F0
			public bool StylingStation { get; private set; }

			// Token: 0x0600D4F8 RID: 54520 RVA: 0x004060FC File Offset: 0x004042FC
			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom, Rot4 rotation, Dictionary<Apparel, Color> overrideApparelColors = null, bool stylingStation = false)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
				this.Rotation = rotation;
				this.OverrideApparelColors = overrideApparelColors;
				this.StylingStation = stylingStation;
			}
		}
	}
}
