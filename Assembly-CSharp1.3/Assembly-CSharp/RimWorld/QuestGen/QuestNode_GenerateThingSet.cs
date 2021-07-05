using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001652 RID: 5714
	public class QuestNode_GenerateThingSet : QuestNode
	{
		// Token: 0x0600855E RID: 34142 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600855F RID: 34143 RVA: 0x002FE290 File Offset: 0x002FC490
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = this.totalMarketValueRange.GetValue(slate);
			Thing value = this.factionOf.GetValue(slate);
			parms.makingFaction = ((value == null) ? null : value.Faction);
			parms.qualityGenerator = this.qualityGenerator.GetValue(slate);
			List<Thing> list = this.thingSetMaker.GetValue(slate).root.Generate(parms);
			QuestGen.slate.Set<List<Thing>>(this.storeAs.GetValue(slate), list, false);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
		}

		// Token: 0x0400533C RID: 21308
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400533D RID: 21309
		public SlateRef<ThingSetMakerDef> thingSetMaker;

		// Token: 0x0400533E RID: 21310
		public SlateRef<FloatRange?> totalMarketValueRange;

		// Token: 0x0400533F RID: 21311
		public SlateRef<Thing> factionOf;

		// Token: 0x04005340 RID: 21312
		public SlateRef<QualityGenerator?> qualityGenerator;
	}
}
