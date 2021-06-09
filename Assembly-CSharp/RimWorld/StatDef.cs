using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE4 RID: 4068
	public class StatDef : Def
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x060058B6 RID: 22710 RVA: 0x001D08B4 File Offset: 0x001CEAB4
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

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x060058B7 RID: 22711 RVA: 0x0003DA48 File Offset: 0x0003BC48
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

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x060058B8 RID: 22712 RVA: 0x0003DA69 File Offset: 0x0003BC69
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

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x060058B9 RID: 22713 RVA: 0x0003DA85 File Offset: 0x0003BC85
		public string LabelForFullStatListCap
		{
			get
			{
				return this.LabelForFullStatList.CapitalizeFirst(this);
			}
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x0003DA93 File Offset: 0x0003BC93
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

		// Token: 0x060058BB RID: 22715 RVA: 0x0003DAA3 File Offset: 0x0003BCA3
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
		{
			return this.Worker.ValueToString(val, finalized, numberSense);
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x0003DAB3 File Offset: 0x0003BCB3
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x001D0924 File Offset: 0x001CEB24
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x0003DABC File Offset: 0x0003BCBC
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x001D098C File Offset: 0x001CEB8C
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

		// Token: 0x04003AFB RID: 15099
		public StatCategoryDef category;

		// Token: 0x04003AFC RID: 15100
		public Type workerClass = typeof(StatWorker);

		// Token: 0x04003AFD RID: 15101
		public string labelForFullStatList;

		// Token: 0x04003AFE RID: 15102
		public bool forInformationOnly;

		// Token: 0x04003AFF RID: 15103
		public float hideAtValue = -2.1474836E+09f;

		// Token: 0x04003B00 RID: 15104
		public bool alwaysHide;

		// Token: 0x04003B01 RID: 15105
		public bool showNonAbstract = true;

		// Token: 0x04003B02 RID: 15106
		public bool showIfUndefined = true;

		// Token: 0x04003B03 RID: 15107
		public bool showOnPawns = true;

		// Token: 0x04003B04 RID: 15108
		public bool showOnHumanlikes = true;

		// Token: 0x04003B05 RID: 15109
		public bool showOnNonWildManHumanlikes = true;

		// Token: 0x04003B06 RID: 15110
		public bool showOnAnimals = true;

		// Token: 0x04003B07 RID: 15111
		public bool showOnMechanoids = true;

		// Token: 0x04003B08 RID: 15112
		public bool showOnNonWorkTables = true;

		// Token: 0x04003B09 RID: 15113
		public bool showOnDefaultValue = true;

		// Token: 0x04003B0A RID: 15114
		public bool showOnUnhaulables = true;

		// Token: 0x04003B0B RID: 15115
		public bool showOnUntradeables = true;

		// Token: 0x04003B0C RID: 15116
		public List<string> showIfModsLoaded;

		// Token: 0x04003B0D RID: 15117
		public List<HediffDef> showIfHediffsPresent;

		// Token: 0x04003B0E RID: 15118
		public bool neverDisabled;

		// Token: 0x04003B0F RID: 15119
		public bool showZeroBaseValue;

		// Token: 0x04003B10 RID: 15120
		public int displayPriorityInCategory;

		// Token: 0x04003B11 RID: 15121
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		// Token: 0x04003B12 RID: 15122
		public ToStringStyle toStringStyle;

		// Token: 0x04003B13 RID: 15123
		private ToStringStyle? toStringStyleUnfinalized;

		// Token: 0x04003B14 RID: 15124
		[MustTranslate]
		public string formatString;

		// Token: 0x04003B15 RID: 15125
		[MustTranslate]
		public string formatStringUnfinalized;

		// Token: 0x04003B16 RID: 15126
		public bool finalizeEquippedStatOffset = true;

		// Token: 0x04003B17 RID: 15127
		public float defaultBaseValue = 1f;

		// Token: 0x04003B18 RID: 15128
		public List<SkillNeed> skillNeedOffsets;

		// Token: 0x04003B19 RID: 15129
		public float noSkillOffset;

		// Token: 0x04003B1A RID: 15130
		public List<PawnCapacityOffset> capacityOffsets;

		// Token: 0x04003B1B RID: 15131
		public List<StatDef> statFactors;

		// Token: 0x04003B1C RID: 15132
		public bool applyFactorsIfNegative = true;

		// Token: 0x04003B1D RID: 15133
		public List<SkillNeed> skillNeedFactors;

		// Token: 0x04003B1E RID: 15134
		public float noSkillFactor = 1f;

		// Token: 0x04003B1F RID: 15135
		public List<PawnCapacityFactor> capacityFactors;

		// Token: 0x04003B20 RID: 15136
		public SimpleCurve postProcessCurve;

		// Token: 0x04003B21 RID: 15137
		public List<StatDef> postProcessStatFactors;

		// Token: 0x04003B22 RID: 15138
		public float minValue = -9999999f;

		// Token: 0x04003B23 RID: 15139
		public float maxValue = 9999999f;

		// Token: 0x04003B24 RID: 15140
		public float valueIfMissing;

		// Token: 0x04003B25 RID: 15141
		public bool roundValue;

		// Token: 0x04003B26 RID: 15142
		public float roundToFiveOver = float.MaxValue;

		// Token: 0x04003B27 RID: 15143
		public bool minifiedThingInherits;

		// Token: 0x04003B28 RID: 15144
		public bool supressDisabledError;

		// Token: 0x04003B29 RID: 15145
		public bool scenarioRandomizable;

		// Token: 0x04003B2A RID: 15146
		public List<StatPart> parts;

		// Token: 0x04003B2B RID: 15147
		[Unsaved(false)]
		private StatWorker workerInt;
	}
}
