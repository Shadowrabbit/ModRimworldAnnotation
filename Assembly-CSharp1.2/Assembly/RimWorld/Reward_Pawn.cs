using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CF0 RID: 7408
	public class Reward_Pawn : Reward
	{
		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x0600A114 RID: 41236 RVA: 0x0006B3B2 File Offset: 0x000695B2
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.pawn == null)
				{
					yield break;
				}
				foreach (GenUI.AnonymousStackElement anonymousStackElement in QuestPartUtility.GetRewardStackElementsForThings(Gen.YieldSingle<Pawn>(this.pawn), this.detailsHidden))
				{
					yield return anonymousStackElement;
				}
				IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A115 RID: 41237 RVA: 0x0006B3C2 File Offset: 0x000695C2
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.SpaceRefugee, null);
			this.arrivalMode = (Rand.Bool ? Reward_Pawn.ArrivalMode.WalkIn : Reward_Pawn.ArrivalMode.DropPod);
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x0600A116 RID: 41238 RVA: 0x0006B3E9 File Offset: 0x000695E9
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			QuestGen.AddToGeneratedPawns(this.pawn);
			if (!this.pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(this.pawn, PawnDiscardDecideMode.Decide);
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					inSignal = slate.Get<string>("inSignal", null, false),
					Things = Gen.YieldSingle<Pawn>(this.pawn)
				};
			}
			else
			{
				QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
				pawnsArrive.inSignal = slate.Get<string>("inSignal", null, false);
				pawnsArrive.pawns.Add(this.pawn);
				pawnsArrive.arrivalMode = ((this.arrivalMode == Reward_Pawn.ArrivalMode.DropPod) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeWalkIn);
				pawnsArrive.joinPlayer = true;
				pawnsArrive.mapParent = slate.Get<Map>("map", null, false).Parent;
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						pawnsArrive.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						pawnsArrive.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				yield return pawnsArrive;
			}
			yield break;
		}

		// Token: 0x0600A117 RID: 41239 RVA: 0x002F1258 File Offset: 0x002EF458
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Pawn_Caravan".Translate(this.pawn);
			}
			Reward_Pawn.ArrivalMode arrivalMode = this.arrivalMode;
			if (arrivalMode == Reward_Pawn.ArrivalMode.WalkIn)
			{
				return "Reward_Pawn_WalkIn".Translate(this.pawn);
			}
			if (arrivalMode != Reward_Pawn.ArrivalMode.DropPod)
			{
				throw new Exception("Unknown arrival mode: " + this.arrivalMode);
			}
			return "Reward_Pawn_DropPod".Translate(this.pawn);
		}

		// Token: 0x0600A118 RID: 41240 RVA: 0x002F12E8 File Offset: 0x002EF4E8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" (",
				this.pawn.MarketValue.ToStringMoney(null),
				" pawn=",
				this.pawn.ToStringSafe<Pawn>(),
				", arrivalMode=",
				this.arrivalMode,
				")"
			});
		}

		// Token: 0x0600A119 RID: 41241 RVA: 0x0006B41F File Offset: 0x0006961F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Values.Look<Reward_Pawn.ArrivalMode>(ref this.arrivalMode, "arrivalMode", Reward_Pawn.ArrivalMode.WalkIn, false);
			Scribe_Values.Look<bool>(ref this.detailsHidden, "detailsHidden", false, false);
		}

		// Token: 0x04006D55 RID: 27989
		public Pawn pawn;

		// Token: 0x04006D56 RID: 27990
		public Reward_Pawn.ArrivalMode arrivalMode;

		// Token: 0x04006D57 RID: 27991
		public bool detailsHidden;

		// Token: 0x04006D58 RID: 27992
		private const string RootSymbol = "root";

		// Token: 0x02001CF1 RID: 7409
		public enum ArrivalMode
		{
			// Token: 0x04006D5A RID: 27994
			WalkIn,
			// Token: 0x04006D5B RID: 27995
			DropPod
		}
	}
}
