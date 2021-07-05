using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000721 RID: 1825
	public class JobDriver_LayDownAwake : JobDriver_LayDown
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanSleep
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool LookForOtherJobs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060032B4 RID: 12980 RVA: 0x001233BC File Offset: 0x001215BC
		public override Rot4 ForcedLayingRotation
		{
			get
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing != null)
				{
					return thing.Rotation.Rotated(RotationDirection.Clockwise);
				}
				return base.ForcedLayingRotation;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x001233F8 File Offset: 0x001215F8
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing != null && thing.def.Size.z > 1)
				{
					return new Vector3(0f, 0f, 0.5f).RotatedBy(thing.Rotation);
				}
				return base.ForcedBodyOffset;
			}
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00123456 File Offset: 0x00121656
		public override string GetReport()
		{
			return "ReportLayingDown".Translate();
		}
	}
}
