using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200116F RID: 4463
	[StaticConstructorOnStartup]
	public class CompPlantable : ThingComp
	{
		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06006B27 RID: 27431 RVA: 0x0023F741 File Offset: 0x0023D941
		private static TargetingParameters TargetingParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetPawns = false,
					canTargetLocations = true
				};
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06006B28 RID: 27432 RVA: 0x0023F756 File Offset: 0x0023D956
		public CompProperties_Plantable Props
		{
			get
			{
				return (CompProperties_Plantable)this.props;
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06006B29 RID: 27433 RVA: 0x0023F763 File Offset: 0x0023D963
		public IntVec3 PlantCell
		{
			get
			{
				return this.plantCell;
			}
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x0023F76B File Offset: 0x0023D96B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (ModsConfig.IdeologyActive)
			{
				if (this.PlantCell.IsValid)
				{
					yield return new Command_Action
					{
						defaultLabel = "CancelPlantThing".Translate(this.Props.plantDefToSpawn),
						defaultDesc = "CancelPlantThingDesc".Translate(this.Props.plantDefToSpawn),
						icon = CompPlantable.CancelCommandTex,
						hotKey = KeyBindingDefOf.Designator_Cancel,
						action = delegate()
						{
							this.plantCell = IntVec3.Invalid;
						}
					};
				}
				yield return new Command_Action
				{
					defaultLabel = "PlantThing".Translate(this.Props.plantDefToSpawn),
					defaultDesc = "PlantThingDesc".Translate(this.Props.plantDefToSpawn),
					icon = this.Props.plantDefToSpawn.uiIcon,
					action = delegate()
					{
						this.BeginTargeting();
					}
				};
			}
			yield break;
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x0023F77C File Offset: 0x0023D97C
		private void BeginTargeting()
		{
			Find.Targeter.BeginTargeting(CompPlantable.TargetingParams, delegate(LocalTargetInfo t)
			{
				if (!this.ValidateTarget(t))
				{
					this.BeginTargeting();
					return;
				}
				List<Thing> list;
				if (this.ConnectionStrengthReducedByNearbyBuilding(t.Cell, out list))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("TreeBlockedByNearbyBuildingsDesc".Translate(this.Props.plantDefToSpawn.Named("PLANT")), delegate
					{
						this.plantCell = t.Cell;
					}, false, null));
					return;
				}
				this.plantCell = t.Cell;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}, delegate(LocalTargetInfo t)
			{
				if (this.CanPlantAt(t.Cell, this.parent.Map).Accepted)
				{
					GenDraw.DrawTargetHighlight(t);
					this.DrawSurroundingsInfo(t.Cell);
				}
			}, (LocalTargetInfo t) => true, null, null, this.Props.plantDefToSpawn.uiIcon, false);
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x0023F7E4 File Offset: 0x0023D9E4
		private bool ConnectionStrengthReducedByNearbyBuilding(IntVec3 cell, out List<Thing> buildings)
		{
			CompProperties_TreeConnection compProperties = this.Props.plantDefToSpawn.GetCompProperties<CompProperties_TreeConnection>();
			if (compProperties != null)
			{
				buildings = this.parent.Map.listerArtificialBuildingsForMeditation.GetForCell(cell, compProperties.radiusToBuildingForConnectionStrengthLoss);
				if (buildings.Any<Thing>())
				{
					return true;
				}
			}
			buildings = null;
			return false;
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x0023F834 File Offset: 0x0023DA34
		private void DrawSurroundingsInfo(IntVec3 cell)
		{
			CompProperties_TreeConnection compProperties = this.Props.plantDefToSpawn.GetCompProperties<CompProperties_TreeConnection>();
			if (compProperties != null)
			{
				Color color = Color.white;
				List<Thing> list;
				if (this.ConnectionStrengthReducedByNearbyBuilding(cell, out list))
				{
					color = Color.red;
					foreach (Thing t in list)
					{
						GenDraw.DrawLineBetween(cell.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), t.TrueCenter(), SimpleColor.Red, 0.2f);
					}
				}
				color.a = 0.5f;
				GenDraw.DrawRadiusRing(cell, compProperties.radiusToBuildingForConnectionStrengthLoss, color, null);
			}
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x0023F8DC File Offset: 0x0023DADC
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.PlantCell.IsValid)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.PlantCell.ToVector3Shifted(), AltitudeLayer.Blueprint.AltitudeFor());
				GhostDrawer.DrawGhostThing(this.PlantCell, Rot4.North, this.Props.plantDefToSpawn, this.Props.plantDefToSpawn.graphic, Color.white, AltitudeLayer.Blueprint, null, true, null);
				this.DrawSurroundingsInfo(this.PlantCell);
			}
		}

		// Token: 0x06006B2F RID: 27439 RVA: 0x0023F960 File Offset: 0x0023DB60
		public void DoPlant(Pawn planter, IntVec3 cell, Map map)
		{
			Plant plant = (Plant)ThingMaker.MakeThing(this.Props.plantDefToSpawn, null);
			if (GenPlace.TryPlaceThing(plant, cell, map, ThingPlaceMode.Direct, null, null, default(Rot4)))
			{
				planter.records.Increment(RecordDefOf.PlantsSown);
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, planter.Named(HistoryEventArgsNames.Doer)), true);
				if (this.Props.plantDefToSpawn.plant.humanFoodPlant)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, planter.Named(HistoryEventArgsNames.Doer)), true);
				}
				EffecterDefOf.GauranlenLeavesBatch.Spawn(cell, map, 1f).Cleanup();
				plant.Growth = 0.0001f;
				plant.sown = true;
				map.mapDrawer.MapMeshDirty(cell, MapMeshFlag.Things);
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006B30 RID: 27440 RVA: 0x0023FA48 File Offset: 0x0023DC48
		private AcceptanceReport CanPlantAt(IntVec3 cell, Map map)
		{
			if (cell.Fogged(map))
			{
				return false;
			}
			Thing thing;
			AcceptanceReport acceptanceReport = this.Props.plantDefToSpawn.CanEverPlantAt(cell, map, out thing, true);
			if (!acceptanceReport.Accepted)
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + acceptanceReport.Reason.CapitalizeFirst();
			}
			Thing thing2 = PlantUtility.AdjacentSowBlocker(this.Props.plantDefToSpawn, cell, map);
			if (thing2 != null)
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + "AdjacentSowBlocker".Translate(thing2);
			}
			if (this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant > 0f)
			{
				foreach (Thing thing3 in map.listerThings.ThingsOfDef(this.Props.plantDefToSpawn))
				{
					if (thing3.Position.InHorDistOf(cell, this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant))
					{
						return "CannotPlantThing".Translate(this.parent) + ": " + "TooCloseToOtherPlant".Translate(thing3);
					}
				}
				foreach (Thing thing4 in map.listerThings.ThingsOfDef(this.parent.def))
				{
					CompPlantable compPlantable = thing4.TryGetComp<CompPlantable>();
					if (compPlantable != null && compPlantable.PlantCell.IsValid && compPlantable.PlantCell.InHorDistOf(cell, this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant))
					{
						return "CannotPlantThing".Translate(this.parent) + ": " + "TooCloseToOtherSeedPlantCell".Translate(thing4);
					}
				}
			}
			return true;
		}

		// Token: 0x06006B31 RID: 27441 RVA: 0x0023FCC4 File Offset: 0x0023DEC4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<IntVec3>(ref this.plantCell, "cell", IntVec3.Invalid, false);
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x0023FCE4 File Offset: 0x0023DEE4
		public bool ValidateTarget(LocalTargetInfo target)
		{
			AcceptanceReport acceptanceReport = this.CanPlantAt(target.Cell, this.parent.Map);
			if (!acceptanceReport.Accepted)
			{
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					Messages.Message(acceptanceReport.Reason, this.parent, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04003B9C RID: 15260
		private IntVec3 plantCell = IntVec3.Invalid;

		// Token: 0x04003B9D RID: 15261
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
