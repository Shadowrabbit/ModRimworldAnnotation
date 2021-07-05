using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A68 RID: 2664
	public class FleshTypeDef : Def
	{
		// Token: 0x06003FFE RID: 16382 RVA: 0x0015AE70 File Offset: 0x00159070
		public FleshTypeDef.ResolvedWound ChooseBandagedOverlay()
		{
			if (this.bandagedWounds == null)
			{
				return null;
			}
			if (this.bandagedWoundsResolved == null)
			{
				this.bandagedWoundsResolved = (from wound in this.bandagedWounds
				select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
			}
			return this.bandagedWoundsResolved.RandomElement<FleshTypeDef.ResolvedWound>();
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0015AED0 File Offset: 0x001590D0
		public FleshTypeDef.ResolvedWound ChooseWoundOverlay(Hediff hediff)
		{
			if (this.genericWounds == null)
			{
				return null;
			}
			if (this.hediffWounds != null)
			{
				foreach (FleshTypeDef.HediffWound hediffWound in this.hediffWounds)
				{
					if (hediffWound.hediff == hediff.def)
					{
						FleshTypeDef.ResolvedWound resolvedWound = hediffWound.ChooseWoundOverlay(hediff);
						if (resolvedWound != null)
						{
							return resolvedWound;
						}
					}
				}
				if (hediff.IsTended())
				{
					return this.ChooseBandagedOverlay();
				}
			}
			if (!(hediff is Hediff_Injury))
			{
				return null;
			}
			if (hediff.IsTended())
			{
				return this.ChooseBandagedOverlay();
			}
			if (this.woundsResolved == null)
			{
				this.woundsResolved = (from wound in this.genericWounds
				select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
			}
			return this.woundsResolved.RandomElement<FleshTypeDef.ResolvedWound>();
		}

		// Token: 0x0400243E RID: 9278
		public ThoughtDef ateDirect;

		// Token: 0x0400243F RID: 9279
		public ThoughtDef ateAsIngredient;

		// Token: 0x04002440 RID: 9280
		public ThingCategoryDef corpseCategory;

		// Token: 0x04002441 RID: 9281
		public EffecterDef damageEffecter;

		// Token: 0x04002442 RID: 9282
		public List<FleshTypeDef.Wound> genericWounds;

		// Token: 0x04002443 RID: 9283
		public List<FleshTypeDef.Wound> bandagedWounds;

		// Token: 0x04002444 RID: 9284
		public List<FleshTypeDef.HediffWound> hediffWounds;

		// Token: 0x04002445 RID: 9285
		private List<FleshTypeDef.ResolvedWound> bandagedWoundsResolved;

		// Token: 0x04002446 RID: 9286
		private List<FleshTypeDef.ResolvedWound> woundsResolved;

		// Token: 0x02002013 RID: 8211
		public class HediffWound
		{
			// Token: 0x0600B7AB RID: 47019 RVA: 0x003BADDC File Offset: 0x003B8FDC
			public FleshTypeDef.ResolvedWound ChooseWoundOverlay(Hediff hediff)
			{
				if (this.wounds == null || (this.onlyHumanlikes && !hediff.pawn.RaceProps.Humanlike))
				{
					return null;
				}
				if (this.woundsResolved == null)
				{
					this.woundsResolved = (from wound in this.wounds
					select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
				}
				return (from w in this.woundsResolved
				where w.wound.Fits(hediff)
				select w).RandomElementWithFallback(null);
			}

			// Token: 0x04007AE4 RID: 31460
			public bool onlyHumanlikes = true;

			// Token: 0x04007AE5 RID: 31461
			public HediffDef hediff;

			// Token: 0x04007AE6 RID: 31462
			public List<FleshTypeDef.Wound> wounds;

			// Token: 0x04007AE7 RID: 31463
			private List<FleshTypeDef.ResolvedWound> woundsResolved;
		}

		// Token: 0x02002014 RID: 8212
		public class Wound
		{
			// Token: 0x0600B7AD RID: 47021 RVA: 0x003BAE88 File Offset: 0x003B9088
			public bool Fits(Hediff hediff)
			{
				Hediff_MissingPart hediff_MissingPart;
				return (this.onlyOnPart == null || (hediff.Part != null && hediff.Part.def == this.onlyOnPart)) && (this.missingBodyPartFresh == null || (hediff_MissingPart = (hediff as Hediff_MissingPart)) == null || this.missingBodyPartFresh.Value == (hediff_MissingPart.IsFresh || hediff_MissingPart.Bleeding));
			}

			// Token: 0x0600B7AE RID: 47022 RVA: 0x003BAEF4 File Offset: 0x003B90F4
			public FleshTypeDef.ResolvedWound Resolve()
			{
				Shader shader = this.tintWithSkinColor ? ShaderDatabase.WoundSkin : ShaderDatabase.Wound;
				if (this.texture != null)
				{
					return new FleshTypeDef.ResolvedWound(this, MaterialPool.MatFrom(this.texture, shader, this.color));
				}
				return new FleshTypeDef.ResolvedWound(this, MaterialPool.MatFrom(this.textureSouth, shader, this.color), MaterialPool.MatFrom(this.textureEast, shader, this.color), MaterialPool.MatFrom(this.textureNorth, shader, this.color), MaterialPool.MatFrom(this.textureWest, shader, this.color), this.flipSouth, this.flipEast, this.flipNorth, this.flipWest);
			}

			// Token: 0x04007AE8 RID: 31464
			[NoTranslate]
			public string texture;

			// Token: 0x04007AE9 RID: 31465
			[NoTranslate]
			public string textureSouth;

			// Token: 0x04007AEA RID: 31466
			[NoTranslate]
			public string textureEast;

			// Token: 0x04007AEB RID: 31467
			[NoTranslate]
			public string textureNorth;

			// Token: 0x04007AEC RID: 31468
			[NoTranslate]
			public string textureWest;

			// Token: 0x04007AED RID: 31469
			public bool flipSouth;

			// Token: 0x04007AEE RID: 31470
			public bool flipEast;

			// Token: 0x04007AEF RID: 31471
			public bool flipNorth;

			// Token: 0x04007AF0 RID: 31472
			public bool flipWest;

			// Token: 0x04007AF1 RID: 31473
			public float scale = 1f;

			// Token: 0x04007AF2 RID: 31474
			public Color color = Color.white;

			// Token: 0x04007AF3 RID: 31475
			public bool tintWithSkinColor;

			// Token: 0x04007AF4 RID: 31476
			[NoTranslate]
			public string flipOnWoundAnchorTag;

			// Token: 0x04007AF5 RID: 31477
			public Rot4 flipOnRotation;

			// Token: 0x04007AF6 RID: 31478
			public BodyPartDef onlyOnPart;

			// Token: 0x04007AF7 RID: 31479
			public bool displayOverApparel = true;

			// Token: 0x04007AF8 RID: 31480
			public bool displayPermanent;

			// Token: 0x04007AF9 RID: 31481
			public bool? missingBodyPartFresh;
		}

		// Token: 0x02002015 RID: 8213
		public class ResolvedWound
		{
			// Token: 0x0600B7B0 RID: 47024 RVA: 0x003BAFC4 File Offset: 0x003B91C4
			public ResolvedWound(FleshTypeDef.Wound wound, Material material)
			{
				this.wound = wound;
				this.matWest = material;
				this.matNorth = material;
				this.matEast = material;
				this.matSouth = material;
			}

			// Token: 0x0600B7B1 RID: 47025 RVA: 0x003BB000 File Offset: 0x003B9200
			public ResolvedWound(FleshTypeDef.Wound wound, Material matSouth, Material matEast, Material matNorth, Material matWest, bool flipSouth, bool flipEast, bool flipNorth, bool flipWest)
			{
				this.wound = wound;
				this.matSouth = matSouth;
				this.matEast = matEast;
				this.matNorth = matNorth;
				this.matWest = matWest;
				this.flipSouth = flipSouth;
				this.flipEast = flipEast;
				this.flipNorth = flipNorth;
				this.flipWest = flipWest;
			}

			// Token: 0x0600B7B2 RID: 47026 RVA: 0x003BB058 File Offset: 0x003B9258
			public Material GetMaterial(Rot4 rotation, out bool flip)
			{
				flip = false;
				if (rotation == Rot4.South)
				{
					flip = this.flipSouth;
					return this.matSouth;
				}
				if (rotation == Rot4.East)
				{
					flip = this.flipEast;
					return this.matEast;
				}
				if (rotation == Rot4.West)
				{
					flip = this.flipWest;
					return this.matWest;
				}
				if (rotation == Rot4.North)
				{
					flip = this.flipNorth;
					return this.matNorth;
				}
				return this.matSouth;
			}

			// Token: 0x04007AFA RID: 31482
			public Material material;

			// Token: 0x04007AFB RID: 31483
			public Material matSouth;

			// Token: 0x04007AFC RID: 31484
			public Material matEast;

			// Token: 0x04007AFD RID: 31485
			public Material matNorth;

			// Token: 0x04007AFE RID: 31486
			public Material matWest;

			// Token: 0x04007AFF RID: 31487
			public bool flipSouth;

			// Token: 0x04007B00 RID: 31488
			public bool flipEast;

			// Token: 0x04007B01 RID: 31489
			public bool flipNorth;

			// Token: 0x04007B02 RID: 31490
			public bool flipWest;

			// Token: 0x04007B03 RID: 31491
			public FleshTypeDef.Wound wound;
		}
	}
}
