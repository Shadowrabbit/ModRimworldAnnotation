using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F6 RID: 5878
	public class QuestNode_Root_ArchonexusVictory_ThirdCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x060087AE RID: 34734 RVA: 0x00034716 File Offset: 0x00032916
		protected override int ArchonexusCycle
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060087AF RID: 34735 RVA: 0x0030860C File Offset: 0x0030680C
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Faction faction = slate.Get<Faction>("roughTribe", null, false);
			int tile;
			this.TryFindSiteTile(out tile, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally);
			}
			base.TryAddstudyRequirement(quest, 3200, slate);
			quest.Dialog("[questDescriptionBeforeAccepted]", null, quest.AddedSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly);
			quest.DescriptionPart("[questDescriptionBeforeAccepted]", quest.AddedSignal, quest.InitiateSignal, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			quest.DescriptionPart("[questDescriptionAfterAccepted]", quest.InitiateSignal, null, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questAcceptedLetterText]", null, "[questAcceptedLetterLabel]", null, null);
			SitePartParams parms = new SitePartParams
			{
				threatPoints = 0f
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.Archonexus, parms)), tile, Faction.OfAncients, false, null);
			quest.SetSitePartThreatPointsToCurrent(site, SitePartDefOf.Archonexus, this.map.Parent, null, QuestNode_Root_ArchonexusVictory_ThirdCycle.ThreatPointsFactor);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<bool>("factionless", faction == null, false);
		}

		// Token: 0x060087B0 RID: 34736 RVA: 0x0030872E File Offset: 0x0030692E
		private bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 10, 40, false, TileFinderMode.Near, -1, exitOnFirstTileFound);
		}

		// Token: 0x060087B1 RID: 34737 RVA: 0x00308740 File Offset: 0x00306940
		protected override bool TestRunInt(Slate slate)
		{
			int num;
			return base.TestRunInt(slate) && this.TryFindSiteTile(out num, true);
		}

		// Token: 0x040055BB RID: 21947
		private const int MinDistanceFromColony = 10;

		// Token: 0x040055BC RID: 21948
		private const int MaxDistanceFromColony = 40;

		// Token: 0x040055BD RID: 21949
		private static float ThreatPointsFactor = 0.6f;

		// Token: 0x040055BE RID: 21950
		private const int ArchonexusSuperstructureResearchCost = 3200;
	}
}
