using System;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB0 RID: 2992
	public class QuestPart_SubquestGenerator_ArchonexusVictory : QuestPart_SubquestGenerator
	{
		// Token: 0x060045D4 RID: 17876 RVA: 0x00171E2F File Offset: 0x0017002F
		protected override Slate InitSlate()
		{
			Slate slate = new Slate();
			slate.Set<Faction>("civilOutlander", this.civilOutlander, false);
			slate.Set<Faction>("roughTribe", this.roughTribe, false);
			slate.Set<Faction>("roughOutlander", this.roughOutlander, false);
			return slate;
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x00171E6C File Offset: 0x0017006C
		protected override QuestScriptDef GetNextSubquestDef()
		{
			int index = this.quest.GetSubquests(new QuestState?(QuestState.EndedSuccess)).Count<Quest>() % this.subquestDefs.Count;
			QuestScriptDef questScriptDef = this.subquestDefs[index];
			if (!questScriptDef.CanRun(this.InitSlate()))
			{
				return null;
			}
			return questScriptDef;
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x00171EBA File Offset: 0x001700BA
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.civilOutlander == faction)
			{
				this.civilOutlander = null;
			}
			if (this.roughTribe == faction)
			{
				this.roughTribe = null;
			}
			if (this.roughOutlander == faction)
			{
				this.roughOutlander = null;
			}
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x00171EEC File Offset: 0x001700EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.civilOutlander, "civilOutlander", false);
			Scribe_References.Look<Faction>(ref this.roughTribe, "roughTribe", false);
			Scribe_References.Look<Faction>(ref this.roughOutlander, "roughOutlander", false);
		}

		// Token: 0x04002A8A RID: 10890
		public Faction civilOutlander;

		// Token: 0x04002A8B RID: 10891
		public Faction roughTribe;

		// Token: 0x04002A8C RID: 10892
		public Faction roughOutlander;
	}
}
