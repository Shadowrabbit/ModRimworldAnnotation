using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CC RID: 2508
	public class ThoughtWorker_ListeningToPiano : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003E36 RID: 15926 RVA: 0x00154A5D File Offset: 0x00152C5D
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Piano;
			}
		}
	}
}
