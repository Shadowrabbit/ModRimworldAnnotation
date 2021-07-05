using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CA RID: 2506
	public class ThoughtWorker_ListeningToHarp : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003E32 RID: 15922 RVA: 0x00154A47 File Offset: 0x00152C47
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harp;
			}
		}
	}
}
