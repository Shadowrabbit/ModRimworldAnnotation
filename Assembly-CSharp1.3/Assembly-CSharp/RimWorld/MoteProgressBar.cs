using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B6 RID: 4278
	[StaticConstructorOnStartup]
	public class MoteProgressBar : MoteDualAttached
	{
		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x0600663B RID: 26171 RVA: 0x00228598 File Offset: 0x00226798
		protected virtual bool OnlyShowForClosestZoom
		{
			get
			{
				return !this.alwaysShow;
			}
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x002285A4 File Offset: 0x002267A4
		public override void Draw()
		{
			base.UpdatePositionAndRotation();
			if (!this.OnlyShowForClosestZoom || Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.exactPosition;
				r.center.z = r.center.z + this.offsetZ;
				r.size = new Vector2(this.exactScale.x, this.exactScale.z);
				r.fillPercent = this.progress;
				r.filledMat = MoteProgressBar.FilledMat;
				r.unfilledMat = MoteProgressBar.UnfilledMat;
				r.margin = 0.12f;
				if (this.offsetZ >= -0.8f && this.offsetZ <= -0.3f && this.AnyThingWithQualityHere())
				{
					r.center.z = r.center.z + 0.25f;
				}
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x0022868C File Offset: 0x0022688C
		private bool AnyThingWithQualityHere()
		{
			IntVec3 c = this.exactPosition.ToIntVec3();
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<CompQuality>() != null && (thingList[i].DrawPos - this.exactPosition).MagnitudeHorizontalSquared() < 0.0001f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040039B4 RID: 14772
		public float progress;

		// Token: 0x040039B5 RID: 14773
		public float offsetZ;

		// Token: 0x040039B6 RID: 14774
		public bool alwaysShow;

		// Token: 0x040039B7 RID: 14775
		private static readonly Material UnfilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 0.65f), ShaderDatabase.MetaOverlay);

		// Token: 0x040039B8 RID: 14776
		private static readonly Material FilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f, 0.65f), ShaderDatabase.MetaOverlay);
	}
}
