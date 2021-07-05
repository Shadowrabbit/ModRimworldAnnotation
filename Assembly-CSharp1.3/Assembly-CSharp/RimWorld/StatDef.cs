using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC4 RID: 2756
	public class StatDef : Def
	{
		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600412B RID: 16683 RVA: 0x0015ED98 File Offset: 0x0015CF98
		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600412C RID: 16684 RVA: 0x0015EE05 File Offset: 0x0015D005
		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				if (this.toStringStyleUnfinalized == null)
				{
					return this.toStringStyle;
				}
				return this.toStringStyleUnfinalized.Value;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600412D RID: 16685 RVA: 0x0015EE26 File Offset: 0x0015D026
		public string LabelForFullStatList
		{
			get
			{
				if (!this.labelForFullStatList.NullOrEmpty())
				{
					return this.labelForFullStatList;
				}
				return this.label;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600412E RID: 16686 RVA: 0x0015EE42 File Offset: 0x0015D042
		public string LabelForFullStatListCap
		{
			get
			{
				return this.LabelForFullStatList.CapitalizeFirst(this);
			}
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0015EE50 File Offset: 0x0015D050
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.capacityFactors != null)
			{
				using (List<PawnCapacityFactor>.Enumerator enumerator2 = this.capacityFactors.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.weight > 1f)
						{
							yield return this.defName + " has activity factor with weight > 1";
						}
					}
				}
				List<PawnCapacityFactor>.Enumerator enumerator2 = default(List<PawnCapacityFactor>.Enumerator);
			}
			if (this.parts != null)
			{
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					foreach (string text2 in this.parts[i].ConfigErrors())
					{
						yield return string.Concat(new string[]
						{
							this.defName,
							" has error in StatPart ",
							this.parts[i].ToString(),
							": ",
							text2
						});
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0015EE60 File Offset: 0x0015D060
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
		{
			return this.Worker.ValueToString(val, finalized, numberSense);
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x0015EE70 File Offset: 0x0015D070
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x0015EE7C File Offset: 0x0015D07C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x0015EEE4 File Offset: 0x0015D0E4
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0015EEF8 File Offset: 0x0015D0F8
		public bool CanShowWithLoadedMods()
		{
			if (!this.showIfModsLoaded.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.showIfModsLoaded.Count; i++)
				{
					if (!ModsConfig.IsActive(this.showIfModsLoaded[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040026B4 RID: 9908
		public StatCategoryDef category;

		// Token: 0x040026B5 RID: 9909
		public Type workerClass = typeof(StatWorker);

		// Token: 0x040026B6 RID: 9910
		public string labelForFullStatList;

		// Token: 0x040026B7 RID: 9911
		public bool forInformationOnly;

		// Token: 0x040026B8 RID: 9912
		public float hideAtValue = -2.1474836E+09f;

		// Token: 0x040026B9 RID: 9913
		public bool alwaysHide;

		// Token: 0x040026BA RID: 9914
		public bool showNonAbstract = true;

		// Token: 0x040026BB RID: 9915
		public bool showIfUndefined = true;

		// Token: 0x040026BC RID: 9916
		public bool showOnPawns = true;

		// Token: 0x040026BD RID: 9917
		public bool showOnHumanlikes = true;

		// Token: 0x040026BE RID: 9918
		public bool showOnNonWildManHumanlikes = true;

		// Token: 0x040026BF RID: 9919
		public bool showOnAnimals = true;

		// Token: 0x040026C0 RID: 9920
		public bool showOnMechanoids = true;

		// Token: 0x040026C1 RID: 9921
		public bool showOnNonWorkTables = true;

		// Token: 0x040026C2 RID: 9922
		public bool showOnDefaultValue = true;

		// Token: 0x040026C3 RID: 9923
		public bool showOnUnhaulables = true;

		// Token: 0x040026C4 RID: 9924
		public bool showOnUntradeables = true;

		// Token: 0x040026C5 RID: 9925
		public List<string> showIfModsLoaded;

		// Token: 0x040026C6 RID: 9926
		public List<HediffDef> showIfHediffsPresent;

		// Token: 0x040026C7 RID: 9927
		public bool neverDisabled;

		// Token: 0x040026C8 RID: 9928
		public bool showZeroBaseValue;

		// Token: 0x040026C9 RID: 9929
		public bool showOnSlavesOnly;

		// Token: 0x040026CA RID: 9930
		public int displayPriorityInCategory;

		// Token: 0x040026CB RID: 9931
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		// Token: 0x040026CC RID: 9932
		public ToStringStyle toStringStyle;

		// Token: 0x040026CD RID: 9933
		private ToStringStyle? toStringStyleUnfinalized;

		// Token: 0x040026CE RID: 9934
		[MustTranslate]
		public string formatString;

		// Token: 0x040026CF RID: 9935
		[MustTranslate]
		public string formatStringUnfinalized;

		// Token: 0x040026D0 RID: 9936
		public bool finalizeEquippedStatOffset = true;

		// Token: 0x040026D1 RID: 9937
		public float defaultBaseValue = 1f;

		// Token: 0x040026D2 RID: 9938
		public List<SkillNeed> skillNeedOffsets;

		// Token: 0x040026D3 RID: 9939
		public float noSkillOffset;

		// Token: 0x040026D4 RID: 9940
		public List<PawnCapacityOffset> capacityOffsets;

		// Token: 0x040026D5 RID: 9941
		public List<StatDef> statFactors;

		// Token: 0x040026D6 RID: 9942
		public bool applyFactorsIfNegative = true;

		// Token: 0x040026D7 RID: 9943
		public List<SkillNeed> skillNeedFactors;

		// Token: 0x040026D8 RID: 9944
		public float noSkillFactor = 1f;

		// Token: 0x040026D9 RID: 9945
		public List<PawnCapacityFactor> capacityFactors;

		// Token: 0x040026DA RID: 9946
		public SimpleCurve postProcessCurve;

		// Token: 0x040026DB RID: 9947
		public List<StatDef> postProcessStatFactors;

		// Token: 0x040026DC RID: 9948
		public float minValue = -9999999f;

		// Token: 0x040026DD RID: 9949
		public float maxValue = 9999999f;

		// Token: 0x040026DE RID: 9950
		public float valueIfMissing;

		// Token: 0x040026DF RID: 9951
		public bool roundValue;

		// Token: 0x040026E0 RID: 9952
		public float roundToFiveOver = float.MaxValue;

		// Token: 0x040026E1 RID: 9953
		public bool minifiedThingInherits;

		// Token: 0x040026E2 RID: 9954
		public bool supressDisabledError;

		// Token: 0x040026E3 RID: 9955
		public bool scenarioRandomizable;

		// Token: 0x040026E4 RID: 9956
		public SkillDef disableIfSkillDisabled;

		// Token: 0x040026E5 RID: 9957
		public List<StatPart> parts;

		// Token: 0x040026E6 RID: 9958
		[Unsaved(false)]
		private StatWorker workerInt;
	}
}
