using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBF RID: 3775
	public class FactionRelation : IExposable
	{
		// Token: 0x06005912 RID: 22802 RVA: 0x001E6120 File Offset: 0x001E4320
		public void CheckKindThresholds(Faction faction, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			int num = faction.GoodwillWith(this.other);
			FactionRelationKind previousKind = this.kind;
			sentLetter = false;
			if (this.kind != FactionRelationKind.Hostile && num <= -75)
			{
				this.kind = FactionRelationKind.Hostile;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind != FactionRelationKind.Ally && num >= 75)
			{
				this.kind = FactionRelationKind.Ally;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Hostile && num >= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Ally && num <= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x001E61E1 File Offset: 0x001E43E1
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.kind, "kind", FactionRelationKind.Neutral, false);
			Scribe_Values.Look<int>(ref this.baseGoodwill, "goodwill", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x001E621E File Offset: 0x001E441E
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.other,
				", kind=",
				this.kind,
				")"
			});
		}

		// Token: 0x0400345B RID: 13403
		public Faction other;

		// Token: 0x0400345C RID: 13404
		public int baseGoodwill = 100;

		// Token: 0x0400345D RID: 13405
		public FactionRelationKind kind = FactionRelationKind.Neutral;
	}
}
