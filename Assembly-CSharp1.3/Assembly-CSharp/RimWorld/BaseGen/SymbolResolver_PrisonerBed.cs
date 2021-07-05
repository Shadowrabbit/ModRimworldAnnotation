using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C5 RID: 5573
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x0600833F RID: 33599 RVA: 0x002EBE18 File Offset: 0x002EA018
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
