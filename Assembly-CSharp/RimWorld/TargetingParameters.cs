using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D72 RID: 7538
	public class TargetingParameters
	{
		// Token: 0x0600A3E2 RID: 41954 RVA: 0x002FBA00 File Offset: 0x002F9C00
		public bool CanTarget(TargetInfo targ)
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
					if (pawn.Faction == Faction.OfMechanoids)
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
				if (this.onlyTargetPsychicSensitive && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) <= 0f)
				{
					return false;
				}
				if (this.neverTargetHostileFaction && !pawn.IsPrisonerOfColony)
				{
					Faction factionOrExtraMiniOrHomeFaction = pawn.FactionOrExtraMiniOrHomeFaction;
					if (factionOrExtraMiniOrHomeFaction != null && factionOrExtraMiniOrHomeFaction.HostileTo(Faction.OfPlayer))
					{
						return false;
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

		// Token: 0x0600A3E3 RID: 41955 RVA: 0x0006CB59 File Offset: 0x0006AD59
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

		// Token: 0x0600A3E4 RID: 41956 RVA: 0x002FBCB4 File Offset: 0x002F9EB4
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
					return pawn != null && pawn != arrester && pawn.CanBeArrestedBy(arrester) && !pawn.Downed;
				}
			};
		}

		// Token: 0x0600A3E5 RID: 41957 RVA: 0x002FBCFC File Offset: 0x002F9EFC
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

		// Token: 0x0600A3E6 RID: 41958 RVA: 0x0006CB7C File Offset: 0x0006AD7C
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

		// Token: 0x0600A3E7 RID: 41959 RVA: 0x0006CB9F File Offset: 0x0006AD9F
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

		// Token: 0x0600A3E8 RID: 41960 RVA: 0x002FBD50 File Offset: 0x002F9F50
		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = ((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

		// Token: 0x0600A3E9 RID: 41961 RVA: 0x002FBD9C File Offset: 0x002F9F9C
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

		// Token: 0x0600A3EA RID: 41962 RVA: 0x002FBDE8 File Offset: 0x002F9FE8
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

		// Token: 0x0600A3EB RID: 41963 RVA: 0x002FBE4C File Offset: 0x002FA04C
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

		// Token: 0x0600A3EC RID: 41964 RVA: 0x002FBE98 File Offset: 0x002FA098
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

		// Token: 0x0600A3ED RID: 41965 RVA: 0x002FBEE4 File Offset: 0x002FA0E4
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

		// Token: 0x04006F02 RID: 28418
		public bool canTargetLocations;

		// Token: 0x04006F03 RID: 28419
		public bool canTargetSelf;

		// Token: 0x04006F04 RID: 28420
		public bool canTargetPawns = true;

		// Token: 0x04006F05 RID: 28421
		public bool canTargetFires;

		// Token: 0x04006F06 RID: 28422
		public bool canTargetBuildings = true;

		// Token: 0x04006F07 RID: 28423
		public bool canTargetItems;

		// Token: 0x04006F08 RID: 28424
		public bool canTargetAnimals = true;

		// Token: 0x04006F09 RID: 28425
		public bool canTargetHumans = true;

		// Token: 0x04006F0A RID: 28426
		public bool canTargetMechs = true;

		// Token: 0x04006F0B RID: 28427
		public List<Faction> onlyTargetFactions;

		// Token: 0x04006F0C RID: 28428
		public Predicate<TargetInfo> validator;

		// Token: 0x04006F0D RID: 28429
		public bool onlyTargetFlammables;

		// Token: 0x04006F0E RID: 28430
		public Thing targetSpecificThing;

		// Token: 0x04006F0F RID: 28431
		public bool mustBeSelectable;

		// Token: 0x04006F10 RID: 28432
		public bool neverTargetDoors;

		// Token: 0x04006F11 RID: 28433
		public bool neverTargetIncapacitated;

		// Token: 0x04006F12 RID: 28434
		public bool neverTargetHostileFaction;

		// Token: 0x04006F13 RID: 28435
		public bool onlyTargetThingsAffectingRegions;

		// Token: 0x04006F14 RID: 28436
		public bool onlyTargetDamagedThings;

		// Token: 0x04006F15 RID: 28437
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		// Token: 0x04006F16 RID: 28438
		public bool onlyTargetIncapacitatedPawns;

		// Token: 0x04006F17 RID: 28439
		public bool onlyTargetControlledPawns;

		// Token: 0x04006F18 RID: 28440
		public bool onlyTargetColonists;

		// Token: 0x04006F19 RID: 28441
		public bool onlyTargetPrisonersOfColony;

		// Token: 0x04006F1A RID: 28442
		public bool onlyTargetPsychicSensitive;

		// Token: 0x04006F1B RID: 28443
		public ThingCategory thingCategory;
	}
}
