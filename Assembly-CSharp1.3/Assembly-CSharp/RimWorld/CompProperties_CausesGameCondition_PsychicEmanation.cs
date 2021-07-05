using System;

namespace RimWorld
{
	// Token: 0x020010F1 RID: 4337
	public class CompProperties_CausesGameCondition_PsychicEmanation : CompProperties_CausesGameCondition
	{
		// Token: 0x060067D0 RID: 26576 RVA: 0x002321B7 File Offset: 0x002303B7
		public CompProperties_CausesGameCondition_PsychicEmanation()
		{
			this.compClass = typeof(CompCauseGameCondition_PsychicEmanation);
		}

		// Token: 0x04003A76 RID: 14966
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04003A77 RID: 14967
		public int droneLevelIncreaseInterval = int.MinValue;
	}
}
