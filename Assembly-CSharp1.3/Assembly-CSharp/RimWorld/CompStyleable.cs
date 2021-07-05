using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AE RID: 4526
	public class CompStyleable : ThingComp
	{
		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06006D03 RID: 27907 RVA: 0x002493CC File Offset: 0x002475CC
		public StyleCategoryDef StyleCategoryDef
		{
			get
			{
				if (this.cachedStyleCategoryDef == null)
				{
					bool flag = false;
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						for (int i = 0; i < styleCategoryDef.thingDefStyles.Count; i++)
						{
							if (styleCategoryDef.thingDefStyles[i].StyleDef == this.styleDef && styleCategoryDef.thingDefStyles[i].ThingDef == this.parent.def)
							{
								this.cachedStyleCategoryDef = styleCategoryDef;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				return this.cachedStyleCategoryDef;
			}
		}

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x06006D04 RID: 27908 RVA: 0x00249484 File Offset: 0x00247684
		public CompProperties_Styleable Props
		{
			get
			{
				return (CompProperties_Styleable)this.props;
			}
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x06006D05 RID: 27909 RVA: 0x00249491 File Offset: 0x00247691
		// (set) Token: 0x06006D06 RID: 27910 RVA: 0x0024949C File Offset: 0x0024769C
		public Precept_ThingStyle SourcePrecept
		{
			get
			{
				return this.sourcePrecept;
			}
			set
			{
				if (this.sourcePrecept == value)
				{
					return;
				}
				this.sourcePrecept = value;
				if (this.parent.def.randomStyleChance > 0f)
				{
					return;
				}
				BuildableDef buildableDef = this.parent.def.IsBlueprint ? this.parent.def.entityDefToBuild : this.parent.def;
				StyleCategoryPair styleCategoryPair = this.sourcePrecept.ideo.style.StyleForThingDef((ThingDef)buildableDef, this.sourcePrecept);
				this.cachedStyleCategoryDef = ((styleCategoryPair != null) ? styleCategoryPair.category : null);
				if (this.cachedStyleCategoryDef != null)
				{
					this.styleDef = styleCategoryPair.styleDef;
				}
			}
		}

		// Token: 0x06006D07 RID: 27911 RVA: 0x0024954A File Offset: 0x0024774A
		public override string TransformLabel(string label)
		{
			if (this.sourcePrecept != null)
			{
				label = this.sourcePrecept.TransformThingLabel(label);
			}
			return label;
		}

		// Token: 0x06006D08 RID: 27912 RVA: 0x00249564 File Offset: 0x00247764
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.sourcePrecept == null)
			{
				return text;
			}
			string text2 = this.sourcePrecept.InspectStringExtra(this.parent);
			if (text2.NullOrEmpty())
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + text2;
		}

		// Token: 0x06006D09 RID: 27913 RVA: 0x002495B9 File Offset: 0x002477B9
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.StyleCategoryDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleLabel".Translate(), this.StyleCategoryDef.LabelCap, "Stat_Thing_StyleDesc".Translate(), 1108, null, null, false);
			}
			if (this.SourcePrecept != null)
			{
				foreach (StatDrawEntry statDrawEntry in this.SourcePrecept.SpecialDisplayStats(this.parent))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x002495C9 File Offset: 0x002477C9
		public override void PostExposeData()
		{
			Scribe_References.Look<Precept_ThingStyle>(ref this.sourcePrecept, "sourcePrecept", false);
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", false, false);
		}

		// Token: 0x04003CA0 RID: 15520
		private Precept_ThingStyle sourcePrecept;

		// Token: 0x04003CA1 RID: 15521
		public ThingStyleDef styleDef;

		// Token: 0x04003CA2 RID: 15522
		public bool everSeenByPlayer;

		// Token: 0x04003CA3 RID: 15523
		[Unsaved(false)]
		public StyleCategoryDef cachedStyleCategoryDef;
	}
}
