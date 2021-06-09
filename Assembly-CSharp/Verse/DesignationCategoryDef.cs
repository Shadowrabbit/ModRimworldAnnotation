using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200012A RID: 298
	public class DesignationCategoryDef : Def
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0000C758 File Offset: 0x0000A958
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
				yield break;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0000C768 File Offset: 0x0000A968
		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0000C770 File Offset: 0x0000A970
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + this.defName + "-Closed";
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00094BA8 File Offset: 0x00092DA8
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
					}), false);
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this
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

		// Token: 0x040005CF RID: 1487
		public List<Type> specialDesignatorClasses = new List<Type>();

		// Token: 0x040005D0 RID: 1488
		public int order;

		// Token: 0x040005D1 RID: 1489
		public bool showPowerGrid;

		// Token: 0x040005D2 RID: 1490
		[Unsaved(false)]
		private List<Designator> resolvedDesignators = new List<Designator>();

		// Token: 0x040005D3 RID: 1491
		[Unsaved(false)]
		public KeyBindingCategoryDef bindingCatDef;

		// Token: 0x040005D4 RID: 1492
		[Unsaved(false)]
		public string cachedHighlightClosedTag;
	}
}
