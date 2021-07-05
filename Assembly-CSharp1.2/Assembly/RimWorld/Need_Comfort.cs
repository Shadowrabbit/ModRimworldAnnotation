using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E5 RID: 5349
	public class Need_Comfort : Need_Seeker
	{
		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x06007348 RID: 29512 RVA: 0x0004D931 File Offset: 0x0004BB31
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastComfortUseTick >= Find.TickManager.TicksGame - 10)
				{
					return Mathf.Clamp01(this.lastComfortUsed);
				}
				return 0f;
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x06007349 RID: 29513 RVA: 0x002333D8 File Offset: 0x002315D8
		public ComfortCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.1f)
				{
					return ComfortCategory.Uncomfortable;
				}
				if (this.CurLevel < 0.6f)
				{
					return ComfortCategory.Normal;
				}
				if (this.CurLevel < 0.7f)
				{
					return ComfortCategory.Comfortable;
				}
				if (this.CurLevel < 0.8f)
				{
					return ComfortCategory.VeryComfortable;
				}
				if (this.CurLevel < 0.9f)
				{
					return ComfortCategory.ExtremelyComfortable;
				}
				return ComfortCategory.LuxuriantlyComfortable;
			}
		}

		// Token: 0x0600734A RID: 29514 RVA: 0x00233434 File Offset: 0x00231634
		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		// Token: 0x0600734B RID: 29515 RVA: 0x0004D959 File Offset: 0x0004BB59
		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04004C03 RID: 19459
		public float lastComfortUsed;

		// Token: 0x04004C04 RID: 19460
		public int lastComfortUseTick;

		// Token: 0x04004C05 RID: 19461
		private const float MinNormal = 0.1f;

		// Token: 0x04004C06 RID: 19462
		private const float MinComfortable = 0.6f;

		// Token: 0x04004C07 RID: 19463
		private const float MinVeryComfortable = 0.7f;

		// Token: 0x04004C08 RID: 19464
		private const float MinExtremelyComfortablee = 0.8f;

		// Token: 0x04004C09 RID: 19465
		private const float MinLuxuriantlyComfortable = 0.9f;

		// Token: 0x04004C0A RID: 19466
		public const int ComfortUseInterval = 10;
	}
}
