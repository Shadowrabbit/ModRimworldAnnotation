using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A3 RID: 163
	public class GraphicData
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001B4BD File Offset: 0x000196BD
		public bool Linked
		{
			get
			{
				return this.linkType > LinkDrawerType.None;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001B4C8 File Offset: 0x000196C8
		public Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.Init();
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001B4DE File Offset: 0x000196DE
		public void ExplicitlyInitCachedGraphic()
		{
			this.cachedGraphic = this.Graphic;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001B4EC File Offset: 0x000196EC
		public void CopyFrom(GraphicData other)
		{
			this.texPath = other.texPath;
			this.graphicClass = other.graphicClass;
			this.shaderType = other.shaderType;
			this.color = other.color;
			this.colorTwo = other.colorTwo;
			this.drawSize = other.drawSize;
			this.drawOffset = other.drawOffset;
			this.drawOffsetNorth = other.drawOffsetNorth;
			this.drawOffsetEast = other.drawOffsetEast;
			this.drawOffsetSouth = other.drawOffsetSouth;
			this.drawOffsetWest = other.drawOffsetSouth;
			this.onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
			this.drawRotated = other.drawRotated;
			this.allowFlip = other.allowFlip;
			this.flipExtraRotation = other.flipExtraRotation;
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
			this.asymmetricLink = other.asymmetricLink;
			this.allowAtlasing = other.allowAtlasing;
			this.cachedGraphic = null;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001B5FC File Offset: 0x000197FC
		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
				return;
			}
			ShaderTypeDef cutout = this.shaderType;
			if (cutout == null)
			{
				cutout = ShaderTypeDefOf.Cutout;
			}
			Shader shader = cutout.Shader;
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters, null);
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001B6AB File Offset: 0x000198AB
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001B6C0 File Offset: 0x000198C0
		public Vector3 DrawOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.drawOffsetNorth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.drawOffsetEast;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.drawOffsetSouth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector = this.drawOffsetWest;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.drawOffset;
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001B770 File Offset: 0x00019970
		public Graphic GraphicColoredFor(Thing t)
		{
			if (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001B7D6 File Offset: 0x000199D6
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.graphicClass == null)
			{
				yield return "graphicClass is null";
			}
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is null or empty";
			}
			if (thingDef != null)
			{
				if (thingDef.drawerType == DrawerType.RealtimeOnly && this.Linked)
				{
					yield return "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
				}
				if (!thingDef.rotatable && (this.drawOffsetNorth != null || this.drawOffsetEast != null || this.drawOffsetSouth != null || this.drawOffsetWest != null))
				{
					yield return "not rotatable but has rotational draw offset(s).";
				}
			}
			if ((this.shaderType == ShaderTypeDefOf.Cutout || this.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
			{
				yield return "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
			}
			if (this.linkType == LinkDrawerType.Asymmetric != (this.asymmetricLink != null))
			{
				yield return "linkType=Asymmetric requires <asymmetricLink> and vice versa";
			}
			yield break;
		}

		// Token: 0x040002A9 RID: 681
		[NoTranslate]
		public string texPath;

		// Token: 0x040002AA RID: 682
		public Type graphicClass;

		// Token: 0x040002AB RID: 683
		public ShaderTypeDef shaderType;

		// Token: 0x040002AC RID: 684
		public List<ShaderParameter> shaderParameters;

		// Token: 0x040002AD RID: 685
		public Color color = Color.white;

		// Token: 0x040002AE RID: 686
		public Color colorTwo = Color.white;

		// Token: 0x040002AF RID: 687
		public Vector2 drawSize = Vector2.one;

		// Token: 0x040002B0 RID: 688
		public Vector3 drawOffset = Vector3.zero;

		// Token: 0x040002B1 RID: 689
		public Vector3? drawOffsetNorth;

		// Token: 0x040002B2 RID: 690
		public Vector3? drawOffsetEast;

		// Token: 0x040002B3 RID: 691
		public Vector3? drawOffsetSouth;

		// Token: 0x040002B4 RID: 692
		public Vector3? drawOffsetWest;

		// Token: 0x040002B5 RID: 693
		public float onGroundRandomRotateAngle;

		// Token: 0x040002B6 RID: 694
		public bool drawRotated = true;

		// Token: 0x040002B7 RID: 695
		public bool allowFlip = true;

		// Token: 0x040002B8 RID: 696
		public float flipExtraRotation;

		// Token: 0x040002B9 RID: 697
		public bool renderInstanced;

		// Token: 0x040002BA RID: 698
		public bool allowAtlasing = true;

		// Token: 0x040002BB RID: 699
		public ShadowData shadowData;

		// Token: 0x040002BC RID: 700
		public DamageGraphicData damageData;

		// Token: 0x040002BD RID: 701
		public LinkDrawerType linkType;

		// Token: 0x040002BE RID: 702
		public LinkFlags linkFlags;

		// Token: 0x040002BF RID: 703
		public AsymmetricLinkData asymmetricLink;

		// Token: 0x040002C0 RID: 704
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
