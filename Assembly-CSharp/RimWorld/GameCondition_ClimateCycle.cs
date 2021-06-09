using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001184 RID: 4484
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x060062D9 RID: 25305 RVA: 0x00044008 File Offset: 0x00042208
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value < 0.5f) ? 0 : 7200000);
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x00044024 File Offset: 0x00042224
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x0004403E File Offset: 0x0004223E
		public override float TemperatureOffset()
		{
			return Mathf.Sin((GenDate.YearsPassedFloat + (float)this.ticksOffset / 3600000f) / 4f * 3.1415927f * 2f) * 20f;
		}

		// Token: 0x04004223 RID: 16931
		private int ticksOffset;

		// Token: 0x04004224 RID: 16932
		private const float PeriodYears = 4f;

		// Token: 0x04004225 RID: 16933
		private const float MaxTempOffset = 20f;
	}
}
