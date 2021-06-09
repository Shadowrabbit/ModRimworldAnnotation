using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F1B RID: 7963
	public class QuestNode_GetFaction : QuestNode
	{
		// Token: 0x0600AA5A RID: 43610 RVA: 0x0031B5D0 File Offset: 0x003197D0
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			if (slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, slate))
			{
				return true;
			}
			if (this.TryFindFaction(out faction, slate))
			{
				slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600AA5B RID: 43611 RVA: 0x0031B624 File Offset: 0x00319824
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Faction faction;
			if (QuestGen.slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, QuestGen.slate))
			{
				return;
			}
			if (this.TryFindFaction(out faction, QuestGen.slate))
			{
				QuestGen.slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
				if (!faction.Hidden)
				{
					QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
					questPart_InvolvedFactions.factions.Add(faction);
					QuestGen.quest.AddPart(questPart_InvolvedFactions);
				}
			}
		}

		// Token: 0x0600AA5C RID: 43612 RVA: 0x0031B6B0 File Offset: 0x003198B0
		private bool TryFindFaction(out Faction faction, Slate slate)
		{
			return (from x in Find.FactionManager.GetFactions_NewTemp(true, false, true, TechLevel.Undefined, false)
			where this.IsGoodFaction(x, slate)
			select x).TryRandomElement(out faction);
		}

		// Token: 0x0600AA5D RID: 43613 RVA: 0x0031B6F8 File Offset: 0x003198F8
		private bool IsGoodFaction(Faction faction, Slate slate)
		{
			if (faction.Hidden && (this.allowedHiddenFactions.GetValue(slate) == null || !this.allowedHiddenFactions.GetValue(slate).Contains(faction)))
			{
				return false;
			}
			if (this.ofPawn.GetValue(slate) != null && faction != this.ofPawn.GetValue(slate).Faction)
			{
				return false;
			}
			if (this.exclude.GetValue(slate) != null && this.exclude.GetValue(slate).Contains(faction))
			{
				return false;
			}
			if (this.mustBePermanentEnemy.GetValue(slate) && !faction.def.permanentEnemy)
			{
				return false;
			}
			if (!this.allowEnemy.GetValue(slate) && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (!this.allowNeutral.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				return false;
			}
			if (!this.allowAlly.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return false;
			}
			if (!(this.allowPermanentEnemy.GetValue(slate) ?? true) && faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.playerCantBeAttackingCurrently.GetValue(slate) && SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))
			{
				return false;
			}
			if (this.mustHaveGoodwillRewardsEnabled.GetValue(slate) && !faction.allowGoodwillRewards)
			{
				return false;
			}
			if (this.peaceTalksCantExist.GetValue(slate))
			{
				if (this.PeaceTalksExist(faction))
				{
					return false;
				}
				string tag = QuestNode_QuestUnique.GetProcessedTag("PeaceTalks", faction);
				if (Find.QuestManager.questsInDisplayOrder.Any((Quest q) => q.tags.Contains(tag)))
				{
					return false;
				}
			}
			if (this.leaderMustBeSafe.GetValue(slate) && (faction.leader == null || faction.leader.Spawned || faction.leader.IsPrisoner))
			{
				return false;
			}
			Thing value = this.mustBeHostileToFactionOf.GetValue(slate);
			return value == null || value.Faction == null || (value.Faction != faction && faction.HostileTo(value.Faction));
		}

		// Token: 0x0600AA5E RID: 43614 RVA: 0x0031B8FC File Offset: 0x00319AFC
		private bool PeaceTalksExist(Faction faction)
		{
			List<PeaceTalks> peaceTalks = Find.WorldObjects.PeaceTalks;
			for (int i = 0; i < peaceTalks.Count; i++)
			{
				if (peaceTalks[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040073B6 RID: 29622
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073B7 RID: 29623
		public SlateRef<bool> allowEnemy;

		// Token: 0x040073B8 RID: 29624
		public SlateRef<bool> allowNeutral;

		// Token: 0x040073B9 RID: 29625
		public SlateRef<bool> allowAlly;

		// Token: 0x040073BA RID: 29626
		public SlateRef<bool> allowAskerFaction;

		// Token: 0x040073BB RID: 29627
		public SlateRef<bool?> allowPermanentEnemy;

		// Token: 0x040073BC RID: 29628
		public SlateRef<bool> mustBePermanentEnemy;

		// Token: 0x040073BD RID: 29629
		public SlateRef<bool> playerCantBeAttackingCurrently;

		// Token: 0x040073BE RID: 29630
		public SlateRef<bool> peaceTalksCantExist;

		// Token: 0x040073BF RID: 29631
		public SlateRef<bool> leaderMustBeSafe;

		// Token: 0x040073C0 RID: 29632
		public SlateRef<bool> mustHaveGoodwillRewardsEnabled;

		// Token: 0x040073C1 RID: 29633
		public SlateRef<Pawn> ofPawn;

		// Token: 0x040073C2 RID: 29634
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x040073C3 RID: 29635
		public SlateRef<IEnumerable<Faction>> exclude;

		// Token: 0x040073C4 RID: 29636
		public SlateRef<IEnumerable<Faction>> allowedHiddenFactions;
	}
}
