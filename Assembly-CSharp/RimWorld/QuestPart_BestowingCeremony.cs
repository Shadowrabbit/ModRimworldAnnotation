using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200108B RID: 4235
	public class QuestPart_BestowingCeremony : QuestPart_MakeLord
	{
		// Token: 0x06005C46 RID: 23622 RVA: 0x001D9F74 File Offset: 0x001D8174
		public static bool TryGetCeremonySpot(Pawn pawn, Faction bestowingFaction, out LocalTargetInfo spot, out IntVec3 absoluteSpot)
		{
			if (pawn != null)
			{
				RoyalTitleDef titleAwardedWhenUpdating = pawn.royalty.GetTitleAwardedWhenUpdating(bestowingFaction, pawn.royalty.GetFavor(bestowingFaction));
				if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null && pawn.ownership.AssignedThrone != null)
				{
					QuestPart_BestowingCeremony.<>c__DisplayClass5_0 CS$<>8__locals1 = new QuestPart_BestowingCeremony.<>c__DisplayClass5_0();
					CS$<>8__locals1.throne = pawn.ownership.AssignedThrone;
					CS$<>8__locals1.throneRoom = CS$<>8__locals1.throne.GetRoom(RegionType.Set_Passable);
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
				if (pawn.Map != null && pawn.Map.IsPlayerHome && (RCellFinder.TryFindGatheringSpot_NewTemp(pawn, GatheringDefOf.Party, true, out intVec) || RCellFinder.TryFindRandomSpotJustOutsideColony(pawn.Position, pawn.Map, out intVec)))
				{
					spot = (absoluteSpot = intVec);
					return true;
				}
			}
			spot = LocalTargetInfo.Invalid;
			absoluteSpot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x001DA164 File Offset: 0x001D8364
		protected override Lord MakeLord()
		{
			LocalTargetInfo spot;
			IntVec3 spotCell;
			if (!QuestPart_BestowingCeremony.TryGetCeremonySpot(this.target, this.bestower.Faction, out spot, out spotCell))
			{
				Log.Error("Cannot find ceremony spot for bestowing ceremony!", false);
				return null;
			}
			Lord lord = LordMaker.MakeNewLord(this.faction, new LordJob_BestowingCeremony(this.bestower, this.target, spot, spotCell, this.shuttle, this.questTag + ".QuestEnded"), base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x00040032 File Offset: 0x0003E232
		public override void Cleanup()
		{
			Find.SignalManager.SendSignal(new Signal(this.questTag + ".QuestEnded", this.quest.Named("SUBJECT")));
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x001DA1E8 File Offset: 0x001D83E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
		}

		// Token: 0x04003DCB RID: 15819
		public const int PreferredDistanceFromThrone = 3;

		// Token: 0x04003DCC RID: 15820
		public Pawn bestower;

		// Token: 0x04003DCD RID: 15821
		public Pawn target;

		// Token: 0x04003DCE RID: 15822
		public Thing shuttle;

		// Token: 0x04003DCF RID: 15823
		public string questTag;
	}
}
