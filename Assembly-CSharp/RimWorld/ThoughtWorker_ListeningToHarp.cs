using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED7 RID: 3799
	public class ThoughtWorker_ListeningToHarp : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06005420 RID: 21536 RVA: 0x0003A6E2 File Offset: 0x000388E2
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harp;
			}
		}
	}
}
