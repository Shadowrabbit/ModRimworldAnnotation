using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020000BF RID: 191
	public class DesignationCategoryDef : Def
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001DDD9 File Offset: 0x0001BFD9
		public IEnumerable<Designator> ResolvedAllowedDesignators
		{
			get
			{
				GameRules rules = Current.Game.Rules;
				int num;
				for (int i = 0; i < this.resolvedDesignators.Count; i = num + 1)
				{
					Designator designator = this.resolvedDesignators[i];
					if (rules == null || rules.DesignatorAllowed(designator))
					{
						yield return designator;
					}
					num = i;
				}
				foreach (Designator designator2 in this.AllIdeoDesignators)
				{
					yield return designator2;
				}
				IEnumerator<Designator> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0001DDE9 File Offset: 0x0001BFE9
		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001DDF1 File Offset: 0x0001BFF1
		public IEnumerable<Designator> AllIdeoDesignators
		{
			get
			{
				if (ModsConfig.IdeologyActive)
				{
					if (this.cachedPlayerFaction != Faction.OfPlayer)
					{
						this.ideoBuildingDesignatorsCached.Clear();
						this.ideoDropdownsCached.Clear();
						this.cachedPlayerFaction = Faction.OfPlayer;
					}
					foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
					{
						int num;
						for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
						{
							Precept precept = ideo.PreceptsListForReading[i];
							bool flag = precept is Precept_Building;
							bool flag2 = precept is Precept_RitualSeat;
							if (flag || flag2)
							{
								Precept_ThingDef precept_ThingDef = (Precept_ThingDef)precept;
								if (precept_ThingDef.ThingDef.designationCategory == this)
								{
									yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|14_0(precept_ThingDef.ThingDef, precept_ThingDef as Precept_Building);
								}
							}
							num = i;
						}
						for (int i = 0; i < ideo.thingStyleCategories.Count; i = num + 1)
						{
							ThingStyleCategoryWithPriority styleCat = ideo.thingStyleCategories[i];
							if (styleCat.category.addDesignators != null)
							{
								for (int j = 0; j < styleCat.category.addDesignators.Count; j = num + 1)
								{
									if (styleCat.category.addDesignators[j].designationCategory == this)
									{
										yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|14_0(styleCat.category.addDesignators[j], null);
									}
									num = j;
								}
							}
							if (styleCat.category.addDesignatorGroups != null)
							{
								for (int j = 0; j < styleCat.category.addDesignatorGroups.Count; j = num + 1)
								{
									Designator_Dropdown designator_Dropdown = this.<get_AllIdeoDesignators>g__GetCachedDropdown|14_1(styleCat.category.addDesignatorGroups[j]);
									if (designator_Dropdown != null)
									{
										yield return designator_Dropdown;
									}
									num = j;
								}
							}
							styleCat = null;
							num = i;
						}
						for (int i = 0; i < ideo.memes.Count; i = num + 1)
						{
							MemeDef meme = ideo.memes[i];
							if (meme.addDesignators != null)
							{
								for (int j = 0; j < meme.addDesignators.Count; j = num + 1)
								{
									if (meme.addDesignators[j].designationCategory == this)
									{
										yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|14_0(meme.addDesignators[j], null);
									}
									num = j;
								}
							}
							if (meme.addDesignatorGroups != null)
							{
								for (int j = 0; j < meme.addDesignatorGroups.Count; j = num + 1)
								{
									Designator_Dropdown designator_Dropdown2 = this.<get_AllIdeoDesignators>g__GetCachedDropdown|14_1(meme.addDesignatorGroups[j]);
									if (designator_Dropdown2 != null)
									{
										yield return designator_Dropdown2;
									}
									num = j;
								}
							}
							meme = null;
							num = i;
						}
						ideo = null;
					}
					IEnumerator<Ideo> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001DE01 File Offset: 0x0001C001
		public IEnumerable<Designator> AllResolvedAndIdeoDesignators
		{
			get
			{
				foreach (Designator designator in this.resolvedDesignators)
				{
					yield return designator;
				}
				List<Designator>.Enumerator enumerator = default(List<Designator>.Enumerator);
				foreach (Designator designator2 in this.AllIdeoDesignators)
				{
					yield return designator2;
				}
				IEnumerator<Designator> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001DE11 File Offset: 0x0001C011
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + this.defName + "-Closed";
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001DE48 File Offset: 0x0001C048
		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			foreach (Type type in this.specialDesignatorClasses)
			{
				Designator designator = null;
				try
				{
					designator = (Designator)Activator.CreateInstance(type);
					designator.isOrder = true;
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"DesignationCategoryDef",
						this.defName,
						" could not instantiate special designator from class ",
						type,
						".\n Exception: \n",
						ex.ToString()
					}));
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this && tDef.canGenerateDefaultDesignator
			select tDef;
			Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> dictionary = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();
			foreach (BuildableDef buildableDef in enumerable)
			{
				if (buildableDef.designatorDropdown != null)
				{
					if (!dictionary.ContainsKey(buildableDef.designatorDropdown))
					{
						dictionary[buildableDef.designatorDropdown] = new Designator_Dropdown();
						this.resolvedDesignators.Add(dictionary[buildableDef.designatorDropdown]);
					}
					dictionary[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				else
				{
					this.resolvedDesignators.Add(new Designator_Build(buildableDef));
				}
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001E020 File Offset: 0x0001C220
		[CompilerGenerated]
		private Designator <get_AllIdeoDesignators>g__GetCachedDesignator|14_0(BuildableDef def, Precept_Building buildingPrecept)
		{
			Designator designator;
			if (!this.ideoBuildingDesignatorsCached.TryGetValue(def, out designator))
			{
				Designator_Build designator_Build = new Designator_Build(def);
				designator = designator_Build;
				if (buildingPrecept != null)
				{
					designator_Build.sourcePrecept = buildingPrecept;
				}
				this.ideoBuildingDesignatorsCached[def] = designator;
			}
			return designator;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001E060 File Offset: 0x0001C260
		[CompilerGenerated]
		private Designator_Dropdown <get_AllIdeoDesignators>g__GetCachedDropdown|14_1(DesignatorDropdownGroupDef group)
		{
			Designator_Dropdown result;
			if (!this.ideoDropdownsCached.TryGetValue(group, out result))
			{
				IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
				where tDef.designationCategory == this && !tDef.canGenerateDefaultDesignator && tDef.designatorDropdown == @group
				select tDef;
				if (!enumerable.Any<BuildableDef>())
				{
					this.ideoDropdownsCached[group] = null;
					return this.ideoDropdownsCached[group];
				}
				foreach (BuildableDef buildableDef in enumerable)
				{
					if (!this.ideoDropdownsCached.ContainsKey(buildableDef.designatorDropdown))
					{
						this.ideoDropdownsCached[buildableDef.designatorDropdown] = new Designator_Dropdown();
					}
					this.ideoDropdownsCached[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				result = this.ideoDropdownsCached[group];
			}
			return result;
		}

		// Token: 0x040003E4 RID: 996
		public List<Type> specialDesignatorClasses = new List<Type>();

		// Token: 0x040003E5 RID: 997
		public int order;

		// Token: 0x040003E6 RID: 998
		public bool showPowerGrid;

		// Token: 0x040003E7 RID: 999
		[Unsaved(false)]
		private List<Designator> resolvedDesignators = new List<Designator>();

		// Token: 0x040003E8 RID: 1000
		[Unsaved(false)]
		public KeyBindingCategoryDef bindingCatDef;

		// Token: 0x040003E9 RID: 1001
		[Unsaved(false)]
		public string cachedHighlightClosedTag;

		// Token: 0x040003EA RID: 1002
		[Unsaved(false)]
		private Dictionary<BuildableDef, Designator> ideoBuildingDesignatorsCached = new Dictionary<BuildableDef, Designator>();

		// Token: 0x040003EB RID: 1003
		[Unsaved(false)]
		private Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> ideoDropdownsCached = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();

		// Token: 0x040003EC RID: 1004
		[Unsaved(false)]
		private Faction cachedPlayerFaction;
	}
}
