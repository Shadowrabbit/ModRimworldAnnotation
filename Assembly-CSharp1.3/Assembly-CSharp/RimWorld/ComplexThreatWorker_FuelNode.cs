using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class ComplexThreatWorker_FuelNode : ComplexThreatWorker
	{
		// Token: 0x06004A49 RID: 19017 RVA: 0x001891C0 File Offset: 0x001873C0
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			if (base.CanResolveInt(parms))
			{
				IntVec3 intVec;
				return ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientFuelNode, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out intVec, 1, null);
			}
			return false;
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x00189220 File Offset: 0x00187420
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientFuelNode, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out loc, 1, null);
			Thing thing = GenSpawn.Spawn(ThingDefOf.AncientFuelNode, loc, parms.map, WipeMode.Vanish);
			SignalAction_StartWick signalAction_StartWick = (SignalAction_StartWick)ThingMaker.MakeThing(ThingDefOf.SignalAction_StartWick, null);
			signalAction_StartWick.thingWithWick = thing;
			signalAction_StartWick.signalTag = parms.triggerSignal;
			signalAction_StartWick.completedSignalTag = "CompletedStartWickAction" + Find.UniqueIDsManager.GetNextSignalTagID();
			if (parms.delayTicks != null)
			{
				signalAction_StartWick.delayTicks = parms.delayTicks.Value;
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = parms.triggerSignal;
				signalAction_Message.lookTargets = thing;
				signalAction_Message.messageType = MessageTypeDefOf.ThreatBig;
				signalAction_Message.message = "MessageFuelNodeDelayActivated".Translate(ThingDefOf.AncientFuelNode.label);
				GenSpawn.Spawn(signalAction_Message, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(signalAction_StartWick, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			CompExplosive compExplosive = thing.TryGetComp<CompExplosive>();
			float randomInRange = ComplexThreatWorker_FuelNode.ExplosiveRadiusRandomRange.RandomInRange;
			compExplosive.customExplosiveRadius = new float?(randomInRange);
			SignalAction_Message signalAction_Message2 = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
			signalAction_Message2.message = "MessageFuelNodeTriggered".Translate(thing.LabelShort);
			signalAction_Message2.messageType = MessageTypeDefOf.NegativeEvent;
			signalAction_Message2.lookTargets = thing;
			signalAction_Message2.signalTag = signalAction_StartWick.completedSignalTag;
			GenSpawn.Spawn(signalAction_Message2, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed = randomInRange * 10f;
		}

		// Token: 0x04002D25 RID: 11557
		private const string TriggerStartWickAction = "TriggerStartWickAction";

		// Token: 0x04002D26 RID: 11558
		private const string CompletedStartWickAction = "CompletedStartWickAction";

		// Token: 0x04002D27 RID: 11559
		private static readonly FloatRange ExplosiveRadiusRandomRange = new FloatRange(2f, 12f);

		// Token: 0x04002D28 RID: 11560
		private const float ExplosiveRadiusThreatPointsFactor = 10f;

		// Token: 0x04002D29 RID: 11561
		private const float RoomEntryTriggerChance = 0.25f;
	}
}
