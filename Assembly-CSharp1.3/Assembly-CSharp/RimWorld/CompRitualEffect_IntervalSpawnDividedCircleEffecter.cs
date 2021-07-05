using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC2 RID: 4034
	public class CompRitualEffect_IntervalSpawnDividedCircleEffecter : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06005F0D RID: 24333 RVA: 0x002084F4 File Offset: 0x002066F4
		protected new CompProperties_RitualEffectIntervalSpawnDividedCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnDividedCircle)this.props;
			}
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x000FE248 File Offset: 0x000FC448
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x00208504 File Offset: 0x00206704
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			float num = 360f / (float)this.Props.numCopies;
			for (int i = 0; i < this.Props.numCopies; i++)
			{
				Vector3 a = Quaternion.AngleAxis(num * (float)i, Vector3.up) * Vector3.forward;
				IntVec3 cell = parent.ritual.selectedTarget.Cell;
				TargetInfo targetInfo = new TargetInfo(cell, parent.ritual.Map, false);
				Vector3 vector = (a * this.Props.radius + this.Props.offset) * base.ScaleForRoom(ritual);
				Room room = (cell + vector.ToIntVec3() + this.Props.roomCheckOffset).GetRoom(ritual.Map);
				if (!this.props.onlySpawnInSameRoom || room == ritual.GetRoom)
				{
					Effecter effecter = this.Props.effecterDef.Spawn(cell, parent.ritual.Map, vector, 1f);
					effecter.Trigger(targetInfo, TargetInfo.Invalid);
					parent.AddEffecterToMaintain(targetInfo, effecter);
				}
			}
		}
	}
}
