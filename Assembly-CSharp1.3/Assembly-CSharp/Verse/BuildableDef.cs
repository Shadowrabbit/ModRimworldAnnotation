using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008C RID: 140
	public abstract class BuildableDef : Def
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00006098 File Offset: 0x00004298
		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00019FA8 File Offset: 0x000181A8
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00019FB8 File Offset: 0x000181B8
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00019FC3 File Offset: 0x000181C3
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00019FDA File Offset: 0x000181DA
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00019FE7 File Offset: 0x000181E7
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00019FFC File Offset: 0x000181FC
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001A07C File Offset: 0x0001827C
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001A0BD File Offset: 0x000182BD
		public List<ThingDefCountClass> CostList
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costList;
				}
				return this.costList;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001A0E6 File Offset: 0x000182E6
		public int CostStuffCount
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costStuffCount;
				}
				return this.costStuffCount;
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001A110 File Offset: 0x00018310
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

		// Token: 0x06000501 RID: 1281 RVA: 0x0001A154 File Offset: 0x00018354
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

		// Token: 0x06000502 RID: 1282 RVA: 0x0001A170 File Offset: 0x00018370
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

		// Token: 0x06000503 RID: 1283 RVA: 0x0001A200 File Offset: 0x00018400
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

		// Token: 0x06000504 RID: 1284 RVA: 0x0001A264 File Offset: 0x00018464
		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001A26C File Offset: 0x0001846C
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
			if (this.costListForDifficulty != null && this.costListForDifficulty.difficultyVar.NullOrEmpty())
			{
				yield return "costListForDifficulty is not referencing a difficulty.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001A27C File Offset: 0x0001847C
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
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), array.ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_TerrainRequirement_Desc".Translate(), 1101, null, null, false);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00019781 File Offset: 0x00017981
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00019789 File Offset: 0x00017989
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x040001EA RID: 490
		public List<StatModifier> statBases;

		// Token: 0x040001EB RID: 491
		public Traversability passability;

		// Token: 0x040001EC RID: 492
		public int pathCost;

		// Token: 0x040001ED RID: 493
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x040001EE RID: 494
		public float fertility = -1f;

		// Token: 0x040001EF RID: 495
		public List<ThingDefCountClass> costList;

		// Token: 0x040001F0 RID: 496
		public int costStuffCount;

		// Token: 0x040001F1 RID: 497
		public List<StuffCategoryDef> stuffCategories;

		// Token: 0x040001F2 RID: 498
		public CostListForDifficulty costListForDifficulty;

		// Token: 0x040001F3 RID: 499
		public int placingDraggableDimensions;

		// Token: 0x040001F4 RID: 500
		public bool clearBuildingArea = true;

		// Token: 0x040001F5 RID: 501
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x040001F6 RID: 502
		public float resourcesFractionWhenDeconstructed = 0.5f;

		// Token: 0x040001F7 RID: 503
		public List<AltitudeLayer> blocksAltitudes;

		// Token: 0x040001F8 RID: 504
		public StyleCategoryDef dominantStyleCategory;

		// Token: 0x040001F9 RID: 505
		public bool isAltar;

		// Token: 0x040001FA RID: 506
		public bool useStuffTerrainAffordance;

		// Token: 0x040001FB RID: 507
		public TerrainAffordanceDef terrainAffordanceNeeded;

		// Token: 0x040001FC RID: 508
		public List<ThingDef> buildingPrerequisites;

		// Token: 0x040001FD RID: 509
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040001FE RID: 510
		public int constructionSkillPrerequisite;

		// Token: 0x040001FF RID: 511
		public int artisticSkillPrerequisite;

		// Token: 0x04000200 RID: 512
		public TechLevel minTechLevelToBuild;

		// Token: 0x04000201 RID: 513
		public TechLevel maxTechLevelToBuild;

		// Token: 0x04000202 RID: 514
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x04000203 RID: 515
		public EffecterDef repairEffect;

		// Token: 0x04000204 RID: 516
		public EffecterDef constructEffect;

		// Token: 0x04000205 RID: 517
		public List<ColorForStuff> colorPerStuff;

		// Token: 0x04000206 RID: 518
		public bool canGenerateDefaultDesignator = true;

		// Token: 0x04000207 RID: 519
		public float specialDisplayRadius;

		// Token: 0x04000208 RID: 520
		public List<Type> placeWorkers;

		// Token: 0x04000209 RID: 521
		public DesignationCategoryDef designationCategory;

		// Token: 0x0400020A RID: 522
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x0400020B RID: 523
		public KeyBindingDef designationHotKey;

		// Token: 0x0400020C RID: 524
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x0400020D RID: 525
		public Vector2 uiIconOffset;

		// Token: 0x0400020E RID: 526
		public Color uiIconColor = Color.white;

		// Token: 0x0400020F RID: 527
		public int uiIconForStackCount = -1;

		// Token: 0x04000210 RID: 528
		[Unsaved(false)]
		public ThingDef blueprintDef;

		// Token: 0x04000211 RID: 529
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		// Token: 0x04000212 RID: 530
		[Unsaved(false)]
		public ThingDef frameDef;

		// Token: 0x04000213 RID: 531
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		// Token: 0x04000214 RID: 532
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x04000215 RID: 533
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04000216 RID: 534
		[Unsaved(false)]
		public float uiIconAngle;
	}
}
