using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B8 RID: 6072
	public class CompEmptyStateGraphic : ThingComp
	{
		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x06008639 RID: 34361 RVA: 0x0005A114 File Offset: 0x00058314
		private CompProperties_EmptyStateGraphic Props
		{
			get
			{
				return (CompProperties_EmptyStateGraphic)this.props;
			}
		}

		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x0600863A RID: 34362 RVA: 0x002780A8 File Offset: 0x002762A8
		public bool ParentIsEmpty
		{
			get
			{
				Building_Casket building_Casket = this.parent as Building_Casket;
				if (building_Casket != null && !building_Casket.HasAnyContents)
				{
					return true;
				}
				CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = this.parent.TryGetComp<CompPawnSpawnOnWakeup>();
				return compPawnSpawnOnWakeup != null && !compPawnSpawnOnWakeup.CanSpawn;
			}
		}

		// Token: 0x0600863B RID: 34363 RVA: 0x002780E8 File Offset: 0x002762E8
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.ParentIsEmpty)
			{
				Mesh mesh = this.Props.graphicData.Graphic.MeshAt(this.parent.Rotation);
				Vector3 drawPos = this.parent.DrawPos;
				drawPos.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
				Graphics.DrawMesh(mesh, drawPos + this.Props.graphicData.drawOffset.RotatedBy(this.parent.Rotation), Quaternion.identity, this.Props.graphicData.Graphic.MatAt(this.parent.Rotation, null), 0);
			}
		}
	}
}
