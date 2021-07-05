using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200033A RID: 826
	public class Building : ThingWithComps
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x0008B918 File Offset: 0x00089B18
		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x0008B920 File Offset: 0x00089B20
		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (set) Token: 0x06001765 RID: 5989 RVA: 0x0008B944 File Offset: 0x00089B44
		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
			}
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0008B966 File Offset: 0x00089B66
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", true, false);
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0008B980 File Offset: 0x00089B80
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def.IsEdifice())
			{
				map.edificeGrid.Register(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			base.SpawnSetup(map, respawningAfterLoad);
			base.Map.listerBuildings.Add(this);
			if (this.def.coversFloor)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Terrain, true, false);
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					base.Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlag.Buildings);
					base.Map.glowGrid.MarkGlowGridDirty(intVec);
					if (!SnowGrid.CanCoexistWithSnow(this.def))
					{
						base.Map.snowGrid.SetDepth(intVec, 0f);
					}
				}
			}
			if (base.Faction == Faction.OfPlayer && this.def.building != null && this.def.building.spawnedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(this.def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
			AutoHomeAreaMaker.Notify_BuildingSpawned(this);
			if (this.def.building != null && !this.def.building.soundAmbient.NullOrUndefined())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					SoundInfo info = SoundInfo.InMap(this, MaintenanceType.None);
					this.sustainerAmbient = this.def.building.soundAmbient.TrySpawnSustainer(info);
				});
			}
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
			base.Map.listerBuldingOfDefInProximity.Notify_BuildingSpawned(this);
			base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
			map.avoidGrid.Notify_BuildingSpawned(this);
			map.lordManager.Notify_BuildingSpawned(this);
			map.animalPenManager.Notify_BuildingSpawned(this);
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0008BBA8 File Offset: 0x00089DA8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.def.IsEdifice())
			{
				map.edificeGrid.DeRegister(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			if (mode != DestroyMode.WillReplace)
			{
				if (this.def.MakeFog)
				{
					map.fogGrid.Notify_FogBlockerRemoved(base.Position);
				}
				if (this.def.holdsRoof)
				{
					RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
				}
				if (this.def.IsSmoothable)
				{
					SmoothSurfaceDesignatorUtility.Notify_BuildingDespawned(this, map);
				}
			}
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.End();
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 loc = new IntVec3(j, 0, i);
					MapMeshFlag mapMeshFlag = MapMeshFlag.Buildings;
					if (this.def.coversFloor)
					{
						mapMeshFlag |= MapMeshFlag.Terrain;
					}
					if (this.def.Fillage == FillCategory.Full)
					{
						mapMeshFlag |= MapMeshFlag.Roofs;
						mapMeshFlag |= MapMeshFlag.Snow;
					}
					map.mapDrawer.MapMeshDirty(loc, mapMeshFlag);
					map.glowGrid.MarkGlowGridDirty(loc);
				}
			}
			map.listerBuildings.Remove(this);
			map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			map.listerArtificialBuildingsForMeditation.Notify_BuildingDeSpawned(this);
			map.listerBuldingOfDefInProximity.Notify_BuildingDeSpawned(this);
			map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
			if (this.def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && this.canChangeTerrainOnDestroyed)
			{
				foreach (IntVec3 c in this.OccupiedRect())
				{
					map.terrainGrid.SetTerrain(c, this.def.building.leaveTerrain);
				}
			}
			map.designationManager.Notify_BuildingDespawned(this);
			if (!this.CanBeSeenOver())
			{
				map.exitMapGrid.Notify_LOSBlockerDespawned();
			}
			if (this.def.building.hasFuelingPort)
			{
				CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation), map);
				if (compLaunchable != null)
				{
					compLaunchable.Notify_FuelingPortSourceDeSpawned();
				}
			}
			map.avoidGrid.Notify_BuildingDespawned(this);
			map.lordManager.Notify_BuildingDespawned(this);
			map.animalPenManager.Notify_BuildingDespawned(this);
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0008BE3C File Offset: 0x0008A03C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			SmoothableWallUtility.Notify_BuildingDestroying(this, mode);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_BuildingLost(this, null);
			}
			base.Destroy(mode);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
			if (spawned)
			{
				if (mode == DestroyMode.Deconstruct)
				{
					SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				else if (mode == DestroyMode.KillFinalize)
				{
					this.DoDestroyEffects(map);
				}
			}
			if (spawned)
			{
				ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def);
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0008BEC4 File Offset: 0x0008A0C4
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildings.Remove(this);
			}
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
				base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
				base.Map.listerBuildings.Add(this);
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.PowerGrid, true, false);
				if (newFaction == Faction.OfPlayer)
				{
					AutoHomeAreaMaker.Notify_BuildingClaimed(this);
				}
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0008BF8C File Offset: 0x0008A18C
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			if (base.Faction != null && base.Spawned && base.Faction != Faction.OfPlayer)
			{
				for (int i = 0; i < base.Map.lordManager.lords.Count; i++)
				{
					Lord lord = base.Map.lordManager.lords[i];
					if (lord.faction == base.Faction)
					{
						lord.Notify_BuildingDamaged(this, dinfo);
					}
				}
			}
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed && base.Faction != null)
			{
				base.Faction.Notify_BuildingTookDamage(this, dinfo);
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0008C02E File Offset: 0x0008A22E
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0008C054 File Offset: 0x0008A254
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0008C082 File Offset: 0x0008A282
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (((this.def.BuildableByPlayer && this.def.passability != Traversability.Impassable && !this.def.IsDoor) || this.def.building.forceShowRoomStats) && Gizmo_RoomStats.GetRoomToShowStatsFor(this) != null && Find.Selector.SingleSelectedObject == this)
			{
				yield return new Gizmo_RoomStats(this);
			}
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			if (this.def.Minifiable && base.Faction == Faction.OfPlayer)
			{
				yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			}
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def, base.Stuff);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0008C094 File Offset: 0x0008A294
		public virtual bool ClaimableBy(Faction by)
		{
			if (!this.def.Claimable)
			{
				return false;
			}
			if (base.Faction != null)
			{
				if (base.Faction == by)
				{
					return false;
				}
				if (by == Faction.OfPlayer)
				{
					if (base.Faction == Faction.OfInsects)
					{
						if (HiveUtility.AnyHivePreventsClaiming(this))
						{
							return false;
						}
					}
					else
					{
						if (base.Faction == Faction.OfMechanoids)
						{
							return false;
						}
						if (base.Spawned)
						{
							List<Pawn> list = base.Map.mapPawns.SpawnedPawnsInFaction(base.Faction);
							for (int i = 0; i < list.Count; i++)
							{
								if (list[i].RaceProps.ToolUser && GenHostility.IsActiveThreatToPlayer(list[i]))
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0008C148 File Offset: 0x0008A348
		public virtual bool DeconstructibleBy(Faction faction)
		{
			return DebugSettings.godMode || (this.def.building.IsDeconstructible && (base.Faction == faction || this.ClaimableBy(faction) || this.def.building.alwaysDeconstructible));
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0008C198 File Offset: 0x0008A398
		private void DoDestroyEffects(Map map)
		{
			if (this.def.building.destroyEffecter != null)
			{
				Effecter effecter = this.def.building.destroyEffecter.Spawn(base.Position, map, 1f);
				effecter.Trigger(new TargetInfo(base.Position, map, false), TargetInfo.Invalid);
				effecter.Cleanup();
				return;
			}
			if (!this.def.IsEdifice())
			{
				return;
			}
			SoundDef destroySound = this.GetDestroySound();
			if (destroySound != null)
			{
				destroySound.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			foreach (IntVec3 intVec in this.OccupiedRect())
			{
				int num = this.def.building.isNaturalRock ? 1 : Rand.RangeInclusive(3, 5);
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 2f), Color.white);
				}
			}
			if (Find.CurrentMap == map)
			{
				float num2 = this.def.building.destroyShakeAmount;
				if (num2 < 0f)
				{
					num2 = Building.ShakeAmountPerAreaCurve.Evaluate((float)this.def.Size.Area);
				}
				CompLifespan compLifespan = this.TryGetComp<CompLifespan>();
				if (compLifespan == null || compLifespan.age < compLifespan.Props.lifespanTicks)
				{
					Find.CameraDriver.shaker.DoShake(num2);
				}
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0008C330 File Offset: 0x0008A530
		private SoundDef GetDestroySound()
		{
			if (!this.def.building.destroySound.NullOrUndefined())
			{
				return this.def.building.destroySound;
			}
			StuffCategoryDef stuffCategoryDef;
			if (this.def.MadeFromStuff && base.Stuff != null && !base.Stuff.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
			{
				stuffCategoryDef = base.Stuff.stuffProps.categories[0];
			}
			else
			{
				if (this.def.CostList.NullOrEmpty<ThingDefCountClass>() || !this.def.CostList[0].thingDef.IsStuff || this.def.CostList[0].thingDef.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
				{
					return null;
				}
				stuffCategoryDef = this.def.CostList[0].thingDef.stuffProps.categories[0];
			}
			switch (this.def.building.buildingSizeCategory)
			{
			case BuildingSizeCategory.None:
			{
				int area = this.def.Size.Area;
				if (area <= 1 && !stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				if (area <= 4 && !stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			case BuildingSizeCategory.Small:
				if (!stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				break;
			case BuildingSizeCategory.Medium:
				if (!stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				break;
			case BuildingSizeCategory.Large:
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			return null;
		}

		// Token: 0x04001029 RID: 4137
		private Sustainer sustainerAmbient;

		// Token: 0x0400102A RID: 4138
		public bool canChangeTerrainOnDestroyed = true;

		// Token: 0x0400102B RID: 4139
		private static readonly SimpleCurve ShakeAmountPerAreaCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.07f),
				true
			},
			{
				new CurvePoint(2f, 0.07f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			},
			{
				new CurvePoint(9f, 0.2f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			}
		};
	}
}
