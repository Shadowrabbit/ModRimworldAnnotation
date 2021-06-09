using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028F RID: 655
	public class Section
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x000BD38C File Offset: 0x000BB58C
		public CellRect CellRect
		{
			get
			{
				if (!this.foundRect)
				{
					this.calculatedRect = new CellRect(this.botLeft.x, this.botLeft.z, 17, 17);
					this.calculatedRect.ClipInsideMap(this.map);
					this.foundRect = true;
				}
				return this.calculatedRect;
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000BD3E8 File Offset: 0x000BB5E8
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				this.layers.Add((SectionLayer)Activator.CreateInstance(type, new object[]
				{
					this
				}));
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000BD480 File Offset: 0x000BB680
		public void DrawSection(bool drawSunShadowsOnly)
		{
			int count = this.layers.Count;
			for (int i = 0; i < count; i++)
			{
				if (!drawSunShadowsOnly || this.layers[i] is SectionLayer_SunShadows)
				{
					this.layers[i].DrawLayer();
				}
			}
			if (!drawSunShadowsOnly && DebugViewSettings.drawSectionEdges)
			{
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(0f, 0f, 17f));
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(17f, 0f, 0f));
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000BD540 File Offset: 0x000BB740
		public void RegenerateAllLayers()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					try
					{
						this.layers[i].Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							this.layers[i].ToStringSafe<SectionLayer>(),
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x000BD5D8 File Offset: 0x000BB7D8
		public void RegenerateLayers(MapMeshFlag changeType)
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				SectionLayer sectionLayer = this.layers[i];
				if ((sectionLayer.relevantChangeTypes & changeType) != MapMeshFlag.None)
				{
					try
					{
						sectionLayer.Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							sectionLayer.ToStringSafe<SectionLayer>(),
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000BD65C File Offset: 0x000BB85C
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}

		// Token: 0x04000DDD RID: 3549
		public IntVec3 botLeft;

		// Token: 0x04000DDE RID: 3550
		public Map map;

		// Token: 0x04000DDF RID: 3551
		public MapMeshFlag dirtyFlags;

		// Token: 0x04000DE0 RID: 3552
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04000DE1 RID: 3553
		private bool foundRect;

		// Token: 0x04000DE2 RID: 3554
		private CellRect calculatedRect;

		// Token: 0x04000DE3 RID: 3555
		public const int Size = 17;
	}
}
