using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6F RID: 3183
	public class ComplexThreatWorker_Infestations : ComplexThreatWorker
	{
		// Token: 0x06004A4D RID: 19021 RVA: 0x0018943E File Offset: 0x0018763E
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfInsects);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x00189464 File Offset: 0x00187664
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			float num = Mathf.Max(200f, parms.points);
			int num2 = Mathf.CeilToInt(num / 500f);
			SignalAction_Infestation signalAction_Infestation = (SignalAction_Infestation)ThingMaker.MakeThing(ThingDefOf.SignalAction_Infestation, null);
			signalAction_Infestation.signalTag = parms.triggerSignal;
			signalAction_Infestation.hivesCount = num2;
			signalAction_Infestation.spawnAnywhereIfNoGoodCell = true;
			signalAction_Infestation.ignoreRoofedRequirement = true;
			signalAction_Infestation.sendStandardLetter = true;
			signalAction_Infestation.insectsPoints = new float?(num / (float)num2);
			Map map = parms.map;
			foreach (CellRect cellRect in parms.room.InRandomOrder(null))
			{
				foreach (IntVec3 intVec in cellRect.Cells.InRandomOrder(null))
				{
					if (intVec.GetThingList(map).Count == 0)
					{
						signalAction_Infestation.overrideLoc = new IntVec3?(intVec);
						break;
					}
				}
			}
			if (parms.delayTicks != null)
			{
				signalAction_Infestation.delayTicks = parms.delayTicks.Value;
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = signalAction_Infestation.completedSignalTag;
				signalAction_Message.lookTargets = new LookTargets(new GlobalTargetInfo[]
				{
					new GlobalTargetInfo(signalAction_Infestation.overrideLoc.Value, parms.map, false)
				});
				signalAction_Message.messageType = MessageTypeDefOf.ThreatBig;
				signalAction_Message.message = "MessageInfestationDelayActivated".Translate();
				GenSpawn.Spawn(signalAction_Message, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(signalAction_Infestation, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed += num;
		}
	}
}
