using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA0 RID: 2976
	public class QuestPart_RequirementsToAcceptThroneRoom : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x0600457E RID: 17790 RVA: 0x00170756 File Offset: 0x0016E956
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.forTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x00170768 File Offset: 0x0016E968
		public override AcceptanceReport CanAccept()
		{
			this.acceptanceReportUnmetRequirements.Clear();
			if (this.forTitle.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return true;
			}
			Building_Throne assignedThrone = this.forPawn.ownership.AssignedThrone;
			if (assignedThrone == null)
			{
				return "QuestNoThroneRoom".Translate(this.forPawn.Named("PAWN"));
			}
			foreach (RoomRequirement roomRequirement in this.forTitle.throneRoomRequirements)
			{
				if (!roomRequirement.MetOrDisabled(assignedThrone.GetRoom(RegionType.Set_All), this.forPawn))
				{
					this.acceptanceReportUnmetRequirements.Add(roomRequirement.LabelCap(assignedThrone.GetRoom(RegionType.Set_All)));
				}
			}
			if (this.acceptanceReportUnmetRequirements.Count != 0)
			{
				return new AcceptanceReport("QuestThroneRoomRequirementsUnsatisfied".Translate(this.forPawn.Named("PAWN"), this.forTitle.GetLabelFor(this.forPawn).Named("TITLE")) + ":\n\n" + this.acceptanceReportUnmetRequirements.ToLineList("- "));
			}
			return true;
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x001708B4 File Offset: 0x0016EAB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.forTitle, "forTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.forPawn, "forPawn", false);
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x001708EE File Offset: 0x0016EAEE
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.forPawn == replace)
			{
				this.forPawn = with;
			}
		}

		// Token: 0x04002A51 RID: 10833
		public RoyalTitleDef forTitle;

		// Token: 0x04002A52 RID: 10834
		public Faction faction;

		// Token: 0x04002A53 RID: 10835
		public Pawn forPawn;

		// Token: 0x04002A54 RID: 10836
		private List<string> acceptanceReportUnmetRequirements = new List<string>();
	}
}
