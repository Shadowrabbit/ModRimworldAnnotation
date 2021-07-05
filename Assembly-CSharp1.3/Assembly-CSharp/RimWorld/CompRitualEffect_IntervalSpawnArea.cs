using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBC RID: 4028
	public class CompRitualEffect_IntervalSpawnArea : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06005EFF RID: 24319 RVA: 0x002081B0 File Offset: 0x002063B0
		protected new CompProperties_RitualEffectIntervalSpawnArea Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnArea)this.props;
			}
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x002081C0 File Offset: 0x002063C0
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			CellRect cellRect = CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map);
			if (this.Props.smoothEdges)
			{
				return cellRect.Cells.RandomElementByWeight((IntVec3 c) => 1f / Mathf.Max((c - ritual.selectedTarget.Cell).LengthHorizontal, 1f)).ToVector3Shifted() + Rand.UnitVector3 * 0.5f;
			}
			return cellRect.RandomVector3;
		}
	}
}
