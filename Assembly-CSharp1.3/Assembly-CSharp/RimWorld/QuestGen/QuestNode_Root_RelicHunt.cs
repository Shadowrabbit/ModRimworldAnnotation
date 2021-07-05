using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001703 RID: 5891
	public class QuestNode_Root_RelicHunt : QuestNode
	{
		// Token: 0x06008806 RID: 34822 RVA: 0x0030CB48 File Offset: 0x0030AD48
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Relic hunt rescue"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			float num = slate.Get<float>("points", 0f, false);
			Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			Precept_Relic precept_Relic;
			this.TryGetRandomPlayerRelic(out precept_Relic);
			int tile;
			this.TryFindSiteTile(out tile, false);
			string text = QuestGen.GenerateNewSignal("SubquestsCompleted", true);
			string awakenSecurityThreatsSignal = QuestGen.GenerateNewSignal("AwakenSecurityThreats", true);
			QuestGen.GenerateNewSignal("PostMapAdded", true);
			string text2 = QuestGen.GenerateNewSignal("RelicLostFromMap", true);
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			QuestPart_SubquestGenerator_RelicHunt questPart_SubquestGenerator_RelicHunt = new QuestPart_SubquestGenerator_RelicHunt();
			questPart_SubquestGenerator_RelicHunt.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_SubquestGenerator_RelicHunt.interval = new IntRange(300000, 600000);
			questPart_SubquestGenerator_RelicHunt.relic = precept_Relic;
			questPart_SubquestGenerator_RelicHunt.relicSlateName = "relic";
			questPart_SubquestGenerator_RelicHunt.useMapParentThreatPoints = map.Parent;
			questPart_SubquestGenerator_RelicHunt.expiryInfoPartKey = "RelicInfoFound";
			questPart_SubquestGenerator_RelicHunt.maxSuccessfulSubquests = 5;
			questPart_SubquestGenerator_RelicHunt.subquestDefs.AddRange(this.GetAllSubquests(QuestGen.Root));
			questPart_SubquestGenerator_RelicHunt.outSignalsCompleted.Add(text);
			quest.AddPart(questPart_SubquestGenerator_RelicHunt);
			QuestGenUtility.RunAdjustPointsForDistantFight();
			num = slate.Get<float>("points", 0f, false);
			Thing thing = precept_Relic.GenerateRelic();
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("relicThing.StartedExtractingFromContainer");
			quest.SignalPass(null, inSignal, awakenSecurityThreatsSignal);
			Reward_Items reward_Items = new Reward_Items();
			reward_Items.items.Add(thing);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_Items);
			questPart_Choice.choices.Add(choice);
			float num2 = allowViolentQuests ? num : 0f;
			SitePartParams sitePartParams = new SitePartParams
			{
				points = num2,
				relicThing = thing,
				triggerSecuritySignal = awakenSecurityThreatsSignal,
				relicLostSignal = text2
			};
			if (num2 > 0f)
			{
				sitePartParams.exteriorThreatPoints = QuestNode_Root_RelicHunt.ExteriorThreatPointsOverPoints.Evaluate(num2);
				sitePartParams.interiorThreatPoints = QuestNode_Root_RelicHunt.InteriorThreatPointsOverPoints.Evaluate(num2);
			}
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.AncientAltar, sitePartParams)), tile, Faction.OfAncientsHostile, false, null);
			quest.SpawnWorldObject(site, null, text);
			TaggedString taggedString = "LetterTextRelicFoundLocation".Translate(precept_Relic.Label);
			if (allowViolentQuests)
			{
				taggedString += "\n\n" + "LetterTextRelicFoundSecurityThreats".Translate(180000.ToStringTicksToPeriodVague(true, true));
			}
			Quest quest2 = quest;
			LetterDef relicHuntInstallationFound = LetterDefOf.RelicHuntInstallationFound;
			string inSignal2 = text;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			string label = "LetterLabelRelicFound".Translate(precept_Relic.Label);
			string text3 = taggedString;
			quest2.Letter(relicHuntInstallationFound, inSignal2, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Site>(site), false, text3, null, label, null, null);
			quest.DescriptionPart("RelicHuntFindRelicSite".Translate(thing.Label), quest.AddedSignal, text, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			if (allowViolentQuests)
			{
				quest.DescriptionPart("RelicHuntFoundRelicSite".Translate(), text, null, QuestPart.SignalListenMode.OngoingOnly, null);
				QuestPart_Delay questPart_Delay = new QuestPart_Delay();
				questPart_Delay.delayTicks = 180000;
				questPart_Delay.alertLabel = "AncientAltarThreatsWaking".Translate();
				questPart_Delay.alertExplanation = "AncientAltarThreatsWakingDesc".Translate();
				questPart_Delay.ticksLeftAlertCritical = 2500;
				questPart_Delay.inSignalEnable = text;
				questPart_Delay.alertCulprits.Add(thing);
				questPart_Delay.alertCulprits.Add(site);
				questPart_Delay.isBad = true;
				questPart_Delay.outSignalsCompleted.Add(awakenSecurityThreatsSignal);
				quest.AddPart(questPart_Delay);
				string text4 = QuestGen.GenerateNewSignal("ReTriggerSecurityThreats", true);
				QuestPart_PassWhileActive questPart_PassWhileActive = new QuestPart_PassWhileActive();
				questPart_PassWhileActive.inSignalEnable = awakenSecurityThreatsSignal;
				questPart_PassWhileActive.inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
				questPart_PassWhileActive.outSignal = text4;
				quest.AddPart(questPart_PassWhileActive);
				quest.SignalPass(delegate
				{
					quest.SignalPass(null, null, awakenSecurityThreatsSignal);
					quest.Message("MessageAncientAltarThreatsAlerted".Translate(), MessageTypeDefOf.NegativeEvent, false, null, null, null);
				}, text4, null);
				quest.AnyHostileThreatToPlayer(site, true, delegate
				{
					quest.Message("MessageAncientAltarThreatsWokenUp".Translate(), MessageTypeDefOf.NegativeEvent, false, null, null, null);
				}, null, awakenSecurityThreatsSignal, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}
			else
			{
				quest.DescriptionPart("RelicHuntFoundRelicSitePeaceful".Translate(), text, null, QuestPart.SignalListenMode.OngoingOnly, null);
			}
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("relicThing.Destroyed"), QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, true);
			slate.Set<Ideo>("ideo", primaryIdeo, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Thing>("relicThing", thing, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x06008807 RID: 34823 RVA: 0x0030D063 File Offset: 0x0030B263
		private bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, TileFinderMode.Near, -1, exitOnFirstTileFound);
		}

		// Token: 0x06008808 RID: 34824 RVA: 0x0030D072 File Offset: 0x0030B272
		private bool TryGetRandomPlayerRelic(out Precept_Relic relic)
		{
			return (from p in Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>()
			where p.CanGenerateRelic
			select p).TryRandomElement(out relic);
		}

		// Token: 0x06008809 RID: 34825 RVA: 0x0030D0B4 File Offset: 0x0030B2B4
		private IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
		{
			return from q in DefDatabase<QuestScriptDef>.AllDefs
			where q.epicParent == parent
			select q;
		}

		// Token: 0x0600880A RID: 34826 RVA: 0x0030D0E4 File Offset: 0x0030B2E4
		protected override bool TestRunInt(Slate slate)
		{
			Precept_Relic precept_Relic;
			int num;
			return this.TryGetRandomPlayerRelic(out precept_Relic) && this.TryFindSiteTile(out num, true) && this.GetAllSubquests(QuestGen.Root).Any<QuestScriptDef>();
		}

		// Token: 0x040055F3 RID: 22003
		private const int RelicsInfoRequiredCount = 5;

		// Token: 0x040055F4 RID: 22004
		private const int MinIntervalTicks = 300000;

		// Token: 0x040055F5 RID: 22005
		private const int MaxIntervalTicks = 600000;

		// Token: 0x040055F6 RID: 22006
		private const int MinDistanceFromColony = 2;

		// Token: 0x040055F7 RID: 22007
		private const int MaxDistanceFromColony = 10;

		// Token: 0x040055F8 RID: 22008
		private const int SecurityWakeupDelayTicks = 180000;

		// Token: 0x040055F9 RID: 22009
		private const int SecurityWakupDelayCriticalTicks = 2500;

		// Token: 0x040055FA RID: 22010
		private static readonly SimpleCurve ExteriorThreatPointsOverPoints = new SimpleCurve
		{
			{
				new CurvePoint(0f, 500f),
				true
			},
			{
				new CurvePoint(500f, 500f),
				true
			},
			{
				new CurvePoint(10000f, 10000f),
				true
			}
		};

		// Token: 0x040055FB RID: 22011
		private static readonly SimpleCurve InteriorThreatPointsOverPoints = new SimpleCurve
		{
			{
				new CurvePoint(0f, 300f),
				true
			},
			{
				new CurvePoint(300f, 300f),
				true
			},
			{
				new CurvePoint(10000f, 5000f),
				true
			}
		};
	}
}
