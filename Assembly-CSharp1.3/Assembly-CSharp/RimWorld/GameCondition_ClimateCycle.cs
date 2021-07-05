using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEB RID: 3051
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x060047D0 RID: 18384 RVA: 0x0017B6F7 File Offset: 0x001798F7
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value < 0.5f) ? 0 : 7200000);
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x0017B713 File Offset: 0x00179913
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x0017B72D File Offset: 0x0017992D
		public override float TemperatureOffset()
		{
			return Mathf.Sin((GenDate.YearsPassedFloat + (float)this.ticksOffset / 3600000f) / 4f * 3.1415927f * 2f) * 20f;
		}

		// Token: 0x04002C06 RID: 11270
		private int ticksOffset;

		// Token: 0x04002C07 RID: 11271
		private const float PeriodYears = 4f;

		// Token: 0x04002C08 RID: 11272
		private const float MaxTempOffset = 20f;
	}
}
