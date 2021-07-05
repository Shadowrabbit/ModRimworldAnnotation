using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000194 RID: 404
	public class FleckDef : Def
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0003E243 File Offset: 0x0003C443
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0003E259 File Offset: 0x0003C459
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.fleckSystemClass == null)
			{
				yield return "FleckDef without system class type set!";
			}
			else if (!typeof(FleckSystem).IsAssignableFrom(this.fleckSystemClass))
			{
				yield return "FleckDef has system class type assigned which is not assignable to FleckSystemBase!";
			}
			if (this.graphicData == null && this.randomGraphics.NullOrEmpty<GraphicData>())
			{
				yield return "Fleck graphic data and random graphics are null!";
			}
			else if (this.graphicData != null && !typeof(Graphic_Fleck).IsAssignableFrom(this.graphicData.graphicClass))
			{
				yield return "Fleck graphic class is not derived from Graphic_Fleck!";
			}
			else if (!this.randomGraphics.NullOrEmpty<GraphicData>())
			{
				if (this.randomGraphics.Any((GraphicData g) => !typeof(Graphic_Fleck).IsAssignableFrom(g.graphicClass)))
				{
					yield return "random fleck graphic class is not derived from Graphic_Fleck!";
				}
			}
			yield break;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003E26C File Offset: 0x0003C46C
		public GraphicData GetGraphicData(int id)
		{
			if (this.graphicData != null)
			{
				return this.graphicData;
			}
			Rand.PushState(id);
			GraphicData result;
			try
			{
				result = this.randomGraphics.RandomElement<GraphicData>();
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x0400096D RID: 2413
		public Type fleckSystemClass;

		// Token: 0x0400096E RID: 2414
		public AltitudeLayer altitudeLayer;

		// Token: 0x0400096F RID: 2415
		public float altitudeLayerIncOffset;

		// Token: 0x04000970 RID: 2416
		public bool drawGUIOverlay;

		// Token: 0x04000971 RID: 2417
		public GraphicData graphicData;

		// Token: 0x04000972 RID: 2418
		public List<GraphicData> randomGraphics;

		// Token: 0x04000973 RID: 2419
		public bool realTime;

		// Token: 0x04000974 RID: 2420
		public bool attachedToHead;

		// Token: 0x04000975 RID: 2421
		public float fadeInTime;

		// Token: 0x04000976 RID: 2422
		public float solidTime = 1f;

		// Token: 0x04000977 RID: 2423
		public float fadeOutTime;

		// Token: 0x04000978 RID: 2424
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x04000979 RID: 2425
		public float speedPerTime;

		// Token: 0x0400097A RID: 2426
		public float growthRate;

		// Token: 0x0400097B RID: 2427
		public bool collide;

		// Token: 0x0400097C RID: 2428
		public SoundDef landSound;

		// Token: 0x0400097D RID: 2429
		public Vector3 unattachedDrawOffset;

		// Token: 0x0400097E RID: 2430
		public Vector3 attachedDrawOffset;

		// Token: 0x0400097F RID: 2431
		public bool rotateTowardsMoveDirection;
	}
}
