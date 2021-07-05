using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015CE RID: 5582
	public class SymbolResolver_TriggerUnfogged : SymbolResolver
	{
		// Token: 0x0600835C RID: 33628 RVA: 0x002ECDFE File Offset: 0x002EAFFE
		public override void Resolve(ResolveParams rp)
		{
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = rp.unfoggedSignalTag;
			GenSpawn.Spawn(triggerUnfogged, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
