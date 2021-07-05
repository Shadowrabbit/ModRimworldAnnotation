using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.QuestGen;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200158C RID: 5516
	public static class FactionDialogMaker
	{
		// Token: 0x060077CA RID: 30666 RVA: 0x002473AC File Offset: 0x002455AC
		public static DiaNode FactionDialogFor(Pawn negotiator, Faction faction)
		{
			FactionDialogMaker.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.negotiator = negotiator;
			Map map = CS$<>8__locals1.negotiator.Map;
			Pawn pawn;
			string value;
			if (faction.leader != null)
			{
				pawn = faction.leader;
				value = faction.leader.Name.ToStringFull.Colorize(ColoredText.NameColor);
			}
			else
			{
				Log.Error("Faction " + faction + " has no leader.", false);
				pawn = CS$<>8__locals1.negotiator;
				value = faction.Name;
			}
			if (faction.PlayerRelationKind == FactionRelationKind.Hostile)
			{
				string key;
				if (!faction.def.permanentEnemy && "FactionGreetingHostileAppreciative".CanTranslate())
				{
					key = "FactionGreetingHostileAppreciative";
				}
				else
				{
					key = "FactionGreetingHostile";
				}
				CS$<>8__locals1.root = new DiaNode(key.Translate(value).AdjustedFor(pawn, "PAWN", true));
			}
			else if (faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				CS$<>8__locals1.root = new DiaNode("FactionGreetingWary".Translate(value, CS$<>8__locals1.negotiator.LabelShort, CS$<>8__locals1.negotiator.Named("NEGOTIATOR"), pawn.Named("LEADER")).AdjustedFor(pawn, "PAWN", true));
			}
			else
			{
				CS$<>8__locals1.root = new DiaNode("FactionGreetingWarm".Translate(value, CS$<>8__locals1.negotiator.LabelShort, CS$<>8__locals1.negotiator.Named("NEGOTIATOR"), pawn.Named("LEADER")).AdjustedFor(pawn, "PAWN", true));
			}
			if (map != null && map.IsPlayerHome)
			{
				FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestTraderOption(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestMilitaryAidOption(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				Pawn_RoyaltyTracker royalty = CS$<>8__locals1.negotiator.royalty;
				if (royalty != null && royalty.HasAnyTitleIn(faction))
				{
					foreach (RoyalTitle royalTitle in royalty.AllTitlesInEffectForReading)
					{
						if (royalTitle.def.permits != null)
						{
							foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
							{
								IEnumerable<DiaOption> factionCommDialogOptions = royalTitlePermitDef.Worker.GetFactionCommDialogOptions(map, CS$<>8__locals1.negotiator, faction);
								if (factionCommDialogOptions != null)
								{
									foreach (DiaOption opt in factionCommDialogOptions)
									{
										FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(opt, true, ref CS$<>8__locals1);
									}
								}
							}
						}
					}
					if (royalty.GetCurrentTitle(faction).canBeInherited && !CS$<>8__locals1.negotiator.IsQuestLodger())
					{
						FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestRoyalHeirChangeOption(map, faction, pawn, CS$<>8__locals1.negotiator), false, ref CS$<>8__locals1);
					}
				}
				if (DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef rp) => rp.HasTag(ResearchProjectTagDefOf.ShipRelated) && rp.IsFinished))
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestAICoreQuest(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				}
			}
			if (Prefs.DevMode)
			{
				foreach (DiaOption opt2 in FactionDialogMaker.DebugOptions(faction, CS$<>8__locals1.negotiator))
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(opt2, false, ref CS$<>8__locals1);
				}
			}
			FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(new DiaOption("(" + "Disconnect".Translate() + ")")
			{
				resolveTree = true
			}, false, ref CS$<>8__locals1);
			return CS$<>8__locals1.root;
		}

		// Token: 0x060077CB RID: 30667 RVA: 0x00050C15 File Offset: 0x0004EE15
		private static IEnumerable<DiaOption> DebugOptions(Faction faction, Pawn negotiator)
		{
			yield return new DiaOption("(Debug) Goodwill +10")
			{
				action = delegate()
				{
					faction.TryAffectGoodwillWith(Faction.OfPlayer, 10, false, true, null, null);
				},
				linkLateBind = (() => FactionDialogMaker.FactionDialogFor(negotiator, faction))
			};
			yield return new DiaOption("(Debug) Goodwill -10")
			{
				action = delegate()
				{
					faction.TryAffectGoodwillWith(Faction.OfPlayer, -10, false, true, null, null);
				},
				linkLateBind = (() => FactionDialogMaker.FactionDialogFor(negotiator, faction))
			};
			yield break;
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x0024777C File Offset: 0x0024597C
		private static int AmountSendableSilver(Map map)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount);
		}

		// Token: 0x060077CD RID: 30669 RVA: 0x002477D8 File Offset: 0x002459D8
		private static DiaOption RequestAICoreQuest(Map map, Faction faction, Pawn negotiator)
		{
			TaggedString taggedString = "RequestAICoreInformation".Translate(ThingDefOf.AIPersonaCore.label, 1500.ToString());
			if (faction.PlayerGoodwill < 40)
			{
				DiaOption diaOption = new DiaOption(taggedString);
				diaOption.Disable("NeedGoodwill".Translate(40.ToString("F0")));
				return diaOption;
			}
			bool flag = PlayerItemAccessibilityUtility.ItemStashHas(ThingDefOf.AIPersonaCore);
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(Find.World), false);
			slate.Set<Pawn>("asker", faction.leader, false);
			slate.Set<ThingDef>("itemStashSingleThing", ThingDefOf.AIPersonaCore, false);
			bool flag2 = QuestScriptDefOf.OpportunitySite_ItemStash.CanRun(slate);
			if (flag || !flag2)
			{
				DiaOption diaOption2 = new DiaOption(taggedString);
				diaOption2.Disable("NoKnownAICore".Translate(1500));
				return diaOption2;
			}
			if (FactionDialogMaker.AmountSendableSilver(map) < 1500)
			{
				DiaOption diaOption3 = new DiaOption(taggedString);
				diaOption3.Disable("NeedSilverLaunchable".Translate(1500));
				return diaOption3;
			}
			return new DiaOption(taggedString)
			{
				action = delegate()
				{
					Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.OpportunitySite_ItemStash, slate);
					if (!quest.hidden)
					{
						QuestUtility.SendLetterQuestAvailable(quest);
					}
					TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, 1500, map, null);
					Current.Game.GetComponent<GameComponent_OnetimeNotification>().sendAICoreRequestReminder = false;
				},
				link = new DiaNode("RequestAICoreInformationResult".Translate(faction.leader).CapitalizeFirst())
				{
					options = 
					{
						FactionDialogMaker.OKToRoot(faction, negotiator)
					}
				}
			};
		}

		// Token: 0x060077CE RID: 30670 RVA: 0x00247994 File Offset: 0x00245B94
		private static DiaOption RequestTraderOption(Map map, Faction faction, Pawn negotiator)
		{
			TaggedString taggedString = "RequestTrader".Translate(15);
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(taggedString);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(taggedString);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = faction.lastTraderRequestTick + 240000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(taggedString);
				diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod(true, false, true, true)));
				return diaOption3;
			}
			DiaOption diaOption4 = new DiaOption(taggedString);
			DiaNode diaNode = new DiaNode("TraderSent".Translate(faction.leader).CapitalizeFirst());
			diaNode.options.Add(FactionDialogMaker.OKToRoot(faction, negotiator));
			DiaNode diaNode2 = new DiaNode("ChooseTraderKind".Translate(faction.leader));
			foreach (TraderKindDef localTk2 in from x in faction.def.caravanTraderKinds
			where x.requestable
			select x)
			{
				TraderKindDef localTk = localTk2;
				DiaOption diaOption5 = new DiaOption(localTk.LabelCap);
				if (localTk.TitleRequiredToTrade != null && (negotiator.royalty == null || localTk.TitleRequiredToTrade.seniority > negotiator.GetCurrentTitleSeniorityIn(faction)))
				{
					DiaNode diaNode3 = new DiaNode("TradeCaravanRequestDeniedDueTitle".Translate(negotiator.Named("NEGOTIATOR"), localTk.TitleRequiredToTrade.GetLabelCapFor(negotiator).Named("TITLE"), faction.Named("FACTION")));
					DiaOption diaOption6 = new DiaOption("GoBack".Translate());
					diaNode3.options.Add(diaOption6);
					diaOption5.link = diaNode3;
					diaOption6.link = diaNode2;
				}
				else
				{
					diaOption5.action = delegate()
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = map;
						incidentParms.faction = faction;
						incidentParms.traderKind = localTk;
						incidentParms.forced = true;
						Find.Storyteller.incidentQueue.Add(IncidentDefOf.TraderCaravanArrival, Find.TickManager.TicksGame + 120000, incidentParms, 240000);
						faction.lastTraderRequestTick = Find.TickManager.TicksGame;
						faction.TryAffectGoodwillWith(Faction.OfPlayer, -15, false, true, "GoodwillChangedReason_RequestedTrader".Translate(), null);
					};
					diaOption5.link = diaNode;
				}
				diaNode2.options.Add(diaOption5);
			}
			DiaOption diaOption7 = new DiaOption("GoBack".Translate());
			diaOption7.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode2.options.Add(diaOption7);
			diaOption4.link = diaNode2;
			return diaOption4;
		}

		// Token: 0x060077CF RID: 30671 RVA: 0x00247CF0 File Offset: 0x00245EF0
		private static DiaOption RequestMilitaryAidOption(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestMilitaryAid".Translate(25);
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = faction.lastMilitaryAidRequestTick + 60000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(text);
				diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod(true, false, true, true)));
				return diaOption3;
			}
			if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, faction))
			{
				DiaOption diaOption4 = new DiaOption(text);
				diaOption4.Disable("HostileVisitorsPresent".Translate());
				return diaOption4;
			}
			DiaOption diaOption5 = new DiaOption(text);
			if (faction.def.techLevel < TechLevel.Industrial)
			{
				diaOption5.link = FactionDialogMaker.CantMakeItInTime(faction, negotiator);
			}
			else
			{
				IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
				where GenHostility.IsActiveThreatToPlayer(x)
				select ((Thing)x).Faction into x
				where x != null && !x.HostileTo(faction)
				select x).Distinct<Faction>();
				if (source.Any<Faction>())
				{
					DiaNode diaNode = new DiaNode("MilitaryAidConfirmMutualEnemy".Translate(faction.Name, (from fa in source
					select fa.Name).ToCommaList(true)));
					DiaOption diaOption6 = new DiaOption("CallConfirm".Translate());
					diaOption6.action = delegate()
					{
						FactionDialogMaker.CallForAid(map, faction);
					};
					diaOption6.link = FactionDialogMaker.FightersSent(faction, negotiator);
					DiaOption diaOption7 = new DiaOption("CallCancel".Translate());
					diaOption7.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
					diaNode.options.Add(diaOption6);
					diaNode.options.Add(diaOption7);
					diaOption5.link = diaNode;
				}
				else
				{
					diaOption5.action = delegate()
					{
						FactionDialogMaker.CallForAid(map, faction);
					};
					diaOption5.link = FactionDialogMaker.FightersSent(faction, negotiator);
				}
			}
			return diaOption5;
		}

		// Token: 0x060077D0 RID: 30672 RVA: 0x00247FD0 File Offset: 0x002461D0
		private static DiaOption RequestRoyalHeirChangeOption(Map map, Faction faction, Pawn factionRepresentative, Pawn negotiator)
		{
			RoyalTitleDef currentTitle = negotiator.royalty.GetCurrentTitle(faction);
			Pawn heir = negotiator.royalty.GetHeir(faction);
			DiaOption diaOption = new DiaOption((heir != null) ? "RequestChangeRoyalHeir".Translate(negotiator.Named("HOLDER"), currentTitle.GetLabelCapFor(negotiator).Named("TITLE"), heir.Named("HEIR")) : "RequestSetRoyalHeir".Translate(negotiator.Named("HOLDER"), currentTitle.GetLabelCapFor(negotiator).Named("TITLE")));
			Predicate<QuestPart> <>9__1;
			bool flag = Find.QuestManager.QuestsListForReading.Any(delegate(Quest q)
			{
				if (q.root == QuestScriptDefOf.ChangeRoyalHeir && q.State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = q.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = delegate(QuestPart p)
						{
							QuestPart_ChangeHeir questPart_ChangeHeir = p as QuestPart_ChangeHeir;
							return questPart_ChangeHeir != null && !questPart_ChangeHeir.done && questPart_ChangeHeir.holder == negotiator;
						});
					}
					return partsListForReading.Any(predicate);
				}
				return false;
			});
			diaOption.link = FactionDialogMaker.RoyalHeirChangeCandidates(faction, factionRepresentative, negotiator);
			if (flag)
			{
				diaOption.Disable("RequestChangeRoyalHeirAlreadyInProgress".Translate(negotiator.Named("PAWN")));
			}
			return diaOption;
		}

		// Token: 0x060077D1 RID: 30673 RVA: 0x002480E0 File Offset: 0x002462E0
		public static DiaNode RoyalHeirChangeCandidates(Faction faction, Pawn factionRepresentative, Pawn negotiator)
		{
			DiaNode diaNode = new DiaNode("ChooseHeir".Translate(negotiator.Named("HOLDER")));
			RoyalTitleDef title = negotiator.royalty.GetCurrentTitle(faction);
			Pawn heir2 = negotiator.royalty.GetHeir(faction);
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsAndPrisonersSpawned)
			{
				DiaOption diaOption = new DiaOption(pawn.Name.ToStringFull);
				if (pawn != negotiator && pawn != heir2)
				{
					if (pawn.royalty != null)
					{
						RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
						if (currentTitle != null && currentTitle.seniority >= title.seniority)
						{
							continue;
						}
					}
					if (!pawn.IsQuestLodger())
					{
						Pawn heir = pawn;
						Action confirmedAct = delegate()
						{
							QuestScriptDef changeRoyalHeir = QuestScriptDefOf.ChangeRoyalHeir;
							Slate slate = new Slate();
							slate.Set<int>("points", title.changeHeirQuestPoints, false);
							slate.Set<Pawn>("asker", factionRepresentative, false);
							slate.Set<Pawn>("titleHolder", negotiator, false);
							slate.Set<Pawn>("titleHeir", heir, false);
							slate.Set<Pawn>("titlePreviousHeir", negotiator.royalty.GetHeir(faction), false);
							Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(changeRoyalHeir, slate);
							if (!quest.hidden)
							{
								QuestUtility.SendLetterQuestAvailable(quest);
							}
						};
						diaOption.link = FactionDialogMaker.RoyalHeirChangeConfirm(faction, negotiator, heir2, confirmedAct);
						diaNode.options.Add(diaOption);
					}
				}
			}
			DiaOption diaOption2 = new DiaOption("GoBack".Translate());
			diaOption2.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode.options.Add(diaOption2);
			return diaNode;
		}

		// Token: 0x060077D2 RID: 30674 RVA: 0x002482B0 File Offset: 0x002464B0
		public static DiaNode RoyalHeirChangeConfirm(Faction faction, Pawn negotiator, Pawn currentHeir, Action confirmedAct)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("MakeHeirConfirm".Translate(faction, negotiator.Named("HOLDER")));
			if (currentHeir != null)
			{
				stringBuilder.Append(" " + "MakeHeirPreviousHeirWarning".Translate(negotiator.Named("HOLDER"), currentHeir.Named("HEIR")));
			}
			stringBuilder.Append(" " + "AreYouSure".Translate());
			DiaNode diaNode = new DiaNode(stringBuilder.ToString());
			DiaOption diaOption = new DiaOption("Confirm".Translate());
			diaOption.action = confirmedAct;
			diaOption.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode.options.Add(diaOption);
			DiaOption diaOption2 = new DiaOption("GoBack".Translate());
			diaOption2.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode.options.Add(diaOption2);
			return diaNode;
		}

		// Token: 0x060077D3 RID: 30675 RVA: 0x002483B8 File Offset: 0x002465B8
		public static DiaNode CantMakeItInTime(Faction faction, Pawn negotiator)
		{
			return new DiaNode("CantSendMilitaryAidInTime".Translate(faction.leader).CapitalizeFirst())
			{
				options = 
				{
					FactionDialogMaker.OKToRoot(faction, negotiator)
				}
			};
		}

		// Token: 0x060077D4 RID: 30676 RVA: 0x002483FC File Offset: 0x002465FC
		public static DiaNode FightersSent(Faction faction, Pawn negotiator)
		{
			return new DiaNode("MilitaryAidSent".Translate(faction.leader).CapitalizeFirst())
			{
				options = 
				{
					FactionDialogMaker.OKToRoot(faction, negotiator)
				}
			};
		}

		// Token: 0x060077D5 RID: 30677 RVA: 0x00248440 File Offset: 0x00246640
		private static void CallForAid(Map map, Faction faction)
		{
			faction.TryAffectGoodwillWith(Faction.OfPlayer, -25, false, true, "GoodwillChangedReason_RequestedMilitaryAid".Translate(), null);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = DiplomacyTuning.RequestedMilitaryAidPointsRange.RandomInRange;
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x00050C2C File Offset: 0x0004EE2C
		private static DiaOption OKToRoot(Faction faction, Pawn negotiator)
		{
			return new DiaOption("OK".Translate())
			{
				linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator)
			};
		}

		// Token: 0x060077D7 RID: 30679 RVA: 0x00050C4F File Offset: 0x0004EE4F
		public static Func<DiaNode> ResetToRoot(Faction faction, Pawn negotiator)
		{
			return () => FactionDialogMaker.FactionDialogFor(negotiator, faction);
		}

		// Token: 0x060077D8 RID: 30680 RVA: 0x002484C4 File Offset: 0x002466C4
		[CompilerGenerated]
		internal static void <FactionDialogFor>g__AddAndDecorateOption|0_0(DiaOption opt, bool needsSocial, ref FactionDialogMaker.<>c__DisplayClass0_0 A_2)
		{
			if (needsSocial && A_2.negotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				opt.Disable("WorkTypeDisablesOption".Translate(SkillDefOf.Social.label));
			}
			A_2.root.options.Add(opt);
		}
	}
}
