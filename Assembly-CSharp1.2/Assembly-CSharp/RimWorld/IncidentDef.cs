using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA6 RID: 4006
	public class IncidentDef : Def
	{
		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x060057AA RID: 22442 RVA: 0x0003CC87 File Offset: 0x0003AE87
		public bool NeedsParmsPoints
		{
			get
			{
				return this.category.needsParmsPoints;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x060057AB RID: 22443 RVA: 0x0003CC94 File Offset: 0x0003AE94
		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x001CDC98 File Offset: 0x001CBE98
		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				if (this.refireCheckTags == null)
				{
					return null;
				}
				if (this.cachedRefireCheckIncidents == null)
				{
					this.cachedRefireCheckIncidents = new List<IncidentDef>();
					List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
						{
							this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedRefireCheckIncidents;
			}
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x0003CCC6 File Offset: 0x0003AEC6
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x001CDD00 File Offset: 0x001CBF00
		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			if (other.tags == null)
			{
				return false;
			}
			if (other == this)
			{
				return false;
			}
			for (int i = 0; i < other.tags.Count; i++)
			{
				for (int j = 0; j < this.refireCheckTags.Count; j++)
				{
					if (other.tags[i] == this.refireCheckTags[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x0003CCCF File Offset: 0x0003AECF
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category == null)
			{
				yield return "category is undefined.";
			}
			if (this.targetTags == null || this.targetTags.Count == 0)
			{
				yield return "no target type";
			}
			if (this.TargetTagAllowed(IncidentTargetTagDefOf.World))
			{
				if (this.targetTags.Any((IncidentTargetTagDef tt) => tt != IncidentTargetTagDefOf.World))
				{
					yield return "allows world target type along with other targets. World targeting incidents should only target the world.";
				}
			}
			if (this.TargetTagAllowed(IncidentTargetTagDefOf.World) && this.allowedBiomes != null)
			{
				yield return "world-targeting incident has a biome restriction list";
			}
			yield break;
			yield break;
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x0003CCDF File Offset: 0x0003AEDF
		public bool TargetTagAllowed(IncidentTargetTagDef target)
		{
			return this.targetTags.Contains(target);
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x0003CCED File Offset: 0x0003AEED
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTags.Intersect(target.IncidentTargetTags()).Any<IncidentTargetTagDef>();
		}

		// Token: 0x04003963 RID: 14691
		public Type workerClass;

		// Token: 0x04003964 RID: 14692
		public IncidentCategoryDef category;

		// Token: 0x04003965 RID: 14693
		public List<IncidentTargetTagDef> targetTags;

		// Token: 0x04003966 RID: 14694
		public float baseChance;

		// Token: 0x04003967 RID: 14695
		public float baseChanceWithRoyalty = -1f;

		// Token: 0x04003968 RID: 14696
		public IncidentPopulationEffect populationEffect;

		// Token: 0x04003969 RID: 14697
		public int earliestDay;

		// Token: 0x0400396A RID: 14698
		public int minPopulation;

		// Token: 0x0400396B RID: 14699
		public bool requireColonistsPresent;

		// Token: 0x0400396C RID: 14700
		public float minRefireDays;

		// Token: 0x0400396D RID: 14701
		[Obsolete]
		public int minDifficulty;

		// Token: 0x0400396E RID: 14702
		public DifficultyConditionConfig disabledWhen = new DifficultyConditionConfig();

		// Token: 0x0400396F RID: 14703
		public bool pointsScaleable;

		// Token: 0x04003970 RID: 14704
		public float minThreatPoints = float.MinValue;

		// Token: 0x04003971 RID: 14705
		public List<BiomeDef> allowedBiomes;

		// Token: 0x04003972 RID: 14706
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04003973 RID: 14707
		[NoTranslate]
		public List<string> refireCheckTags;

		// Token: 0x04003974 RID: 14708
		public SimpleCurve chanceFactorByPopulationCurve;

		// Token: 0x04003975 RID: 14709
		public TaleDef tale;

		// Token: 0x04003976 RID: 14710
		public int minGreatestPopulation = -1;

		// Token: 0x04003977 RID: 14711
		[MustTranslate]
		public string letterText;

		// Token: 0x04003978 RID: 14712
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04003979 RID: 14713
		public LetterDef letterDef;

		// Token: 0x0400397A RID: 14714
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x0400397B RID: 14715
		public PawnKindDef pawnKind;

		// Token: 0x0400397C RID: 14716
		public bool pawnMustBeCapableOfViolence;

		// Token: 0x0400397D RID: 14717
		public Gender pawnFixedGender;

		// Token: 0x0400397E RID: 14718
		public HediffDef pawnHediff;

		// Token: 0x0400397F RID: 14719
		public GameConditionDef gameCondition;

		// Token: 0x04003980 RID: 14720
		public FloatRange durationDays;

		// Token: 0x04003981 RID: 14721
		public HediffDef diseaseIncident;

		// Token: 0x04003982 RID: 14722
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		// Token: 0x04003983 RID: 14723
		public int diseaseMaxVictims = 99999;

		// Token: 0x04003984 RID: 14724
		public List<BiomeDiseaseRecord> diseaseBiomeRecords;

		// Token: 0x04003985 RID: 14725
		public List<BodyPartDef> diseasePartsToAffect;

		// Token: 0x04003986 RID: 14726
		public ThingDef mechClusterBuilding;

		// Token: 0x04003987 RID: 14727
		public List<MTBByBiome> mtbDaysByBiome;

		// Token: 0x04003988 RID: 14728
		public QuestScriptDef questScriptDef;

		// Token: 0x04003989 RID: 14729
		[Unsaved(false)]
		private IncidentWorker workerInt;

		// Token: 0x0400398A RID: 14730
		[Unsaved(false)]
		private List<IncidentDef> cachedRefireCheckIncidents;
	}
}
