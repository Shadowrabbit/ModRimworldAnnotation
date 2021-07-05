using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001709 RID: 5897
	public class QuestNode_Root_WorkSite : QuestNode
	{
		// Token: 0x0600882E RID: 34862 RVA: 0x0030E918 File Offset: 0x0030CB18
		private static QuestNode_Root_WorkSite.SiteSpawnCandidate GetSpawnCandidate(int aroundTile)
		{
			List<QuestNode_Root_WorkSite.SiteSpawnCandidate> source = QuestNode_Root_WorkSite.GetCandidates(aroundTile).ToList<QuestNode_Root_WorkSite.SiteSpawnCandidate>();
			SitePartDef campTypeToGenerate = (from tct in source
			select tct.sitePart).Distinct<SitePartDef>().RandomElement<SitePartDef>();
			return (from tct in source
			where tct.sitePart == campTypeToGenerate
			select tct).RandomElementByWeightWithFallback((QuestNode_Root_WorkSite.SiteSpawnCandidate t) => Find.WorldGrid[t.tile].biome.campSelectionWeight, default(QuestNode_Root_WorkSite.SiteSpawnCandidate));
		}

		// Token: 0x0600882F RID: 34863 RVA: 0x0030E9AC File Offset: 0x0030CBAC
		private static IEnumerable<QuestNode_Root_WorkSite.SiteSpawnCandidate> GetCandidates(int aroundTile)
		{
			IEnumerable<SitePartDef> source = from def in DefDatabase<SitePartDef>.AllDefs
			where def.tags != null && def.tags.Contains("WorkSite") && typeof(SitePartWorker_WorkSite).IsAssignableFrom(def.workerClass)
			select def;
			List<int> potentialTiles = QuestNode_Root_WorkSite.PotentialSiteTiles(aroundTile);
			return source.SelectMany(delegate(SitePartDef sitePart)
			{
				SitePartWorker_WorkSite worker = (SitePartWorker_WorkSite)sitePart.Worker;
				return from tile in potentialTiles
				where worker.CanSpawnOn(tile)
				select tile into t
				select new QuestNode_Root_WorkSite.SiteSpawnCandidate
				{
					tile = t,
					sitePart = sitePart
				};
			});
		}

		// Token: 0x06008830 RID: 34864 RVA: 0x0030EA08 File Offset: 0x0030CC08
		public static Site GenerateSite(float points, int aroundTile)
		{
			QuestNode_Root_WorkSite.SiteSpawnCandidate spawnCandidate = QuestNode_Root_WorkSite.GetSpawnCandidate(aroundTile);
			SitePartWorker_WorkSite sitePartWorker = (SitePartWorker_WorkSite)spawnCandidate.sitePart.Worker;
			Faction faction = null;
			QuestNode_Root_WorkSite.FactionType item = QuestNode_Root_WorkSite.FactionChances.RandomElementByWeight((Tuple<float, QuestNode_Root_WorkSite.FactionType> e) => e.Item1).Item2;
			Predicate<Faction> factionValidator = null;
			if (item != QuestNode_Root_WorkSite.FactionType.Enemy)
			{
				if (item == QuestNode_Root_WorkSite.FactionType.AllyOrNeutral)
				{
					factionValidator = ((Faction f) => f.AllyOrNeutralTo(Faction.OfPlayer));
				}
			}
			else
			{
				factionValidator = ((Faction f) => f.HostileTo(Faction.OfPlayer));
			}
			if (factionValidator != null)
			{
				faction = (from f in Find.FactionManager.AllFactionsListForReading
				where base.<GenerateSite>g__FactionUseable|2(f) && factionValidator(f)
				select f).RandomElementWithFallback(null);
			}
			if (faction == null)
			{
				List<FactionRelation> list = new List<FactionRelation>();
				foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
				{
					if (!faction2.def.permanentEnemy)
					{
						if (faction2 == Faction.OfPlayer)
						{
							list.Add(new FactionRelation
							{
								other = faction2,
								kind = FactionRelationKind.Hostile
							});
						}
						else
						{
							list.Add(new FactionRelation
							{
								other = faction2,
								kind = FactionRelationKind.Neutral
							});
						}
					}
				}
				FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms((from def in DefDatabase<FactionDef>.AllDefsListForReading
				where base.<GenerateSite>g__FactionDefUseable|1(def)
				select def).RandomElement<FactionDef>(), default(IdeoGenerationParms), new bool?(true));
				factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, sitePartWorker.DisallowedPrecepts.ToList<PreceptDef>(), null);
				faction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list);
				faction.temporary = true;
				Find.FactionManager.Add(faction);
			}
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(spawnCandidate.sitePart, new SitePartParams
				{
					threatPoints = points
				})
			}, spawnCandidate.tile, faction, false, null);
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x06008831 RID: 34865 RVA: 0x0030EC40 File Offset: 0x0030CE40
		public static List<int> PotentialSiteTiles(int root)
		{
			List<int> tiles = new List<int>();
			Find.WorldFloodFiller.FloodFill(root, (int p) => !Find.World.Impassable(p) && Find.WorldGrid.ApproxDistanceInTiles(p, root) <= 9f, delegate(int p)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(p, root) >= 3f)
				{
					tiles.Add(p);
				}
			}, int.MaxValue, null);
			return tiles;
		}

		// Token: 0x06008832 RID: 34866 RVA: 0x0030EC9C File Offset: 0x0030CE9C
		public static float AppearanceFrequency(Map map)
		{
			float num = 1f;
			float num2 = 0f;
			List<int> list = QuestNode_Root_WorkSite.PotentialSiteTiles(map.Tile);
			if (list.Count == 0)
			{
				return 0f;
			}
			if (QuestNode_Root_WorkSite.GetSpawnCandidate(map.Tile).sitePart == null)
			{
				return 0f;
			}
			foreach (int tileID in list)
			{
				num2 += Find.WorldGrid[tileID].biome.campSelectionWeight;
			}
			num2 /= (float)list.Count;
			num *= num2;
			int num3 = 0;
			foreach (Site site in Find.WorldObjects.Sites)
			{
				if (site.MainSitePartDef.tags != null && site.MainSitePartDef.tags.Contains("WorkSite"))
				{
					num3++;
				}
			}
			num *= QuestNode_Root_WorkSite.ExistingCampsAppearanceFrequencyMultiplier.Evaluate((float)num3);
			int num4 = map.mapPawns.FreeColonists.Count<Pawn>();
			if (num4 <= 1)
			{
				return 0f;
			}
			if (num4 == 2)
			{
				return num / 2f;
			}
			return num;
		}

		// Token: 0x06008833 RID: 34867 RVA: 0x0030EDF4 File Offset: 0x0030CFF4
		public static float BestAppearanceFrequency()
		{
			float num = 0f;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					num = Mathf.Max(num, QuestNode_Root_WorkSite.AppearanceFrequency(map));
				}
			}
			return num;
		}

		// Token: 0x06008834 RID: 34868 RVA: 0x0030EE5C File Offset: 0x0030D05C
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Work site"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestGenUtility.RunAdjustPointsForDistantFight();
			float num = slate.Get<float>("points", 0f, false);
			if (num < 120f)
			{
				num = 120f;
			}
			Map map = (from m in Find.Maps
			where m.IsPlayerHome && QuestNode_Root_WorkSite.GetCandidates(m.Tile).Any<QuestNode_Root_WorkSite.SiteSpawnCandidate>()
			select m).RandomElementByWeight((Map m) => QuestNode_Root_WorkSite.AppearanceFrequency(m));
			slate.Set<Map>("map", map, false);
			Site site = QuestNode_Root_WorkSite.GenerateSite(num, map.Tile);
			quest.SpawnWorldObject(site, null, null);
			quest.ReserveFaction(site.Faction);
			int num2 = 1800000;
			quest.WorldObjectTimeout(site, num2, null, null, false, null, true);
			quest.Delay(num2, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly);
			quest.Message("MessageCampDetected".Translate(site.Named("CAMP"), site.Faction.Named("FACTION")), MessageTypeDefOf.NeutralEvent, false, null, new LookTargets(site), null);
			SitePart sitePart = site.parts[0];
			if (!sitePart.things.NullOrEmpty<Thing>())
			{
				ThingDef def = sitePart.things.First<Thing>().def;
				int num3 = 0;
				foreach (Thing thing in ((IEnumerable<Thing>)sitePart.things))
				{
					if (thing.def == def)
					{
						num3 += thing.stackCount;
					}
				}
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("loot", def.label + " x" + num3)
				});
			}
			slate.Set<Site>("campSite", site, false);
			slate.Set<Faction>("faction", site.Faction, false);
			slate.Set<int>("timeout", num2, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("campSite.AllEnemiesDefeated");
			string inSignalEnabled = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapRemoved");
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
			if (num >= 400f)
			{
				quest.SurpriseReinforcements(inSignalEnabled, site, site.Faction, 0.35f);
			}
			quest.Notify_PlayerRaidedSomeone(null, site, inSignal);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("siteLabel", site.Label)
			});
		}

		// Token: 0x06008835 RID: 34869 RVA: 0x0030F174 File Offset: 0x0030D374
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			if (slate.Get<float>("points", 0f, false) < 120f)
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome && QuestNode_Root_WorkSite.AppearanceFrequency(map) > 0f && QuestNode_Root_WorkSite.GetCandidates(map.Tile).Any<QuestNode_Root_WorkSite.SiteSpawnCandidate>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005618 RID: 22040
		private const string QuestTag = "WorkSite";

		// Token: 0x04005619 RID: 22041
		private const int SpawnRange = 9;

		// Token: 0x0400561A RID: 22042
		private const int MinSpawnDist = 3;

		// Token: 0x0400561B RID: 22043
		private const float MinPointsForSurpriseReinforcements = 400f;

		// Token: 0x0400561C RID: 22044
		private const float SurpriseReinforcementChance = 0.35f;

		// Token: 0x0400561D RID: 22045
		private static readonly SimpleCurve ExistingCampsAppearanceFrequencyMultiplier = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 0.3f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			}
		};

		// Token: 0x0400561E RID: 22046
		private const float MinPoints = 120f;

		// Token: 0x0400561F RID: 22047
		private const string SitePartTag = "WorkSite";

		// Token: 0x04005620 RID: 22048
		private const float TemporaryFactionChance = 0.6f;

		// Token: 0x04005621 RID: 22049
		private const float EnemyFactionChance = 0.25f;

		// Token: 0x04005622 RID: 22050
		private const float AllyFactionChance = 0.15f;

		// Token: 0x04005623 RID: 22051
		private static readonly Tuple<float, QuestNode_Root_WorkSite.FactionType>[] FactionChances = new Tuple<float, QuestNode_Root_WorkSite.FactionType>[]
		{
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.6f, QuestNode_Root_WorkSite.FactionType.Temporary),
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.25f, QuestNode_Root_WorkSite.FactionType.Enemy),
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.15f, QuestNode_Root_WorkSite.FactionType.AllyOrNeutral)
		};

		// Token: 0x02002977 RID: 10615
		private struct SiteSpawnCandidate
		{
			// Token: 0x04009C00 RID: 39936
			public int tile;

			// Token: 0x04009C01 RID: 39937
			public SitePartDef sitePart;
		}

		// Token: 0x02002978 RID: 10616
		private enum FactionType
		{
			// Token: 0x04009C03 RID: 39939
			Temporary,
			// Token: 0x04009C04 RID: 39940
			Enemy,
			// Token: 0x04009C05 RID: 39941
			AllyOrNeutral
		}
	}
}
