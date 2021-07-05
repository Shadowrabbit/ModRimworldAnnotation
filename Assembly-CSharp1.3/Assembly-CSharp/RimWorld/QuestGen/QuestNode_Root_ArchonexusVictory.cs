using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F2 RID: 5874
	public class QuestNode_Root_ArchonexusVictory : QuestNode
	{
		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x0600879C RID: 34716 RVA: 0x00307FA3 File Offset: 0x003061A3
		public Faction CivilOutlander
		{
			get
			{
				return (from f in Find.FactionManager.AllFactionsVisible
				where f.def == FactionDefOf.OutlanderCivil
				select f).FirstOrDefault<Faction>();
			}
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x0600879D RID: 34717 RVA: 0x00307FD8 File Offset: 0x003061D8
		public Faction RoughOutlander
		{
			get
			{
				return (from f in Find.FactionManager.AllFactionsVisible
				where f.def == FactionDefOf.OutlanderRough
				select f).FirstOrDefault<Faction>();
			}
		}

		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x0600879E RID: 34718 RVA: 0x0030800D File Offset: 0x0030620D
		public Faction RoughTribe
		{
			get
			{
				return (from f in Find.FactionManager.AllFactionsVisible
				where f.def == FactionDefOf.TribeRough
				select f).FirstOrDefault<Faction>();
			}
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x00308044 File Offset: 0x00306244
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Archonexus victory"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			quest.AddPart(new QuestPart_SubquestGenerator_ArchonexusVictory
			{
				inSignalEnable = slate.Get<string>("inSignal", null, false),
				interval = new IntRange(0, 0),
				maxSuccessfulSubquests = 3,
				maxActiveSubquests = 1,
				civilOutlander = this.CivilOutlander,
				roughOutlander = this.RoughOutlander,
				roughTribe = this.RoughTribe,
				subquestDefs = 
				{
					QuestScriptDefOf.EndGame_ArchonexusVictory_FirstCycle,
					QuestScriptDefOf.EndGame_ArchonexusVictory_SecondCycle,
					QuestScriptDefOf.EndGame_ArchonexusVictory_ThirdCycle
				}
			});
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}
	}
}
