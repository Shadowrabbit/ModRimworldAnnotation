using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D3 RID: 1235
	public class Graphic
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000FDEE8 File Offset: 0x000FC0E8
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

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x0001B3A5 File Offset: 0x000195A5
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

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x0001B3E0 File Offset: 0x000195E0
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x0001B3E8 File Offset: 0x000195E8
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x0001B3F0 File Offset: 0x000195F0
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0001B3F7 File Offset: 0x000195F7
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x0001B3F7 File Offset: 0x000195F7
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0001B3F7 File Offset: 0x000195F7
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x0001B3F7 File Offset: 0x000195F7
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x0001B3FF File Offset: 0x000195FF
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x0001B414 File Offset: 0x00019614
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0001B42B File Offset: 0x0001962B
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928, false);
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000FDF14 File Offset: 0x000FC114
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

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000FDF64 File Offset: 0x000FC164
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

		// Token: 0x06001EDA RID: 7898 RVA: 0x0001B3F7 File Offset: 0x000195F7
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0001B44D File Offset: 0x0001964D
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0001B469 File Offset: 0x00019669
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0001B47C File Offset: 0x0001967C
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000FDFC8 File Offset: 0x000FC1C8
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

		// Token: 0x06001EDF RID: 7903 RVA: 0x0001B48A File Offset: 0x0001968A
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000FE044 File Offset: 0x000FC244
		public virtual void Print(SectionLayer layer, Thing thing)
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
			float num = this.AngleFromRot(thing.Rotation);
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Vector3 center = thing.TrueCenter() + this.DrawOffset(thing.Rotation);
			Printer_Plane.PrintPlane(layer, center, size, this.MatAt(thing.Rotation, thing), num, flag, null, null, 0.01f, 0f);
			if (this.ShadowGraphic != null && thing != null)
			{
				this.ShadowGraphic.Print(layer, thing);
			}
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0001B497 File Offset: 0x00019697
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300, false);
			return BaseContent.BadGraphic;
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0001B4BE File Offset: 0x000196BE
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, this.Shader, newDrawSize, this.color, this.colorTwo);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000FE13C File Offset: 0x000FC33C
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

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000FE194 File Offset: 0x000FC394
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

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000FE1F4 File Offset: 0x000FC3F4
		protected Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x040015D7 RID: 5591
		public GraphicData data;

		// Token: 0x040015D8 RID: 5592
		public string path;

		// Token: 0x040015D9 RID: 5593
		public Color color = Color.white;

		// Token: 0x040015DA RID: 5594
		public Color colorTwo = Color.white;

		// Token: 0x040015DB RID: 5595
		public Vector2 drawSize = Vector2.one;

		// Token: 0x040015DC RID: 5596
		private Graphic_Shadow cachedShadowGraphicInt;

		// Token: 0x040015DD RID: 5597
		private Graphic cachedShadowlessGraphicInt;
	}
}
