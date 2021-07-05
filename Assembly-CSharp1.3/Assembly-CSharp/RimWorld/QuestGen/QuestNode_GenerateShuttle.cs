using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200164F RID: 5711
	public class QuestNode_GenerateShuttle : QuestNode
	{
		// Token: 0x06008555 RID: 34133 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008556 RID: 34134 RVA: 0x002FDFC4 File Offset: 0x002FC1C4
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Shuttle"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
			if (this.owningFaction.GetValue(slate) != null)
			{
				thing.SetFaction(this.owningFaction.GetValue(slate), null);
			}
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (this.requiredPawns.GetValue(slate) != null)
			{
				compShuttle.requiredPawns.AddRange(this.requiredPawns.GetValue(slate));
			}
			if (this.requiredItems.GetValue(slate) != null)
			{
				compShuttle.requiredItems.AddRange(this.requiredItems.GetValue(slate));
			}
			compShuttle.acceptColonists = this.acceptColonists.GetValue(slate);
			compShuttle.onlyAcceptColonists = this.onlyAcceptColonists.GetValue(slate);
			compShuttle.onlyAcceptHealthy = this.onlyAcceptHealthy.GetValue(slate);
			compShuttle.requiredColonistCount = this.requireColonistCount.GetValue(slate);
			compShuttle.permitShuttle = this.permitShuttle.GetValue(slate);
			QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
		}

		// Token: 0x04005327 RID: 21287
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005328 RID: 21288
		public SlateRef<IEnumerable<Pawn>> requiredPawns;

		// Token: 0x04005329 RID: 21289
		public SlateRef<IEnumerable<ThingDefCount>> requiredItems;

		// Token: 0x0400532A RID: 21290
		public SlateRef<int> requireColonistCount;

		// Token: 0x0400532B RID: 21291
		public SlateRef<bool> acceptColonists;

		// Token: 0x0400532C RID: 21292
		public SlateRef<bool> onlyAcceptColonists;

		// Token: 0x0400532D RID: 21293
		public SlateRef<bool> onlyAcceptHealthy;

		// Token: 0x0400532E RID: 21294
		public SlateRef<Faction> owningFaction;

		// Token: 0x0400532F RID: 21295
		public SlateRef<bool> permitShuttle;
	}
}
