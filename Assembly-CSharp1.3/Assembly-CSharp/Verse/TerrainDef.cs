using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010E RID: 270
	public class TerrainDef : BuildableDef
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00021D13 File Offset: 0x0001FF13
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00021D1B File Offset: 0x0001FF1B
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00021D37 File Offset: 0x0001FF37
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00021D44 File Offset: 0x0001FF44
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00021D51 File Offset: 0x0001FF51
		public bool IsFine
		{
			get
			{
				return this.HasTag("FineFloor");
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00021D5E File Offset: 0x0001FF5E
		public bool IsSoil
		{
			get
			{
				return this.HasTag("Soil");
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00021D6B File Offset: 0x0001FF6B
		public bool IsRoad
		{
			get
			{
				return this.HasTag("Road");
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00021D78 File Offset: 0x0001FF78
		public override void PostLoad()
		{
			this.placingDraggableDimensions = 2;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Shader shader = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					shader = ShaderDatabase.TerrainHard;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					shader = ShaderDatabase.TerrainFade;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					shader = ShaderDatabase.TerrainFadeRough;
					break;
				case TerrainDef.TerrainEdgeType.Water:
					shader = ShaderDatabase.TerrainWater;
					break;
				}
				this.graphic = GraphicDatabase.Get<Graphic_Terrain>(this.texturePath, shader, Vector2.one, this.color, 2000 + this.renderPrecedence);
				if (shader == ShaderDatabase.TerrainFadeRough || shader == ShaderDatabase.TerrainWater)
				{
					this.graphic.MatSingle.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
				}
				if (!this.waterDepthShader.NullOrEmpty())
				{
					this.waterDepthMaterial = MaterialAllocator.Create(ShaderDatabase.LoadShader(this.waterDepthShader));
					this.waterDepthMaterial.renderQueue = 2000 + this.renderPrecedence;
					this.waterDepthMaterial.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
					if (this.waterDepthShaderParameters != null)
					{
						for (int j = 0; j < this.waterDepthShaderParameters.Count; j++)
						{
							this.waterDepthShaderParameters[j].Apply(this.waterDepthMaterial);
						}
					}
				}
			});
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			base.PostLoad();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00021DD9 File Offset: 0x0001FFD9
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00021DED File Offset: 0x0001FFED
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.texturePath.NullOrEmpty())
			{
				yield return "missing texturePath";
			}
			if (this.fertility < 0f)
			{
				yield return "Terrain Def " + this + " has no fertility value set.";
			}
			if (this.renderPrecedence > 400)
			{
				yield return "Render order " + this.renderPrecedence + " is out of range (must be < 400)";
			}
			if (this.generatedFilth != null && (this.filthAcceptanceMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None)
			{
				yield return this.defName + " makes terrain filth and also accepts it.";
			}
			if (this.Flammable() && this.burnedDef == null && !this.layerable)
			{
				yield return "flammable but burnedDef is null and not layerable";
			}
			if (this.burnedDef != null && this.burnedDef.Flammable())
			{
				yield return "burnedDef is flammable";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00021DFD File Offset: 0x0001FFFD
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00021E06 File Offset: 0x00020006
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00021E1E File Offset: 0x0002001E
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			string[] array = (from ta in this.affordances.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (array.Length != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Terrain, "Supports".Translate(), array.ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_Terrain_Supports_Desc".Translate(), 2000, null, null, false);
			}
			if (this.IsFine && ModsConfig.RoyaltyActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Terrain, "Stat_Thing_Terrain_Fine_Name".Translate(), "Stat_Thing_Terrain_Fine_Value".Translate(), "Stat_Thing_Terrain_Fine_Desc".Translate(), 2000, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400065E RID: 1630
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400065F RID: 1631
		public TerrainDef.TerrainEdgeType edgeType;

		// Token: 0x04000660 RID: 1632
		[NoTranslate]
		public string waterDepthShader;

		// Token: 0x04000661 RID: 1633
		public List<ShaderParameter> waterDepthShaderParameters;

		// Token: 0x04000662 RID: 1634
		public int renderPrecedence;

		// Token: 0x04000663 RID: 1635
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x04000664 RID: 1636
		public bool layerable;

		// Token: 0x04000665 RID: 1637
		[NoTranslate]
		public string scatterType;

		// Token: 0x04000666 RID: 1638
		public bool takeFootprints;

		// Token: 0x04000667 RID: 1639
		public bool takeSplashes;

		// Token: 0x04000668 RID: 1640
		public bool avoidWander;

		// Token: 0x04000669 RID: 1641
		public bool changeable = true;

		// Token: 0x0400066A RID: 1642
		public TerrainDef smoothedTerrain;

		// Token: 0x0400066B RID: 1643
		public bool holdSnow = true;

		// Token: 0x0400066C RID: 1644
		public bool extinguishesFire;

		// Token: 0x0400066D RID: 1645
		public bool bridge;

		// Token: 0x0400066E RID: 1646
		public Color color = Color.white;

		// Token: 0x0400066F RID: 1647
		public TerrainDef driesTo;

		// Token: 0x04000670 RID: 1648
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04000671 RID: 1649
		public TerrainDef burnedDef;

		// Token: 0x04000672 RID: 1650
		public List<Tool> tools;

		// Token: 0x04000673 RID: 1651
		public float extraDeteriorationFactor;

		// Token: 0x04000674 RID: 1652
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x04000675 RID: 1653
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x04000676 RID: 1654
		public ThoughtDef traversedThought;

		// Token: 0x04000677 RID: 1655
		public int extraDraftedPerceivedPathCost;

		// Token: 0x04000678 RID: 1656
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x04000679 RID: 1657
		public EffecterDef destroyEffect;

		// Token: 0x0400067A RID: 1658
		public EffecterDef destroyEffectWater;

		// Token: 0x0400067B RID: 1659
		public bool autoRebuildable;

		// Token: 0x0400067C RID: 1660
		public ThingDef generatedFilth;

		// Token: 0x0400067D RID: 1661
		public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

		// Token: 0x0400067E RID: 1662
		[Unsaved(false)]
		public Material waterDepthMaterial;

		// Token: 0x020018F6 RID: 6390
		public enum TerrainEdgeType : byte
		{
			// Token: 0x04005FB7 RID: 24503
			Hard,
			// Token: 0x04005FB8 RID: 24504
			Fade,
			// Token: 0x04005FB9 RID: 24505
			FadeRough,
			// Token: 0x04005FBA RID: 24506
			Water
		}
	}
}
