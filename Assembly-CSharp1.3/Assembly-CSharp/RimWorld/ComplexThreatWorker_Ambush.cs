using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6C RID: 3180
	public class ComplexThreatWorker_Ambush : ComplexThreatWorker
	{
		// Token: 0x06004A42 RID: 19010 RVA: 0x00188D01 File Offset: 0x00186F01
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || (this.def.signalActionAmbushType == SignalActionAmbushType.Mechanoids && parms.hostileFaction == Faction.OfMechanoids));
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x00188D38 File Offset: 0x00186F38
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.signalTag = parms.triggerSignal;
			signalAction_Ambush.points = parms.points;
			signalAction_Ambush.ambushType = this.def.signalActionAmbushType;
			signalAction_Ambush.useDropPods = this.def.useDropPods;
			if (this.def.spawnAroundComplex)
			{
				signalAction_Ambush.spawnAround = parms.complexRect.ExpandedBy(5);
			}
			GenSpawn.Spawn(signalAction_Ambush, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed += parms.points;
		}

		// Token: 0x04002D21 RID: 11553
		private const int SpawnAroundDistance = 5;
	}
}
