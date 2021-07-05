using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E9 RID: 4329
	public class ThingSetMaker_Techprints : ThingSetMaker
	{
		// Token: 0x0600678E RID: 26510 RVA: 0x0023001C File Offset: 0x0022E21C
		public override float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
		{
			int num = 0;
			bool flag = false;
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (!researchProjectDef.IsFinished && researchProjectDef.PrerequisitesCompleted)
				{
					if (!researchProjectDef.TechprintRequirementMet && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(researchProjectDef.Techprint, researchProjectDef.TechprintCount - researchProjectDef.TechprintsApplied))
					{
						flag = true;
					}
					else
					{
						num++;
					}
				}
			}
			if (!flag)
			{
				return 1f;
			}
			return (float)Mathf.RoundToInt(ThingSetMaker_Techprints.ResearchableProjectsCountToSelectionWeightCurve.Evaluate((float)num));
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x002300BC File Offset: 0x0022E2BC
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			ThingDef thingDef;
			return (parms.countRange == null || parms.countRange.Value.max > 0) && TechprintUtility.TryGetTechprintDefToGenerate(parms.makingFaction, out thingDef, null, (parms.totalMarketValueRange == null) ? float.MaxValue : (parms.totalMarketValueRange.Value.max * this.marketValueFactor));
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x00230128 File Offset: 0x0022E328
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_Techprints.tmpGenerated.Clear();
			ThingDef thingDef3;
			if (parms.countRange != null)
			{
				int num = Mathf.Max(parms.countRange.Value.RandomInRange, 1);
				for (int i = 0; i < num; i++)
				{
					ThingDef thingDef;
					if (!TechprintUtility.TryGetTechprintDefToGenerate(parms.makingFaction, out thingDef, ThingSetMaker_Techprints.tmpGenerated, 3.4028235E+38f))
					{
						break;
					}
					ThingSetMaker_Techprints.tmpGenerated.Add(thingDef);
					outThings.Add(ThingMaker.MakeThing(thingDef, null));
				}
			}
			else if (parms.totalMarketValueRange != null)
			{
				float num2 = parms.totalMarketValueRange.Value.RandomInRange * this.marketValueFactor;
				float num3 = 0f;
				ThingDef thingDef2;
				while (TechprintUtility.TryGetTechprintDefToGenerate(parms.makingFaction, out thingDef2, ThingSetMaker_Techprints.tmpGenerated, num2 - num3) || (!ThingSetMaker_Techprints.tmpGenerated.Any<ThingDef>() && TechprintUtility.TryGetTechprintDefToGenerate(parms.makingFaction, out thingDef2, ThingSetMaker_Techprints.tmpGenerated, 3.4028235E+38f)))
				{
					ThingSetMaker_Techprints.tmpGenerated.Add(thingDef2);
					outThings.Add(ThingMaker.MakeThing(thingDef2, null));
					num3 += thingDef2.BaseMarketValue;
				}
			}
			else if (TechprintUtility.TryGetTechprintDefToGenerate(parms.makingFaction, out thingDef3, ThingSetMaker_Techprints.tmpGenerated, 3.4028235E+38f))
			{
				ThingSetMaker_Techprints.tmpGenerated.Add(thingDef3);
				outThings.Add(ThingMaker.MakeThing(thingDef3, null));
			}
			ThingSetMaker_Techprints.tmpGenerated.Clear();
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x00230286 File Offset: 0x0022E486
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return from x in DefDatabase<ThingDef>.AllDefs
			where x.HasComp(typeof(CompTechprint))
			select x;
		}

		// Token: 0x04003A61 RID: 14945
		private float marketValueFactor = 1f;

		// Token: 0x04003A62 RID: 14946
		private static readonly SimpleCurve ResearchableProjectsCountToSelectionWeightCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 1f),
				true
			},
			{
				new CurvePoint(0f, 5f),
				true
			}
		};

		// Token: 0x04003A63 RID: 14947
		private static List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
