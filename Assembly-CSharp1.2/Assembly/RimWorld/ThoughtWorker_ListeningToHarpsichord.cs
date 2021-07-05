using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED8 RID: 3800
	public class ThoughtWorker_ListeningToHarpsichord : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06005422 RID: 21538 RVA: 0x0003A6F1 File Offset: 0x000388F1
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harpsichord;
			}
		}
	}
}
