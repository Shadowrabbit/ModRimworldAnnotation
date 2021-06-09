using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020020D6 RID: 8406
	public class Caravan_Tweener
	{
		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x0600B2AC RID: 45740 RVA: 0x0007424F File Offset: 0x0007244F
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x0600B2AD RID: 45741 RVA: 0x00074257 File Offset: 0x00072457
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x0600B2AE RID: 45742 RVA: 0x0007426A File Offset: 0x0007246A
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x0600B2AF RID: 45743 RVA: 0x00074287 File Offset: 0x00072487
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B2B0 RID: 45744 RVA: 0x0033C43C File Offset: 0x0033A63C
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x0600B2B1 RID: 45745 RVA: 0x000742A1 File Offset: 0x000724A1
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x04007ADC RID: 31452
		private Caravan caravan;

		// Token: 0x04007ADD RID: 31453
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x04007ADE RID: 31454
		private Vector3 lastTickSpringPos;

		// Token: 0x04007ADF RID: 31455
		private const float SpringTightness = 0.09f;
	}
}
