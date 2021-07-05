using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016FE RID: 5886
	public class QuestNode_Root_Loot_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x060087D6 RID: 34774 RVA: 0x0030AFC8 File Offset: 0x003091C8
		protected override ComplexDef ComplexDef
		{
			get
			{
				return ComplexDefOf.AncientComplex_Loot;
			}
		}

		// Token: 0x060087D7 RID: 34775 RVA: 0x0030AFD0 File Offset: 0x003091D0
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Ancient complex rescue"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Map map = QuestGen_Get.GetMap(false, null);
			float num = slate.Get<float>("points", 0f, false);
			int tile;
			this.TryFindSiteTile(out tile);
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.Destroyed");
			ComplexSketch ancientComplexSketch = this.GenerateSketch(num, false);
			SitePartParams parms = new SitePartParams
			{
				threatPoints = (Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(num) : 0f),
				ancientComplexSketch = ancientComplexSketch,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Better
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.AncientComplex, parms)), tile, Faction.OfAncients, false, null);
			quest.SpawnWorldObject(site, null, null);
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x060087D8 RID: 34776 RVA: 0x00309A7B File Offset: 0x00307C7B
		private bool TryFindSiteTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, TileFinderMode.Near, -1, false);
		}

		// Token: 0x060087D9 RID: 34777 RVA: 0x00309A8A File Offset: 0x00307C8A
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			enemyFaction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			return enemyFaction != null;
		}

		// Token: 0x060087DA RID: 34778 RVA: 0x0030B0F8 File Offset: 0x003092F8
		protected override bool TestRunInt(Slate slate)
		{
			int num;
			Faction faction;
			return this.TryFindSiteTile(out num) && this.TryFindEnemyFaction(out faction);
		}

		// Token: 0x040055DC RID: 21980
		private const int MinDistanceFromColony = 2;

		// Token: 0x040055DD RID: 21981
		private const int MaxDistanceFromColony = 10;
	}
}
