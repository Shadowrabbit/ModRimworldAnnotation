using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017EF RID: 6127
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06008EDF RID: 36575 RVA: 0x003342B4 File Offset: 0x003324B4
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06008EE0 RID: 36576 RVA: 0x003342BC File Offset: 0x003324BC
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x06008EE1 RID: 36577 RVA: 0x003342D0 File Offset: 0x003324D0
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x06008EE2 RID: 36578 RVA: 0x00334307 File Offset: 0x00332507
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06008EE3 RID: 36579 RVA: 0x00334324 File Offset: 0x00332524
		public override void CompTick()
		{
			base.CompTick();
			if (this.active)
			{
				MapParent mapParent = this.parent as MapParent;
				if (mapParent != null)
				{
					this.CheckAllEnemiesDefeated(mapParent);
				}
			}
		}

		// Token: 0x06008EE4 RID: 36580 RVA: 0x00334355 File Offset: 0x00332555
		private void CheckAllEnemiesDefeated(MapParent mapParent)
		{
			if (!mapParent.HasMap)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true, true))
			{
				return;
			}
			this.GiveRewardsAndSendLetter();
			this.StopQuest();
		}

		// Token: 0x06008EE5 RID: 36581 RVA: 0x0033437C File Offset: 0x0033257C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.relationsImprovement, "relationsImprovement", 0, false);
			Scribe_References.Look<Faction>(ref this.requestingFaction, "requestingFaction", false);
			Scribe_Deep.Look<ThingOwner>(ref this.rewards, "rewards", new object[]
			{
				this
			});
		}

		// Token: 0x06008EE6 RID: 36582 RVA: 0x003343E0 File Offset: 0x003325E0
		private void GiveRewardsAndSendLetter()
		{
			Map map = Find.AnyPlayerHomeMap ?? ((MapParent)this.parent).Map;
			DefeatAllEnemiesQuestComp.tmpRewards.AddRange(this.rewards);
			this.rewards.Clear();
			IntVec3 intVec = DropCellFinder.TradeDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, DefeatAllEnemiesQuestComp.tmpRewards, 110, false, false, false, true);
			DefeatAllEnemiesQuestComp.tmpRewards.Clear();
			FactionRelationKind playerRelationKind = this.requestingFaction.PlayerRelationKind;
			TaggedString text = "LetterDefeatAllEnemiesQuestCompleted".Translate(this.requestingFaction.Name, this.relationsImprovement.ToString());
			Faction.OfPlayer.TryAffectGoodwillWith(this.requestingFaction, this.relationsImprovement, false, false, HistoryEventDefOf.QuestGoodwillReward, null);
			this.requestingFaction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, this.requestingFaction.PlayerRelationKind, null);
			Find.LetterStack.ReceiveLetter("LetterLabelDefeatAllEnemiesQuestCompleted".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(intVec, map, false), this.requestingFaction, null, null, null);
		}

		// Token: 0x06008EE7 RID: 36583 RVA: 0x003344EE File Offset: 0x003326EE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06008EE8 RID: 36584 RVA: 0x003344FC File Offset: 0x003326FC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x06008EE9 RID: 36585 RVA: 0x00334504 File Offset: 0x00332704
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06008EEA RID: 36586 RVA: 0x00334518 File Offset: 0x00332718
		public override string CompInspectStringExtra()
		{
			if (this.active)
			{
				string value = GenThing.ThingsToCommaList(this.rewards, true, true, 5).CapitalizeFirst();
				return "QuestTargetDestroyInspectString".Translate(this.requestingFaction.Name, value, GenThing.GetMarketValue(this.rewards).ToStringMoney(null)).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x04005A02 RID: 23042
		private bool active;

		// Token: 0x04005A03 RID: 23043
		public Faction requestingFaction;

		// Token: 0x04005A04 RID: 23044
		public int relationsImprovement;

		// Token: 0x04005A05 RID: 23045
		public ThingOwner rewards;

		// Token: 0x04005A06 RID: 23046
		private static List<Thing> tmpRewards = new List<Thing>();
	}
}
