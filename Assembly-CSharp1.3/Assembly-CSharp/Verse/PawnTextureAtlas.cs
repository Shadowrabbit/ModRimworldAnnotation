using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002FE RID: 766
	public class PawnTextureAtlas
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600162C RID: 5676 RVA: 0x0008123E File Offset: 0x0007F43E
		public RenderTexture RawTexture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x00081246 File Offset: 0x0007F446
		public int FreeCount
		{
			get
			{
				return this.freeFrameSets.Count;
			}
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00081254 File Offset: 0x0007F454
		public PawnTextureAtlas()
		{
			this.texture = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32, 0);
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < 2048; i += 128)
			{
				for (int j = 0; j < 2048; j += 128)
				{
					list.Add(new Rect((float)i / 2048f, (float)j / 2048f, 0.0625f, 0.0625f));
				}
			}
			while (list.Count >= 8)
			{
				PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = new PawnTextureAtlasFrameSet();
				pawnTextureAtlasFrameSet.uvRects = new Rect[]
				{
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>()
				};
				pawnTextureAtlasFrameSet.meshes = (from u in pawnTextureAtlasFrameSet.uvRects
				select TextureAtlasHelper.CreateMeshForUV(u, 1f)).ToArray<Mesh>();
				pawnTextureAtlasFrameSet.atlas = this.texture;
				this.freeFrameSets.Add(pawnTextureAtlasFrameSet);
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000813C1 File Offset: 0x0007F5C1
		public bool ContainsFrameSet(Pawn pawn)
		{
			return this.frameAssignments.ContainsKey(pawn);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000813D0 File Offset: 0x0007F5D0
		public bool TryGetFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew)
		{
			createdNew = false;
			if (this.frameAssignments.TryGetValue(pawn, out frameSet))
			{
				return true;
			}
			if (this.FreeCount == 0)
			{
				return false;
			}
			createdNew = true;
			frameSet = this.freeFrameSets.Pop<PawnTextureAtlasFrameSet>();
			for (int i = 0; i < frameSet.isDirty.Length; i++)
			{
				frameSet.isDirty[i] = true;
			}
			this.frameAssignments.Add(pawn, frameSet);
			return true;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00081438 File Offset: 0x0007F638
		public void GC()
		{
			try
			{
				foreach (Pawn pawn in this.frameAssignments.Keys)
				{
					if (!pawn.SpawnedOrAnyParentSpawned)
					{
						PawnTextureAtlas.tmpPawnsToFree.Add(pawn);
					}
				}
				foreach (Pawn key in PawnTextureAtlas.tmpPawnsToFree)
				{
					this.freeFrameSets.Add(this.frameAssignments[key]);
					this.frameAssignments.Remove(key);
				}
			}
			finally
			{
				PawnTextureAtlas.tmpPawnsToFree.Clear();
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00081514 File Offset: 0x0007F714
		public void Destroy()
		{
			foreach (PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet in this.frameAssignments.Values.Concat(this.freeFrameSets))
			{
				Mesh[] meshes = pawnTextureAtlasFrameSet.meshes;
				for (int i = 0; i < meshes.Length; i++)
				{
					UnityEngine.Object.Destroy(meshes[i]);
				}
			}
			this.frameAssignments.Clear();
			this.freeFrameSets.Clear();
			UnityEngine.Object.Destroy(this.texture);
		}

		// Token: 0x04000F61 RID: 3937
		private RenderTexture texture;

		// Token: 0x04000F62 RID: 3938
		private Dictionary<Pawn, PawnTextureAtlasFrameSet> frameAssignments = new Dictionary<Pawn, PawnTextureAtlasFrameSet>();

		// Token: 0x04000F63 RID: 3939
		private List<PawnTextureAtlasFrameSet> freeFrameSets = new List<PawnTextureAtlasFrameSet>();

		// Token: 0x04000F64 RID: 3940
		private static List<Pawn> tmpPawnsToFree = new List<Pawn>();

		// Token: 0x04000F65 RID: 3941
		private const int AtlasSize = 2048;

		// Token: 0x04000F66 RID: 3942
		public const int FrameSize = 128;

		// Token: 0x04000F67 RID: 3943
		private const int PawnsHeldPerAtlas = 32;

		// Token: 0x04000F68 RID: 3944
		private const int FramesPerPawn = 8;
	}
}
