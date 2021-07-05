using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001688 RID: 5768
	public class QuestNode_GetRandomFactionForSite : QuestNode
	{
		// Token: 0x0600862E RID: 34350 RVA: 0x00301FFD File Offset: 0x003001FD
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate, true);
		}

		// Token: 0x0600862F RID: 34351 RVA: 0x00302007 File Offset: 0x00300207
		protected override void RunInt()
		{
			this.TrySetVars(QuestGen.slate, false);
		}

		// Token: 0x06008630 RID: 34352 RVA: 0x00302018 File Offset: 0x00300218
		private bool TrySetVars(Slate slate, bool test)
		{
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(this.sitePartDefs.GetValue(slate), out faction, true, (Faction x) => (asker == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))))
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.allowViolentQuests && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
			if (!test && faction != null && !faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			return true;
		}

		// Token: 0x040053FE RID: 21502
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x040053FF RID: 21503
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005400 RID: 21504
		public SlateRef<Thing> mustBeHostileToFactionOf;
	}
}
