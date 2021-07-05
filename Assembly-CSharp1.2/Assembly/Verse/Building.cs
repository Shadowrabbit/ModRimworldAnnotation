using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004C4 RID: 1220
	public class Building : ThingWithComps
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0001ADE4 File Offset: 0x00018FE4
		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x000FB60C File Offset: 0x000F980C
		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (set) Token: 0x06001E3D RID: 7741 RVA: 0x000FB630 File Offset: 0x000F9830
		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0001ADEC File Offset: 0x00018FEC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", true, false);
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000FB654 File Offset: 0x000F9854
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
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
			map.avoidGrid.Notify_BuildingSpawned(this);
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000FB854 File Offset: 0x000F9A54
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
					foreach (IntVec3 c in this.OccupiedRect())
					{
						map.fogGrid.Notify_FogBlockerRemoved(c);
					}
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
			if (this.def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && this.canChangeTerrainOnDestroyed)
			{
				foreach (IntVec3 c2 in this.OccupiedRect())
				{
					map.terrainGrid.SetTerrain(c2, this.def.building.leaveTerrain);
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
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000FBB08 File Offset: 0x000F9D08
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

		// Token: 0x06001E42 RID: 7746 RVA: 0x0001AE06 File Offset: 0x00019006
		public override void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
				return;
			}
			base.Comps_PostDraw();
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000FBB90 File Offset: 0x000F9D90
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildings.Remove(this);
			}
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
				base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
				base.Map.listerBuildings.Add(this);
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.PowerGrid, true, false);
				if (newFaction == Faction.OfPlayer)
				{
					AutoHomeAreaMaker.Notify_BuildingClaimed(this);
				}
			}
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000FBC38 File Offset: 0x000F9E38
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

		// Token: 0x06001E45 RID: 7749 RVA: 0x0001AE23 File Offset: 0x00019023
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000FBCDC File Offset: 0x000F9EDC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0001AE46 File Offset: 0x00019046
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
				foreach (Command command2 in BuildFacilityCommandUtility.BuildFacilityCommands(this.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000FBD0C File Offset: 0x000F9F0C
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

		// Token: 0x06001E49 RID: 7753 RVA: 0x000FBDC0 File Offset: 0x000F9FC0
		public virtual bool DeconstructibleBy(Faction faction)
		{
			return DebugSettings.godMode || (this.def.building.IsDeconstructible && (base.Faction == faction || this.ClaimableBy(faction) || this.def.building.alwaysDeconstructible));
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000FBE10 File Offset: 0x000FA010
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
					MoteMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 2f), Color.white);
				}
			}
			if (Find.CurrentMap == map)
			{
				float num2 = this.def.building.destroyShakeAmount;
				if (num2 < 0f)
				{
					num2 = Building.ShakeAmountPerAreaCurve.Evaluate((float)this.def.Size.Area);
				}
				Find.CameraDriver.shaker.DoShake(num2);
			}
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000FBF88 File Offset: 0x000FA188
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
				if (this.def.costList.NullOrEmpty<ThingDefCountClass>() || !this.def.costList[0].thingDef.IsStuff || this.def.costList[0].thingDef.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
				{
					return null;
				}
				stuffCategoryDef = this.def.costList[0].thingDef.stuffProps.categories[0];
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

		// Token: 0x04001586 RID: 5510
		private Sustainer sustainerAmbient;

		// Token: 0x04001587 RID: 5511
		public bool canChangeTerrainOnDestroyed = true;

		// Token: 0x04001588 RID: 5512
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
