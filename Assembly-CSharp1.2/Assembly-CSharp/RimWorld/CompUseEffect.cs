using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D2 RID: 6354
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06008CCD RID: 36045 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06008CCE RID: 36046 RVA: 0x0005E669 File Offset: 0x0005C869
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x06008CCF RID: 36047 RVA: 0x0028DAC4 File Offset: 0x0028BCC4
		public virtual void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap)
			{
				if (this.Props.doCameraShake && usedBy.Spawned)
				{
					Find.CameraDriver.shaker.DoShake(1f);
				}
				if (this.Props.moteOnUsed != null)
				{
					MoteMaker.MakeAttachedOverlay(usedBy, this.Props.moteOnUsed, Vector3.zero, this.Props.moteOnUsedScale, -1f);
				}
			}
		}

		// Token: 0x06008CD0 RID: 36048 RVA: 0x0005E676 File Offset: 0x0005C876
		public virtual TaggedString ConfirmMessage(Pawn p)
		{
			return null;
		}

		// Token: 0x06008CD1 RID: 36049 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x06008CD2 RID: 36050 RVA: 0x0005E67E File Offset: 0x0005C87E
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}

		// Token: 0x04005A01 RID: 23041
		private const float CameraShakeMag = 1f;
	}
}
