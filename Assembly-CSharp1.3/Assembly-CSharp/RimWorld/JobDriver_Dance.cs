using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E7 RID: 1767
	public class JobDriver_Dance : JobDriver
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x0011F6A4 File Offset: 0x0011D8A4
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x0011F6B8 File Offset: 0x0011D8B8
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				float num = Mathf.Sin((float)this.AgeTicks / 60f * 8f);
				if (this.jumping)
				{
					float z = Mathf.Max(Mathf.Pow((num + 1f) * 0.5f, 2f) * 0.2f - 0.06f, 0f);
					return new Vector3(0f, 0f, z);
				}
				float num2 = Mathf.Sign(num);
				return new Vector3(JobDriver_Dance.<get_ForcedBodyOffset>g__EaseInOutQuad|5_0(Mathf.Abs(num) * 0.6f) * 0.09f * num2, 0f, 0f);
			}
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x0011F754 File Offset: 0x0011D954
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.pawn.Rotation = Rot4.Random;
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x0011F76C File Offset: 0x0011D96C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Dance job"))
			{
				yield break;
			}
			Toil toil = new Toil();
			this.jumping = Rand.Bool;
			toil.tickAction = delegate()
			{
				if (this.AgeTicks % this.moveChangeInterval == 0)
				{
					this.jumping = !this.jumping;
				}
				if (this.AgeTicks % 120 == 0 && !this.jumping)
				{
					this.pawn.Rotation = Rot4.Random;
				}
			};
			toil.socialMode = RandomSocialMode.SuperActive;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			yield return toil;
			yield break;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x0011F78F File Offset: 0x0011D98F
		[CompilerGenerated]
		internal static float <get_ForcedBodyOffset>g__EaseInOutQuad|5_0(float v)
		{
			if ((double)v >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * v + 2f, 4f) / 2f;
			}
			return 8f * v * v * v * v;
		}

		// Token: 0x04001D6D RID: 7533
		private bool jumping;

		// Token: 0x04001D6E RID: 7534
		private int moveChangeInterval = 240;
	}
}
