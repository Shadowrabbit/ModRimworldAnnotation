using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000264 RID: 612
	public class PawnGraphicSet
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00062472 File Offset: 0x00060672
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00062480 File Offset: 0x00060680
		public List<Material> MatsBodyBaseAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool drawClothes = true)
		{
			int num = facing.AsInt + 1000 * (int)bodyCondition;
			if (drawClothes)
			{
				num += 10000;
			}
			if (num != this.cachedMatsBodyBaseHash)
			{
				this.cachedMatsBodyBase.Clear();
				this.cachedMatsBodyBaseHash = num;
				if (bodyCondition == RotDrawMode.Fresh)
				{
					this.cachedMatsBodyBase.Add(this.nakedGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Rotting || this.dessicatedGraphic == null)
				{
					this.cachedMatsBodyBase.Add(this.rottingGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Dessicated)
				{
					this.cachedMatsBodyBase.Add(this.dessicatedGraphic.MatAt(facing, null));
				}
				if (drawClothes)
				{
					for (int i = 0; i < this.apparelGraphics.Count; i++)
					{
						if ((this.apparelGraphics[i].sourceApparel.def.apparel.shellRenderedBehindHead || this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Shell) && !PawnRenderer.RenderAsPack(this.apparelGraphics[i].sourceApparel) && this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Overhead)
						{
							this.cachedMatsBodyBase.Add(this.apparelGraphics[i].graphic.MatAt(facing, null));
						}
					}
				}
			}
			return this.cachedMatsBodyBase;
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x000625F8 File Offset: 0x000607F8
		public GraphicMeshSet HairMeshSet
		{
			get
			{
				if (this.pawn.story.crownType == CrownType.Average)
				{
					return MeshPool.humanlikeHairSetAverage;
				}
				if (this.pawn.story.crownType == CrownType.Narrow)
				{
					return MeshPool.humanlikeHairSetNarrow;
				}
				Log.Error("Unknown crown type: " + this.pawn.story.crownType);
				return MeshPool.humanlikeHairSetAverage;
			}
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00062660 File Offset: 0x00060860
		public Material HeadMatAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false, bool portrait = false, bool allowOverride = true)
		{
			Material material = null;
			if (bodyCondition == RotDrawMode.Fresh)
			{
				if (stump)
				{
					material = this.headStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.headGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Rotting)
			{
				if (stump)
				{
					material = this.desiccatedHeadStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.desiccatedHeadGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Dessicated && !stump)
			{
				material = this.skullGraphic.MatAt(facing, null);
			}
			if (material != null && allowOverride)
			{
				if (!portrait && this.pawn.IsInvisible())
				{
					material = InvisibilityMatPool.GetInvisibleMat(material);
				}
				material = this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00062704 File Offset: 0x00060904
		public Material HairMatAt(Rot4 facing, bool portrait = false, bool cached = false)
		{
			if (this.hairGraphic == null)
			{
				return null;
			}
			Material material = this.hairGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			if (!cached)
			{
				return this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00062754 File Offset: 0x00060954
		public Material BeardMatAt(Rot4 facing, bool portrait = false, bool cached = false)
		{
			if (this.beardGraphic == null)
			{
				return null;
			}
			Material material = this.beardGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			if (!cached)
			{
				return this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000627A1 File Offset: 0x000609A1
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000627D9 File Offset: 0x000609D9
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x000627E4 File Offset: 0x000609E4
		public void ResolveAllGraphics()
		{
			this.ClearCache();
			if (this.pawn.RaceProps.Humanlike)
			{
				Color color = this.pawn.story.SkinColorOverriden ? (PawnGraphicSet.RottingColorDefault * this.pawn.story.SkinColor) : PawnGraphicSet.RottingColorDefault;
				this.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden), Vector2.one, this.pawn.story.SkinColor);
				this.rottingGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden), Vector2.one, color);
				this.dessicatedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyDessicatedGraphicPath, ShaderDatabase.Cutout);
				if (this.pawn.style != null && ModsConfig.IdeologyActive)
				{
					Color skinColor = this.pawn.story.SkinColor;
					skinColor.a *= 0.8f;
					if (this.pawn.style.FaceTattoo != null && this.pawn.style.FaceTattoo != TattooDefOf.NoTattoo_Face)
					{
						this.faceTattooGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.style.FaceTattoo.texPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, skinColor, Color.white, null, this.pawn.story.HeadGraphicPath);
					}
					else
					{
						this.faceTattooGraphic = null;
					}
					if (this.pawn.style.BodyTattoo != null && this.pawn.style.BodyTattoo != TattooDefOf.NoTattoo_Body)
					{
						this.bodyTattooGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.style.BodyTattoo.texPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, skinColor, Color.white, null, this.pawn.story.bodyType.bodyNakedGraphicPath);
					}
					else
					{
						this.bodyTattooGraphic = null;
					}
				}
				this.headGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, this.pawn.story.SkinColor, this.pawn.story.SkinColorOverriden);
				this.desiccatedHeadGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, color, this.pawn.story.SkinColorOverriden);
				this.skullGraphic = GraphicDatabaseHeadRecords.GetSkull();
				this.headStumpGraphic = GraphicDatabaseHeadRecords.GetStump(this.pawn.story.SkinColor);
				this.desiccatedHeadStumpGraphic = GraphicDatabaseHeadRecords.GetStump(color);
				if (this.pawn.story.hairDef != null)
				{
					this.hairGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.hairDef.texPath, ShaderDatabase.Transparent, Vector2.one, this.pawn.story.hairColor);
				}
				if (this.pawn.style != null && this.pawn.style.beardDef != null)
				{
					this.beardGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.style.beardDef.texPath, ShaderDatabase.Transparent, Vector2.one, this.pawn.story.hairColor);
				}
				this.ResolveApparelGraphics();
				return;
			}
			PawnKindLifeStage curKindLifeStage = this.pawn.ageTracker.CurKindLifeStage;
			if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
			{
				this.nakedGraphic = curKindLifeStage.bodyGraphicData.Graphic;
			}
			else
			{
				this.nakedGraphic = curKindLifeStage.femaleGraphicData.Graphic;
			}
			if (this.pawn.RaceProps.packAnimal)
			{
				this.packGraphic = GraphicDatabase.Get<Graphic_Multi>(this.nakedGraphic.path + "Pack", ShaderDatabase.Cutout, this.nakedGraphic.drawSize, Color.white);
			}
			Shader newShader = (this.pawn.story == null) ? ShaderDatabase.CutoutSkin : ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden);
			this.rottingGraphic = this.nakedGraphic.GetColoredVersion(newShader, PawnGraphicSet.RottingColorDefault, PawnGraphicSet.RottingColorDefault);
			if (curKindLifeStage.dessicatedBodyGraphicData != null)
			{
				if (this.pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid)
				{
					if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
					{
						this.dessicatedGraphic = curKindLifeStage.dessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, PawnGraphicSet.DessicatedColorInsect, PawnGraphicSet.DessicatedColorInsect);
					}
					else
					{
						this.dessicatedGraphic = curKindLifeStage.femaleDessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, PawnGraphicSet.DessicatedColorInsect, PawnGraphicSet.DessicatedColorInsect);
					}
				}
				else if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
				{
					this.dessicatedGraphic = curKindLifeStage.dessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
				else
				{
					this.dessicatedGraphic = curKindLifeStage.femaleDessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
			}
			if (!this.pawn.kindDef.alternateGraphics.NullOrEmpty<AlternateGraphic>())
			{
				Rand.PushState(this.pawn.thingIDNumber ^ 46101);
				if (Rand.Value <= this.pawn.kindDef.alternateGraphicChance)
				{
					this.nakedGraphic = this.pawn.kindDef.alternateGraphics.RandomElementByWeight((AlternateGraphic x) => x.Weight).GetGraphic(this.nakedGraphic);
				}
				Rand.PopState();
			}
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00062D87 File Offset: 0x00060F87
		public void SetAllGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveAllGraphics();
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00062D98 File Offset: 0x00060F98
		public void ResolveApparelGraphics()
		{
			this.ClearCache();
			this.apparelGraphics.Clear();
			using (List<Apparel>.Enumerator enumerator = this.pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApparelGraphicRecord item;
					if (ApparelGraphicRecordGetter.TryGetGraphicApparel(enumerator.Current, this.pawn.story.bodyType, out item))
					{
						this.apparelGraphics.Add(item);
					}
				}
			}
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00062E24 File Offset: 0x00061024
		public void SetApparelGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveApparelGraphics();
			}
		}

		// Token: 0x04000D37 RID: 3383
		public Pawn pawn;

		// Token: 0x04000D38 RID: 3384
		public Graphic nakedGraphic;

		// Token: 0x04000D39 RID: 3385
		public Graphic rottingGraphic;

		// Token: 0x04000D3A RID: 3386
		public Graphic dessicatedGraphic;

		// Token: 0x04000D3B RID: 3387
		public Graphic packGraphic;

		// Token: 0x04000D3C RID: 3388
		public DamageFlasher flasher;

		// Token: 0x04000D3D RID: 3389
		public Graphic headGraphic;

		// Token: 0x04000D3E RID: 3390
		public Graphic desiccatedHeadGraphic;

		// Token: 0x04000D3F RID: 3391
		public Graphic skullGraphic;

		// Token: 0x04000D40 RID: 3392
		public Graphic headStumpGraphic;

		// Token: 0x04000D41 RID: 3393
		public Graphic desiccatedHeadStumpGraphic;

		// Token: 0x04000D42 RID: 3394
		public Graphic hairGraphic;

		// Token: 0x04000D43 RID: 3395
		public Graphic beardGraphic;

		// Token: 0x04000D44 RID: 3396
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		// Token: 0x04000D45 RID: 3397
		public Graphic bodyTattooGraphic;

		// Token: 0x04000D46 RID: 3398
		public Graphic faceTattooGraphic;

		// Token: 0x04000D47 RID: 3399
		private List<Material> cachedMatsBodyBase = new List<Material>();

		// Token: 0x04000D48 RID: 3400
		private int cachedMatsBodyBaseHash = -1;

		// Token: 0x04000D49 RID: 3401
		public static readonly Color RottingColorDefault = new Color(0.34f, 0.32f, 0.3f);

		// Token: 0x04000D4A RID: 3402
		public static readonly Color DessicatedColorInsect = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x04000D4B RID: 3403
		private const float TattooOpacity = 0.8f;
	}
}
