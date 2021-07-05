using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001756 RID: 5974
	public class ThingSetMaker_MapGen_AncientPodContents : ThingSetMaker
	{
		// Token: 0x060083BC RID: 33724 RVA: 0x0027071C File Offset: 0x0026E91C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PodContentsType podContentsType = parms.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(true);
			switch (podContentsType)
			{
			case PodContentsType.Empty:
				break;
			case PodContentsType.AncientFriendly:
				outThings.Add(this.GenerateFriendlyAncient());
				return;
			case PodContentsType.AncientIncapped:
				outThings.Add(this.GenerateIncappedAncient());
				return;
			case PodContentsType.AncientHalfEaten:
				outThings.Add(this.GenerateHalfEatenAncient());
				outThings.AddRange(this.GenerateScarabs());
				return;
			case PodContentsType.AncientHostile:
				outThings.Add(this.GenerateAngryAncient());
				return;
			case PodContentsType.Slave:
				outThings.Add(this.GenerateSlave());
				return;
			default:
				Log.Error("Pod contents type not handled: " + podContentsType, false);
				break;
			}
		}

		// Token: 0x060083BD RID: 33725 RVA: 0x002707CC File Offset: 0x0026E9CC
		private Pawn GenerateFriendlyAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060083BE RID: 33726 RVA: 0x0027084C File Offset: 0x0026EA4C
		private Pawn GenerateIncappedAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, true);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060083BF RID: 33727 RVA: 0x002708D4 File Offset: 0x0026EAD4
		private Pawn GenerateSlave()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			HealthUtility.DamageUntilDowned(pawn, true);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			if (Rand.Value < 0.5f)
			{
				HealthUtility.DamageUntilDead(pawn);
			}
			return pawn;
		}

		// Token: 0x060083C0 RID: 33728 RVA: 0x0027096C File Offset: 0x0026EB6C
		private Pawn GenerateAngryAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060083C1 RID: 33729 RVA: 0x002709EC File Offset: 0x0026EBEC
		private Pawn GenerateHalfEatenAncient()
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, true, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			int num = Rand.Range(6, 10);
			for (int i = 0; i < num; i++)
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, (float)Rand.Range(3, 8), 0f, -1f, pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060083C2 RID: 33730 RVA: 0x00270AAC File Offset: 0x0026ECAC
		private List<Thing> GenerateScarabs()
		{
			List<Thing> list = new List<Thing>();
			int num = Rand.Range(3, 6);
			for (int i = 0; i < num; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megascarab, null);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
				list.Add(pawn);
			}
			return list;
		}

		// Token: 0x060083C3 RID: 33731 RVA: 0x00270B04 File Offset: 0x0026ED04
		private void GiveRandomLootInventoryForTombPawn(Pawn p)
		{
			if (Rand.Value < 0.65f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Gold, Rand.Range(10, 50));
			}
			else
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Plasteel, Rand.Range(10, 50));
			}
			if (Rand.Value < 0.7f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentIndustrial, Rand.Range(-2, 4));
				return;
			}
			this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentSpacer, Rand.Range(-2, 4));
		}

		// Token: 0x060083C4 RID: 33732 RVA: 0x00270BA8 File Offset: 0x0026EDA8
		private void MakeIntoContainer(ThingOwner container, ThingDef def, int count)
		{
			if (count <= 0)
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(def, null);
			thing.stackCount = count;
			container.TryAdd(thing, true);
		}

		// Token: 0x060083C5 RID: 33733 RVA: 0x00058661 File Offset: 0x00056861
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.AncientSoldier.race;
			yield return PawnKindDefOf.Slave.race;
			yield return PawnKindDefOf.Megascarab.race;
			yield return ThingDefOf.Gold;
			yield return ThingDefOf.Plasteel;
			yield return ThingDefOf.ComponentIndustrial;
			yield return ThingDefOf.ComponentSpacer;
			yield break;
		}
	}
}
