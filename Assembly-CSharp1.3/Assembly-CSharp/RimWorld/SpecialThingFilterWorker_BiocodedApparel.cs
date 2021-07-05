using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B7 RID: 5303
	public class SpecialThingFilterWorker_BiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x06007EBB RID: 32443 RVA: 0x002CE795 File Offset: 0x002CC995
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x002CE7AC File Offset: 0x002CC9AC
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.HasComp(typeof(CompBiocodable));
		}
	}
}
