using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E9 RID: 233
	public abstract class BuildableDef : Def
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00007907 File Offset: 0x00005B07
		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0000B9EA File Offset: 0x00009BEA
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0000B9FA File Offset: 0x00009BFA
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0000BA05 File Offset: 0x00009C05
		public Material DrawMatSingle
		{
			get
			{
				if (this.graphic == null)
				{
					return null;
				}
				return this.graphic.MatSingle;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0000BA1C File Offset: 0x00009C1C
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0000BA29 File Offset: 0x00009C29
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x000903D0 File Offset: 0x0008E5D0
		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				if (this.placeWorkers == null)
				{
					return null;
				}
				this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
				foreach (Type type in this.placeWorkers)
				{
					this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
				}
				return this.placeWorkersInstantiatedInt;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00090450 File Offset: 0x0008E650
		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00090494 File Offset: 0x0008E694
		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			if (this.PlaceWorkers == null)
			{
				return false;
			}
			for (int i = 0; i < this.PlaceWorkers.Count; i++)
			{
				if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0000BA3B File Offset: 0x00009C3B
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
					return;
				}
				this.ResolveIcon();
			});
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000904D8 File Offset: 0x0008E6D8
		protected virtual void ResolveIcon()
		{
			if (this.graphic != null && this.graphic != BaseContent.BadGraphic)
			{
				Graphic outerGraphic = this.graphic;
				if (this.uiIconForStackCount >= 1 && this is ThingDef)
				{
					Graphic_StackCount graphic_StackCount = this.graphic as Graphic_StackCount;
					if (graphic_StackCount != null)
					{
						outerGraphic = graphic_StackCount.SubGraphicForStackCount(this.uiIconForStackCount, (ThingDef)this);
					}
				}
				Material material = outerGraphic.ExtractInnerGraphicFor(null).MatAt(this.defaultPlacingRot, null);
				this.uiIcon = (Texture2D)material.mainTexture;
				this.uiIconColor = material.color;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00090568 File Offset: 0x0008E768
		public Color GetColorForStuff(ThingDef stuff)
		{
			if (this.colorPerStuff.NullOrEmpty<ColorForStuff>())
			{
				return stuff.stuffProps.color;
			}
			for (int i = 0; i < this.colorPerStuff.Count; i++)
			{
				ColorForStuff colorForStuff = this.colorPerStuff[i];
				if (colorForStuff.Stuff == stuff)
				{
					return colorForStuff.Color;
				}
			}
			return stuff.stuffProps.color;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0000BA54 File Offset: 0x00009C54
		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0000BA5C File Offset: 0x00009C5C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.useStuffTerrainAffordance && !this.MadeFromStuff)
			{
				yield return "useStuffTerrainAffordance is true but it's not made from stuff";
			}
			yield break;
			yield break;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0000BA6C File Offset: 0x00009C6C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.BuildableByPlayer)
			{
				IEnumerable<TerrainAffordanceDef> enumerable = Enumerable.Empty<TerrainAffordanceDef>();
				if (this.PlaceWorkers != null)
				{
					enumerable = enumerable.Concat(this.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
				}
				TerrainAffordanceDef terrainAffordanceNeed = this.GetTerrainAffordanceNeed(req.StuffDef);
				if (terrainAffordanceNeed != null)
				{
					enumerable = enumerable.Concat(terrainAffordanceNeed);
				}
				string[] array = (from ta in enumerable.Distinct<TerrainAffordanceDef>()
				orderby ta.order
				select ta.label).ToArray<string>();
				if (array.Length != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), array.ToCommaList(false).CapitalizeFirst(), "Stat_Thing_TerrainRequirement_Desc".Translate(), 1101, null, null, false);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0000B48D File Offset: 0x0000968D
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0000B495 File Offset: 0x00009695
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x040003AA RID: 938
		public List<StatModifier> statBases;

		// Token: 0x040003AB RID: 939
		public Traversability passability;

		// Token: 0x040003AC RID: 940
		public int pathCost;

		// Token: 0x040003AD RID: 941
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x040003AE RID: 942
		public float fertility = -1f;

		// Token: 0x040003AF RID: 943
		public List<ThingDefCountClass> costList;

		// Token: 0x040003B0 RID: 944
		public int costStuffCount;

		// Token: 0x040003B1 RID: 945
		public List<StuffCategoryDef> stuffCategories;

		// Token: 0x040003B2 RID: 946
		public int placingDraggableDimensions;

		// Token: 0x040003B3 RID: 947
		public bool clearBuildingArea = true;

		// Token: 0x040003B4 RID: 948
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x040003B5 RID: 949
		public float resourcesFractionWhenDeconstructed = 0.75f;

		// Token: 0x040003B6 RID: 950
		public bool useStuffTerrainAffordance;

		// Token: 0x040003B7 RID: 951
		public TerrainAffordanceDef terrainAffordanceNeeded;

		// Token: 0x040003B8 RID: 952
		public List<ThingDef> buildingPrerequisites;

		// Token: 0x040003B9 RID: 953
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040003BA RID: 954
		public int constructionSkillPrerequisite;

		// Token: 0x040003BB RID: 955
		public int artisticSkillPrerequisite;

		// Token: 0x040003BC RID: 956
		public TechLevel minTechLevelToBuild;

		// Token: 0x040003BD RID: 957
		public TechLevel maxTechLevelToBuild;

		// Token: 0x040003BE RID: 958
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x040003BF RID: 959
		public EffecterDef repairEffect;

		// Token: 0x040003C0 RID: 960
		public EffecterDef constructEffect;

		// Token: 0x040003C1 RID: 961
		public List<ColorForStuff> colorPerStuff;

		// Token: 0x040003C2 RID: 962
		public bool menuHidden;

		// Token: 0x040003C3 RID: 963
		public float specialDisplayRadius;

		// Token: 0x040003C4 RID: 964
		public List<Type> placeWorkers;

		// Token: 0x040003C5 RID: 965
		public DesignationCategoryDef designationCategory;

		// Token: 0x040003C6 RID: 966
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x040003C7 RID: 967
		public KeyBindingDef designationHotKey;

		// Token: 0x040003C8 RID: 968
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x040003C9 RID: 969
		public Vector2 uiIconOffset;

		// Token: 0x040003CA RID: 970
		public Color uiIconColor = Color.white;

		// Token: 0x040003CB RID: 971
		public int uiIconForStackCount = -1;

		// Token: 0x040003CC RID: 972
		[Unsaved(false)]
		public ThingDef blueprintDef;

		// Token: 0x040003CD RID: 973
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		// Token: 0x040003CE RID: 974
		[Unsaved(false)]
		public ThingDef frameDef;

		// Token: 0x040003CF RID: 975
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		// Token: 0x040003D0 RID: 976
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x040003D1 RID: 977
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x040003D2 RID: 978
		[Unsaved(false)]
		public float uiIconAngle;
	}
}
