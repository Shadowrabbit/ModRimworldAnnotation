using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E40 RID: 3648
	public class Need_Beauty : Need_Seeker
	{
		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06005486 RID: 21638 RVA: 0x001CA604 File Offset: 0x001C8804
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

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06005487 RID: 21639 RVA: 0x001CA654 File Offset: 0x001C8854
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

		// Token: 0x06005488 RID: 21640 RVA: 0x001CA6BC File Offset: 0x001C88BC
		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x001CA71B File Offset: 0x001C891B
		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x001CA735 File Offset: 0x001C8935
		public float CurrentInstantBeauty()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return 0.5f;
			}
			return BeautyUtility.AverageBeautyPerceptible(this.pawn.PositionHeld, this.pawn.MapHeld);
		}

		// Token: 0x040031D3 RID: 12755
		private const float BeautyImpactFactor = 0.1f;

		// Token: 0x040031D4 RID: 12756
		private const float ThreshVeryUgly = 0.01f;

		// Token: 0x040031D5 RID: 12757
		private const float ThreshUgly = 0.15f;

		// Token: 0x040031D6 RID: 12758
		private const float ThreshNeutral = 0.35f;

		// Token: 0x040031D7 RID: 12759
		private const float ThreshPretty = 0.65f;

		// Token: 0x040031D8 RID: 12760
		private const float ThreshVeryPretty = 0.85f;

		// Token: 0x040031D9 RID: 12761
		private const float ThreshBeautiful = 0.99f;
	}
}
