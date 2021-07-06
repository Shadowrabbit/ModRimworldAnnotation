using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B07 RID: 6919
	public abstract class ITab_Pawn_Visitor : ITab
	{
		// Token: 0x06009862 RID: 39010 RVA: 0x00065873 File Offset: 0x00063A73
		public ITab_Pawn_Visitor()
		{
			this.size = new Vector2(280f, 0f);
		}

		// Token: 0x06009863 RID: 39011 RVA: 0x002CC898 File Offset: 0x002CAA98
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.PrisonerTab, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			bool isPrisonerOfColony = base.SelPawn.IsPrisonerOfColony;
			bool flag = base.SelPawn.IsWildMan();
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.maxOneColumn = true;
			listing_Standard.Begin(rect);
			Rect rect2 = listing_Standard.GetRect(28f);
			rect2.width = 140f;
			MedicalCareUtility.MedicalCareSetter(rect2, ref base.SelPawn.playerSettings.medCare);
			listing_Standard.Gap(4f);
			if (isPrisonerOfColony)
			{
				if (!flag)
				{
					Rect rect3 = listing_Standard.Label("RecruitmentDifficulty".Translate() + ": " + base.SelPawn.RecruitDifficulty(Faction.OfPlayer).ToStringPercent(), -1f, null);
					if (base.SelPawn.royalty != null)
					{
						RoyalTitle title = base.SelPawn.royalty.MostSeniorTitle;
						if (title != null && Mouse.IsOver(rect3))
						{
							string valueString = title.def.recruitmentDifficultyOffset.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Offset);
							TooltipHandler.TipRegion(rect3, () => "RecruitmentValueOffsetRoyal".Translate() + " (" + title.def.GetLabelCapFor(this.SelPawn) + ")" + ": " + valueString, 947584751);
							Widgets.DrawHighlight(rect3);
						}
					}
					string value = RecruitUtility.RecruitChanceFactorForRecruitDifficulty(base.SelPawn, Faction.OfPlayer).ToStringPercent();
					string value2 = RecruitUtility.RecruitChanceFactorForMood(base.SelPawn).ToStringPercent();
					string text = base.SelPawn.RecruitChanceFinalByFaction(Faction.OfPlayer).ToStringPercent();
					Rect rect4 = listing_Standard.Label("RecruitmentChance".Translate() + ": " + text, -1f, null);
					if (Mouse.IsOver(rect4))
					{
						string recruitmentChanceTooltip = null;
						recruitmentChanceTooltip = "RecruitmentChanceExplanation".Translate(value, value2, text);
						if (!base.SelPawn.guest.lastRecruiterName.NullOrEmpty())
						{
							recruitmentChanceTooltip += "RecruitmentChanceWithLastRecruiterExplanationPart".Translate().Formatted(new NamedArgument[]
							{
								value,
								value2,
								text,
								base.SelPawn.guest.lastRecruiterName,
								base.SelPawn.guest.lastRecruiterNegotiationAbilityFactor.ToStringPercent(),
								base.SelPawn.guest.lastRecruiterOpinionChanceFactor.ToStringPercent(),
								base.SelPawn.guest.hasOpinionOfLastRecruiter ? base.SelPawn.guest.lastRecruiterOpinion.ToStringWithSign() : "-",
								base.SelPawn.guest.lastRecruiterFinalChance.ToStringPercent(),
								base.SelPawn.guest.lastRecruiterResistanceReduce.ToString("0.0")
							});
							if (base.SelPawn.guest.lastRecruiterResistanceReduce > 0f)
							{
								recruitmentChanceTooltip += "RecruitmentLastRecruiterResistanceReduceExplanationPart".Translate(base.SelPawn.guest.lastRecruiterResistanceReduce.ToString("0.0"));
							}
						}
						TooltipHandler.TipRegion(rect4, () => recruitmentChanceTooltip, 947584753);
					}
					Widgets.DrawHighlightIfMouseover(rect4);
					Rect rect5 = listing_Standard.Label("RecruitmentResistance".Translate() + ": " + base.SelPawn.guest.resistance.ToString("F1"), -1f, null);
					if (base.SelPawn.royalty != null)
					{
						RoyalTitle title = base.SelPawn.royalty.MostSeniorTitle;
						if (title != null && Mouse.IsOver(rect5))
						{
							TooltipHandler.TipRegion(rect5, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								if (title.def.recruitmentResistanceOffset != 1f)
								{
									stringBuilder.AppendLine("RecruitmentValueFactorRoyal".Translate() + " (" + title.def.GetLabelCapFor(this.SelPawn) + ")" + ": " + title.def.recruitmentResistanceFactor.ToStringPercent());
								}
								if (title.def.recruitmentResistanceOffset != 0f)
								{
									string t2 = title.def.recruitmentDifficultyOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Offset);
									stringBuilder.AppendLine("RecruitmentValueOffsetRoyal".Translate() + " (" + title.def.GetLabelCapFor(this.SelPawn) + ")" + ": " + t2);
								}
								return stringBuilder.ToString().TrimEndNewlines();
							}, 947584755);
							Widgets.DrawHighlight(rect5);
						}
					}
				}
				listing_Standard.Label("SlavePrice".Translate() + ": " + base.SelPawn.GetStatValue(StatDefOf.MarketValue, true).ToStringMoney(null), -1f, null);
				TaggedString t;
				if (base.SelPawn.Faction == null || base.SelPawn.Faction.IsPlayer || !base.SelPawn.Faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
				{
					t = "None".Translate();
				}
				else
				{
					bool flag2;
					int goodwillGainForPrisonerRelease = base.SelPawn.Faction.GetGoodwillGainForPrisonerRelease(base.SelPawn, out flag2);
					if (flag2)
					{
						t = base.SelPawn.Faction.NameColored + " " + goodwillGainForPrisonerRelease.ToStringWithSign();
					}
					else
					{
						t = "None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")";
					}
				}
				TooltipHandler.TipRegionByKey(listing_Standard.Label("PrisonerReleasePotentialRelationGains".Translate() + ": " + t, -1f, null), "PrisonerReleaseRelationGainsDesc");
				if (base.SelPawn.guilt.IsGuilty)
				{
					if (!base.SelPawn.InAggroMentalState)
					{
						listing_Standard.Label("ConsideredGuilty".Translate(base.SelPawn.guilt.TicksUntilInnocent.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor)), -1f, null);
					}
					else
					{
						listing_Standard.Label("ConsideredGuiltyNoTimer".Translate() + " (" + base.SelPawn.MentalStateDef.label + ")", -1f, null);
					}
				}
				int num = (int)PrisonBreakUtility.InitiatePrisonBreakMtbDays(base.SelPawn);
				string text2 = "PrisonBreakMTBDays".Translate() + ": ";
				if (!base.SelPawn.Awake())
				{
					text2 += "NotWhileAsleep".Translate();
				}
				else if (num < 0)
				{
					text2 += "Never".Translate();
				}
				else
				{
					text2 += "PeriodDays".Translate(num).ToString().Colorize(ColoredText.DateTimeColor);
				}
				TooltipHandler.TipRegionByKey(listing_Standard.Label(text2, -1f, null), "PrisonBreakMTBDaysDescription");
				Rect rect6 = listing_Standard.GetRect(160f).Rounded();
				Widgets.DrawMenuSection(rect6);
				Rect position = rect6.ContractedBy(10f);
				GUI.BeginGroup(position);
				Rect rect7 = new Rect(0f, 0f, position.width, 30f);
				foreach (PrisonerInteractionModeDef prisonerInteractionModeDef in from pim in DefDatabase<PrisonerInteractionModeDef>.AllDefs
				orderby pim.listOrder
				select pim)
				{
					if (!flag || prisonerInteractionModeDef.allowOnWildMan)
					{
						if (Widgets.RadioButtonLabeled(rect7, prisonerInteractionModeDef.LabelCap, base.SelPawn.guest.interactionMode == prisonerInteractionModeDef))
						{
							base.SelPawn.guest.interactionMode = prisonerInteractionModeDef;
							if (prisonerInteractionModeDef == PrisonerInteractionModeDefOf.Execution && base.SelPawn.MapHeld != null && !this.ColonyHasAnyWardenCapableOfViolence(base.SelPawn.MapHeld))
							{
								Messages.Message("MessageCantDoExecutionBecauseNoWardenCapableOfViolence".Translate(), base.SelPawn, MessageTypeDefOf.CautionInput, false);
							}
						}
						rect7.y += 28f;
					}
				}
				GUI.EndGroup();
			}
			listing_Standard.End();
			this.size = new Vector2(280f, listing_Standard.CurHeight + 10f + 24f);
		}

		// Token: 0x06009864 RID: 39012 RVA: 0x002CD168 File Offset: 0x002CB368
		private bool ColonyHasAnyWardenCapableOfViolence(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && !pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400615D RID: 24925
		private const float CheckboxInterval = 30f;

		// Token: 0x0400615E RID: 24926
		private const float CheckboxMargin = 50f;
	}
}
