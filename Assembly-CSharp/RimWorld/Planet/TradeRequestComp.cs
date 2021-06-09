using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021A5 RID: 8613
	[StaticConstructorOnStartup]
	public class TradeRequestComp : WorldObjectComp
	{
		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x0600B808 RID: 47112 RVA: 0x000776D1 File Offset: 0x000758D1
		public bool ActiveRequest
		{
			get
			{
				return this.expiration > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600B809 RID: 47113 RVA: 0x0034FAD8 File Offset: 0x0034DCD8
		public override string CompInspectStringExtra()
		{
			if (this.ActiveRequest)
			{
				return "CaravanRequestInfo".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount).CapitalizeFirst(), (this.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F1"), (this.requestThingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)this.requestCount).ToStringMoney(null));
			}
			return null;
		}

		// Token: 0x0600B80A RID: 47114 RVA: 0x000776E5 File Offset: 0x000758E5
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == this.parent)
			{
				yield return this.FulfillRequestCommand(caravan);
			}
			yield break;
		}

		// Token: 0x0600B80B RID: 47115 RVA: 0x000776FC File Offset: 0x000758FC
		public void Disable()
		{
			this.expiration = -1;
		}

		// Token: 0x0600B80C RID: 47116 RVA: 0x0034FB60 File Offset: 0x0034DD60
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.requestThingDef, "requestThingDef");
			Scribe_Values.Look<int>(ref this.requestCount, "requestCount", 0, false);
			Scribe_Values.Look<int>(ref this.expiration, "expiration", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600B80D RID: 47117 RVA: 0x0034FBB0 File Offset: 0x0034DDB0
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
					Log.Error("Attempted to fulfill an unavailable request", false);
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

		// Token: 0x0600B80E RID: 47118 RVA: 0x0034FC70 File Offset: 0x0034DE70
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
				this.parent.Faction.TryAffectGoodwillWith(Faction.OfPlayer, 12, true, true, "GoodwillChangedReason_FulfilledTradeRequest".Translate(), new GlobalTargetInfo?(this.parent));
			}
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "TradeRequestFulfilled", this.parent.Named("SUBJECT"), caravan.Named("CARAVAN"));
			this.Disable();
		}

		// Token: 0x0600B80F RID: 47119 RVA: 0x0034FD44 File Offset: 0x0034DF44
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

		// Token: 0x04007DBA RID: 32186
		public ThingDef requestThingDef;

		// Token: 0x04007DBB RID: 32187
		public int requestCount;

		// Token: 0x04007DBC RID: 32188
		public int expiration = -1;

		// Token: 0x04007DBD RID: 32189
		public string outSignalFulfilled;

		// Token: 0x04007DBE RID: 32190
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest", true);
	}
}
