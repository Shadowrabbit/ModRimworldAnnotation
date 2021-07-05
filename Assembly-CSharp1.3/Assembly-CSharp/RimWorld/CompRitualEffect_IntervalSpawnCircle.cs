using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC0 RID: 4032
	public class CompRitualEffect_IntervalSpawnCircle : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06005F09 RID: 24329 RVA: 0x0020841A File Offset: 0x0020661A
		protected new CompProperties_RitualEffectIntervalSpawnCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnCircle)this.props;
			}
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x00208428 File Offset: 0x00206628
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return CellRect.CenteredOn(ritual.selectedTarget.Cell, this.Props.area.x / 2, this.Props.area.z / 2).ClipInsideMap(ritual.Map).Cells.RandomElementByWeight(delegate(IntVec3 c)
			{
				float f = Mathf.Max(Mathf.Abs((c - ritual.selectedTarget.Cell).LengthHorizontal - this.Props.radius), 1f);
				return 1f / Mathf.Pow(f, this.Props.concentration);
			}).ToVector3Shifted() + Rand.UnitVector3 * 0.5f;
		}
	}
}
