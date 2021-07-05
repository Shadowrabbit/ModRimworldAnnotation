using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001467 RID: 5223
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_CallShuttle : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x060070B9 RID: 28857 RVA: 0x00227ADC File Offset: 0x00225CDC
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, this.map), MessageTypeDefOf.RejectInput, false);
			}
			return acceptanceReport.Accepted;
		}

		// Token: 0x060070BA RID: 28858 RVA: 0x0004BF69 File Offset: 0x0004A169
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(target, this.map);
		}

		// Token: 0x060070BB RID: 28859 RVA: 0x0004BF9D File Offset: 0x0004A19D
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallShuttle(target.Cell);
		}

		// Token: 0x060070BC RID: 28860 RVA: 0x00227B74 File Offset: 0x00225D74
		public override void OnGUI(LocalTargetInfo target)
		{
			if (!target.IsValid || !RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map).Accepted)
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		// Token: 0x060070BD RID: 28861 RVA: 0x0004BFAC File Offset: 0x0004A1AC
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			string label = this.def.LabelCap + ": ";
			Action action = null;
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallShuttle(pawn, pawn.MapHeld, faction, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		// Token: 0x060070BE RID: 28862 RVA: 0x0004BFCA File Offset: 0x0004A1CA
		public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			string defaultDesc;
			bool flag;
			if (!base.FillCaravanAidOption(pawn, faction, out defaultDesc, out this.free, out flag))
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action
			{
				defaultLabel = this.def.LabelCap + " (" + pawn.LabelShort + ")",
				defaultDesc = defaultDesc,
				icon = RoyalTitlePermitWorker_CallShuttle.CommandTex,
				action = delegate()
				{
					this.CallShuttleToCaravan(pawn, faction, this.free);
				}
			};
			if (faction.HostileTo(Faction.OfPlayer))
			{
				command_Action.Disable("CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")));
			}
			if (flag)
			{
				command_Action.Disable("CommandCallRoyalAidNotEnoughFavor".Translate());
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x00227BAC File Offset: 0x00225DAC
		private void BeginCallShuttle(Pawn caller, Map map, Faction faction, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = false;
			this.targetingParameters.canTargetPawns = false;
			this.targetingParameters.canTargetFires = false;
			this.targetingParameters.canTargetBuildings = true;
			this.targetingParameters.canTargetItems = true;
			this.targetingParameters.validator = ((TargetInfo target) => this.def.royalAid.targetingRange <= 0f || target.Cell.DistanceTo(caller.Position) <= this.def.royalAid.targetingRange);
			this.caller = caller;
			this.map = map;
			this.calledFaction = faction;
			this.free = free;
			Find.Targeter.BeginTargeting(this, null);
		}

		// Token: 0x060070C0 RID: 28864 RVA: 0x00227C68 File Offset: 0x00225E68
		private void CallShuttle(IntVec3 landingCell)
		{
			if (this.caller.Spawned)
			{
				QuestScriptDef permit_CallShuttle = QuestScriptDefOf.Permit_CallShuttle;
				Slate slate = new Slate();
				slate.Set<Pawn>("asker", this.caller, false);
				slate.Set<Map>("map", this.caller.Map, false);
				slate.Set<IntVec3>("landingCell", landingCell, false);
				slate.Set<Faction>("permitFaction", this.calledFaction, false);
				QuestUtility.GenerateQuestAndMakeAvailable(permit_CallShuttle, slate);
				this.caller.royalty.GetPermit(this.def, this.calledFaction).Notify_Used();
				if (!this.free)
				{
					this.caller.royalty.TryRemoveFavor(this.calledFaction, this.def.royalAid.favorCost);
				}
			}
		}

		// Token: 0x060070C1 RID: 28865 RVA: 0x00227D30 File Offset: 0x00225F30
		private void CallShuttleToCaravan(Pawn caller, Faction faction, bool free)
		{
			RoyalTitlePermitWorker_CallShuttle.<>c__DisplayClass10_0 CS$<>8__locals1 = new RoyalTitlePermitWorker_CallShuttle.<>c__DisplayClass10_0();
			CS$<>8__locals1.caller = caller;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.free = free;
			CS$<>8__locals1.caravan = CS$<>8__locals1.caller.GetCaravan();
			CS$<>8__locals1.maxLaunchDistance = ThingDefOf.Shuttle.GetCompProperties<CompProperties_Launchable>().fixedLaunchDistanceMax;
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(CS$<>8__locals1.caravan));
			Find.WorldSelector.ClearSelection();
			CS$<>8__locals1.caravanTile = CS$<>8__locals1.caravan.Tile;
			Find.WorldTargeter.BeginTargeting_NewTemp(new Func<GlobalTargetInfo, bool>(CS$<>8__locals1.<CallShuttleToCaravan>g__ChoseWorldTarget|1), true, CompLaunchable.TargeterMouseAttachment, false, delegate
			{
				GenDraw.DrawWorldRadiusRing(CS$<>8__locals1.caravanTile, CS$<>8__locals1.maxLaunchDistance);
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, CS$<>8__locals1.caravanTile, CS$<>8__locals1.maxLaunchDistance, Gen.YieldSingle<Caravan>(CS$<>8__locals1.caravan), new Action<int, TransportPodsArrivalAction>(base.<CallShuttleToCaravan>g__Launch|0), null), null);
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x00227DEC File Offset: 0x00225FEC
		public static void DrawShuttleGhost(LocalTargetInfo target, Map map)
		{
			Color ghostCol = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, map).Accepted ? Designator_Place.CanPlaceColor : Designator_Place.CannotPlaceColor;
			GhostDrawer.DrawGhostThing_NewTmp(target.Cell, Rot4.North, ThingDefOf.Shuttle, ThingDefOf.Shuttle.graphic, ghostCol, AltitudeLayer.Blueprint, null, true);
			Vector3 position = (target.Cell + IntVec3.South * 2).ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x00227E74 File Offset: 0x00226074
		public static AcceptanceReport ShuttleCanLandHere(LocalTargetInfo target, Map map)
		{
			TaggedString t = "CannotCallShuttle".Translate() + ": ";
			if (!target.IsValid)
			{
				return new AcceptanceReport(t + "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst());
			}
			foreach (IntVec3 cell in GenAdj.OccupiedRect(target.Cell, Rot4.North, ThingDefOf.Shuttle.size))
			{
				string reportFromCell = RoyalTitlePermitWorker_CallShuttle.GetReportFromCell(cell, map);
				if (reportFromCell != null)
				{
					return new AcceptanceReport(t + reportFromCell);
				}
			}
			string reportFromCell2 = RoyalTitlePermitWorker_CallShuttle.GetReportFromCell(target.Cell + CompShuttle.DropoffSpotOffset, map);
			if (reportFromCell2 != null)
			{
				return new AcceptanceReport(t + reportFromCell2);
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x00227F70 File Offset: 0x00226170
		private static string GetReportFromCell(IntVec3 cell, Map map)
		{
			if (!cell.InBounds(map))
			{
				return "OutOfBounds".Translate().CapitalizeFirst();
			}
			if (cell.Fogged(map))
			{
				return "ShuttleCannotLand_Fogged".Translate().CapitalizeFirst();
			}
			if (!cell.Walkable(map))
			{
				return "ShuttleCannotLand_Unwalkable".Translate().CapitalizeFirst();
			}
			RoofDef roof = cell.GetRoof(map);
			if (roof != null && (roof.isNatural || roof.isThickRoof))
			{
				return "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst();
			}
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is IActiveDropPod || thing is Skyfaller || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
				{
					return "BlockedBy".Translate(thing).CapitalizeFirst();
				}
				PlantProperties plant = thing.def.plant;
				if (plant != null && plant.IsTree)
				{
					return "BlockedBy".Translate(thing).CapitalizeFirst();
				}
			}
			return null;
		}

		// Token: 0x04004A54 RID: 19028
		private Faction calledFaction;

		// Token: 0x04004A55 RID: 19029
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallShuttle", true);
	}
}
