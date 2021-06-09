using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E65 RID: 7781
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x0600A7CC RID: 42956 RVA: 0x0030D590 File Offset: 0x0030B790
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			Action<Thing> prevPostThingSpawn = resolveParams.postThingSpawn;
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				if (prevPostThingSpawn != null)
				{
					prevPostThingSpawn(x);
				}
				Building_Bed building_Bed = x as Building_Bed;
				if (building_Bed != null)
				{
					building_Bed.ForPrisoners = true;
				}
			};
			BaseGen.symbolStack.Push("bed", resolveParams, null);
		}
	}
}
