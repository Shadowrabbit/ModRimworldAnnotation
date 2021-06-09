using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F0D RID: 7949
	public class QuestNode_GenerateThingSet : QuestNode
	{
		// Token: 0x0600AA28 RID: 43560 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA29 RID: 43561 RVA: 0x0031AF18 File Offset: 0x00319118
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

		// Token: 0x0400738C RID: 29580
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400738D RID: 29581
		public SlateRef<ThingSetMakerDef> thingSetMaker;

		// Token: 0x0400738E RID: 29582
		public SlateRef<FloatRange?> totalMarketValueRange;

		// Token: 0x0400738F RID: 29583
		public SlateRef<Thing> factionOf;

		// Token: 0x04007390 RID: 29584
		public SlateRef<QualityGenerator?> qualityGenerator;
	}
}
