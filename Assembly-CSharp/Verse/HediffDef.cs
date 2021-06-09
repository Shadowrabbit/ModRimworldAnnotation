using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000138 RID: 312
	public class HediffDef : Def
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0000C940 File Offset: 0x0000AB40
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0009510C File Offset: 0x0009330C
		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0000C957 File Offset: 0x0000AB57
		public Hediff ConcreteExample
		{
			get
			{
				if (this.concreteExampleInt == null)
				{
					this.concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this);
				}
				return this.concreteExampleInt;
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000951AC File Offset: 0x000933AC
		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000951F4 File Offset: 0x000933F4
		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00095248 File Offset: 0x00093448
		public T CompProps<T>() where T : HediffCompProperties
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000952A0 File Offset: 0x000934A0
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0000C973 File Offset: 0x0000AB73
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return string.Format(this.labelNounPretty, this.label, bodyPart.Label);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0000C99B File Offset: 0x0000AB9B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
			}
			if (string.IsNullOrEmpty(this.description))
			{
				yield return "Hediff with defName " + this.defName + " has no description!";
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string arg in this.comps[i].ConfigErrors(this))
					{
						yield return this.comps[i] + ": " + arg;
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.stages != null)
			{
				int num;
				if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					for (int i = 0; i < this.stages.Count; i = num + 1)
					{
						if (i >= 1 && this.stages[i].minSeverity <= this.stages[i - 1].minSeverity)
						{
							yield return "stages are not in order of minSeverity";
						}
						num = i;
					}
				}
				for (int i = 0; i < this.stages.Count; i = num + 1)
				{
					if (this.stages[i].makeImmuneTo != null)
					{
						if (!this.stages[i].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
						{
							yield return "makes immune to hediff which doesn't have comp immunizable";
						}
					}
					if (this.stages[i].hediffGivers != null)
					{
						for (int j = 0; j < this.stages[i].hediffGivers.Count; j = num + 1)
						{
							foreach (string text2 in this.stages[i].hediffGivers[j].ConfigErrors())
							{
								yield return text2;
							}
							enumerator = null;
							num = j;
						}
					}
					num = i;
				}
			}
			if (this.hediffGivers != null)
			{
				int num;
				for (int i = 0; i < this.hediffGivers.Count; i = num + 1)
				{
					foreach (string text3 in this.hediffGivers[i].ConfigErrors())
					{
						yield return text3;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0000C9AB File Offset: 0x0000ABAB
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry statDrawEntry in this.stages[0].SpecialDisplayStats())
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0000C9BB File Offset: 0x0000ABBB
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}

		// Token: 0x0400061A RID: 1562
		public Type hediffClass = typeof(Hediff);

		// Token: 0x0400061B RID: 1563
		public List<HediffCompProperties> comps;

		// Token: 0x0400061C RID: 1564
		public float initialSeverity = 0.5f;

		// Token: 0x0400061D RID: 1565
		public float lethalSeverity = -1f;

		// Token: 0x0400061E RID: 1566
		public List<HediffStage> stages;

		// Token: 0x0400061F RID: 1567
		public bool tendable;

		// Token: 0x04000620 RID: 1568
		public bool isBad = true;

		// Token: 0x04000621 RID: 1569
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04000622 RID: 1570
		public float chanceToCauseNoPain;

		// Token: 0x04000623 RID: 1571
		public bool makesSickThought;

		// Token: 0x04000624 RID: 1572
		public bool makesAlert = true;

		// Token: 0x04000625 RID: 1573
		public NeedDef causesNeed;

		// Token: 0x04000626 RID: 1574
		public NeedDef disablesNeed;

		// Token: 0x04000627 RID: 1575
		public float minSeverity;

		// Token: 0x04000628 RID: 1576
		public float maxSeverity = float.MaxValue;

		// Token: 0x04000629 RID: 1577
		public bool scenarioCanAdd;

		// Token: 0x0400062A RID: 1578
		public List<HediffGiver> hediffGivers;

		// Token: 0x0400062B RID: 1579
		public bool cureAllAtOnceIfCuredByItem;

		// Token: 0x0400062C RID: 1580
		public TaleDef taleOnVisible;

		// Token: 0x0400062D RID: 1581
		public bool everCurableByItem = true;

		// Token: 0x0400062E RID: 1582
		public string battleStateLabel;

		// Token: 0x0400062F RID: 1583
		public string labelNounPretty;

		// Token: 0x04000630 RID: 1584
		public string targetPrefix;

		// Token: 0x04000631 RID: 1585
		public List<string> tags;

		// Token: 0x04000632 RID: 1586
		public bool priceImpact;

		// Token: 0x04000633 RID: 1587
		public float priceOffset;

		// Token: 0x04000634 RID: 1588
		public bool chronic;

		// Token: 0x04000635 RID: 1589
		public bool keepOnBodyPartRestoration;

		// Token: 0x04000636 RID: 1590
		public bool countsAsAddedPartOrImplant;

		// Token: 0x04000637 RID: 1591
		public SimpleCurve removeOnRedressChanceByDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x04000638 RID: 1592
		public bool removeOnQuestLodgers;

		// Token: 0x04000639 RID: 1593
		public bool displayWound;

		// Token: 0x0400063A RID: 1594
		public Color defaultLabelColor = Color.white;

		// Token: 0x0400063B RID: 1595
		public InjuryProps injuryProps;

		// Token: 0x0400063C RID: 1596
		public AddedBodyPartProps addedPartProps;

		// Token: 0x0400063D RID: 1597
		[MustTranslate]
		public string labelNoun;

		// Token: 0x0400063E RID: 1598
		private bool alwaysAllowMothballCached;

		// Token: 0x0400063F RID: 1599
		private bool alwaysAllowMothball;

		// Token: 0x04000640 RID: 1600
		private Hediff concreteExampleInt;
	}
}
