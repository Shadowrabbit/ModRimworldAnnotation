using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B89 RID: 2953
	public static class MoveColonyUtility
	{
		// Token: 0x06004510 RID: 17680 RVA: 0x0016E1A4 File Offset: 0x0016C3A4
		public static void PickNewColonyTile(Action<int> targetChosen, Action noTileChosen = null)
		{
			Find.TilePicker.StartTargeting(delegate(int tile)
			{
				MoveColonyUtility.cannotPlaceTileReason.Clear();
				if (!TileFinder.IsValidTileForNewSettlement(tile, MoveColonyUtility.cannotPlaceTileReason))
				{
					Messages.Message(MoveColonyUtility.cannotPlaceTileReason.ToString(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				return true;
			}, delegate(int tile)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				targetChosen(tile);
			}, false, noTileChosen);
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x0016E1F8 File Offset: 0x0016C3F8
		public static Settlement MoveColonyAndReset(int tile, IEnumerable<Thing> colonyThings, Faction takeoverFaction = null, WorldObjectDef worldObjectDef = null)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.root.endOnColonyMove && (quest.State == QuestState.NotYetAccepted || quest.State == QuestState.Ongoing))
				{
					quest.End(QuestEndOutcome.Unknown, false);
				}
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction.ToList<Pawn>())
			{
				if (pawn.Spawned)
				{
					pawn.DeSpawn(DestroyMode.Vanish);
				}
				if (pawn.IsCaravanMember())
				{
					pawn.GetCaravan().RemovePawn(pawn);
				}
				if (pawn.holdingOwner != null)
				{
					pawn.holdingOwner.Remove(pawn);
				}
				pawn.SetFaction(takeoverFaction, null);
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = caravans.Count - 1; i >= 0; i--)
			{
				if (caravans[i].IsPlayerControlled)
				{
					caravans[i].RemoveAllPawns();
					caravans[i].Destroy();
				}
			}
			List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
			for (int j = travelingTransportPods.Count - 1; j >= 0; j--)
			{
				travelingTransportPods[j].Destroy();
			}
			foreach (Thing thing3 in colonyThings)
			{
				if (thing3.Spawned)
				{
					thing3.DeSpawn(DestroyMode.Vanish);
				}
				if (thing3.holdingOwner != null)
				{
					thing3.holdingOwner.Remove(thing3);
				}
			}
			foreach (MapParent mapParent in Find.World.worldObjects.MapParents)
			{
				mapParent.CheckRemoveMapNow();
			}
			MoveColonyUtility.playerSettlementsRemoved.Clear();
			List<Map> maps = Find.Maps;
			for (int k = maps.Count - 1; k >= 0; k--)
			{
				Map map = maps[k];
				if (map.IsPlayerHome)
				{
					MoveColonyUtility.playerSettlementsRemoved.Add(map.Tile);
					map.Parent.SetFaction(null);
					Current.Game.DeinitAndRemoveMap(map);
					map.Parent.Destroy();
				}
			}
			WorldObjectDef worldObjectDef2 = worldObjectDef ?? WorldObjectDefOf.Settlement;
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(worldObjectDef2);
			settlement.SetFaction(Faction.OfPlayer);
			settlement.Tile = tile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Faction.OfPlayer.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, worldObjectDef2);
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			List<List<Thing>> list = new List<List<Thing>>();
			foreach (Thing thing2 in colonyThings)
			{
				if (thing2 is Pawn)
				{
					list.Add(new List<Thing>
					{
						thing2
					});
				}
			}
			int num = 0;
			using (IEnumerator<Thing> enumerator3 = colonyThings.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Thing thing = enumerator3.Current;
					if (thing.def.CanHaveFaction && thing.Faction != Faction.OfPlayer)
					{
						thing.SetFaction(Faction.OfPlayer, null);
					}
					if (!list.Any((List<Thing> g) => g.Contains(thing)))
					{
						list[num].Add(thing);
						num = (num + 1) % list.Count;
					}
				}
			}
			DropPodUtility.DropThingGroupsNear(playerStartSpot, orGenerateMap, list, 110, false, false, true, true, true, false);
			if (takeoverFaction != null)
			{
				foreach (int tile2 in MoveColonyUtility.playerSettlementsRemoved)
				{
					SettleUtility.AddNewHome(tile2, takeoverFaction);
				}
			}
			MoveColonyUtility.playerSettlementsRemoved.Clear();
			Find.ResearchManager.ResetAllProgress();
			ResearchUtility.ApplyPlayerStartingResearch();
			FactionUtility.ResetAllFactionRelations();
			IdeoUtility.Notify_NewColonyStarted();
			return settlement;
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x0016E6A4 File Offset: 0x0016C8A4
		public static IEnumerable<Thing> GetStartingThingsForNewColony()
		{
			using (IEnumerator<ScenPart> enumerator = Find.Scenario.AllParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ScenPart_StartingThing_Defined scenPart_StartingThing_Defined;
					if ((scenPart_StartingThing_Defined = (enumerator.Current as ScenPart_StartingThing_Defined)) != null)
					{
						foreach (Thing thing in scenPart_StartingThing_Defined.PlayerStartingThings())
						{
							yield return thing;
						}
						IEnumerator<Thing> enumerator2 = null;
					}
				}
			}
			IEnumerator<ScenPart> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040029F6 RID: 10742
		private static StringBuilder cannotPlaceTileReason = new StringBuilder();

		// Token: 0x040029F7 RID: 10743
		private static List<int> playerSettlementsRemoved = new List<int>();
	}
}
