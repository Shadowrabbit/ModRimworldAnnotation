using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED7 RID: 3799
	public class IdeoStyleTracker : IExposable
	{
		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x001EDF6F File Offset: 0x001EC16F
		public int NumHairAndBeardStylesAvailable
		{
			get
			{
				if (this.hairAndBeardStylesAvailable < 0)
				{
					this.RecacheStyleItemCounts();
				}
				return this.hairAndBeardStylesAvailable;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06005A0D RID: 23053 RVA: 0x001EDF86 File Offset: 0x001EC186
		public int NumTattooStylesAvailable
		{
			get
			{
				if (this.tattooStylesAvailable < 0)
				{
					this.RecacheStyleItemCounts();
				}
				return this.tattooStylesAvailable;
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06005A0E RID: 23054 RVA: 0x001EDFA0 File Offset: 0x001EC1A0
		public HairDef DisplayedHairDef
		{
			get
			{
				if (this.cachedDisplayHairDef == null)
				{
					if (this.hairFrequencies == null)
					{
						return null;
					}
					Rand.PushState(this.ideo.id);
					this.cachedDisplayHairDef = this.hairFrequencies.RandomElementByWeight((KeyValuePair<HairDef, StyleItemSpawningProperties> x) => (float)x.Value.frequency).Key;
					Rand.PopState();
				}
				return this.cachedDisplayHairDef;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06005A0F RID: 23055 RVA: 0x001EE014 File Offset: 0x001EC214
		public TattooDef DisplayedTattooDef
		{
			get
			{
				if (this.cachedDisplayTattooDef == null)
				{
					if (this.tattooFrequencies == null)
					{
						return null;
					}
					Rand.PushState(this.ideo.id);
					KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair;
					if (!this.tattooFrequencies.TryRandomElementByWeight((KeyValuePair<TattooDef, StyleItemSpawningProperties> x) => (float)IdeoStyleTracker.<get_DisplayedTattooDef>g__GetWeight|16_0(x, false), out keyValuePair))
					{
						this.tattooFrequencies.TryRandomElementByWeight((KeyValuePair<TattooDef, StyleItemSpawningProperties> x) => (float)IdeoStyleTracker.<get_DisplayedTattooDef>g__GetWeight|16_0(x, true), out keyValuePair);
					}
					Rand.PopState();
					this.cachedDisplayTattooDef = keyValuePair.Key;
				}
				return this.cachedDisplayTattooDef;
			}
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x001EE0B9 File Offset: 0x001EC2B9
		public IdeoStyleTracker(Ideo ideo)
		{
			this.ideo = ideo;
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x001EE0EC File Offset: 0x001EC2EC
		private void InitializeDefMaps()
		{
			this.hairFrequencies = new DefMap<HairDef, StyleItemSpawningProperties>();
			this.beardFrequencies = new DefMap<BeardDef, StyleItemSpawningProperties>();
			this.tattooFrequencies = new DefMap<TattooDef, StyleItemSpawningProperties>();
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x001EE110 File Offset: 0x001EC310
		public StyleItemFrequency GetFrequency(StyleItemDef def)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				return this.hairFrequencies[def2].frequency;
			}
			BeardDef def3;
			if ((def3 = (def as BeardDef)) != null)
			{
				return this.beardFrequencies[def3].frequency;
			}
			TattooDef def4;
			if ((def4 = (def as TattooDef)) != null)
			{
				return this.tattooFrequencies[def4].frequency;
			}
			return StyleItemFrequency.Never;
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x001EE190 File Offset: 0x001EC390
		public StyleGender GetGender(StyleItemDef def)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				return this.hairFrequencies[def2].gender;
			}
			BeardDef def3;
			if ((def3 = (def as BeardDef)) != null)
			{
				return this.beardFrequencies[def3].gender;
			}
			TattooDef def4;
			if ((def4 = (def as TattooDef)) != null)
			{
				return this.tattooFrequencies[def4].gender;
			}
			return StyleGender.Any;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x001EE210 File Offset: 0x001EC410
		public void SetFrequency(StyleItemDef def, StyleItemFrequency freq)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef def2;
			BeardDef def3;
			TattooDef def4;
			if ((def2 = (def as HairDef)) != null)
			{
				this.hairFrequencies[def2].frequency = freq;
			}
			else if ((def3 = (def as BeardDef)) != null)
			{
				this.beardFrequencies[def3].frequency = freq;
			}
			else if ((def4 = (def as TattooDef)) != null)
			{
				this.tattooFrequencies[def4].frequency = freq;
			}
			this.hairAndBeardStylesAvailable = -1;
			this.tattooStylesAvailable = -1;
			this.cachedDisplayHairDef = null;
			this.cachedDisplayTattooDef = null;
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x001EE2B0 File Offset: 0x001EC4B0
		public void SetGender(StyleItemDef def, StyleGender gender)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef def2;
			if ((def2 = (def as HairDef)) != null)
			{
				this.hairFrequencies[def2].gender = gender;
				return;
			}
			BeardDef def3;
			if ((def3 = (def as BeardDef)) != null)
			{
				this.beardFrequencies[def3].gender = gender;
				return;
			}
			TattooDef def4;
			if ((def4 = (def as TattooDef)) != null)
			{
				this.tattooFrequencies[def4].gender = gender;
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x001EE334 File Offset: 0x001EC534
		public void EnsureAtLeastOneStyleItemAvailable()
		{
			bool flag = false;
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				if (keyValuePair.Value.frequency > StyleItemFrequency.Never)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.hairFrequencies[HairDefOf.Shaved].frequency = StyleItemFrequency.Normal;
				this.hairFrequencies[HairDefOf.Shaved].gender = StyleGender.Any;
			}
			flag = false;
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				if (keyValuePair2.Value.frequency != StyleItemFrequency.Never)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.beardFrequencies[BeardDefOf.NoBeard].frequency = StyleItemFrequency.Normal;
				this.beardFrequencies[BeardDefOf.NoBeard].gender = StyleGender.Any;
			}
			bool flag2 = false;
			bool flag3 = false;
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				if (keyValuePair3.Value.frequency != StyleItemFrequency.Never)
				{
					if (keyValuePair3.Key.tattooType == TattooType.Face)
					{
						flag2 = true;
					}
					else if (keyValuePair3.Key.tattooType == TattooType.Body)
					{
						flag3 = true;
					}
				}
				if (flag2 && flag3)
				{
					break;
				}
			}
			if (!flag2)
			{
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].frequency = StyleItemFrequency.Normal;
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].gender = StyleGender.Any;
			}
			if (!flag3)
			{
				this.tattooFrequencies[TattooDefOf.NoTattoo_Body].frequency = StyleItemFrequency.Normal;
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].gender = StyleGender.Any;
			}
			this.hairAndBeardStylesAvailable = -1;
			this.tattooStylesAvailable = -1;
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x001EE520 File Offset: 0x001EC720
		private void RecacheStyleItemCounts()
		{
			if (this.ideo.culture == null)
			{
				return;
			}
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			this.hairAndBeardStylesAvailable = 0;
			foreach (HairDef def in DefDatabase<HairDef>.AllDefs)
			{
				if (this.hairFrequencies[def].frequency != StyleItemFrequency.Never)
				{
					this.hairAndBeardStylesAvailable++;
				}
			}
			foreach (BeardDef def2 in DefDatabase<BeardDef>.AllDefs)
			{
				if (this.beardFrequencies[def2].frequency != StyleItemFrequency.Never)
				{
					this.hairAndBeardStylesAvailable++;
				}
			}
			this.tattooStylesAvailable = 0;
			foreach (TattooDef tattooDef in DefDatabase<TattooDef>.AllDefs)
			{
				if (tattooDef != TattooDefOf.NoTattoo_Face && tattooDef != TattooDefOf.NoTattoo_Body && this.tattooFrequencies[tattooDef].frequency != StyleItemFrequency.Never)
				{
					this.tattooStylesAvailable++;
				}
			}
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x001EE684 File Offset: 0x001EC884
		public void RecalculateAvailableStyleItems()
		{
			if (!this.ideo.culture.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
			{
				this.memberStyleTags.AddRange(this.ideo.culture.styleItemTags);
			}
			int i;
			int num;
			for (i = 0; i < this.ideo.memes.Count; i = num + 1)
			{
				if (!this.ideo.memes[i].styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
				{
					int j;
					for (j = 0; j < this.ideo.memes[i].styleItemTags.Count; j = num + 1)
					{
						StyleItemTagWeighted styleItemTagWeighted = this.memberStyleTags.Find((StyleItemTagWeighted x) => x.Tag == this.ideo.memes[i].styleItemTags[j].Tag);
						if (styleItemTagWeighted == null)
						{
							this.memberStyleTags.Add(this.ideo.memes[i].styleItemTags[j]);
						}
						else
						{
							styleItemTagWeighted.Add(this.ideo.memes[i].styleItemTags[j]);
						}
						num = j;
					}
				}
				num = i;
			}
			this.InitializeDefMaps();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|26_0<HairDef>();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|26_0<BeardDef>();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|26_0<TattooDef>();
			this.EnsureAtLeastOneStyleItemAvailable();
			this.memberStyleTags.Clear();
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x001EE834 File Offset: 0x001ECA34
		private bool NeedToRecacheStyleItems()
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				return true;
			}
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				if (keyValuePair.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				if (keyValuePair2.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				if (keyValuePair3.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x001EE944 File Offset: 0x001ECB44
		public StyleCategoryPair StyleForThingDef(ThingDef thing, Precept precept = null)
		{
			if (this.styleForThingDef.ContainsKey(thing))
			{
				return this.styleForThingDef[thing];
			}
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.ideo.thingStyleCategories)
			{
				ThingStyleDef thingStyleDef = thingStyleCategoryWithPriority.category.GetStyleForThingDef(thing, precept);
				if (thingStyleDef != null)
				{
					StyleCategoryPair styleCategoryPair = new StyleCategoryPair
					{
						styleDef = thingStyleDef,
						category = thingStyleCategoryWithPriority.category
					};
					this.styleForThingDef.Add(thing, styleCategoryPair);
					return styleCategoryPair;
				}
			}
			return null;
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x001EE9F0 File Offset: 0x001ECBF0
		public void SetStyleForThingDef(ThingDef thing, StyleCategoryPair styleAndCat)
		{
			this.styleForThingDef.SetOrAdd(thing, styleAndCat);
		}

		// Token: 0x06005A1C RID: 23068 RVA: 0x001EE9FF File Offset: 0x001ECBFF
		public void ResetStylesForThingDef()
		{
			this.styleForThingDef.RemoveAll((KeyValuePair<ThingDef, StyleCategoryPair> kvp) => !this.ideo.PreceptsListForReading.Any(delegate(Precept p)
			{
				Precept_ThingDef precept_ThingDef;
				return (precept_ThingDef = (p as Precept_ThingDef)) != null && precept_ThingDef.ThingDef == kvp.Key;
			}) || !kvp.Key.canEditAnyStyle);
		}

		// Token: 0x06005A1D RID: 23069 RVA: 0x001EEA19 File Offset: 0x001ECC19
		public void ResetStyleForThing(ThingDef thingDef)
		{
			this.styleForThingDef.Remove(thingDef);
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x001EEA28 File Offset: 0x001ECC28
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.ideo, "ideo", false);
			Scribe_Deep.Look<DefMap<HairDef, StyleItemSpawningProperties>>(ref this.hairFrequencies, "hairFrequencies", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<BeardDef, StyleItemSpawningProperties>>(ref this.beardFrequencies, "beardFrequencies", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TattooDef, StyleItemSpawningProperties>>(ref this.tattooFrequencies, "tattooFrequencies", Array.Empty<object>());
			Scribe_Collections.Look<ThingDef, StyleCategoryPair>(ref this.styleForThingDef, "styleForThingDef", LookMode.Def, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.NeedToRecacheStyleItems())
			{
				this.RecalculateAvailableStyleItems();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.styleForThingDef == null)
				{
					this.styleForThingDef = new Dictionary<ThingDef, StyleCategoryPair>();
					return;
				}
				this.styleForThingDef.RemoveAll((KeyValuePair<ThingDef, StyleCategoryPair> x) => x.Key == null || x.Value == null);
			}
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x001EEAF4 File Offset: 0x001ECCF4
		[CompilerGenerated]
		internal static int <get_DisplayedTattooDef>g__GetWeight|16_0(KeyValuePair<TattooDef, StyleItemSpawningProperties> pair, bool ignoreFrequency)
		{
			if (pair.Key == TattooDefOf.NoTattoo_Body || pair.Key == TattooDefOf.NoTattoo_Face)
			{
				return 0;
			}
			if (ignoreFrequency)
			{
				return 1;
			}
			return (int)pair.Value.frequency;
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x001EEB28 File Offset: 0x001ECD28
		[CompilerGenerated]
		private void <RecalculateAvailableStyleItems>g__SetupFrequency|26_0<T>() where T : StyleItemDef
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				float freq = PawnStyleItemChooser.StyleItemChoiceLikelihoodFromTags(t, this.memberStyleTags);
				this.SetFrequency(t, PawnStyleItemChooser.GetStyleItemFrequency(freq));
				this.SetGender(t, t.styleGender);
			}
		}

		// Token: 0x040034B6 RID: 13494
		private Ideo ideo;

		// Token: 0x040034B7 RID: 13495
		private DefMap<HairDef, StyleItemSpawningProperties> hairFrequencies;

		// Token: 0x040034B8 RID: 13496
		private DefMap<BeardDef, StyleItemSpawningProperties> beardFrequencies;

		// Token: 0x040034B9 RID: 13497
		private DefMap<TattooDef, StyleItemSpawningProperties> tattooFrequencies;

		// Token: 0x040034BA RID: 13498
		private Dictionary<ThingDef, StyleCategoryPair> styleForThingDef = new Dictionary<ThingDef, StyleCategoryPair>();

		// Token: 0x040034BB RID: 13499
		private int hairAndBeardStylesAvailable = -1;

		// Token: 0x040034BC RID: 13500
		private int tattooStylesAvailable = -1;

		// Token: 0x040034BD RID: 13501
		private HairDef cachedDisplayHairDef;

		// Token: 0x040034BE RID: 13502
		private TattooDef cachedDisplayTattooDef;

		// Token: 0x040034BF RID: 13503
		private List<StyleItemTagWeighted> memberStyleTags = new List<StyleItemTagWeighted>();
	}
}
