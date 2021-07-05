using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x020017CB RID: 6091
	public class PeaceTalks : WorldObject
	{
		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x06008D83 RID: 36227 RVA: 0x0032E04C File Offset: 0x0032C24C
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x06008D84 RID: 36228 RVA: 0x0032E0AC File Offset: 0x0032C2AC
		public void Notify_CaravanArrived(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn == null)
			{
				Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), caravan, MessageTypeDefOf.NegativeEvent, false);
				return;
			}
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(pawn, caravan);
			float num = 1f / badOutcomeWeightFactor;
			PeaceTalks.tmpPossibleOutcomes.Clear();
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Disaster(caravan);
			}, 0.05f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Backfire(caravan);
			}, 0.1f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_TalksFlounder(caravan);
			}, 0.2f));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Success(caravan);
			}, 0.55f * num));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Triumph(caravan);
			}, 0.1f * num));
			PeaceTalks.tmpPossibleOutcomes.RandomElementByWeight((Pair<Action, float> x) => x.Second).First();
			pawn.skills.Learn(SkillDefOf.Social, 6000f, true);
			QuestUtility.SendQuestTargetSignals(this.questTags, "Resolved", this.Named("SUBJECT"));
			this.Destroy();
		}

		// Token: 0x06008D85 RID: 36229 RVA: 0x0032E237 File Offset: 0x0032C437
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitPeaceTalks.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008D86 RID: 36230 RVA: 0x0032E24E File Offset: 0x0032C44E
		private void Outcome_Disaster(Caravan caravan)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				FactionRelationKind playerRelationKind = this.Faction.PlayerRelationKind;
				int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
				Faction.OfPlayer.TryAffectGoodwillWith(this.Faction, Mathf.Min(randomInRange, Faction.OfPlayer.GoodwillToMakeHostile(this.Faction)), false, false, HistoryEventDefOf.PeaceTalksDisaster, null);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, caravan);
				incidentParms.faction = this.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list, false);
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(this.Faction, true, true, false, false, true, false, false), map, list);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				GlobalTargetInfo target = list.Any<Pawn>() ? new GlobalTargetInfo(list[0].Position, map, false) : GlobalTargetInfo.Invalid;
				TaggedString label = "LetterLabelPeaceTalks_Disaster".Translate();
				TaggedString text = this.GetLetterText("LetterPeaceTalks_Disaster".Translate(this.Faction.def.pawnsPlural.CapitalizeFirst(), this.Faction.NameColored, Mathf.RoundToInt((float)randomInRange)), caravan, playerRelationKind, 0);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, target, this.Faction, null, null, null);
			}, "GeneratingMapForNewEncounter", false, null, true);
		}

		// Token: 0x06008D87 RID: 36231 RVA: 0x0032E27C File Offset: 0x0032C47C
		private void Outcome_Backfire(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksBackfireRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksBackfire, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), this.GetLetterText("LetterPeaceTalks_Backfire".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, 0), LetterDefOf.NegativeEvent, caravan, base.Faction, null, null, null);
		}

		// Token: 0x06008D88 RID: 36232 RVA: 0x0032E320 File Offset: 0x0032C520
		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(base.Faction.NameColored), caravan, base.Faction.PlayerRelationKind, 0), LetterDefOf.NeutralEvent, caravan, base.Faction, null, null, null);
		}

		// Token: 0x06008D89 RID: 36233 RVA: 0x0032E38C File Offset: 0x0032C58C
		private void Outcome_Success(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksSuccessRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksSuccess, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), this.GetLetterText("LetterPeaceTalks_Success".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null);
		}

		// Token: 0x06008D8A RID: 36234 RVA: 0x0032E438 File Offset: 0x0032C638
		private void Outcome_Triumph(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksTriumphRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksTriumph, null);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.makingFaction = base.Faction;
			parms.techLevel = new TechLevel?(base.Faction.def.techLevel);
			parms.maxTotalMass = new float?((float)20);
			parms.totalMarketValueRange = new FloatRange?(new FloatRange(500f, 1200f));
			parms.tile = new int?(base.Tile);
			List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				caravan.AddPawnOrItem(list[i], true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), this.GetLetterText("LetterPeaceTalks_Triumph".Translate(base.Faction.NameColored, randomInRange, GenLabel.ThingsLabel(list, "  - ")), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null);
		}

		// Token: 0x06008D8B RID: 36235 RVA: 0x0032E598 File Offset: 0x0032C798
		private int TryGainRoyalFavor(Caravan caravan)
		{
			int num = 0;
			if (base.Faction.def.HasRoyalTitles)
			{
				num = DiplomacyTuning.RoyalFavor_PeaceTalksSuccessRange.RandomInRange;
				Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
				if (pawn != null)
				{
					pawn.royalty.GainFavor(base.Faction, num);
				}
			}
			return num;
		}

		// Token: 0x06008D8C RID: 36236 RVA: 0x0032E5E4 File Offset: 0x0032C7E4
		private string GetLetterText(string baseText, Caravan caravan, FactionRelationKind previousRelationKind, int royalFavorGained = 0)
		{
			TaggedString taggedString = baseText;
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn != null)
			{
				taggedString += "\n\n" + "PeaceTalksSocialXPGain".Translate(pawn.LabelShort, 6000f.ToString("F0"), pawn.Named("PAWN"));
				if (royalFavorGained > 0)
				{
					taggedString += "\n\n" + "PeaceTalksRoyalFavorGain".Translate(pawn.LabelShort, royalFavorGained.ToString(), base.Faction.Named("FACTION"), pawn.Named("PAWN"));
				}
			}
			base.Faction.TryAppendRelationKindChangedInfo(ref taggedString, previousRelationKind, base.Faction.PlayerRelationKind, null);
			return taggedString;
		}

		// Token: 0x06008D8D RID: 36237 RVA: 0x0032E6C0 File Offset: 0x0032C8C0
		private static float GetBadOutcomeWeightFactor(Pawn diplomat, Caravan caravan)
		{
			float statValue = diplomat.GetStatValue(StatDefOf.NegotiationAbility, true);
			float num = 0f;
			if (ModsConfig.IdeologyActive)
			{
				bool flag = false;
				using (List<Pawn>.Enumerator enumerator = caravan.pawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == caravan.Faction.leader)
						{
							flag = true;
							break;
						}
					}
				}
				num = (flag ? -0.05f : 0.05f);
			}
			return PeaceTalks.GetBadOutcomeWeightFactor(statValue) * (1f + num);
		}

		// Token: 0x06008D8E RID: 36238 RVA: 0x0032E758 File Offset: 0x0032C958
		private static float GetBadOutcomeWeightFactor(float negotationAbility)
		{
			return PeaceTalks.BadOutcomeChanceFactorByNegotiationAbility.Evaluate(negotationAbility);
		}

		// Token: 0x06008D8F RID: 36239 RVA: 0x0032E765 File Offset: 0x0032C965
		[DebugOutput("Incidents", false)]
		private static void PeaceTalksChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008D90 RID: 36240 RVA: 0x0032E798 File Offset: 0x0032C998
		private static void AppendDebugChances(StringBuilder sb, float negotiationAbility)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			sb.AppendLine("--- NegotiationAbility = " + negotiationAbility.ToStringPercent() + " ---");
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(negotiationAbility);
			float num = 1f / badOutcomeWeightFactor;
			sb.AppendLine("Bad outcome weight factor: " + badOutcomeWeightFactor.ToString("0.##"));
			float num2 = 0.05f * badOutcomeWeightFactor;
			float num3 = 0.1f * badOutcomeWeightFactor;
			float num4 = 0.2f;
			float num5 = 0.55f * num;
			float num6 = 0.1f * num;
			float num7 = num2 + num3 + num4 + num5 + num6;
			sb.AppendLine("Disaster: " + (num2 / num7).ToStringPercent());
			sb.AppendLine("Backfire: " + (num3 / num7).ToStringPercent());
			sb.AppendLine("Talks flounder: " + (num4 / num7).ToStringPercent());
			sb.AppendLine("Success: " + (num5 / num7).ToStringPercent());
			sb.AppendLine("Triumph: " + (num6 / num7).ToStringPercent());
		}

		// Token: 0x04005991 RID: 22929
		private Material cachedMat;

		// Token: 0x04005992 RID: 22930
		private static readonly SimpleCurve BadOutcomeChanceFactorByNegotiationAbility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 4f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.4f),
				true
			}
		};

		// Token: 0x04005993 RID: 22931
		private const float BaseWeight_Disaster = 0.05f;

		// Token: 0x04005994 RID: 22932
		private const float BaseWeight_Backfire = 0.1f;

		// Token: 0x04005995 RID: 22933
		private const float BaseWeight_TalksFlounder = 0.2f;

		// Token: 0x04005996 RID: 22934
		private const float BaseWeight_Success = 0.55f;

		// Token: 0x04005997 RID: 22935
		private const float BaseWeight_Triumph = 0.1f;

		// Token: 0x04005998 RID: 22936
		public const float LeaderOffset = 0.05f;

		// Token: 0x04005999 RID: 22937
		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();
	}
}
