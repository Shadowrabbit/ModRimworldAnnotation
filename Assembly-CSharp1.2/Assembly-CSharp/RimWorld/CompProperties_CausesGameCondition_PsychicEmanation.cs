using System;

namespace RimWorld
{
	// Token: 0x0200177F RID: 6015
	public class CompProperties_CausesGameCondition_PsychicEmanation : CompProperties_CausesGameCondition
	{
		// Token: 0x06008499 RID: 33945 RVA: 0x00058CD6 File Offset: 0x00056ED6
		public CompProperties_CausesGameCondition_PsychicEmanation()
		{
			this.compClass = typeof(CompCauseGameCondition_PsychicEmanation);
		}

		// Token: 0x040055E6 RID: 21990
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x040055E7 RID: 21991
		public int droneLevelIncreaseInterval = int.MinValue;
	}
}
