using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001204 RID: 4612
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06006EDB RID: 28379 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06006EDC RID: 28380 RVA: 0x00251407 File Offset: 0x0024F607
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x06006EDD RID: 28381 RVA: 0x00251414 File Offset: 0x0024F614
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
				if (this.Props.fleckOnUsed != null)
				{
					FleckMaker.AttachedOverlay(usedBy, this.Props.fleckOnUsed, Vector3.zero, this.Props.fleckOnUsedScale, -1f);
				}
			}
		}

		// Token: 0x06006EDE RID: 28382 RVA: 0x002514C1 File Offset: 0x0024F6C1
		public virtual TaggedString ConfirmMessage(Pawn p)
		{
			return null;
		}

		// Token: 0x06006EDF RID: 28383 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x06006EE0 RID: 28384 RVA: 0x0020646E File Offset: 0x0020466E
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}

		// Token: 0x04003D5C RID: 15708
		private const float CameraShakeMag = 1f;
	}
}
