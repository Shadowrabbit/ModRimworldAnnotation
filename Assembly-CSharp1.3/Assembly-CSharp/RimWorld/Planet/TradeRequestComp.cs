using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017FC RID: 6140
	[StaticConstructorOnStartup]
	public class TradeRequestComp : WorldObjectComp
	{
		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06008F54 RID: 36692 RVA: 0x003355E4 File Offset: 0x003337E4
		public bool ActiveRequest
		{
			get
			{
				return this.expiration > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06008F55 RID: 36693 RVA: 0x003355F8 File Offset: 0x003337F8
		public override string CompInspectStringExtra()
		{
			if (this.ActiveRequest)
			{
				return "CaravanRequestInfo".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount).CapitalizeFirst(), (this.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F1"), (this.requestThingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)this.requestCount).ToStringMoney(null));
			}
			return null;
		}

		// Token: 0x06008F56 RID: 36694 RVA: 0x0033567D File Offset: 0x0033387D
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == this.parent)
			{
				yield return this.FulfillRequestCommand(caravan);
			}
			yield break;
		}

		// Token: 0x06008F57 RID: 36695 RVA: 0x00335694 File Offset: 0x00333894
		public void Disable()
		{
			this.expiration = -1;
		}

		// Token: 0x06008F58 RID: 36696 RVA: 0x003356A0 File Offset: 0x003338A0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.requestThingDef, "requestThingDef");
			Scribe_Values.Look<int>(ref this.requestCount, "requestCount", 0, false);
			Scribe_Values.Look<int>(ref this.expiration, "expiration", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008F59 RID: 36697 RVA: 0x003356F0 File Offset: 0x003338F0
		private Command FulfillRequestCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandFulfillTradeOffer".Translate();
			command_Action.defaultDesc = "CommandFulfillTradeOfferDesc".Translate();
			command_Action.icon = TradeRequestComp.TradeCommandTex;
			Action <>9__1;
			command_Action.action = delegate()
			{
				if (!this.ActiveRequest)
				{
					Log.Error("Attempted to fulfill an unavailable request");
					return;
				}
				if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
				{
					Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)), MessageTypeDefOf.RejectInput, false);
					return;
				}
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = "CommandFulfillTradeOfferConfirm".Translate(GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount));
				Action confirmedAct;
				if ((confirmedAct = <>9__1) == null)
				{
					confirmedAct = (<>9__1 = delegate()
					{
						this.Fulfill(caravan);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null));
			};
			if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
			{
				command_Action.Disable("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)));
			}
			return command_Action;
		}

		// Token: 0x06008F5A RID: 36698 RVA: 0x003357B0 File Offset: 0x003339B0
		private void Fulfill(Caravan caravan)
		{
			int remaining = this.requestCount;
			List<Thing> list = CaravanInventoryUtility.TakeThings(caravan, delegate(Thing thing)
			{
				if (this.requestThingDef != thing.def)
				{
					return 0;
				}
				if (!this.PlayerCanGive(thing))
				{
					return 0;
				}
				int num = Mathf.Min(remaining, thing.stackCount);
				remaining -= num;
				return num;
			});
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
			if (this.parent.Faction != null)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.parent.Faction, 12, true, true, HistoryEventDefOf.QuestGoodwillReward, null);
			}
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "TradeRequestFulfilled", this.parent.Named("SUBJECT"), caravan.Named("CARAVAN"));
			this.Disable();
		}

		// Token: 0x06008F5B RID: 36699 RVA: 0x00335874 File Offset: 0x00333A74
		private bool PlayerCanGive(Thing thing)
		{
			if (thing.GetRotStage() != RotStage.Fresh)
			{
				return false;
			}
			Apparel apparel = thing as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			return compQuality == null || compQuality.Quality >= QualityCategory.Normal;
		}

		// Token: 0x04005A1D RID: 23069
		public ThingDef requestThingDef;

		// Token: 0x04005A1E RID: 23070
		public int requestCount;

		// Token: 0x04005A1F RID: 23071
		public int expiration = -1;

		// Token: 0x04005A20 RID: 23072
		public string outSignalFulfilled;

		// Token: 0x04005A21 RID: 23073
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest", true);
	}
}
