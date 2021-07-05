using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CB RID: 2507
	public class ThoughtWorker_ListeningToHarpsichord : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003E34 RID: 15924 RVA: 0x00154A56 File Offset: 0x00152C56
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harpsichord;
			}
		}
	}
}
