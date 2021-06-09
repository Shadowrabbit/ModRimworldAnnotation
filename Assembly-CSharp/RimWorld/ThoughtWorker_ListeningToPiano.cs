using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED9 RID: 3801
	public class ThoughtWorker_ListeningToPiano : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06005424 RID: 21540 RVA: 0x0003A6F8 File Offset: 0x000388F8
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Piano;
			}
		}
	}
}
