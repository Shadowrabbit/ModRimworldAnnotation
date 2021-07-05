using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CB RID: 203
	public class HediffDef : Def
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001E47C File Offset: 0x0001C67C
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001E494 File Offset: 0x0001C694
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

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001E532 File Offset: 0x0001C732
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

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001E550 File Offset: 0x0001C750
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

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001E598 File Offset: 0x0001C798
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

		// Token: 0x060005FA RID: 1530 RVA: 0x0001E5EC File Offset: 0x0001C7EC
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

		// Token: 0x060005FB RID: 1531 RVA: 0x0001E644 File Offset: 0x0001C844
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001E678 File Offset: 0x0001C878
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return string.Format(this.labelNounPretty, this.label, bodyPart.Label);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		public override void ResolveReferences()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].ResolveReferences(this);
				}
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001E6DD File Offset: 0x0001C8DD
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

		// Token: 0x060005FF RID: 1535 RVA: 0x0001E6ED File Offset: 0x0001C8ED
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

		// Token: 0x06000600 RID: 1536 RVA: 0x0001E6FD File Offset: 0x0001C8FD
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}

		// Token: 0x0400042C RID: 1068
		public Type hediffClass = typeof(Hediff);

		// Token: 0x0400042D RID: 1069
		public List<HediffCompProperties> comps;

		// Token: 0x0400042E RID: 1070
		public float initialSeverity = 0.5f;

		// Token: 0x0400042F RID: 1071
		public float lethalSeverity = -1f;

		// Token: 0x04000430 RID: 1072
		public List<HediffStage> stages;

		// Token: 0x04000431 RID: 1073
		public bool tendable;

		// Token: 0x04000432 RID: 1074
		public bool isBad = true;

		// Token: 0x04000433 RID: 1075
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04000434 RID: 1076
		public float chanceToCauseNoPain;

		// Token: 0x04000435 RID: 1077
		public bool makesSickThought;

		// Token: 0x04000436 RID: 1078
		public bool makesAlert = true;

		// Token: 0x04000437 RID: 1079
		public NeedDef causesNeed;

		// Token: 0x04000438 RID: 1080
		public List<NeedDef> disablesNeeds;

		// Token: 0x04000439 RID: 1081
		public float minSeverity;

		// Token: 0x0400043A RID: 1082
		public float maxSeverity = float.MaxValue;

		// Token: 0x0400043B RID: 1083
		public bool scenarioCanAdd;

		// Token: 0x0400043C RID: 1084
		public List<HediffGiver> hediffGivers;

		// Token: 0x0400043D RID: 1085
		public bool cureAllAtOnceIfCuredByItem;

		// Token: 0x0400043E RID: 1086
		public TaleDef taleOnVisible;

		// Token: 0x0400043F RID: 1087
		public bool everCurableByItem = true;

		// Token: 0x04000440 RID: 1088
		public List<string> tags;

		// Token: 0x04000441 RID: 1089
		public bool priceImpact;

		// Token: 0x04000442 RID: 1090
		public float priceOffset;

		// Token: 0x04000443 RID: 1091
		public bool chronic;

		// Token: 0x04000444 RID: 1092
		public bool keepOnBodyPartRestoration;

		// Token: 0x04000445 RID: 1093
		public bool countsAsAddedPartOrImplant;

		// Token: 0x04000446 RID: 1094
		public bool blocksSocialInteraction;

		// Token: 0x04000447 RID: 1095
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x04000448 RID: 1096
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x04000449 RID: 1097
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

		// Token: 0x0400044A RID: 1098
		public bool removeOnQuestLodgers;

		// Token: 0x0400044B RID: 1099
		public bool displayWound;

		// Token: 0x0400044C RID: 1100
		public float? woundAnchorRange;

		// Token: 0x0400044D RID: 1101
		public Color defaultLabelColor = Color.white;

		// Token: 0x0400044E RID: 1102
		public InjuryProps injuryProps;

		// Token: 0x0400044F RID: 1103
		public AddedBodyPartProps addedPartProps;

		// Token: 0x04000450 RID: 1104
		[MustTranslate]
		public string labelNoun;

		// Token: 0x04000451 RID: 1105
		[MustTranslate]
		public string battleStateLabel;

		// Token: 0x04000452 RID: 1106
		[MustTranslate]
		public string labelNounPretty;

		// Token: 0x04000453 RID: 1107
		[MustTranslate]
		public string targetPrefix;

		// Token: 0x04000454 RID: 1108
		private bool alwaysAllowMothballCached;

		// Token: 0x04000455 RID: 1109
		private bool alwaysAllowMothball;

		// Token: 0x04000456 RID: 1110
		private Hediff concreteExampleInt;
	}
}
