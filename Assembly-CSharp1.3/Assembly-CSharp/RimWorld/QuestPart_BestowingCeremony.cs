using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B56 RID: 2902
	public class QuestPart_BestowingCeremony : QuestPart_MakeLord
	{
		// Token: 0x060043E3 RID: 17379 RVA: 0x00169134 File Offset: 0x00167334
		public static bool TryGetCeremonySpot(Pawn pawn, Faction bestowingFaction, out LocalTargetInfo spot, out IntVec3 absoluteSpot)
		{
			if (pawn != null)
			{
				RoyalTitleDef titleAwardedWhenUpdating = pawn.royalty.GetTitleAwardedWhenUpdating(bestowingFaction, pawn.royalty.GetFavor(bestowingFaction));
				if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null && pawn.ownership.AssignedThrone != null)
				{
					QuestPart_BestowingCeremony.<>c__DisplayClass5_0 CS$<>8__locals1 = new QuestPart_BestowingCeremony.<>c__DisplayClass5_0();
					CS$<>8__locals1.throne = pawn.ownership.AssignedThrone;
					CS$<>8__locals1.throneRoom = CS$<>8__locals1.throne.GetRoom(RegionType.Set_All);
					spot = CS$<>8__locals1.throne;
					IntVec3 facingCell = spot.Thing.Rotation.FacingCell;
					absoluteSpot = spot.Thing.InteractionCell + facingCell * 3;
					bool flag = false;
					for (int i = 0; i < 3; i++)
					{
						if (CS$<>8__locals1.<TryGetCeremonySpot>g__ValidateSpot|0(absoluteSpot))
						{
							flag = true;
							break;
						}
						absoluteSpot -= facingCell;
					}
					if (flag)
					{
						return true;
					}
					absoluteSpot = spot.Thing.InteractionCell - facingCell * 3;
					for (int j = 0; j < 3; j++)
					{
						if (CS$<>8__locals1.<TryGetCeremonySpot>g__ValidateSpot|0(absoluteSpot))
						{
							flag = true;
							break;
						}
						absoluteSpot += facingCell;
					}
					if (flag)
					{
						return true;
					}
					if (CS$<>8__locals1.throneRoom != null && (from c in CS$<>8__locals1.throneRoom.Cells
					where base.<TryGetCeremonySpot>g__ValidateSpot|0(c)
					select c).TryRandomElementByWeight((IntVec3 c) => c.DistanceTo(CS$<>8__locals1.throne.Position), out absoluteSpot))
					{
						return true;
					}
				}
				IntVec3 intVec;
				if (pawn.Map != null && pawn.Map.IsPlayerHome && (RCellFinder.TryFindGatheringSpot(pawn, GatheringDefOf.Party, true, out intVec) || RCellFinder.TryFindRandomSpotJustOutsideColony(pawn.Position, pawn.Map, out intVec)))
				{
					spot = (absoluteSpot = intVec);
					return true;
				}
			}
			spot = LocalTargetInfo.Invalid;
			absoluteSpot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x00169324 File Offset: 0x00167524
		protected override Lord MakeLord()
		{
			LocalTargetInfo targetSpot;
			IntVec3 spotCell;
			if (!QuestPart_BestowingCeremony.TryGetCeremonySpot(this.target, this.bestower.Faction, out targetSpot, out spotCell))
			{
				Log.Error("Cannot find ceremony spot for bestowing ceremony!");
				return null;
			}
			Lord lord = LordMaker.MakeNewLord(this.faction, new LordJob_BestowingCeremony(this.bestower, this.target, targetSpot, spotCell, this.shuttle, this.questTag + ".QuestEnded"), base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x001693A5 File Offset: 0x001675A5
		public override void Cleanup()
		{
			Find.SignalManager.SendSignal(new Signal(this.questTag + ".QuestEnded", this.quest.Named("SUBJECT")));
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x001693D8 File Offset: 0x001675D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
		}

		// Token: 0x0400292E RID: 10542
		public const int PreferredDistanceFromThrone = 3;

		// Token: 0x0400292F RID: 10543
		public Pawn bestower;

		// Token: 0x04002930 RID: 10544
		public Pawn target;

		// Token: 0x04002931 RID: 10545
		public Thing shuttle;

		// Token: 0x04002932 RID: 10546
		public string questTag;
	}
}
