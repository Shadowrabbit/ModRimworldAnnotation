using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025F RID: 607
	public sealed class GlowGrid
	{
		// Token: 0x06000F4F RID: 3919 RVA: 0x000B6548 File Offset: 0x000B4748
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00011770 File Offset: 0x0000F970
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x000B65A4 File Offset: 0x000B47A4
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

		// Token: 0x06000F52 RID: 3922 RVA: 0x0001178E File Offset: 0x0000F98E
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			return GlowGrid.PsychGlowAtGlow(this.GameGlowAt(c, false));
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0001179D File Offset: 0x0000F99D
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

		// Token: 0x06000F54 RID: 3924 RVA: 0x000117B4 File Offset: 0x0000F9B4
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x000117F2 File Offset: 0x0000F9F2
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00011812 File Offset: 0x0000FA12
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0001182D File Offset: 0x0000FA2D
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x000B6658 File Offset: 0x000B4858
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

		// Token: 0x04000C94 RID: 3220
		private Map map;

		// Token: 0x04000C95 RID: 3221
		public Color32[] glowGrid;

		// Token: 0x04000C96 RID: 3222
		public Color32[] glowGridNoCavePlants;

		// Token: 0x04000C97 RID: 3223
		private bool glowGridDirty;

		// Token: 0x04000C98 RID: 3224
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04000C99 RID: 3225
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x04000C9A RID: 3226
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x04000C9B RID: 3227
		public const int AlphaOfOverlit = 1;

		// Token: 0x04000C9C RID: 3228
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04000C9D RID: 3229
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04000C9E RID: 3230
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04000C9F RID: 3231
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;
	}
}
