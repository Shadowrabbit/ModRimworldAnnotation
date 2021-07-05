using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE6 RID: 3046
	public class GameCondition_TemperatureOffset : GameCondition
	{
		// Token: 0x060047BC RID: 18364 RVA: 0x0017B366 File Offset: 0x00179566
		public override void Init()
		{
			base.Init();
			this.tempOffset = this.def.temperatureOffset;
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0017B37F File Offset: 0x0017957F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0017B39D File Offset: 0x0017959D
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x04002C02 RID: 11266
		public float tempOffset;
	}
}
