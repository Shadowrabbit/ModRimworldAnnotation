using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9A RID: 2970
	public class QuestPart_RequirementsToAcceptFactionRelation : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06004568 RID: 17768 RVA: 0x001703E8 File Offset: 0x0016E5E8
		public string ReasonText
		{
			get
			{
				switch (this.relationKind)
				{
				case FactionRelationKind.Hostile:
					return "QuestHostileTo".Translate(this.otherFaction);
				case FactionRelationKind.Neutral:
					return "QuestNeutralTo".Translate(this.otherFaction);
				case FactionRelationKind.Ally:
					return "QuestAlliedTo".Translate(this.otherFaction);
				default:
					throw new Exception(string.Format("Unknown faction relation kind: {0}", this.relationKind));
				}
			}
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x0017047B File Offset: 0x0016E67B
		public override AcceptanceReport CanAccept()
		{
			if (this.otherFaction != null && Faction.OfPlayer.RelationKindWith(this.otherFaction) == this.relationKind)
			{
				return true;
			}
			return new AcceptanceReport(this.ReasonText);
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x001704AF File Offset: 0x0016E6AF
		public override void Notify_FactionRemoved(Faction faction)
		{
			base.Notify_FactionRemoved(faction);
			if (this.otherFaction == faction)
			{
				this.otherFaction = null;
			}
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x001704C8 File Offset: 0x0016E6C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_References.Look<Faction>(ref this.otherFaction, "otherFaction", false);
		}

		// Token: 0x04002A4A RID: 10826
		public Faction otherFaction;

		// Token: 0x04002A4B RID: 10827
		public FactionRelationKind relationKind;
	}
}
