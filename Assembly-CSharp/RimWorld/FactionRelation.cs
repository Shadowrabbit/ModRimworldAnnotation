using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A4 RID: 5540
	public class FactionRelation : IExposable
	{
		// Token: 0x0600784D RID: 30797 RVA: 0x00249784 File Offset: 0x00247984
		public void CheckKindThresholds(Faction faction, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			FactionRelationKind previousKind = this.kind;
			sentLetter = false;
			if (this.kind != FactionRelationKind.Hostile && this.goodwill <= -75)
			{
				this.kind = FactionRelationKind.Hostile;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind != FactionRelationKind.Ally && this.goodwill >= 75)
			{
				this.kind = FactionRelationKind.Ally;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Hostile && this.goodwill >= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Ally && this.goodwill <= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
		}

		// Token: 0x0600784E RID: 30798 RVA: 0x00050FF9 File Offset: 0x0004F1F9
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<int>(ref this.goodwill, "goodwill", 0, false);
			Scribe_Values.Look<FactionRelationKind>(ref this.kind, "kind", FactionRelationKind.Neutral, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600784F RID: 30799 RVA: 0x0024984C File Offset: 0x00247A4C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.other,
				", goodwill=",
				this.goodwill.ToString("F1"),
				", kind=",
				this.kind,
				")"
			});
		}

		// Token: 0x04004F53 RID: 20307
		public Faction other;

		// Token: 0x04004F54 RID: 20308
		public int goodwill = 100;

		// Token: 0x04004F55 RID: 20309
		public FactionRelationKind kind = FactionRelationKind.Neutral;
	}
}
