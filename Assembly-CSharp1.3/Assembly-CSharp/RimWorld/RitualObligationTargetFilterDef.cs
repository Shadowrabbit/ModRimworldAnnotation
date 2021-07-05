using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F30 RID: 3888
	public class RitualObligationTargetFilterDef : Def
	{
		// Token: 0x06005C79 RID: 23673 RVA: 0x001FE10B File Offset: 0x001FC30B
		public RitualObligationTargetFilter GetInstance()
		{
			return (RitualObligationTargetFilter)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x040035CF RID: 13775
		public Type workerClass;

		// Token: 0x040035D0 RID: 13776
		public bool colonistThingsOnly = true;

		// Token: 0x040035D1 RID: 13777
		public List<ThingDef> thingDefs;

		// Token: 0x040035D2 RID: 13778
		public int minUnroofedCells;

		// Token: 0x040035D3 RID: 13779
		public int unroofedCellSearchRadius;

		// Token: 0x040035D4 RID: 13780
		public int woodPerParticipant;

		// Token: 0x040035D5 RID: 13781
		public int maxSpeakerDistance;

		// Token: 0x040035D6 RID: 13782
		public int maxDrumDistance;
	}
}
