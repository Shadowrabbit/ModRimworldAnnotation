using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200149F RID: 5279
	public class Reward_Pawn : Reward
	{
		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06007E36 RID: 32310 RVA: 0x002CB5DF File Offset: 0x002C97DF
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

		// Token: 0x06007E37 RID: 32311 RVA: 0x002CB5EF File Offset: 0x002C97EF
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.SpaceRefugee, null);
			this.arrivalMode = (Rand.Bool ? Reward_Pawn.ArrivalMode.WalkIn : Reward_Pawn.ArrivalMode.DropPod);
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x06007E38 RID: 32312 RVA: 0x002CB616 File Offset: 0x002C9816
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

		// Token: 0x06007E39 RID: 32313 RVA: 0x002CB64C File Offset: 0x002C984C
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

		// Token: 0x06007E3A RID: 32314 RVA: 0x002CB6DC File Offset: 0x002C98DC
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

		// Token: 0x06007E3B RID: 32315 RVA: 0x002CB752 File Offset: 0x002C9952
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Values.Look<Reward_Pawn.ArrivalMode>(ref this.arrivalMode, "arrivalMode", Reward_Pawn.ArrivalMode.WalkIn, false);
			Scribe_Values.Look<bool>(ref this.detailsHidden, "detailsHidden", false, false);
		}

		// Token: 0x04004E9A RID: 20122
		public Pawn pawn;

		// Token: 0x04004E9B RID: 20123
		public Reward_Pawn.ArrivalMode arrivalMode;

		// Token: 0x04004E9C RID: 20124
		public bool detailsHidden;

		// Token: 0x04004E9D RID: 20125
		private const string RootSymbol = "root";

		// Token: 0x02002822 RID: 10274
		public enum ArrivalMode
		{
			// Token: 0x040097B3 RID: 38835
			WalkIn,
			// Token: 0x040097B4 RID: 38836
			DropPod
		}
	}
}
