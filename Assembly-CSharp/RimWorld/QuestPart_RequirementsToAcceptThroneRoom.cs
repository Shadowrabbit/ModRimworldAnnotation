using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001107 RID: 4359
	public class QuestPart_RequirementsToAcceptThroneRoom : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005F3D RID: 24381 RVA: 0x00041E2B File Offset: 0x0004002B
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.forTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x001E1E48 File Offset: 0x001E0048
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
				return "QuestThroneRoomRequirementsUnsatisfied".Translate(this.forPawn.Named("PAWN"), this.forTitle.GetLabelFor(this.forPawn).Named("TITLE"));
			}
			foreach (RoomRequirement roomRequirement in this.forTitle.throneRoomRequirements)
			{
				if (!roomRequirement.Met(assignedThrone.GetRoom(RegionType.Set_Passable), this.forPawn))
				{
					this.acceptanceReportUnmetRequirements.Add(roomRequirement.LabelCap(assignedThrone.GetRoom(RegionType.Set_Passable)));
				}
			}
			if (this.acceptanceReportUnmetRequirements.Count != 0)
			{
				return new AcceptanceReport("QuestThroneRoomRequirementsUnsatisfied".Translate(this.forPawn.Named("PAWN"), this.forTitle.GetLabelFor(this.forPawn).Named("TITLE")) + ":\n\n" + this.acceptanceReportUnmetRequirements.ToLineList("- "));
			}
			return true;
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x00041E3B File Offset: 0x0004003B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.forTitle, "forTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.forPawn, "forPawn", false);
		}

		// Token: 0x06005F40 RID: 24384 RVA: 0x00041E75 File Offset: 0x00040075
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.forPawn == replace)
			{
				this.forPawn = with;
			}
		}

		// Token: 0x04003FB6 RID: 16310
		public RoyalTitleDef forTitle;

		// Token: 0x04003FB7 RID: 16311
		public Faction faction;

		// Token: 0x04003FB8 RID: 16312
		public Pawn forPawn;

		// Token: 0x04003FB9 RID: 16313
		private List<string> acceptanceReportUnmetRequirements = new List<string>();
	}
}
