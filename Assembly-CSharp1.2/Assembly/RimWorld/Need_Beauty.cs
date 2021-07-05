using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DD RID: 5341
	public class Need_Beauty : Need_Seeker
	{
		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x06007326 RID: 29478 RVA: 0x00232BAC File Offset: 0x00230DAC
		public override float CurInstantLevel
		{
			get
			{
				if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
				{
					return 0.5f;
				}
				if (!this.pawn.Spawned)
				{
					return 0.5f;
				}
				return this.LevelFromBeauty(this.CurrentInstantBeauty());
			}
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x06007327 RID: 29479 RVA: 0x00232BFC File Offset: 0x00230DFC
		public BeautyCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.99f)
				{
					return BeautyCategory.Beautiful;
				}
				if (this.CurLevel > 0.85f)
				{
					return BeautyCategory.VeryPretty;
				}
				if (this.CurLevel > 0.65f)
				{
					return BeautyCategory.Pretty;
				}
				if (this.CurLevel > 0.35f)
				{
					return BeautyCategory.Neutral;
				}
				if (this.CurLevel > 0.15f)
				{
					return BeautyCategory.Ugly;
				}
				if (this.CurLevel > 0.01f)
				{
					return BeautyCategory.VeryUgly;
				}
				return BeautyCategory.Hideous;
			}
		}

		// Token: 0x06007328 RID: 29480 RVA: 0x00232C64 File Offset: 0x00230E64
		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x06007329 RID: 29481 RVA: 0x0004D750 File Offset: 0x0004B950
		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		// Token: 0x0600732A RID: 29482 RVA: 0x0004D76A File Offset: 0x0004B96A
		public float CurrentInstantBeauty()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return 0.5f;
			}
			return BeautyUtility.AverageBeautyPerceptible(this.pawn.PositionHeld, this.pawn.MapHeld);
		}

		// Token: 0x04004BD5 RID: 19413
		private const float BeautyImpactFactor = 0.1f;

		// Token: 0x04004BD6 RID: 19414
		private const float ThreshVeryUgly = 0.01f;

		// Token: 0x04004BD7 RID: 19415
		private const float ThreshUgly = 0.15f;

		// Token: 0x04004BD8 RID: 19416
		private const float ThreshNeutral = 0.35f;

		// Token: 0x04004BD9 RID: 19417
		private const float ThreshPretty = 0.65f;

		// Token: 0x04004BDA RID: 19418
		private const float ThreshVeryPretty = 0.85f;

		// Token: 0x04004BDB RID: 19419
		private const float ThreshBeautiful = 0.99f;
	}
}
