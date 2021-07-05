using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200166F RID: 5743
	public class QuestNode_GetFaction : QuestNode
	{
		// Token: 0x060085C0 RID: 34240 RVA: 0x002FF710 File Offset: 0x002FD910
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

		// Token: 0x060085C1 RID: 34241 RVA: 0x002FF764 File Offset: 0x002FD964
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

		// Token: 0x060085C2 RID: 34242 RVA: 0x002FF7F0 File Offset: 0x002FD9F0
		private bool TryFindFaction(out Faction faction, Slate slate)
		{
			return (from x in Find.FactionManager.GetFactions(true, false, true, TechLevel.Undefined, false)
			where this.IsGoodFaction(x, slate)
			select x).TryRandomElement(out faction);
		}

		// Token: 0x060085C3 RID: 34243 RVA: 0x002FF838 File Offset: 0x002FDA38
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

		// Token: 0x060085C4 RID: 34244 RVA: 0x002FFA3C File Offset: 0x002FDC3C
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

		// Token: 0x0400538E RID: 21390
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400538F RID: 21391
		public SlateRef<bool> allowEnemy;

		// Token: 0x04005390 RID: 21392
		public SlateRef<bool> allowNeutral;

		// Token: 0x04005391 RID: 21393
		public SlateRef<bool> allowAlly;

		// Token: 0x04005392 RID: 21394
		public SlateRef<bool> allowAskerFaction;

		// Token: 0x04005393 RID: 21395
		public SlateRef<bool?> allowPermanentEnemy;

		// Token: 0x04005394 RID: 21396
		public SlateRef<bool> mustBePermanentEnemy;

		// Token: 0x04005395 RID: 21397
		public SlateRef<bool> playerCantBeAttackingCurrently;

		// Token: 0x04005396 RID: 21398
		public SlateRef<bool> peaceTalksCantExist;

		// Token: 0x04005397 RID: 21399
		public SlateRef<bool> leaderMustBeSafe;

		// Token: 0x04005398 RID: 21400
		public SlateRef<bool> mustHaveGoodwillRewardsEnabled;

		// Token: 0x04005399 RID: 21401
		public SlateRef<Pawn> ofPawn;

		// Token: 0x0400539A RID: 21402
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x0400539B RID: 21403
		public SlateRef<IEnumerable<Faction>> exclude;

		// Token: 0x0400539C RID: 21404
		public SlateRef<IEnumerable<Faction>> allowedHiddenFactions;
	}
}
