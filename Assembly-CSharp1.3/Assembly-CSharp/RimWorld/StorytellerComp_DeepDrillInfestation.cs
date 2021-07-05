using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C3C RID: 3132
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x001852B0 File Offset: 0x001834B0
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004986 RID: 18822 RVA: 0x001852C0 File Offset: 0x001834C0
		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				Difficulty difficulty = Find.Storyteller.difficulty;
				if (difficulty.deepDrillInfestationChanceFactor <= 0f)
				{
					return -1f;
				}
				return this.Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
			}
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x001852FD File Offset: 0x001834FD
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

		// Token: 0x04002CAC RID: 11436
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
