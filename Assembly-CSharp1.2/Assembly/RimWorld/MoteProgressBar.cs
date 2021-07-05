using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001714 RID: 5908
	[StaticConstructorOnStartup]
	public class MoteProgressBar : MoteDualAttached
	{
		// Token: 0x06008231 RID: 33329 RVA: 0x00269A84 File Offset: 0x00267C84
		public override void Draw()
		{
			base.UpdatePositionAndRotation();
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
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

		// Token: 0x06008232 RID: 33330 RVA: 0x00269B64 File Offset: 0x00267D64
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

		// Token: 0x04005466 RID: 21606
		public float progress;

		// Token: 0x04005467 RID: 21607
		public float offsetZ;

		// Token: 0x04005468 RID: 21608
		private static readonly Material UnfilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 0.65f), ShaderDatabase.MetaOverlay);

		// Token: 0x04005469 RID: 21609
		private static readonly Material FilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f, 0.65f), ShaderDatabase.MetaOverlay);
	}
}
