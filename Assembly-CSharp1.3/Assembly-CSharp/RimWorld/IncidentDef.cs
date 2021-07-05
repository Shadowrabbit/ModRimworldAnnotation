using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A7D RID: 2685
	public class IncidentDef : Def
	{
		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x0015B68A File Offset: 0x0015988A
		public bool NeedsParmsPoints
		{
			get
			{
				return this.category.needsParmsPoints;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x0015B697 File Offset: 0x00159897
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

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06004033 RID: 16435 RVA: 0x0015B6CC File Offset: 0x001598CC
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

		// Token: 0x06004034 RID: 16436 RVA: 0x0015B734 File Offset: 0x00159934
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x0015B740 File Offset: 0x00159940
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

		// Token: 0x06004036 RID: 16438 RVA: 0x0015B7AB File Offset: 0x001599AB
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

		// Token: 0x06004037 RID: 16439 RVA: 0x0015B7BB File Offset: 0x001599BB
		public bool TargetTagAllowed(IncidentTargetTagDef target)
		{
			return this.targetTags.Contains(target);
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0015B7C9 File Offset: 0x001599C9
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTags.Intersect(target.IncidentTargetTags()).Any<IncidentTargetTagDef>();
		}

		// Token: 0x0400247F RID: 9343
		public Type workerClass;

		// Token: 0x04002480 RID: 9344
		public IncidentCategoryDef category;

		// Token: 0x04002481 RID: 9345
		public List<IncidentTargetTagDef> targetTags;

		// Token: 0x04002482 RID: 9346
		public float baseChance;

		// Token: 0x04002483 RID: 9347
		public float baseChanceWithRoyalty = -1f;

		// Token: 0x04002484 RID: 9348
		public IncidentPopulationEffect populationEffect;

		// Token: 0x04002485 RID: 9349
		public int earliestDay;

		// Token: 0x04002486 RID: 9350
		public int minPopulation;

		// Token: 0x04002487 RID: 9351
		public bool requireColonistsPresent;

		// Token: 0x04002488 RID: 9352
		public float minRefireDays;

		// Token: 0x04002489 RID: 9353
		[Obsolete]
		public int minDifficulty;

		// Token: 0x0400248A RID: 9354
		public DifficultyConditionConfig disabledWhen = new DifficultyConditionConfig();

		// Token: 0x0400248B RID: 9355
		public bool pointsScaleable;

		// Token: 0x0400248C RID: 9356
		public float minThreatPoints = float.MinValue;

		// Token: 0x0400248D RID: 9357
		public List<BiomeDef> allowedBiomes;

		// Token: 0x0400248E RID: 9358
		[NoTranslate]
		public List<string> tags;

		// Token: 0x0400248F RID: 9359
		[NoTranslate]
		public List<string> refireCheckTags;

		// Token: 0x04002490 RID: 9360
		public SimpleCurve chanceFactorByPopulationCurve;

		// Token: 0x04002491 RID: 9361
		public TaleDef tale;

		// Token: 0x04002492 RID: 9362
		public int minGreatestPopulation = -1;

		// Token: 0x04002493 RID: 9363
		[MustTranslate]
		public string letterText;

		// Token: 0x04002494 RID: 9364
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04002495 RID: 9365
		public LetterDef letterDef;

		// Token: 0x04002496 RID: 9366
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04002497 RID: 9367
		public PawnKindDef pawnKind;

		// Token: 0x04002498 RID: 9368
		public bool pawnMustBeCapableOfViolence;

		// Token: 0x04002499 RID: 9369
		public Gender pawnFixedGender;

		// Token: 0x0400249A RID: 9370
		public HediffDef pawnHediff;

		// Token: 0x0400249B RID: 9371
		public GameConditionDef gameCondition;

		// Token: 0x0400249C RID: 9372
		public FloatRange durationDays;

		// Token: 0x0400249D RID: 9373
		public HediffDef diseaseIncident;

		// Token: 0x0400249E RID: 9374
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		// Token: 0x0400249F RID: 9375
		public int diseaseMaxVictims = 99999;

		// Token: 0x040024A0 RID: 9376
		public List<BiomeDiseaseRecord> diseaseBiomeRecords;

		// Token: 0x040024A1 RID: 9377
		public List<BodyPartDef> diseasePartsToAffect;

		// Token: 0x040024A2 RID: 9378
		public ThingDef mechClusterBuilding;

		// Token: 0x040024A3 RID: 9379
		public List<MTBByBiome> mtbDaysByBiome;

		// Token: 0x040024A4 RID: 9380
		public QuestScriptDef questScriptDef;

		// Token: 0x040024A5 RID: 9381
		[Unsaved(false)]
		private IncidentWorker workerInt;

		// Token: 0x040024A6 RID: 9382
		[Unsaved(false)]
		private List<IncidentDef> cachedRefireCheckIncidents;
	}
}
