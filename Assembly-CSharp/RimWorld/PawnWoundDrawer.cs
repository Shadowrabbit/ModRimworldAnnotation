using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B5 RID: 5045
	[StaticConstructorOnStartup]
	public class PawnWoundDrawer
	{
		// Token: 0x06006D6F RID: 28015 RVA: 0x0004A5FB File Offset: 0x000487FB
		public PawnWoundDrawer(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006D70 RID: 28016 RVA: 0x00218B48 File Offset: 0x00216D48
		public void RenderOverBody(Vector3 drawLoc, Mesh bodyMesh, Quaternion quat, bool forPortrait)
		{
			int num = 0;
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.displayWound)
				{
					Hediff_Injury hediff_Injury = hediffs[i] as Hediff_Injury;
					if (hediff_Injury == null || !hediff_Injury.IsPermanent())
					{
						num++;
					}
				}
			}
			int num2 = Mathf.CeilToInt((float)num / 2f);
			if (num2 > this.MaxDisplayWounds)
			{
				num2 = this.MaxDisplayWounds;
			}
			while (this.wounds.Count < num2)
			{
				this.wounds.Add(new PawnWoundDrawer.Wound(this.pawn));
				PortraitsCache.SetDirty(this.pawn);
			}
			while (this.wounds.Count > num2)
			{
				this.wounds.Remove(this.wounds.RandomElement<PawnWoundDrawer.Wound>());
				PortraitsCache.SetDirty(this.pawn);
			}
			for (int j = 0; j < this.wounds.Count; j++)
			{
				this.wounds[j].DrawWound(drawLoc, quat, this.pawn.Rotation, forPortrait);
			}
		}

		// Token: 0x0400485B RID: 18523
		protected Pawn pawn;

		// Token: 0x0400485C RID: 18524
		private List<PawnWoundDrawer.Wound> wounds = new List<PawnWoundDrawer.Wound>();

		// Token: 0x0400485D RID: 18525
		private int MaxDisplayWounds = 3;

		// Token: 0x020013B6 RID: 5046
		private class Wound
		{
			// Token: 0x06006D71 RID: 28017 RVA: 0x00218C6C File Offset: 0x00216E6C
			public Wound(Pawn pawn)
			{
				this.mat = pawn.RaceProps.FleshType.ChooseWoundOverlay();
				if (this.mat == null)
				{
					Log.ErrorOnce(string.Format("No wound graphics data available for flesh type {0}", pawn.RaceProps.FleshType), 76591733, false);
					this.mat = FleshTypeDefOf.Normal.ChooseWoundOverlay();
				}
				this.quat = Quaternion.AngleAxis((float)Rand.Range(0, 360), Vector3.up);
				for (int i = 0; i < 4; i++)
				{
					this.locsPerSide.Add(new Vector2(Rand.Value, Rand.Value));
				}
			}

			// Token: 0x06006D72 RID: 28018 RVA: 0x00218D20 File Offset: 0x00216F20
			public void DrawWound(Vector3 drawLoc, Quaternion bodyQuat, Rot4 bodyRot, bool forPortrait)
			{
				Vector2 vector = this.locsPerSide[bodyRot.AsInt];
				drawLoc += new Vector3((vector.x - 0.5f) * PawnWoundDrawer.Wound.WoundSpan.x, 0f, (vector.y - 0.5f) * PawnWoundDrawer.Wound.WoundSpan.y);
				drawLoc.z -= 0.3f;
				GenDraw.DrawMeshNowOrLater(MeshPool.plane025, drawLoc, this.quat, this.mat, forPortrait);
			}

			// Token: 0x0400485E RID: 18526
			private List<Vector2> locsPerSide = new List<Vector2>();

			// Token: 0x0400485F RID: 18527
			private Material mat;

			// Token: 0x04004860 RID: 18528
			private Quaternion quat;

			// Token: 0x04004861 RID: 18529
			private static readonly Vector2 WoundSpan = new Vector2(0.18f, 0.3f);
		}
	}
}
