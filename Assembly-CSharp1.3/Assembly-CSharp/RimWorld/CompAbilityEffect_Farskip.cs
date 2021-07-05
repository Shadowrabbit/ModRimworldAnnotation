using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D32 RID: 3378
	public class CompAbilityEffect_Farskip : CompAbilityEffect
	{
		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004F1B RID: 20251 RVA: 0x001A83C4 File Offset: 0x001A65C4
		public new CompProperties_AbilityFarskip Props
		{
			get
			{
				return (CompProperties_AbilityFarskip)this.props;
			}
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x001A83D4 File Offset: 0x001A65D4
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
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(pawn, pawn.Map, 1f), pawn.Position, 60, null);
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
						extraValidator = (<>9__0 = ((IntVec3 cell) => cell != targetCell && cell.GetRoom(targetMap) == targetCell.GetRoom(targetMap)));
					}
					IntVec3 intVec;
					CellFinder.TryFindRandomSpawnCellForPawnNear(targetCell2, targetMap2, out intVec, firstTryWithRadius, extraValidator);
					GenSpawn.Spawn(pawn3, intVec, targetMap, WipeMode.Vanish);
					if (pawn3.drafter != null && pawn3.IsColonistPlayerControlled)
					{
						pawn3.drafter.Drafted = true;
					}
					pawn3.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, true);
					pawn3.Notify_Teleported(true, true);
					if (pawn3.IsPrisoner)
					{
						pawn3.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(pawn3, pawn3.Map, 1f), pawn3.Position, 60, targetMap);
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

		// Token: 0x06004F1D RID: 20253 RVA: 0x001A8888 File Offset: 0x001A6A88
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					foreach (Pawn pawn in this.PawnsToSkip())
					{
						FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, Vector3.zero, 1f, -1f);
						dataAttachedOverlay.link.detachAfterTicks = 5;
						pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x001A8898 File Offset: 0x001A6A98
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

		// Token: 0x06004F1F RID: 20255 RVA: 0x001A88A8 File Offset: 0x001A6AA8
		private Pawn AlliedPawnOnMap(Map targetMap)
		{
			return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => !p.NonHumanlikeOrWildMan() && p.IsColonist && p.HomeFaction == Faction.OfPlayer && !this.PawnsToSkip().Contains(p));
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x001A88C8 File Offset: 0x001A6AC8
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

		// Token: 0x06004F21 RID: 20257 RVA: 0x001A8944 File Offset: 0x001A6B44
		private bool ShouldJoinCaravan(GlobalTargetInfo target)
		{
			Caravan caravan;
			return (caravan = (target.WorldObject as Caravan)) != null && caravan.Faction == this.parent.pawn.Faction;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x001A897C File Offset: 0x001A6B7C
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

		// Token: 0x06004F23 RID: 20259 RVA: 0x001A89DC File Offset: 0x001A6BDC
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			MapParent mapParent = target.WorldObject as MapParent;
			return (mapParent == null || mapParent.Map == null || this.AlliedPawnOnMap(mapParent.Map) != null) && base.CanApplyOn(target);
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x001A8A18 File Offset: 0x001A6C18
		public override Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = this.PawnsToSkip().FirstOrDefault((Pawn p) => p.IsQuestLodger());
			if (pawn != null)
			{
				return Dialog_MessageBox.CreateConfirmation("FarskipConfirmTeleportingLodger".Translate(pawn.Named("PAWN")), confirmAction, false, null);
			}
			return null;
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x001A8A74 File Offset: 0x001A6C74
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
