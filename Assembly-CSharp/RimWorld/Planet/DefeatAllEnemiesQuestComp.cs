using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002192 RID: 8594
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600B776 RID: 46966 RVA: 0x00076F6C File Offset: 0x0007516C
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600B777 RID: 46967 RVA: 0x00076F74 File Offset: 0x00075174
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600B778 RID: 46968 RVA: 0x00076F88 File Offset: 0x00075188
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x0600B779 RID: 46969 RVA: 0x00076FBF File Offset: 0x000751BF
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600B77A RID: 46970 RVA: 0x0034EC00 File Offset: 0x0034CE00
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

		// Token: 0x0600B77B RID: 46971 RVA: 0x00076FDB File Offset: 0x000751DB
		private void CheckAllEnemiesDefeated(MapParent mapParent)
		{
			if (!mapParent.HasMap)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true))
			{
				return;
			}
			this.GiveRewardsAndSendLetter();
			this.StopQuest();
		}

		// Token: 0x0600B77C RID: 46972 RVA: 0x0034EC34 File Offset: 0x0034CE34
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

		// Token: 0x0600B77D RID: 46973 RVA: 0x0034EC98 File Offset: 0x0034CE98
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
			this.requestingFaction.TryAffectGoodwillWith(Faction.OfPlayer, this.relationsImprovement, false, false, null, null);
			this.requestingFaction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, this.requestingFaction.PlayerRelationKind, null);
			Find.LetterStack.ReceiveLetter("LetterLabelDefeatAllEnemiesQuestCompleted".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(intVec, map, false), this.requestingFaction, null, null, null);
		}

		// Token: 0x0600B77E RID: 46974 RVA: 0x00077001 File Offset: 0x00075201
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B77F RID: 46975 RVA: 0x0007700F File Offset: 0x0007520F
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x0600B780 RID: 46976 RVA: 0x00077017 File Offset: 0x00075217
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600B781 RID: 46977 RVA: 0x0034EDA4 File Offset: 0x0034CFA4
		public override string CompInspectStringExtra()
		{
			if (this.active)
			{
				string value = GenThing.ThingsToCommaList(this.rewards, true, true, 5).CapitalizeFirst();
				return "QuestTargetDestroyInspectString".Translate(this.requestingFaction.Name, value, GenThing.GetMarketValue(this.rewards).ToStringMoney(null)).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x04007D8F RID: 32143
		private bool active;

		// Token: 0x04007D90 RID: 32144
		public Faction requestingFaction;

		// Token: 0x04007D91 RID: 32145
		public int relationsImprovement;

		// Token: 0x04007D92 RID: 32146
		public ThingOwner rewards;

		// Token: 0x04007D93 RID: 32147
		private static List<Thing> tmpRewards = new List<Thing>();
	}
}
