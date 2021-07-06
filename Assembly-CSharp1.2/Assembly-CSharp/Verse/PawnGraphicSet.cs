using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000388 RID: 904
	public class PawnGraphicSet
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600169B RID: 5787 RVA: 0x00016042 File Offset: 0x00014242
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x000D7474 File Offset: 0x000D5674
		public List<Material> MatsBodyBaseAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh)
		{
			int num = facing.AsInt + 1000 * (int)bodyCondition;
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
				for (int i = 0; i < this.apparelGraphics.Count; i++)
				{
					if ((this.apparelGraphics[i].sourceApparel.def.apparel.shellRenderedBehindHead || this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Shell) && !PawnRenderer.RenderAsPack(this.apparelGraphics[i].sourceApparel) && this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Overhead)
					{
						this.cachedMatsBodyBase.Add(this.apparelGraphics[i].graphic.MatAt(facing, null));
					}
				}
			}
			return this.cachedMatsBodyBase;
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x000D75DC File Offset: 0x000D57DC
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
				Log.Error("Unknown crown type: " + this.pawn.story.crownType, false);
				return MeshPool.humanlikeHairSetAverage;
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x000D7648 File Offset: 0x000D5848
		public Material HeadMatAt_NewTemp(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false, bool portrait = false)
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
			if (material != null)
			{
				if (!portrait && this.pawn.IsInvisible())
				{
					material = InvisibilityMatPool.GetInvisibleMat(material);
				}
				material = this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0001604D File Offset: 0x0001424D
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public Material HeadMatAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false)
		{
			return this.HeadMatAt_NewTemp(facing, bodyCondition, stump, false);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x000D76E8 File Offset: 0x000D58E8
		public Material HairMatAt_NewTemp(Rot4 facing, bool portrait = false)
		{
			Material baseMat = this.hairGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				baseMat = InvisibilityMatPool.GetInvisibleMat(baseMat);
			}
			return this.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00016059 File Offset: 0x00014259
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public Material HairMatAt(Rot4 facing)
		{
			return this.HairMatAt_NewTemp(facing, false);
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00016063 File Offset: 0x00014263
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0001609B File Offset: 0x0001429B
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x000D7728 File Offset: 0x000D5928
		public void ResolveAllGraphics()
		{
			this.ClearCache();
			if (this.pawn.RaceProps.Humanlike)
			{
				this.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, this.pawn.story.SkinColor);
				this.rottingGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, PawnGraphicSet.RottingColor);
				this.dessicatedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyDessicatedGraphicPath, ShaderDatabase.Cutout);
				this.headGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, this.pawn.story.SkinColor);
				this.desiccatedHeadGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, PawnGraphicSet.RottingColor);
				this.skullGraphic = GraphicDatabaseHeadRecords.GetSkull();
				this.headStumpGraphic = GraphicDatabaseHeadRecords.GetStump(this.pawn.story.SkinColor);
				this.desiccatedHeadStumpGraphic = GraphicDatabaseHeadRecords.GetStump(PawnGraphicSet.RottingColor);
				this.hairGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.hairDef.texPath, ShaderDatabase.Transparent, Vector2.one, this.pawn.story.hairColor);
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
			this.rottingGraphic = this.nakedGraphic.GetColoredVersion(ShaderDatabase.CutoutSkin, PawnGraphicSet.RottingColor, PawnGraphicSet.RottingColor);
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

		// Token: 0x060016A5 RID: 5797 RVA: 0x000160A4 File Offset: 0x000142A4
		public void SetAllGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveAllGraphics();
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000D7AAC File Offset: 0x000D5CAC
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

		// Token: 0x060016A7 RID: 5799 RVA: 0x000160B4 File Offset: 0x000142B4
		public void SetApparelGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveApparelGraphics();
			}
		}

		// Token: 0x0400116B RID: 4459
		public Pawn pawn;

		// Token: 0x0400116C RID: 4460
		public Graphic nakedGraphic;

		// Token: 0x0400116D RID: 4461
		public Graphic rottingGraphic;

		// Token: 0x0400116E RID: 4462
		public Graphic dessicatedGraphic;

		// Token: 0x0400116F RID: 4463
		public Graphic packGraphic;

		// Token: 0x04001170 RID: 4464
		public DamageFlasher flasher;

		// Token: 0x04001171 RID: 4465
		public Graphic headGraphic;

		// Token: 0x04001172 RID: 4466
		public Graphic desiccatedHeadGraphic;

		// Token: 0x04001173 RID: 4467
		public Graphic skullGraphic;

		// Token: 0x04001174 RID: 4468
		public Graphic headStumpGraphic;

		// Token: 0x04001175 RID: 4469
		public Graphic desiccatedHeadStumpGraphic;

		// Token: 0x04001176 RID: 4470
		public Graphic hairGraphic;

		// Token: 0x04001177 RID: 4471
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		// Token: 0x04001178 RID: 4472
		private List<Material> cachedMatsBodyBase = new List<Material>();

		// Token: 0x04001179 RID: 4473
		private int cachedMatsBodyBaseHash = -1;

		// Token: 0x0400117A RID: 4474
		public static readonly Color RottingColor = new Color(0.34f, 0.32f, 0.3f);

		// Token: 0x0400117B RID: 4475
		public static readonly Color DessicatedColorInsect = new Color(0.8f, 0.8f, 0.8f);
	}
}
