using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6D RID: 3181
	public class ComplexThreatWorker_CryptosleepPods : ComplexThreatWorker
	{
		// Token: 0x06004A45 RID: 19013 RVA: 0x00188DE0 File Offset: 0x00186FE0
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			if (base.CanResolveInt(parms))
			{
				IntVec3 intVec;
				if (ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientCryptosleepPod, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out intVec, 1, null) && parms.points >= PawnKindDefOf.AncientSoldier.combatPower)
				{
					return parms.hostileFaction == null || parms.hostileFaction == Faction.OfAncientsHostile;
				}
			}
			return false;
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x00188E6C File Offset: 0x0018706C
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			List<Thing> list = this.SpawnCasketsWithHostiles(parms.room, parms.points, parms.triggerSignal, parms.map);
			SignalAction_OpenCasket signalAction_OpenCasket = (SignalAction_OpenCasket)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenCasket, null);
			signalAction_OpenCasket.signalTag = parms.triggerSignal;
			signalAction_OpenCasket.caskets.AddRange(list);
			signalAction_OpenCasket.completedSignalTag = "CompletedOpenAction" + Find.UniqueIDsManager.GetNextSignalTagID();
			if (parms.delayTicks != null)
			{
				signalAction_OpenCasket.delayTicks = parms.delayTicks.Value;
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = parms.triggerSignal;
				signalAction_Message.lookTargets = list;
				signalAction_Message.messageType = MessageTypeDefOf.ThreatBig;
				signalAction_Message.message = "MessageSleepingThreatDelayActivated".Translate(Faction.OfAncientsHostile);
				GenSpawn.Spawn(signalAction_Message, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(signalAction_OpenCasket, parms.map.Center, parms.map, WipeMode.Vanish);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Casket building_Casket;
				if ((building_Casket = (list[i] as Building_Casket)) != null)
				{
					using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)building_Casket.GetDirectlyHeldThings()).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn;
							if ((pawn = (enumerator.Current as Pawn)) != null)
							{
								threatPointsUsed += pawn.kindDef.combatPower;
							}
						}
					}
				}
			}
			SignalAction_Message signalAction_Message2 = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
			signalAction_Message2.signalTag = signalAction_OpenCasket.completedSignalTag;
			signalAction_Message2.lookTargets = list;
			signalAction_Message2.messageType = MessageTypeDefOf.ThreatBig;
			signalAction_Message2.message = "MessageSleepingPawnsWokenUp".Translate(Faction.OfAncientsHostile.def.pawnsPlural.CapitalizeFirst());
			GenSpawn.Spawn(signalAction_Message2, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x0018908C File Offset: 0x0018728C
		private List<Thing> SpawnCasketsWithHostiles(List<CellRect> room, float threatPoints, string openSignal, Map map)
		{
			int num = Mathf.FloorToInt(threatPoints / PawnKindDefOf.AncientSoldier.combatPower);
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc;
				if (!ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientCryptosleepPod, room.SelectMany((CellRect r) => r.Cells), map, out loc, 1, null))
				{
					break;
				}
				Building_AncientCryptosleepPod building_AncientCryptosleepPod = (Building_AncientCryptosleepPod)GenSpawn.Spawn(ThingDefOf.AncientCryptosleepPod, loc, map, WipeMode.Vanish);
				building_AncientCryptosleepPod.groupID = Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
				building_AncientCryptosleepPod.openedSignal = openSignal;
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
				List<Thing> list2 = ThingSetMakerDefOf.MapGen_AncientPodContents.root.Generate(parms);
				for (int j = 0; j < list2.Count; j++)
				{
					Pawn pawn = list2[j] as Pawn;
					if (!building_AncientCryptosleepPod.TryAcceptThing(list2[j], false))
					{
						if (pawn != null)
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
						}
						else
						{
							list2[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				list.Add(building_AncientCryptosleepPod);
			}
			return list;
		}

		// Token: 0x04002D22 RID: 11554
		private const string TriggerOpenAction = "TriggerOpenAction";

		// Token: 0x04002D23 RID: 11555
		private const string CompletedOpenAction = "CompletedOpenAction";

		// Token: 0x04002D24 RID: 11556
		private const float RoomEntryTriggerChance = 0.25f;
	}
}
