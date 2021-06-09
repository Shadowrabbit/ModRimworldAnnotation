using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000107 RID: 263
	public class GraphicData
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0000BEBD File Offset: 0x0000A0BD
		public bool Linked
		{
			get
			{
				return this.linkType > LinkDrawerType.None;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
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

		// Token: 0x0600074C RID: 1868 RVA: 0x000919C8 File Offset: 0x0008FBC8
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
			this.cachedGraphic = null;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00091AC0 File Offset: 0x0008FCC0
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
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters);
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000BEDE File Offset: 0x0000A0DE
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00091B70 File Offset: 0x0008FD70
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

		// Token: 0x06000750 RID: 1872 RVA: 0x00091C20 File Offset: 0x0008FE20
		public Graphic GraphicColoredFor(Thing t)
		{
			if (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
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
			yield break;
		}

		// Token: 0x04000482 RID: 1154
		[NoTranslate]
		public string texPath;

		// Token: 0x04000483 RID: 1155
		public Type graphicClass;

		// Token: 0x04000484 RID: 1156
		public ShaderTypeDef shaderType;

		// Token: 0x04000485 RID: 1157
		public List<ShaderParameter> shaderParameters;

		// Token: 0x04000486 RID: 1158
		public Color color = Color.white;

		// Token: 0x04000487 RID: 1159
		public Color colorTwo = Color.white;

		// Token: 0x04000488 RID: 1160
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04000489 RID: 1161
		public Vector3 drawOffset = Vector3.zero;

		// Token: 0x0400048A RID: 1162
		public Vector3? drawOffsetNorth;

		// Token: 0x0400048B RID: 1163
		public Vector3? drawOffsetEast;

		// Token: 0x0400048C RID: 1164
		public Vector3? drawOffsetSouth;

		// Token: 0x0400048D RID: 1165
		public Vector3? drawOffsetWest;

		// Token: 0x0400048E RID: 1166
		public float onGroundRandomRotateAngle;

		// Token: 0x0400048F RID: 1167
		public bool drawRotated = true;

		// Token: 0x04000490 RID: 1168
		public bool allowFlip = true;

		// Token: 0x04000491 RID: 1169
		public float flipExtraRotation;

		// Token: 0x04000492 RID: 1170
		public ShadowData shadowData;

		// Token: 0x04000493 RID: 1171
		public DamageGraphicData damageData;

		// Token: 0x04000494 RID: 1172
		public LinkDrawerType linkType;

		// Token: 0x04000495 RID: 1173
		public LinkFlags linkFlags;

		// Token: 0x04000496 RID: 1174
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
