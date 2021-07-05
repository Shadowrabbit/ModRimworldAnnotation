using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CE RID: 462
	public class Section
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00048104 File Offset: 0x00046304
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

		// Token: 0x06000D58 RID: 3416 RVA: 0x00048160 File Offset: 0x00046360
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				SectionLayer sectionLayer = (SectionLayer)Activator.CreateInstance(type, new object[]
				{
					this
				});
				this.layers.Add(sectionLayer);
				SectionLayer_SunShadows sectionLayer_SunShadows;
				if ((sectionLayer_SunShadows = (sectionLayer as SectionLayer_SunShadows)) != null)
				{
					this.layerSunShadows = sectionLayer_SunShadows;
				}
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0004820C File Offset: 0x0004640C
		public void DrawSection(bool drawSunShadowsOnly)
		{
			if (drawSunShadowsOnly)
			{
				this.layerSunShadows.DrawLayer();
			}
			else
			{
				int count = this.layers.Count;
				for (int i = 0; i < count; i++)
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

		// Token: 0x06000D5A RID: 3418 RVA: 0x000482C4 File Offset: 0x000464C4
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
						}));
					}
				}
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00048358 File Offset: 0x00046558
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
						}));
					}
				}
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000483DC File Offset: 0x000465DC
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}

		// Token: 0x04000AFF RID: 2815
		public IntVec3 botLeft;

		// Token: 0x04000B00 RID: 2816
		public Map map;

		// Token: 0x04000B01 RID: 2817
		public MapMeshFlag dirtyFlags;

		// Token: 0x04000B02 RID: 2818
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04000B03 RID: 2819
		private bool foundRect;

		// Token: 0x04000B04 RID: 2820
		private CellRect calculatedRect;

		// Token: 0x04000B05 RID: 2821
		private SectionLayer_SunShadows layerSunShadows;

		// Token: 0x04000B06 RID: 2822
		public const int Size = 17;
	}
}
