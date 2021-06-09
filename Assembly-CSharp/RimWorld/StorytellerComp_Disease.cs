using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001225 RID: 4645
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06006588 RID: 25992 RVA: 0x00045733 File Offset: 0x00043933
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x00045740 File Offset: 0x00043940
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!DebugSettings.enableRandomDiseases)
			{
				yield break;
			}
			if (target is World)
			{
				yield break;
			}
			if (Find.Storyteller.difficultyValues.diseaseIntervalFactor >= 100f)
			{
				yield break;
			}
			BiomeDef biome = Find.WorldGrid[target.Tile].biome;
			float num = biome.diseaseMtbDays;
			num *= Find.Storyteller.difficultyValues.diseaseIntervalFactor;
			if (target is Caravan)
			{
				num *= this.CaravanDiseaseMTBFactor;
			}
			if (Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.category, target);
				IncidentDef def;
				if (base.UsableIncidentsInCategory(this.Props.category, parms).TryRandomElementByWeight((IncidentDef d) => biome.CommonalityOfDisease(d), out def))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x00045757 File Offset: 0x00043957
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}

		// Token: 0x04004392 RID: 17298
		private float CaravanDiseaseMTBFactor = 4f;
	}
}
