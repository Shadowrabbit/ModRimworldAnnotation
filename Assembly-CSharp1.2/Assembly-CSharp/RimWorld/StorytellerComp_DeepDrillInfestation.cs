using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001221 RID: 4641
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06006579 RID: 25977 RVA: 0x000456A9 File Offset: 0x000438A9
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x0600657A RID: 25978 RVA: 0x001F688C File Offset: 0x001F4A8C
		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				Difficulty difficultyValues = Find.Storyteller.difficultyValues;
				if (difficultyValues.deepDrillInfestationChanceFactor <= 0f)
				{
					return -1f;
				}
				return this.Props.baseMtbDaysPerDrill / difficultyValues.deepDrillInfestationChanceFactor;
			}
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x000456B6 File Offset: 0x000438B6
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = (Map)target;
			StorytellerComp_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, StorytellerComp_DeepDrillInfestation.tmpDrills);
			if (!StorytellerComp_DeepDrillInfestation.tmpDrills.Any<Thing>())
			{
				yield break;
			}
			float mtb = this.DeepDrillInfestationMTBDaysPerDrill;
			if (mtb < 0f)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < StorytellerComp_DeepDrillInfestation.tmpDrills.Count; i = num + 1)
			{
				if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
				{
					IncidentParms parms = this.GenerateParms(IncidentCategoryDefOf.DeepDrillInfestation, target);
					IncidentDef def;
					if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, parms).TryRandomElement(out def))
					{
						yield return new FiringIncident(def, this, parms);
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04004387 RID: 17287
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
