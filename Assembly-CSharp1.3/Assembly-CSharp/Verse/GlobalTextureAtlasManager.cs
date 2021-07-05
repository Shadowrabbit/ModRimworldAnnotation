using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002FA RID: 762
	public static class GlobalTextureAtlasManager
	{
		// Token: 0x06001619 RID: 5657 RVA: 0x00080940 File Offset: 0x0007EB40
		public static void ClearStaticAtlasBuildQueue()
		{
			GlobalTextureAtlasManager.buildQueue.Clear();
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0008094C File Offset: 0x0007EB4C
		public static bool TryInsertStatic(TextureAtlasGroup group, Texture2D texture, Texture2D mask = null)
		{
			if (texture.width >= 512 || texture.height >= 512)
			{
				return false;
			}
			List<Texture2D> list;
			if (!GlobalTextureAtlasManager.buildQueue.TryGetValue(group, out list))
			{
				list = new List<Texture2D>();
				GlobalTextureAtlasManager.buildQueue.Add(group, list);
			}
			if (!list.Contains(texture))
			{
				list.Add(texture);
			}
			if (mask != null)
			{
				if (GlobalTextureAtlasManager.buildQueueMasks.ContainsKey(texture))
				{
					if (GlobalTextureAtlasManager.buildQueueMasks[texture] != mask)
					{
						Log.Error(string.Concat(new string[]
						{
							"Same texture with 2 different masks inserted into texture atlas manager (",
							texture.name,
							") - ",
							mask.name,
							" | ",
							GlobalTextureAtlasManager.buildQueueMasks[texture].name,
							"!"
						}));
					}
				}
				else
				{
					GlobalTextureAtlasManager.buildQueueMasks.Add(texture, mask);
				}
			}
			return true;
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00080A38 File Offset: 0x0007EC38
		public static void BakeStaticAtlases()
		{
			BuildingsDamageSectionLayerUtility.TryInsertIntoAtlas();
			MinifiedThing.TryInsertIntoAtlas();
			GlobalTextureAtlasManager.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.pixels = 0;
			CS$<>8__locals1.currentBatch = new List<Texture2D>();
			foreach (KeyValuePair<TextureAtlasGroup, List<Texture2D>> keyValuePair in GlobalTextureAtlasManager.buildQueue)
			{
				foreach (Texture2D texture2D in keyValuePair.Value)
				{
					int num = texture2D.width * texture2D.height;
					if (num + CS$<>8__locals1.pixels > StaticTextureAtlas.MaxPixelsPerAtlas)
					{
						GlobalTextureAtlasManager.<BakeStaticAtlases>g__FlushBatch|7_0(keyValuePair.Key, ref CS$<>8__locals1);
					}
					CS$<>8__locals1.pixels += num;
					CS$<>8__locals1.currentBatch.Add(texture2D);
				}
				GlobalTextureAtlasManager.<BakeStaticAtlases>g__FlushBatch|7_0(keyValuePair.Key, ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00080B40 File Offset: 0x0007ED40
		public static bool TryGetStaticTile(TextureAtlasGroup group, Texture2D texture, out StaticTextureAtlasTile tile, bool ignoreFoundInOtherAtlas = false)
		{
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				if (staticTextureAtlas.group == group && staticTextureAtlas.TryGetTile(texture, out tile))
				{
					return true;
				}
			}
			foreach (StaticTextureAtlas staticTextureAtlas2 in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				if (staticTextureAtlas2.TryGetTile(texture, out tile))
				{
					if (!ignoreFoundInOtherAtlas)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Found texture ",
							texture.name,
							" in another atlas group than requested (found in ",
							staticTextureAtlas2.group,
							", requested in ",
							group,
							")!"
						}));
					}
					return true;
				}
			}
			tile = null;
			return false;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00080C48 File Offset: 0x0007EE48
		public static bool TryGetPawnFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew, bool allowCreatingNew = true)
		{
			using (List<PawnTextureAtlas>.Enumerator enumerator = GlobalTextureAtlasManager.pawnTextureAtlases.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryGetFrameSet(pawn, out frameSet, out createdNew))
					{
						return true;
					}
				}
			}
			if (allowCreatingNew)
			{
				PawnTextureAtlas pawnTextureAtlas = new PawnTextureAtlas();
				GlobalTextureAtlasManager.pawnTextureAtlases.Add(pawnTextureAtlas);
				return pawnTextureAtlas.TryGetFrameSet(pawn, out frameSet, out createdNew);
			}
			createdNew = false;
			frameSet = null;
			return false;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00080CC8 File Offset: 0x0007EEC8
		public static bool TryMarkPawnFrameSetDirty(Pawn pawn)
		{
			PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet;
			bool flag;
			if (!GlobalTextureAtlasManager.TryGetPawnFrameSet(pawn, out pawnTextureAtlasFrameSet, out flag, false))
			{
				return false;
			}
			for (int i = 0; i < pawnTextureAtlasFrameSet.isDirty.Length; i++)
			{
				pawnTextureAtlasFrameSet.isDirty[i] = true;
			}
			return true;
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00080D04 File Offset: 0x0007EF04
		public static void GlobalTextureAtlasManagerUpdate()
		{
			if (GlobalTextureAtlasManager.rebakeAtlas)
			{
				foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
				{
					staticTextureAtlas.Bake(true);
				}
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				PortraitsCache.Clear();
				GlobalTextureAtlasManager.rebakeAtlas = false;
			}
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				pawnTextureAtlas.GC();
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00080DAC File Offset: 0x0007EFAC
		public static void FreeAllRuntimeAtlases()
		{
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				pawnTextureAtlas.Destroy();
			}
			GlobalTextureAtlasManager.pawnTextureAtlases.Clear();
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00080E08 File Offset: 0x0007F008
		public static void DumpPawnAtlases(string folder)
		{
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				TextureAtlasHelper.WriteDebugPNG(pawnTextureAtlas.RawTexture, string.Concat(new object[]
				{
					folder,
					"\\dump_",
					pawnTextureAtlas.RawTexture.GetInstanceID(),
					".png"
				}));
			}
			Log.Message("Atlases have been dumped to " + folder);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00080EA0 File Offset: 0x0007F0A0
		public static void DumpStaticAtlases(string folder)
		{
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				TextureAtlasHelper.WriteDebugPNG(staticTextureAtlas.ColorTexture, folder + "\\" + staticTextureAtlas.ColorTexture.name + ".png");
				TextureAtlasHelper.WriteDebugPNG(staticTextureAtlas.MaskTexture, folder + "\\" + staticTextureAtlas.MaskTexture.name + ".png");
			}
			Log.Message("Atlases have been dumped to " + folder);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00080F48 File Offset: 0x0007F148
		[DebugAction(null, null)]
		public static void AtlasRebuild()
		{
			GlobalTextureAtlasManager.rebakeAtlas = true;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00080F80 File Offset: 0x0007F180
		[CompilerGenerated]
		internal static void <BakeStaticAtlases>g__FlushBatch|7_0(TextureAtlasGroup group, ref GlobalTextureAtlasManager.<>c__DisplayClass7_0 A_1)
		{
			StaticTextureAtlas staticTextureAtlas = new StaticTextureAtlas(group);
			foreach (Texture2D texture2D in A_1.currentBatch)
			{
				Texture2D mask;
				if (!GlobalTextureAtlasManager.buildQueueMasks.TryGetValue(texture2D, out mask))
				{
					mask = null;
				}
				staticTextureAtlas.Insert(texture2D, mask);
			}
			staticTextureAtlas.Bake(false);
			GlobalTextureAtlasManager.staticTextureAtlases.Add(staticTextureAtlas);
			A_1.pixels = 0;
			A_1.currentBatch.Clear();
		}

		// Token: 0x04000F55 RID: 3925
		public static bool rebakeAtlas = false;

		// Token: 0x04000F56 RID: 3926
		private static List<PawnTextureAtlas> pawnTextureAtlases = new List<PawnTextureAtlas>();

		// Token: 0x04000F57 RID: 3927
		private static List<StaticTextureAtlas> staticTextureAtlases = new List<StaticTextureAtlas>();

		// Token: 0x04000F58 RID: 3928
		private static Dictionary<TextureAtlasGroup, List<Texture2D>> buildQueue = new Dictionary<TextureAtlasGroup, List<Texture2D>>();

		// Token: 0x04000F59 RID: 3929
		private static Dictionary<Texture2D, Texture2D> buildQueueMasks = new Dictionary<Texture2D, Texture2D>();
	}
}
