using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001504 RID: 5380
	public class TargetingParameters
	{
		// Token: 0x06008029 RID: 32809 RVA: 0x002D6300 File Offset: 0x002D4500
		public bool CanTarget(TargetInfo targ, ITargetingSource source = null)
		{
			if (this.validator != null && !this.validator(targ))
			{
				return false;
			}
			if (targ.Thing == null)
			{
				return this.canTargetLocations;
			}
			if (this.neverTargetDoors && targ.Thing.def.IsDoor)
			{
				return false;
			}
			if (this.onlyTargetDamagedThings && targ.Thing.HitPoints == targ.Thing.MaxHitPoints)
			{
				return false;
			}
			if (this.onlyTargetFlammables && !targ.Thing.FlammableNow)
			{
				return false;
			}
			if (this.mustBeSelectable && !ThingSelectionUtility.SelectableByMapClick(targ.Thing))
			{
				return false;
			}
			if (this.onlyTargetColonistsOrPrisoners && targ.Thing.def.category != ThingCategory.Pawn)
			{
				return false;
			}
			if (this.onlyTargetColonistsOrPrisonersOrSlaves && targ.Thing.def.category != ThingCategory.Pawn)
			{
				return false;
			}
			if (this.targetSpecificThing != null && targ.Thing == this.targetSpecificThing)
			{
				return true;
			}
			if (this.canTargetFires && targ.Thing.def == ThingDefOf.Fire)
			{
				return true;
			}
			if (this.canTargetPawns && targ.Thing.def.category == ThingCategory.Pawn)
			{
				Pawn pawn = (Pawn)targ.Thing;
				if (pawn.Downed)
				{
					if (this.neverTargetIncapacitated)
					{
						return false;
					}
				}
				else if (this.onlyTargetIncapacitatedPawns)
				{
					return false;
				}
				if (this.onlyTargetFactions != null && !this.onlyTargetFactions.Contains(targ.Thing.Faction))
				{
					return false;
				}
				if (pawn.NonHumanlikeOrWildMan())
				{
					if (pawn.Faction != null && pawn.Faction == Faction.OfMechanoids)
					{
						if (!this.canTargetMechs)
						{
							return false;
						}
					}
					else if (!this.canTargetAnimals)
					{
						return false;
					}
				}
				if (!pawn.NonHumanlikeOrWildMan() && !this.canTargetHumans)
				{
					return false;
				}
				if (this.onlyTargetControlledPawns && !pawn.IsColonistPlayerControlled)
				{
					return false;
				}
				if (this.onlyTargetColonists && (!pawn.IsColonist || pawn.HostFaction != null))
				{
					return false;
				}
				if (this.onlyTargetPrisonersOfColony && !pawn.IsPrisonerOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisoners && !pawn.IsColonistPlayerControlled && !pawn.IsPrisonerOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisonersOrSlaves && !pawn.IsColonistPlayerControlled && !pawn.IsPrisonerOfColony && !pawn.IsSlaveOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisonersOrSlavesAllowMinorMentalBreaks)
				{
					if (!pawn.IsPrisonerOfColony && !pawn.IsSlaveOfColony && (!pawn.IsColonist || (pawn.HostFaction != null && !pawn.IsSlave)))
					{
						return false;
					}
					MentalStateDef mentalStateDef = pawn.MentalStateDef;
					if (mentalStateDef != null && mentalStateDef.IsAggro)
					{
						return false;
					}
				}
				if (this.onlyTargetPsychicSensitive && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) <= 0f)
				{
					return false;
				}
				if (this.neverTargetHostileFaction && !pawn.IsPrisonerOfColony && !pawn.IsSlaveOfColony)
				{
					Faction homeFaction = pawn.HomeFaction;
					if (homeFaction != null && homeFaction.HostileTo(Faction.OfPlayer))
					{
						return false;
					}
				}
				if (this.onlyTargetSameIdeo)
				{
					Verb verb;
					if (source == null)
					{
						Log.Error("Source passed in is null but targeting parameters have onlyTargetSameIdeo set.");
					}
					else if ((verb = (source as Verb)) != null && verb.CasterPawn != null)
					{
						Pawn pawn2;
						Ideo ideo = ((pawn2 = (targ.Thing as Pawn)) != null) ? pawn2.Ideo : null;
						if (verb.CasterPawn.Ideo != ideo)
						{
							return false;
						}
					}
					else
					{
						Log.Error("Source passed in is incompatible type but targeting parameters have onlyTargetSameIdeo set.");
					}
				}
				return true;
			}
			else
			{
				if (this.canTargetBuildings && targ.Thing.def.category == ThingCategory.Building)
				{
					return (!this.onlyTargetThingsAffectingRegions || targ.Thing.def.AffectsRegions) && (this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction));
				}
				if (this.canTargetPlants && targ.Thing.def.category == ThingCategory.Plant)
				{
					return !ModsConfig.RoyaltyActive || !this.onlyTargetAnimaTrees || targ.Thing.def == ThingDefOf.Plant_TreeAnima;
				}
				if (this.canTargetItems)
				{
					if (this.mapObjectTargetsMustBeAutoAttackable && !targ.Thing.def.isAutoAttackableMapObject)
					{
						return false;
					}
					if (this.thingCategory == ThingCategory.None || this.thingCategory == targ.Thing.def.category)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x002D6723 File Offset: 0x002D4923
		public static TargetingParameters ForSelf(Pawn p)
		{
			return new TargetingParameters
			{
				targetSpecificThing = p,
				canTargetPawns = false,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x0600802B RID: 32811 RVA: 0x002D6748 File Offset: 0x002D4948
		public static TargetingParameters ForArrest(Pawn arrester)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					if (!targ.HasThing)
					{
						return false;
					}
					Pawn pawn = targ.Thing as Pawn;
					return pawn != null && pawn != arrester && pawn.CanBeArrestedBy(arrester) && (!pawn.Downed || !pawn.guilt.IsGuilty);
				}
			};
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x002D6790 File Offset: 0x002D4990
		public static TargetingParameters ForAttackHostile()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = true;
			targetingParameters.validator = delegate(TargetInfo targ)
			{
				if (!targ.HasThing)
				{
					return false;
				}
				if (targ.Thing.HostileTo(Faction.OfPlayer))
				{
					return true;
				}
				Pawn pawn = targ.Thing as Pawn;
				return pawn != null && pawn.NonHumanlikeOrWildMan();
			};
			return targetingParameters;
		}

		// Token: 0x0600802D RID: 32813 RVA: 0x002D67E3 File Offset: 0x002D49E3
		public static TargetingParameters ForAttackAny()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = true
			};
		}

		// Token: 0x0600802E RID: 32814 RVA: 0x002D6806 File Offset: 0x002D4A06
		public static TargetingParameters ForRescue(Pawn p)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				onlyTargetIncapacitatedPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x0600802F RID: 32815 RVA: 0x002D682C File Offset: 0x002D4A2C
		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = ((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x002D6878 File Offset: 0x002D4A78
		public static TargetingParameters ForCarry(Pawn p)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				onlyTargetIncapacitatedPawns = true
			};
		}

		// Token: 0x06008031 RID: 32817 RVA: 0x002D6894 File Offset: 0x002D4A94
		public static TargetingParameters ForDraftedCarryBed(Pawn sleeper, Pawn carrier, GuestStatus? guestStatus = null)
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetItems = false,
				canTargetBuildings = true,
				validator = ((TargetInfo target) => target.HasThing && RestUtility.IsValidBedFor(target.Thing, sleeper, carrier, false, true, true, guestStatus))
			};
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x002D68E8 File Offset: 0x002D4AE8
		public static TargetingParameters ForDraftedCarryTransporter(Pawn carriedPawn)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetItems = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.validator = ((TargetInfo target) => target.HasThing && target.Thing.TryGetComp<CompTransporter>() != null);
			return targetingParameters;
		}

		// Token: 0x06008033 RID: 32819 RVA: 0x002D6934 File Offset: 0x002D4B34
		public static TargetingParameters ForDraftedCarryCryptosleepCasket(Pawn carrier)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetItems = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.validator = ((TargetInfo target) => target.HasThing && target.Thing.def.IsCryptosleepCasket);
			return targetingParameters;
		}

		// Token: 0x06008034 RID: 32820 RVA: 0x002D6980 File Offset: 0x002D4B80
		public static TargetingParameters ForTrade()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				ITrader trader = x.Thing as ITrader;
				return trader != null && trader.CanTradeNow;
			};
			return targetingParameters;
		}

		// Token: 0x06008035 RID: 32821 RVA: 0x002D69CC File Offset: 0x002D4BCC
		public static TargetingParameters ForDropPodsDestination()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = ((TargetInfo x) => DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true, true));
			return targetingParameters;
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x002D6A30 File Offset: 0x002D4C30
		public static TargetingParameters ForQuestPawnsWhoWillJoinColony(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				Pawn pawn = x.Thing as Pawn;
				return pawn != null && !pawn.Dead && pawn.mindState.WillJoinColonyIfRescued;
			};
			return targetingParameters;
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x002D6A7C File Offset: 0x002D4C7C
		public static TargetingParameters ForOpen(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				IOpenable openable = x.Thing as IOpenable;
				return openable != null && openable.CanOpen;
			};
			return targetingParameters;
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x002D6AC8 File Offset: 0x002D4CC8
		public static TargetingParameters ForShuttle(Pawn hauler)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					if (!targ.HasThing)
					{
						return false;
					}
					Pawn pawn = targ.Thing as Pawn;
					if (pawn == null || pawn.Dead || pawn == hauler)
					{
						return false;
					}
					if (pawn.Downed)
					{
						return true;
					}
					if (pawn.IsPrisonerOfColony)
					{
						return pawn.guest.PrisonerIsSecure;
					}
					return pawn.AnimalOrWildMan();
				}
			};
		}

		// Token: 0x06008039 RID: 32825 RVA: 0x002D6B10 File Offset: 0x002D4D10
		public static TargetingParameters ForBuilding(ThingDef def = null)
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetItems = false,
				canTargetBuildings = true,
				validator = ((TargetInfo targ) => targ.HasThing && (def == null || targ.Thing.def == def))
			};
		}

		// Token: 0x0600803A RID: 32826 RVA: 0x002D6B58 File Offset: 0x002D4D58
		public static TargetingParameters ForTend(Pawn doctor)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetMechs = false;
			targetingParameters.validator = delegate(TargetInfo targ)
			{
				if (!targ.HasThing)
				{
					return false;
				}
				Pawn pawn = targ.Thing as Pawn;
				return pawn != null && !pawn.NonHumanlikeOrWildMan() && (pawn.IsColonist || pawn.IsQuestLodger() || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony);
			};
			return targetingParameters;
		}

		// Token: 0x0600803B RID: 32827 RVA: 0x002D6BA4 File Offset: 0x002D4DA4
		public static TargetingParameters ForRepair(Pawn repairer)
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				validator = ((TargetInfo targ) => targ.HasThing && RepairUtility.PawnCanRepairEver(repairer, targ.Thing))
			};
		}

		// Token: 0x0600803C RID: 32828 RVA: 0x002D6BDC File Offset: 0x002D4DDC
		public static TargetingParameters ForCarryToBiosculpterPod(Pawn p)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				onlyTargetColonistsOrPrisoners = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x04004FBE RID: 20414
		public bool canTargetLocations;

		// Token: 0x04004FBF RID: 20415
		public bool canTargetSelf;

		// Token: 0x04004FC0 RID: 20416
		public bool canTargetPawns = true;

		// Token: 0x04004FC1 RID: 20417
		public bool canTargetFires;

		// Token: 0x04004FC2 RID: 20418
		public bool canTargetBuildings = true;

		// Token: 0x04004FC3 RID: 20419
		public bool canTargetItems;

		// Token: 0x04004FC4 RID: 20420
		public bool canTargetAnimals = true;

		// Token: 0x04004FC5 RID: 20421
		public bool canTargetHumans = true;

		// Token: 0x04004FC6 RID: 20422
		public bool canTargetMechs = true;

		// Token: 0x04004FC7 RID: 20423
		public bool canTargetPlants;

		// Token: 0x04004FC8 RID: 20424
		public List<Faction> onlyTargetFactions;

		// Token: 0x04004FC9 RID: 20425
		public Predicate<TargetInfo> validator;

		// Token: 0x04004FCA RID: 20426
		public bool onlyTargetFlammables;

		// Token: 0x04004FCB RID: 20427
		public Thing targetSpecificThing;

		// Token: 0x04004FCC RID: 20428
		public bool mustBeSelectable;

		// Token: 0x04004FCD RID: 20429
		public bool neverTargetDoors;

		// Token: 0x04004FCE RID: 20430
		public bool neverTargetIncapacitated;

		// Token: 0x04004FCF RID: 20431
		public bool neverTargetHostileFaction;

		// Token: 0x04004FD0 RID: 20432
		public bool onlyTargetSameIdeo;

		// Token: 0x04004FD1 RID: 20433
		public bool onlyTargetThingsAffectingRegions;

		// Token: 0x04004FD2 RID: 20434
		public bool onlyTargetDamagedThings;

		// Token: 0x04004FD3 RID: 20435
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		// Token: 0x04004FD4 RID: 20436
		public bool onlyTargetIncapacitatedPawns;

		// Token: 0x04004FD5 RID: 20437
		public bool onlyTargetColonistsOrPrisoners;

		// Token: 0x04004FD6 RID: 20438
		public bool onlyTargetColonistsOrPrisonersOrSlaves;

		// Token: 0x04004FD7 RID: 20439
		public bool onlyTargetColonistsOrPrisonersOrSlavesAllowMinorMentalBreaks;

		// Token: 0x04004FD8 RID: 20440
		public bool onlyTargetControlledPawns;

		// Token: 0x04004FD9 RID: 20441
		public bool onlyTargetColonists;

		// Token: 0x04004FDA RID: 20442
		public bool onlyTargetPrisonersOfColony;

		// Token: 0x04004FDB RID: 20443
		public bool onlyTargetPsychicSensitive;

		// Token: 0x04004FDC RID: 20444
		public bool onlyTargetAnimaTrees;

		// Token: 0x04004FDD RID: 20445
		public ThingCategory thingCategory;
	}
}
