using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F4D RID: 8013
	public class QuestNode_GetRandomFactionForSite : QuestNode
	{
		// Token: 0x0600AB0B RID: 43787 RVA: 0x0006FEFD File Offset: 0x0006E0FD
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate, true);
		}

		// Token: 0x0600AB0C RID: 43788 RVA: 0x0006FF07 File Offset: 0x0006E107
		protected override void RunInt()
		{
			this.TrySetVars(QuestGen.slate, false);
		}

		// Token: 0x0600AB0D RID: 43789 RVA: 0x0031DF68 File Offset: 0x0031C168
		private bool TrySetVars(Slate slate, bool test)
		{
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(this.sitePartDefs.GetValue(slate), out faction, true, (Faction x) => (asker == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))))
			{
				return false;
			}
			if (!Find.Storyteller.difficultyValues.allowViolentQuests && faction.HostileTo(Faction.OfPlayer))
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

		// Token: 0x04007463 RID: 29795
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04007464 RID: 29796
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007465 RID: 29797
		public SlateRef<Thing> mustBeHostileToFactionOf;
	}
}
