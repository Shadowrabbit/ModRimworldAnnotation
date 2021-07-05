using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001129 RID: 4393
	public class CompEmptyStateGraphic : ThingComp
	{
		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06006984 RID: 27012 RVA: 0x00238F8D File Offset: 0x0023718D
		private CompProperties_EmptyStateGraphic Props
		{
			get
			{
				return (CompProperties_EmptyStateGraphic)this.props;
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06006985 RID: 27013 RVA: 0x00238F9C File Offset: 0x0023719C
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

		// Token: 0x06006986 RID: 27014 RVA: 0x00238FDC File Offset: 0x002371DC
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
