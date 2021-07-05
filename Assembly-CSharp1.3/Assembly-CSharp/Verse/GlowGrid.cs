using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A9 RID: 425
	public sealed class GlowGrid
	{
		// Token: 0x06000BD9 RID: 3033 RVA: 0x000407B0 File Offset: 0x0003E9B0
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0004080C File Offset: 0x0003EA0C
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0004082C File Offset: 0x0003EA2C
		public float GameGlowAt(IntVec3 c, bool ignoreCavePlants = false)
		{
			float num = 0f;
			if (!this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num == 1f)
				{
					return num;
				}
			}
			Color32 color = (ignoreCavePlants ? this.glowGridNoCavePlants : this.glowGrid)[this.map.cellIndices.CellToIndex(c)];
			if (color.a == 1)
			{
				return 1f;
			}
			float b = (float)(color.r + color.g + color.b) / 3f / 255f * 3.6f;
			b = Mathf.Min(0.5f, b);
			return Mathf.Max(num, b);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000408DF File Offset: 0x0003EADF
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			return GlowGrid.PsychGlowAtGlow(this.GameGlowAt(c, false));
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x000408EE File Offset: 0x0003EAEE
		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			if (glow > 0.9f)
			{
				return PsychGlow.Overlit;
			}
			if (glow > 0.3f)
			{
				return PsychGlow.Lit;
			}
			return PsychGlow.Dark;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00040905 File Offset: 0x0003EB05
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00040943 File Offset: 0x0003EB43
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00040963 File Offset: 0x0003EB63
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0004097E File Offset: 0x0003EB7E
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00040998 File Offset: 0x0003EB98
		private void RecalculateAllGlow()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.initialGlowerLocs != null)
			{
				foreach (IntVec3 loc in this.initialGlowerLocs)
				{
					this.MarkGlowGridDirty(loc);
				}
				this.initialGlowerLocs = null;
			}
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.glowGrid[i] = new Color32(0, 0, 0, 0);
				this.glowGridNoCavePlants[i] = new Color32(0, 0, 0, 0);
			}
			foreach (CompGlower compGlower in this.litGlowers)
			{
				this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGrid);
				if (compGlower.parent.def.category != ThingCategory.Plant || !compGlower.parent.def.plant.cavePlant)
				{
					this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGridNoCavePlants);
				}
			}
		}

		// Token: 0x040009D8 RID: 2520
		private Map map;

		// Token: 0x040009D9 RID: 2521
		public Color32[] glowGrid;

		// Token: 0x040009DA RID: 2522
		public Color32[] glowGridNoCavePlants;

		// Token: 0x040009DB RID: 2523
		private bool glowGridDirty;

		// Token: 0x040009DC RID: 2524
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x040009DD RID: 2525
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x040009DE RID: 2526
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x040009DF RID: 2527
		public const int AlphaOfOverlit = 1;

		// Token: 0x040009E0 RID: 2528
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x040009E1 RID: 2529
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x040009E2 RID: 2530
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x040009E3 RID: 2531
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;
	}
}
