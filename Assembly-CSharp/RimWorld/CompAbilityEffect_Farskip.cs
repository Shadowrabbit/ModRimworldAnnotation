using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001371 RID: 4977
	public class CompAbilityEffect_Farskip : CompAbilityEffect
	{
		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06006C39 RID: 27705 RVA: 0x00049A25 File Offset: 0x00047C25
		public new CompProperties_AbilityFarskip Props
		{
			get
			{
				return (CompProperties_AbilityFarskip)this.props;
			}
		}

		// Token: 0x06006C3A RID: 27706 RVA: 0x00214258 File Offset: 0x00212458
		public override void Apply(GlobalTargetInfo target)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			MapParent mapParent = target.WorldObject as MapParent;
			Map targetMap = (mapParent != null) ? mapParent.Map : null;
			IntVec3 targetCell = IntVec3.Invalid;
			List<Pawn> list = this.PawnsToSkip().ToList<Pawn>();
			if (this.parent.pawn.Spawned)
			{
				foreach (Pawn pawn in list)
				{
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(pawn, pawn.Map, 1f), pawn.Position, 60);
				}
				SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (this.ShouldEnterMap(target))
			{
				Pawn pawn2 = this.AlliedPawnOnMap(targetMap);
				if (pawn2 != null)
				{
					targetCell = pawn2.Position;
				}
				else
				{
					targetCell = this.parent.pawn.Position;
				}
			}
			if (targetCell.IsValid)
			{
				Predicate<IntVec3> <>9__0;
				foreach (Pawn pawn3 in list)
				{
					if (pawn3.Spawned)
					{
						pawn3.teleporting = true;
						pawn3.ExitMap(false, Rot4.Invalid);
						AbilityUtility.DoClamor(pawn3.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
						pawn3.teleporting = false;
					}
					IntVec3 targetCell2 = targetCell;
					Map targetMap2 = targetMap;
					int firstTryWithRadius = 4;
					Predicate<IntVec3> extraValidator;
					if ((extraValidator = <>9__0) == null)
					{
						extraValidator = (<>9__0 = ((IntVec3 cell) => cell != targetCell && cell.GetRoom(targetMap, RegionType.Set_Passable) == targetCell.GetRoom(targetMap, RegionType.Set_Passable)));
					}
					IntVec3 intVec;
					CellFinder.TryFindRandomSpawnCellForPawnNear_NewTmp(targetCell2, targetMap2, out intVec, firstTryWithRadius, extraValidator);
					GenSpawn.Spawn(pawn3, intVec, targetMap, WipeMode.Vanish);
					if (pawn3.drafter != null && pawn3.IsColonistPlayerControlled)
					{
						pawn3.drafter.Drafted = true;
					}
					pawn3.stances.stunner.StunFor_NewTmp(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, true);
					pawn3.Notify_Teleported(true, true);
					if (pawn3.IsPrisoner)
					{
						pawn3.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(pawn3, pawn3.Map, 1f), pawn3.Position, 60);
					SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(intVec, pawn3.Map, false));
					if ((pawn3.IsColonist || pawn3.RaceProps.packAnimal) && pawn3.Map.IsPlayerHome)
					{
						pawn3.inventory.UnloadEverything = true;
					}
				}
				if (caravan != null)
				{
					caravan.Destroy();
					return;
				}
			}
			else
			{
				Caravan caravan2 = target.WorldObject as Caravan;
				if (caravan2 != null && caravan2.Faction == this.parent.pawn.Faction)
				{
					if (caravan != null)
					{
						caravan.pawns.TryTransferAllToContainer(caravan2.pawns, true);
						caravan2.Notify_Merged(new List<Caravan>
						{
							caravan
						});
						caravan.Destroy();
						return;
					}
					using (List<Pawn>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn4 = enumerator.Current;
							caravan2.AddPawn(pawn4, true);
							pawn4.ExitMap(false, Rot4.Invalid);
							AbilityUtility.DoClamor(pawn4.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
						}
						return;
					}
				}
				if (caravan != null)
				{
					caravan.Tile = target.Tile;
					caravan.pather.StopDead();
					return;
				}
				CaravanMaker.MakeCaravan(list, this.parent.pawn.Faction, target.Tile, false);
				foreach (Pawn pawn5 in list)
				{
					pawn5.ExitMap(false, Rot4.Invalid);
				}
			}
		}

		// Token: 0x06006C3B RID: 27707 RVA: 0x00049A32 File Offset: 0x00047C32
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					foreach (Pawn thing in this.PawnsToSkip())
					{
						MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_PsycastSkipFlashEntry, Vector3.zero, 1f, -1f).detachAfterTicks = 5;
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x00049A42 File Offset: 0x00047C42
		private IEnumerable<Pawn> PawnsToSkip()
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null)
			{
				foreach (Pawn pawn in caravan.pawns)
				{
					yield return pawn;
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			}
			else
			{
				bool homeMap = this.parent.pawn.Map.IsPlayerHome;
				using (IEnumerator<Thing> enumerator2 = GenRadial.RadialDistinctThingsAround(this.parent.pawn.Position, this.parent.pawn.Map, this.parent.def.EffectRadius, true).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn pawn2;
						if ((pawn2 = (enumerator2.Current as Pawn)) != null && !pawn2.Dead && (pawn2.IsColonist || pawn2.IsPrisonerOfColony || (!homeMap && pawn2.RaceProps.Animal && pawn2.Faction != null && pawn2.Faction.IsPlayer)))
						{
							yield return pawn2;
						}
					}
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x00049A52 File Offset: 0x00047C52
		private Pawn AlliedPawnOnMap(Map targetMap)
		{
			return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => !p.NonHumanlikeOrWildMan() && p.IsColonist && p.FactionOrExtraMiniOrHomeFaction == Faction.OfPlayer && !this.PawnsToSkip().Contains(p));
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x00214704 File Offset: 0x00212904
		private bool ShouldEnterMap(GlobalTargetInfo target)
		{
			Caravan caravan = target.WorldObject as Caravan;
			if (caravan != null && caravan.Faction == this.parent.pawn.Faction)
			{
				return false;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			return mapParent != null && mapParent.HasMap && (this.AlliedPawnOnMap(mapParent.Map) != null || mapParent.Map == this.parent.pawn.Map);
		}

		// Token: 0x06006C3F RID: 27711 RVA: 0x00214780 File Offset: 0x00212980
		private bool ShouldJoinCaravan(GlobalTargetInfo target)
		{
			Caravan caravan;
			return (caravan = (target.WorldObject as Caravan)) != null && caravan.Faction == this.parent.pawn.Faction;
		}

		// Token: 0x06006C40 RID: 27712 RVA: 0x002147B8 File Offset: 0x002129B8
		public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return false;
			}
			Caravan caravan2 = target.WorldObject as Caravan;
			return (caravan == null || caravan != caravan2) && (this.ShouldEnterMap(target) || this.ShouldJoinCaravan(target)) && base.Valid(target, throwMessages);
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x00214818 File Offset: 0x00212A18
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			MapParent mapParent = target.WorldObject as MapParent;
			return (mapParent == null || mapParent.Map == null || this.AlliedPawnOnMap(mapParent.Map) != null) && base.CanApplyOn(target);
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x00214854 File Offset: 0x00212A54
		public override string ConfirmationDialogText(GlobalTargetInfo target)
		{
			Pawn pawn = this.PawnsToSkip().FirstOrDefault((Pawn p) => p.IsQuestLodger());
			if (pawn != null)
			{
				return "FarskipConfirmTeleportingLodger".Translate(pawn.Named("PAWN"));
			}
			return base.ConfirmationDialogText(target);
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x002148B4 File Offset: 0x00212AB4
		public override string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return "CaravanImmobilizedByMass".Translate();
			}
			if (!this.Valid(target, false))
			{
				return "AbilityNeedAllyToSkip".Translate();
			}
			if (this.ShouldJoinCaravan(target))
			{
				return "AbilitySkipToJoinCaravan".Translate();
			}
			return "AbilitySkipToRandomAlly".Translate();
		}
	}
}
