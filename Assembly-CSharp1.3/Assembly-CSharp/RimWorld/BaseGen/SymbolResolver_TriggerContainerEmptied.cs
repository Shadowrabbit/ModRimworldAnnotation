using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015CC RID: 5580
	public class SymbolResolver_TriggerContainerEmptied : SymbolResolver
	{
		// Token: 0x06008357 RID: 33623 RVA: 0x002ECD2D File Offset: 0x002EAF2D
		public override bool CanResolve(ResolveParams rp)
		{
			return rp.triggerContainerEmptiedThing != null;
		}

		// Token: 0x06008358 RID: 33624 RVA: 0x002ECD38 File Offset: 0x002EAF38
		public override void Resolve(ResolveParams rp)
		{
			TriggerContainerEmptied triggerContainerEmptied = (TriggerContainerEmptied)ThingMaker.MakeThing(ThingDefOf.TriggerContainerEmptied, null);
			triggerContainerEmptied.signalTag = rp.triggerContainerEmptiedSignalTag;
			triggerContainerEmptied.container = rp.triggerContainerEmptiedThing;
			GenSpawn.Spawn(triggerContainerEmptied, rp.triggerContainerEmptiedThing.Position, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
