using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E3 RID: 5603
	public class SymbolResolver_Infestation : SymbolResolver
	{
		// Token: 0x06008399 RID: 33689 RVA: 0x002F02FD File Offset: 0x002EE4FD
		public override bool CanResolve(ResolveParams rp)
		{
			return Faction.OfInsects != null && !rp.infestationSignalTag.NullOrEmpty();
		}

		// Token: 0x0600839A RID: 33690 RVA: 0x002F0318 File Offset: 0x002EE518
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Infestation signalAction_Infestation = (SignalAction_Infestation)ThingMaker.MakeThing(ThingDefOf.SignalAction_Infestation, null);
			signalAction_Infestation.signalTag = rp.infestationSignalTag;
			signalAction_Infestation.hivesCount = (rp.hivesCount ?? 1);
			signalAction_Infestation.insectsPoints = rp.insectsPoints;
			signalAction_Infestation.spawnAnywhereIfNoGoodCell = (rp.spawnAnywhereIfNoGoodCell ?? false);
			signalAction_Infestation.ignoreRoofedRequirement = (rp.ignoreRoofedRequirement ?? false);
			signalAction_Infestation.overrideLoc = rp.overrideLoc;
			signalAction_Infestation.sendStandardLetter = (rp.sendStandardLetter ?? true);
			GenSpawn.Spawn(signalAction_Infestation, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
