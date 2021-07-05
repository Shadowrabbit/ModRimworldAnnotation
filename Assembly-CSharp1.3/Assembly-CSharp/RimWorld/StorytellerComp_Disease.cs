using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C3F RID: 3135
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x00185350 File Offset: 0x00183550
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x0018535D File Offset: 0x0018355D
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
			if (Find.Storyteller.difficulty.diseaseIntervalFactor >= 100f)
			{
				yield break;
			}
			BiomeDef biome = Find.WorldGrid[target.Tile].biome;
			float num = biome.diseaseMtbDays;
			num *= Find.Storyteller.difficulty.diseaseIntervalFactor;
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

		// Token: 0x0600498E RID: 18830 RVA: 0x00185374 File Offset: 0x00183574
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}

		// Token: 0x04002CAF RID: 11439
		private float CaravanDiseaseMTBFactor = 4f;
	}
}
