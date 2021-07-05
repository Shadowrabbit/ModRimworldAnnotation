using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001345 RID: 4933
	public abstract class ITab_Pawn_Visitor : ITab
	{
		// Token: 0x06007782 RID: 30594 RVA: 0x002A04D1 File Offset: 0x0029E6D1
		public ITab_Pawn_Visitor()
		{
			this.size = new Vector2(280f, 0f);
		}

		// Token: 0x06007783 RID: 30595 RVA: 0x002A04F0 File Offset: 0x0029E6F0
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.PrisonerTab, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			bool isPrisonerOfColony = base.SelPawn.IsPrisonerOfColony;
			bool isSlaveOfColony = base.SelPawn.IsSlaveOfColony;
			bool flag = base.SelPawn.IsWildMan();
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.AddRange(from pim in DefDatabase<PrisonerInteractionModeDef>.AllDefs
			orderby pim.listOrder
			select pim);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.maxOneColumn = true;
			listing_Standard.Begin(rect);
			if (!isSlaveOfColony)
			{
				Rect rect2 = listing_Standard.GetRect(28f);
				rect2.width = 140f;
				MedicalCareUtility.MedicalCareSetter(rect2, ref base.SelPawn.playerSettings.medCare);
				listing_Standard.Gap(4f);
			}
			if (isPrisonerOfColony)
			{
				if (!flag)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num = (int)PrisonBreakUtility.InitiatePrisonBreakMtbDays(base.SelPawn, stringBuilder);
					string text = "PrisonBreakMTBDays".Translate() + ": ";
					if (!base.SelPawn.Awake())
					{
						text += "NotWhileAsleep".Translate();
					}
					else if (PrisonBreakUtility.IsPrisonBreaking(base.SelPawn))
					{
						text += "CurrentlyPrisonBreaking".Translate();
					}
					else if (num < 0)
					{
						text += "Never".Translate();
					}
					else
					{
						text += "PeriodDays".Translate(num).ToString().Colorize(ColoredText.DateTimeColor);
					}
					Rect rect3 = listing_Standard.Label(text, -1f, null);
					string text2 = "PrisonBreakMTBDaysDescription".Translate();
					if (stringBuilder.Length > 0)
					{
						text2 = text2 + "\n\n" + stringBuilder.ToString();
					}
					TooltipHandler.TipRegion(rect3, text2);
					Widgets.DrawHighlightIfMouseover(rect3);
					Rect rect4 = listing_Standard.Label("RecruitmentResistance".Translate() + ": " + base.SelPawn.guest.resistance.ToString("F1"), -1f, null);
					if (Mouse.IsOver(rect4))
					{
						TaggedString taggedString = "RecruitmentResistanceDesc".Translate();
						FloatRange value = base.SelPawn.kindDef.initialResistanceRange.Value;
						taggedString += string.Format("\n\n{0}: {1}~{2}", "RecruitmentResistanceFromPawnKind".Translate(base.SelPawn.kindDef.LabelCap), value.min, value.max);
						if (base.SelPawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = base.SelPawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && mostSeniorTitle.def.recruitmentResistanceOffset != 0f)
							{
								string str = (mostSeniorTitle.def.recruitmentResistanceOffset > 0f) ? "+" : "-";
								taggedString += "\n" + "RecruitmentResistanceRoyalTitleOffset".Translate(mostSeniorTitle.Label.CapitalizeFirst()) + (": " + str) + mostSeniorTitle.def.recruitmentResistanceOffset.ToString();
							}
						}
						TooltipHandler.TipRegion(rect4, taggedString);
					}
					Widgets.DrawHighlightIfMouseover(rect4);
					if (ModsConfig.IdeologyActive)
					{
						Rect rect5 = listing_Standard.Label("WillLevel".Translate() + ": " + base.SelPawn.guest.will.ToString("F1"), -1f, null);
						TaggedString taggedString2 = "WillLevelDesc".Translate(2.5f);
						if (!base.SelPawn.guest.EverEnslaved)
						{
							FloatRange value2 = base.SelPawn.kindDef.initialWillRange.Value;
							taggedString2 += string.Format("\n\n{0} : {1}~{2}", "WillFromPawnKind".Translate(base.SelPawn.kindDef.LabelCap), value2.min, value2.max);
						}
						TooltipHandler.TipRegion(rect5, taggedString2);
						Widgets.DrawHighlightIfMouseover(rect5);
					}
				}
				this.DoSlavePriceListing(listing_Standard, base.SelPawn);
				TaggedString t6;
				if (base.SelPawn.Faction == null || base.SelPawn.Faction.IsPlayer || !base.SelPawn.Faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
				{
					t6 = "None".Translate();
				}
				else
				{
					bool flag2;
					bool flag3;
					int i = base.SelPawn.Faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, base.SelPawn.Faction.GetGoodwillGainForPrisonerRelease(base.SelPawn, out flag2, out flag3));
					if (flag2 && !flag3)
					{
						t6 = base.SelPawn.Faction.NameColored + " " + i.ToStringWithSign();
					}
					else if (!flag2)
					{
						t6 = "None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")";
					}
					else if (flag3)
					{
						t6 = "None".Translate() + " (" + base.SelPawn.MentalState.InspectLine + ")";
					}
					else
					{
						t6 = "None".Translate();
					}
				}
				Rect rect6 = listing_Standard.Label("PrisonerReleasePotentialRelationGains".Translate() + ": " + t6, -1f, null);
				TooltipHandler.TipRegionByKey(rect6, "PrisonerReleaseRelationGainsDesc");
				Widgets.DrawHighlightIfMouseover(rect6);
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
				if (ModsConfig.IdeologyActive && isPrisonerOfColony && base.SelPawn.guest.interactionMode == PrisonerInteractionModeDefOf.Convert)
				{
					Rect rect7 = listing_Standard.GetRect(32f);
					Rect rect8 = new Rect(rect7.xMax - 32f - 4f, rect7.y, 32f, 32f);
					rect7.xMax = rect8.xMin;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect7, "IdeoConversionTarget".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.DrawHighlightIfMouseover(rect7);
					TooltipHandler.TipRegionByKey(rect7, "IdeoConversionTargetDesc");
					GUI.color = base.SelPawn.guest.ideoForConversion.Color;
					GUI.DrawTexture(rect8.ContractedBy(2f), base.SelPawn.guest.ideoForConversion.Icon);
					GUI.color = Color.white;
					if (Mouse.IsOver(rect8))
					{
						Widgets.DrawHighlight(rect8);
						TooltipHandler.TipRegion(rect8, base.SelPawn.guest.ideoForConversion.name);
					}
					if (Widgets.ButtonInvisible(rect8, true))
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
						{
							Ideo newIdeo = ideo;
							string text3 = ideo.name;
							Action action = delegate()
							{
								this.SelPawn.guest.ideoForConversion = newIdeo;
							};
							if (!this.ColonyHasAnyWardenOfIdeo(newIdeo, base.SelPawn.MapHeld))
							{
								text3 += " (" + "NoWardenOfIdeo".Translate(newIdeo.memberName.Named("MEMBERNAME")) + ")";
								action = null;
							}
							list.Add(new FloatMenuOption(text3, action, newIdeo.Icon, newIdeo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
				int num2 = 32 * ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Count;
				Rect rect9 = listing_Standard.GetRect((float)num2).Rounded();
				Widgets.DrawMenuSection(rect9);
				Rect position = rect9.ContractedBy(10f);
				GUI.BeginGroup(position);
				Rect rect10 = new Rect(0f, 0f, position.width, 30f);
				foreach (PrisonerInteractionModeDef prisonerInteractionModeDef in ITab_Pawn_Visitor.tmpPrisonerInteractionModes)
				{
					if (!flag || prisonerInteractionModeDef.allowOnWildMan)
					{
						if (Widgets.RadioButtonLabeled(rect10, prisonerInteractionModeDef.LabelCap, base.SelPawn.guest.interactionMode == prisonerInteractionModeDef))
						{
							base.SelPawn.guest.interactionMode = prisonerInteractionModeDef;
							if (prisonerInteractionModeDef == PrisonerInteractionModeDefOf.Execution && base.SelPawn.MapHeld != null && !this.ColonyHasAnyWardenCapableOfViolence(base.SelPawn.MapHeld))
							{
								Messages.Message("MessageCantDoExecutionBecauseNoWardenCapableOfViolence".Translate(), base.SelPawn, MessageTypeDefOf.CautionInput, false);
							}
							if (prisonerInteractionModeDef == PrisonerInteractionModeDefOf.Enslave && base.SelPawn.MapHeld != null && !this.ColonyHasAnyWardenCapableOfEnslavement(base.SelPawn.MapHeld))
							{
								Messages.Message("MessageNoWardenCapableOfEnslavement".Translate(), base.SelPawn, MessageTypeDefOf.CautionInput, false);
							}
							if (prisonerInteractionModeDef == PrisonerInteractionModeDefOf.Convert && base.SelPawn.guest.ideoForConversion == null)
							{
								base.SelPawn.guest.ideoForConversion = Faction.OfPlayer.ideos.PrimaryIdeo;
							}
						}
						if (!prisonerInteractionModeDef.description.NullOrEmpty() && Mouse.IsOver(rect10))
						{
							Widgets.DrawHighlight(rect10);
							TooltipHandler.TipRegion(rect10, prisonerInteractionModeDef.description);
						}
						rect10.y += 28f;
					}
				}
				GUI.EndGroup();
				ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
			}
			if (isSlaveOfColony)
			{
				Need_Suppression need_Suppression = base.SelPawn.needs.TryGetNeed<Need_Suppression>();
				if (need_Suppression != null)
				{
					Rect rect11 = listing_Standard.Label("Suppression".Translate() + ": " + need_Suppression.CurLevel.ToStringPercent(), -1f, null);
					Rect rect12 = listing_Standard.GetRect(30f);
					Rect rect13 = rect12.ContractedBy(7f);
					need_Suppression.DrawSuppressionBar(rect13);
					Rect rect14 = new Rect(rect11.x, rect11.y, rect11.width, rect11.height + rect12.height);
					Widgets.DrawHighlightIfMouseover(rect14);
					TaggedString str2 = "SuppressionDesc".Translate();
					TooltipHandler.TipRegion(rect14, str2);
					float statValue = base.SelPawn.GetStatValue(StatDefOf.SlaveSuppressionFallRate, true);
					string t2 = StatDefOf.SlaveSuppressionFallRate.ValueToString(statValue, ToStringNumberSense.Absolute, true);
					Rect rect15 = listing_Standard.Label("SuppressionFallRate".Translate() + ": " + t2, -1f, null);
					if (Mouse.IsOver(rect15))
					{
						TaggedString t3 = "SuppressionFallRateDesc".Translate(0.2f.ToStringPercent(), 0.3f.ToStringPercent(), 0.1f.ToStringPercent(), 0.15f.ToStringPercent(), 0.15f.ToStringPercent(), 0.05f.ToStringPercent(), 0.15f.ToStringPercent());
						string explanationForTooltip = ((StatWorker_SuppressionFallRate)StatDefOf.SlaveSuppressionFallRate.Worker).GetExplanationForTooltip(StatRequest.For(base.SelPawn));
						TooltipHandler.TipRegion(rect15, t3 + ":\n\n" + explanationForTooltip);
						Widgets.DrawHighlight(rect15);
					}
					Rect rect16 = listing_Standard.Label(string.Format("{0}: {1}", "Terror".Translate(), base.SelPawn.GetStatValue(StatDefOf.Terror, true).ToStringPercent()), -1f, null);
					if (Mouse.IsOver(rect16))
					{
						IOrderedEnumerable<Thought_MemoryObservationTerror> source = from t in TerrorUtility.GetTerrorThoughts(base.SelPawn)
						orderby t.intensity descending
						select t;
						TaggedString taggedString3 = "TerrorDescription".Translate() + ":" + "\n\n" + (from p in TerrorUtility.SuppressionFallRateOverTerror.Points
						select string.Format("- {0} {1}: {2}", "Terror".Translate(), (p.x / 100f).ToStringPercent(), (p.y / 100f).ToStringPercent())).ToLineList(null, false);
						if (source.Count<Thought_MemoryObservationTerror>() > 0)
						{
							string t4 = (from t in source
							select string.Format("{0}: {1}%", t.LabelCap, t.intensity)).ToLineList("- ", true);
							taggedString3 += "\n\n" + "TerrorCurrentThoughts".Translate() + ":" + "\n\n" + t4;
						}
						TooltipHandler.TipRegion(rect16, taggedString3);
						Widgets.DrawHighlight(rect16);
					}
					float num3 = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(base.SelPawn);
					TaggedString taggedString4 = "SlaveRebellionMTBDays".Translate() + ": ";
					if (!base.SelPawn.Awake())
					{
						taggedString4 += "NotWhileAsleep".Translate();
					}
					else if (num3 < 0f)
					{
						taggedString4 += "Never".Translate();
					}
					else
					{
						taggedString4 += ((int)(num3 * 60000f)).ToStringTicksToPeriod(true, false, true, true);
					}
					Rect rect17 = listing_Standard.Label(taggedString4, -1f, null);
					TooltipHandler.TipRegion(rect17, delegate()
					{
						TaggedString taggedString5 = "SlaveRebellionMTBDaysDescription".Translate();
						string slaveRebellionMtbCalculationExplanation = SlaveRebellionUtility.GetSlaveRebellionMtbCalculationExplanation(base.SelPawn);
						if (!slaveRebellionMtbCalculationExplanation.NullOrEmpty())
						{
							taggedString5 += "\n\n" + slaveRebellionMtbCalculationExplanation;
						}
						return taggedString5;
					}, "SlaveRebellionTooltip".GetHashCode());
					Widgets.DrawHighlightIfMouseover(rect17);
					this.DoSlavePriceListing(listing_Standard, base.SelPawn);
					Faction faction = base.SelPawn.SlaveFaction ?? base.SelPawn.Faction;
					TaggedString t5;
					if (faction == null || faction.IsPlayer || !faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
					{
						t5 = "None".Translate();
					}
					else
					{
						bool flag4;
						bool flag5;
						int i2 = faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, faction.GetGoodwillGainForPrisonerRelease(base.SelPawn, out flag4, out flag5));
						if (flag4 && !flag5)
						{
							t5 = faction.NameColored + " " + i2.ToStringWithSign();
						}
						else if (!flag4)
						{
							t5 = "None".Translate() + " (" + "UntendedInjury".Translate().ToLower() + ")";
						}
						else if (flag5)
						{
							t5 = "None".Translate() + " (" + base.SelPawn.MentalState.InspectLine + ")";
						}
						else
						{
							t5 = "None".Translate();
						}
					}
					Rect rect18 = listing_Standard.Label("SlaveReleasePotentialRelationGains".Translate() + ": " + t5, -1f, null);
					TooltipHandler.TipRegionByKey(rect18, "SlaveReleaseRelationGainsDesc");
					Widgets.DrawHighlightIfMouseover(rect18);
					ITab_Pawn_Visitor.tmpSlaveInteractionModes.Clear();
					ITab_Pawn_Visitor.tmpSlaveInteractionModes.AddRange(from pim in DefDatabase<SlaveInteractionModeDef>.AllDefs
					orderby pim.listOrder
					select pim);
					int num4 = 32 * ITab_Pawn_Visitor.tmpSlaveInteractionModes.Count;
					Rect rect19 = listing_Standard.GetRect((float)num4).Rounded();
					Widgets.DrawMenuSection(rect19);
					Rect position2 = rect19.ContractedBy(10f);
					GUI.BeginGroup(position2);
					SlaveInteractionModeDef currentSlaveIteractionMode = base.SelPawn.guest.slaveInteractionMode;
					Rect rect20 = new Rect(0f, 0f, position2.width, 30f);
					Action <>9__8;
					foreach (SlaveInteractionModeDef slaveInteractionModeDef in ITab_Pawn_Visitor.tmpSlaveInteractionModes)
					{
						if (Widgets.RadioButtonLabeled(rect20, slaveInteractionModeDef.LabelCap, base.SelPawn.guest.slaveInteractionMode == slaveInteractionModeDef))
						{
							if (slaveInteractionModeDef == SlaveInteractionModeDefOf.Imprison && RestUtility.FindBedFor(base.SelPawn, base.SelPawn, false, false, new GuestStatus?(GuestStatus.Prisoner)) == null)
							{
								Messages.Message("CannotImprison".Translate() + ": " + "NoPrisonerBed".Translate(), base.SelPawn, MessageTypeDefOf.RejectInput, false);
								continue;
							}
							base.SelPawn.guest.slaveInteractionMode = slaveInteractionModeDef;
							if (slaveInteractionModeDef == SlaveInteractionModeDefOf.Execute && base.SelPawn.SlaveFaction != null && !base.SelPawn.SlaveFaction.HostileTo(Faction.OfPlayer))
							{
								TaggedString text4 = "ExectueNeutralFactionSlave".Translate(base.SelPawn.Named("PAWN"), base.SelPawn.SlaveFaction.Named("FACTION"));
								string buttonAText = "Confirm".Translate();
								Action buttonAAction = delegate()
								{
								};
								string buttonBText = "Cancel".Translate();
								Action buttonBAction;
								if ((buttonBAction = <>9__8) == null)
								{
									buttonBAction = (<>9__8 = delegate()
									{
										this.SelPawn.guest.slaveInteractionMode = currentSlaveIteractionMode;
									});
								}
								Dialog_MessageBox window = new Dialog_MessageBox(text4, buttonAText, buttonAAction, buttonBText, buttonBAction, null, false, null, null);
								Find.WindowStack.Add(window);
							}
						}
						if (!slaveInteractionModeDef.description.NullOrEmpty() && Mouse.IsOver(rect20))
						{
							Widgets.DrawHighlight(rect20);
							string text5 = slaveInteractionModeDef.description;
							if (slaveInteractionModeDef == SlaveInteractionModeDefOf.Emancipate)
							{
								if (base.SelPawn.SlaveFaction == Faction.OfPlayer)
								{
									text5 += " " + "EmancipateCololonistTooltip".Translate();
								}
								else if (base.SelPawn.SlaveFaction != null)
								{
									text5 += " " + "EmancipateNonCololonistWithFactionTooltip".Translate(base.SelPawn.SlaveFaction.NameColored);
								}
								else
								{
									text5 += " " + "EmancipateNonCololonistWithoutFactionTooltip".Translate();
								}
							}
							TooltipHandler.TipRegion(rect20, text5);
						}
						rect20.y += 28f;
					}
					GUI.EndGroup();
					ITab_Pawn_Visitor.tmpSlaveInteractionModes.Clear();
				}
			}
			listing_Standard.End();
			this.size = new Vector2(280f, listing_Standard.CurHeight + 10f + 24f);
		}

		// Token: 0x06007784 RID: 30596 RVA: 0x002A1920 File Offset: 0x0029FB20
		private void DoSlavePriceListing(Listing_Standard listing, Pawn pawn)
		{
			float statValue = base.SelPawn.GetStatValue(StatDefOf.MarketValue, true);
			Rect rect = listing.Label("SlavePrice".Translate() + ": " + statValue.ToStringMoney(null), -1f, null);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TaggedString str = "SlavePriceDescription".Translate("Slave".Translate().ToLower()) + "\n\n" + StatDefOf.MarketValue.Worker.GetExplanationFull(StatRequest.For(base.SelPawn), StatDefOf.MarketValue.toStringNumberSense, statValue);
				TooltipHandler.TipRegion(rect, str);
			}
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x002A19DC File Offset: 0x0029FBDC
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

		// Token: 0x06007786 RID: 30598 RVA: 0x002A1A50 File Offset: 0x0029FC50
		private bool ColonyHasAnyWardenCapableOfEnslavement(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x002A1AD8 File Offset: 0x0029FCD8
		private bool ColonyHasAnyWardenOfIdeo(Ideo ideo, Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && pawn.Ideo == ideo)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400426F RID: 17007
		private const float CheckboxInterval = 30f;

		// Token: 0x04004270 RID: 17008
		private const float CheckboxMargin = 50f;

		// Token: 0x04004271 RID: 17009
		private static List<PrisonerInteractionModeDef> tmpPrisonerInteractionModes = new List<PrisonerInteractionModeDef>();

		// Token: 0x04004272 RID: 17010
		private static List<SlaveInteractionModeDef> tmpSlaveInteractionModes = new List<SlaveInteractionModeDef>();

		// Token: 0x04004273 RID: 17011
		private const float SuppresionBarHeight = 30f;

		// Token: 0x04004274 RID: 17012
		private const float SuppresionBarMargin = 7f;
	}
}
