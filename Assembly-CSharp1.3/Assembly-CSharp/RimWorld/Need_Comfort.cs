using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E46 RID: 3654
	public class Need_Comfort : Need_Seeker
	{
		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x060054A8 RID: 21672 RVA: 0x001CAFEB File Offset: 0x001C91EB
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

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x060054A9 RID: 21673 RVA: 0x001CB014 File Offset: 0x001C9214
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

		// Token: 0x060054AA RID: 21674 RVA: 0x001CB070 File Offset: 0x001C9270
		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x001CB0DF File Offset: 0x001C92DF
		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040031F5 RID: 12789
		public float lastComfortUsed;

		// Token: 0x040031F6 RID: 12790
		public int lastComfortUseTick;

		// Token: 0x040031F7 RID: 12791
		private const float MinNormal = 0.1f;

		// Token: 0x040031F8 RID: 12792
		private const float MinComfortable = 0.6f;

		// Token: 0x040031F9 RID: 12793
		private const float MinVeryComfortable = 0.7f;

		// Token: 0x040031FA RID: 12794
		private const float MinExtremelyComfortablee = 0.8f;

		// Token: 0x040031FB RID: 12795
		private const float MinLuxuriantlyComfortable = 0.9f;

		// Token: 0x040031FC RID: 12796
		public const int ComfortUseInterval = 10;
	}
}
