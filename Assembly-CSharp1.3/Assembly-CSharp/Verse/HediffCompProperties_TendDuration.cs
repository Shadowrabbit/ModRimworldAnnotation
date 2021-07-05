using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B9 RID: 697
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x0006BB4A File Offset: 0x00069D4A
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0006BB59 File Offset: 0x00069D59
		public int TendTicksFull
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksFull on permanent-tend Hediff.", 6163263);
				}
				return Mathf.RoundToInt((this.baseTendDurationHours + this.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x0006BB8A File Offset: 0x00069D8A
		public int TendTicksBase
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksBase on permanent-tend Hediff.", 61621263);
				}
				return Mathf.RoundToInt(this.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0006BBB4 File Offset: 0x00069DB4
		public int TendTicksOverlap
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksOverlap on permanent-tend Hediff.", 1963263);
				}
				return Mathf.RoundToInt(this.tendOverlapHours * 2500f);
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0006BBDE File Offset: 0x00069DDE
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04000E2E RID: 3630
		private float baseTendDurationHours = -1f;

		// Token: 0x04000E2F RID: 3631
		private float tendOverlapHours = 3f;

		// Token: 0x04000E30 RID: 3632
		public bool tendAllAtOnce;

		// Token: 0x04000E31 RID: 3633
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04000E32 RID: 3634
		public float severityPerDayTended;

		// Token: 0x04000E33 RID: 3635
		public bool showTendQuality = true;

		// Token: 0x04000E34 RID: 3636
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		// Token: 0x04000E35 RID: 3637
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		// Token: 0x04000E36 RID: 3638
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
