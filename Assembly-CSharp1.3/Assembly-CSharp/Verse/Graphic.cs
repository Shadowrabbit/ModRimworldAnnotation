using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000344 RID: 836
	public class Graphic
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x0008DFF4 File Offset: 0x0008C1F4
		public Shader Shader
		{
			get
			{
				Material matSingle = this.MatSingle;
				if (matSingle != null)
				{
					return matSingle.shader;
				}
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060017CB RID: 6091 RVA: 0x0008E01D File Offset: 0x0008C21D
		public Graphic_Shadow ShadowGraphic
		{
			get
			{
				if (this.cachedShadowGraphicInt == null && this.data != null && this.data.shadowData != null)
				{
					this.cachedShadowGraphicInt = new Graphic_Shadow(this.data.shadowData);
				}
				return this.cachedShadowGraphicInt;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x0008E058 File Offset: 0x0008C258
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060017CD RID: 6093 RVA: 0x0008E060 File Offset: 0x0008C260
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x0008E068 File Offset: 0x0008C268
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x0008E06F File Offset: 0x0008C26F
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x0008E06F File Offset: 0x0008C26F
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0008E06F File Offset: 0x0008C26F
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x0008E06F File Offset: 0x0008C26F
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0008E077 File Offset: 0x0008C277
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060017D5 RID: 6101 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0008E08C File Offset: 0x0008C28C
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0008E0A4 File Offset: 0x0008C2A4
		public static bool TryGetTextureAtlasReplacementInfo(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors, out Material material, out Vector2[] uvs, out Color32 vertexColor)
		{
			material = mat;
			uvs = null;
			vertexColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Graphic.AtlasReplacementInfoCacheKey key = new Graphic.AtlasReplacementInfoCacheKey(mat, group, flipUv, vertexColors);
			Graphic.CachedAtlasReplacementInfo cachedAtlasReplacementInfo;
			if (Graphic.replacementInfoCache.TryGetValue(key, out cachedAtlasReplacementInfo))
			{
				material = cachedAtlasReplacementInfo.material;
				uvs = cachedAtlasReplacementInfo.uvs;
				if (vertexColors)
				{
					vertexColor = cachedAtlasReplacementInfo.vertexColor;
				}
				return true;
			}
			StaticTextureAtlasTile staticTextureAtlasTile;
			if (!GlobalTextureAtlasManager.TryGetStaticTile(group, (Texture2D)mat.mainTexture, out staticTextureAtlasTile, false))
			{
				return false;
			}
			MaterialRequest materialRequest;
			if (!MaterialPool.TryGetRequestForMat(mat, out materialRequest))
			{
				Log.Error("Tried getting texture atlas replacement info for a material that was not created by MaterialPool!");
				return false;
			}
			uvs = new Vector2[4];
			Printer_Plane.GetUVs(staticTextureAtlasTile.uvRect, out uvs[0], out uvs[1], out uvs[2], out uvs[3], flipUv);
			materialRequest.mainTex = staticTextureAtlasTile.atlas.ColorTexture;
			if (vertexColors)
			{
				vertexColor = materialRequest.color;
				materialRequest.color = Color.white;
			}
			if (materialRequest.maskTex != null)
			{
				materialRequest.maskTex = staticTextureAtlasTile.atlas.MaskTexture;
			}
			material = MaterialPool.MatFrom(materialRequest);
			Graphic.replacementInfoCache.Add(key, new Graphic.CachedAtlasReplacementInfo
			{
				material = material,
				uvs = uvs,
				vertexColor = vertexColor
			});
			return true;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0008E215 File Offset: 0x0008C415
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0008E238 File Offset: 0x0008C438
		public virtual Material MatAt(Rot4 rot, Thing thing = null)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.MatNorth;
			case 1:
				return this.MatEast;
			case 2:
				return this.MatSouth;
			case 3:
				return this.MatWest;
			default:
				return BaseContent.BadMat;
			}
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0008E288 File Offset: 0x0008C488
		public virtual Mesh MeshAt(Rot4 rot)
		{
			Vector2 vector = this.drawSize;
			if (rot.IsHorizontal && !this.ShouldDrawRotated)
			{
				vector = vector.Rotated();
			}
			if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
			{
				return MeshPool.GridPlaneFlip(vector);
			}
			return MeshPool.GridPlane(vector);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0008E06F File Offset: 0x0008C26F
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0008E2EB File Offset: 0x0008C4EB
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0008E307 File Offset: 0x0008C507
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0008E31A File Offset: 0x0008C51A
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0008E328 File Offset: 0x0008C528
		public virtual void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			Quaternion quaternion = this.QuatFromRot(rot);
			if (extraRotation != 0f)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			loc += this.DrawOffset(rot);
			Material mat = this.MatAt(rot, thing);
			this.DrawMeshInt(mesh, loc, quaternion, mat);
			if (this.ShadowGraphic != null)
			{
				this.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0008E3A3 File Offset: 0x0008C5A3
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0008E3B0 File Offset: 0x0008C5B0
		public virtual void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector2 size;
			bool flag;
			if (this.ShouldDrawRotated)
			{
				size = this.drawSize;
				flag = false;
			}
			else
			{
				if (!thing.Rotation.IsHorizontal)
				{
					size = this.drawSize;
				}
				else
				{
					size = this.drawSize.Rotated();
				}
				flag = ((thing.Rotation == Rot4.West && this.WestFlipped) || (thing.Rotation == Rot4.East && this.EastFlipped));
			}
			float num = this.AngleFromRot(thing.Rotation) + extraRotation;
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Vector3 center = thing.TrueCenter() + this.DrawOffset(thing.Rotation);
			Material mat = this.MatAt(thing.Rotation, thing);
			Vector2[] uvs;
			Color32 color;
			Graphic.TryGetTextureAtlasReplacementInfo(mat, thing.def.category.ToAtlasGroup(), flag, true, out mat, out uvs, out color);
			Printer_Plane.PrintPlane(layer, center, size, mat, num, flag, uvs, new Color32[]
			{
				color,
				color,
				color,
				color
			}, 0.01f, 0f);
			if (this.ShadowGraphic != null && thing != null)
			{
				this.ShadowGraphic.Print(layer, thing, 0f);
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0008E4FD File Offset: 0x0008C6FD
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300);
			return BaseContent.BadGraphic;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0008E523 File Offset: 0x0008C723
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, this.Shader, newDrawSize, this.color, this.colorTwo, null);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0008E54C File Offset: 0x0008C74C
		public virtual Graphic GetShadowlessGraphic()
		{
			if (this.data == null || this.data.shadowData == null)
			{
				return this;
			}
			if (this.cachedShadowlessGraphicInt == null)
			{
				GraphicData graphicData = new GraphicData();
				graphicData.CopyFrom(this.data);
				graphicData.shadowData = null;
				this.cachedShadowlessGraphicInt = graphicData.Graphic;
			}
			return this.cachedShadowlessGraphicInt;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0008E5A4 File Offset: 0x0008C7A4
		protected float AngleFromRot(Rot4 rot)
		{
			if (this.ShouldDrawRotated)
			{
				float num = rot.AsAngle;
				num += this.DrawRotatedExtraAngleOffset;
				if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
				{
					num += 180f;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0008E604 File Offset: 0x0008C804
		protected Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x0400105E RID: 4190
		public GraphicData data;

		// Token: 0x0400105F RID: 4191
		public string path;

		// Token: 0x04001060 RID: 4192
		public string maskPath;

		// Token: 0x04001061 RID: 4193
		public Color color = Color.white;

		// Token: 0x04001062 RID: 4194
		public Color colorTwo = Color.white;

		// Token: 0x04001063 RID: 4195
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04001064 RID: 4196
		private Graphic_Shadow cachedShadowGraphicInt;

		// Token: 0x04001065 RID: 4197
		private Graphic cachedShadowlessGraphicInt;

		// Token: 0x04001066 RID: 4198
		private static Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo> replacementInfoCache = new Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo>();

		// Token: 0x02001A66 RID: 6758
		private struct AtlasReplacementInfoCacheKey : IEquatable<Graphic.AtlasReplacementInfoCacheKey>
		{
			// Token: 0x06009CCA RID: 40138 RVA: 0x0036A7C8 File Offset: 0x003689C8
			public AtlasReplacementInfoCacheKey(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors)
			{
				this.mat = mat;
				this.group = group;
				this.flipUv = flipUv;
				this.vertexColors = vertexColors;
				this.hash = Gen.HashCombine<int>(mat.GetHashCode(), group.GetHashCode());
				if (flipUv)
				{
					this.hash = ~this.hash;
				}
				if (vertexColors)
				{
					this.hash ^= 123893723;
				}
			}

			// Token: 0x06009CCB RID: 40139 RVA: 0x0036A836 File Offset: 0x00368A36
			public bool Equals(Graphic.AtlasReplacementInfoCacheKey other)
			{
				return this.mat == other.mat && this.group == other.group && this.flipUv == other.flipUv && this.vertexColors == other.vertexColors;
			}

			// Token: 0x06009CCC RID: 40140 RVA: 0x0036A872 File Offset: 0x00368A72
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x04006505 RID: 25861
			public readonly Material mat;

			// Token: 0x04006506 RID: 25862
			public readonly TextureAtlasGroup group;

			// Token: 0x04006507 RID: 25863
			public readonly bool flipUv;

			// Token: 0x04006508 RID: 25864
			public readonly bool vertexColors;

			// Token: 0x04006509 RID: 25865
			private readonly int hash;
		}

		// Token: 0x02001A67 RID: 6759
		private struct CachedAtlasReplacementInfo
		{
			// Token: 0x0400650A RID: 25866
			public Material material;

			// Token: 0x0400650B RID: 25867
			public Vector2[] uvs;

			// Token: 0x0400650C RID: 25868
			public Color32 vertexColor;
		}
	}
}
