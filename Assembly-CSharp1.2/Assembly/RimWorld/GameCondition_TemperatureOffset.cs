using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117F RID: 4479
	public class GameCondition_TemperatureOffset : GameCondition
	{
		// Token: 0x060062C5 RID: 25285 RVA: 0x00043F28 File Offset: 0x00042128
		public override void Init()
		{
			base.Init();
			this.tempOffset = this.def.temperatureOffset;
		}

		// Token: 0x060062C6 RID: 25286 RVA: 0x00043F41 File Offset: 0x00042141
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x00043F5F File Offset: 0x0004215F
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x0400421F RID: 16927
		public float tempOffset;
	}
}
