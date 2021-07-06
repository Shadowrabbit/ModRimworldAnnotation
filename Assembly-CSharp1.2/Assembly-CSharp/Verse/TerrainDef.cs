using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018C RID: 396
	public class TerrainDef : BuildableDef
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0000DACC File Offset: 0x0000BCCC
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0000DAF5 File Offset: 0x0000BCF5
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0000DB02 File Offset: 0x0000BD02
		public bool IsFine
		{
			get
			{
				return this.HasTag("FineFloor");
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0009A780 File Offset: 0x00098980
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

		// Token: 0x060009DF RID: 2527 RVA: 0x0000DB0F File Offset: 0x0000BD0F
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0000DB23 File Offset: 0x0000BD23
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

		// Token: 0x060009E1 RID: 2529 RVA: 0x0000DB33 File Offset: 0x0000BD33
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0000DB54 File Offset: 0x0000BD54
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
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Supports".Translate(), array.ToCommaList(false).CapitalizeFirst(), "Stat_Thing_Terrain_Supports_Desc".Translate(), 2000, null, null, false);
			}
			if (this.IsFine && ModsConfig.RoyaltyActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Terrain_Fine_Name".Translate(), "Stat_Thing_Terrain_Fine_Value".Translate(), "Stat_Thing_Terrain_Fine_Desc".Translate(), 2000, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04000882 RID: 2178
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000883 RID: 2179
		public TerrainDef.TerrainEdgeType edgeType;

		// Token: 0x04000884 RID: 2180
		[NoTranslate]
		public string waterDepthShader;

		// Token: 0x04000885 RID: 2181
		public List<ShaderParameter> waterDepthShaderParameters;

		// Token: 0x04000886 RID: 2182
		public int renderPrecedence;

		// Token: 0x04000887 RID: 2183
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x04000888 RID: 2184
		public bool layerable;

		// Token: 0x04000889 RID: 2185
		[NoTranslate]
		public string scatterType;

		// Token: 0x0400088A RID: 2186
		public bool takeFootprints;

		// Token: 0x0400088B RID: 2187
		public bool takeSplashes;

		// Token: 0x0400088C RID: 2188
		public bool avoidWander;

		// Token: 0x0400088D RID: 2189
		public bool changeable = true;

		// Token: 0x0400088E RID: 2190
		public TerrainDef smoothedTerrain;

		// Token: 0x0400088F RID: 2191
		public bool holdSnow = true;

		// Token: 0x04000890 RID: 2192
		public bool extinguishesFire;

		// Token: 0x04000891 RID: 2193
		public Color color = Color.white;

		// Token: 0x04000892 RID: 2194
		public TerrainDef driesTo;

		// Token: 0x04000893 RID: 2195
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04000894 RID: 2196
		public TerrainDef burnedDef;

		// Token: 0x04000895 RID: 2197
		public List<Tool> tools;

		// Token: 0x04000896 RID: 2198
		public float extraDeteriorationFactor;

		// Token: 0x04000897 RID: 2199
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x04000898 RID: 2200
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x04000899 RID: 2201
		public ThoughtDef traversedThought;

		// Token: 0x0400089A RID: 2202
		public int extraDraftedPerceivedPathCost;

		// Token: 0x0400089B RID: 2203
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x0400089C RID: 2204
		public EffecterDef destroyEffect;

		// Token: 0x0400089D RID: 2205
		public EffecterDef destroyEffectWater;

		// Token: 0x0400089E RID: 2206
		public bool autoRebuildable;

		// Token: 0x0400089F RID: 2207
		public ThingDef generatedFilth;

		// Token: 0x040008A0 RID: 2208
		public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

		// Token: 0x040008A1 RID: 2209
		[Unsaved(false)]
		public Material waterDepthMaterial;

		// Token: 0x0200018D RID: 397
		public enum TerrainEdgeType : byte
		{
			// Token: 0x040008A3 RID: 2211
			Hard,
			// Token: 0x040008A4 RID: 2212
			Fade,
			// Token: 0x040008A5 RID: 2213
			FadeRough,
			// Token: 0x040008A6 RID: 2214
			Water
		}
	}
}
