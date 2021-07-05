using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F0B RID: 7947
	public class QuestNode_GenerateShuttle : QuestNode
	{
		// Token: 0x0600AA22 RID: 43554 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA23 RID: 43555 RVA: 0x0031ACB8 File Offset: 0x00318EB8
		protected override void RunInt()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
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
			compShuttle.dropEverythingIfUnsatisfied = this.dropEverythingIfUnsatisfied.GetValue(slate);
			compShuttle.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied.GetValue(slate);
			compShuttle.dropEverythingOnArrival = this.dropEverythingOnArrival.GetValue(slate);
			compShuttle.permitShuttle = this.permitShuttle.GetValue(slate);
			compShuttle.hideControls = this.hideControls.GetValue(slate);
			QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
		}

		// Token: 0x04007378 RID: 29560
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007379 RID: 29561
		public SlateRef<IEnumerable<Pawn>> requiredPawns;

		// Token: 0x0400737A RID: 29562
		public SlateRef<IEnumerable<ThingDefCount>> requiredItems;

		// Token: 0x0400737B RID: 29563
		public SlateRef<int> requireColonistCount;

		// Token: 0x0400737C RID: 29564
		public SlateRef<bool> acceptColonists;

		// Token: 0x0400737D RID: 29565
		public SlateRef<bool> onlyAcceptColonists;

		// Token: 0x0400737E RID: 29566
		public SlateRef<bool> onlyAcceptHealthy;

		// Token: 0x0400737F RID: 29567
		public SlateRef<bool> leaveImmediatelyWhenSatisfied;

		// Token: 0x04007380 RID: 29568
		public SlateRef<bool> dropEverythingIfUnsatisfied;

		// Token: 0x04007381 RID: 29569
		public SlateRef<bool> dropEverythingOnArrival;

		// Token: 0x04007382 RID: 29570
		public SlateRef<Faction> owningFaction;

		// Token: 0x04007383 RID: 29571
		public SlateRef<bool> permitShuttle;

		// Token: 0x04007384 RID: 29572
		public SlateRef<bool> hideControls;
	}
}
