using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F4 RID: 1012
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x00017592 File Offset: 0x00015792
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x000175A1 File Offset: 0x000157A1
		public int TendTicksFull
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksFull on permanent-tend Hediff.", 6163263, false);
				}
				return Mathf.RoundToInt((this.baseTendDurationHours + this.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x000175D3 File Offset: 0x000157D3
		public int TendTicksBase
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksBase on permanent-tend Hediff.", 61621263, false);
				}
				return Mathf.RoundToInt(this.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600189E RID: 6302 RVA: 0x000175FE File Offset: 0x000157FE
		public int TendTicksOverlap
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksOverlap on permanent-tend Hediff.", 1963263, false);
				}
				return Mathf.RoundToInt(this.tendOverlapHours * 2500f);
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00017629 File Offset: 0x00015829
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04001295 RID: 4757
		private float baseTendDurationHours = -1f;

		// Token: 0x04001296 RID: 4758
		private float tendOverlapHours = 3f;

		// Token: 0x04001297 RID: 4759
		public bool tendAllAtOnce;

		// Token: 0x04001298 RID: 4760
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04001299 RID: 4761
		public float severityPerDayTended;

		// Token: 0x0400129A RID: 4762
		public bool showTendQuality = true;

		// Token: 0x0400129B RID: 4763
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		// Token: 0x0400129C RID: 4764
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		// Token: 0x0400129D RID: 4765
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
