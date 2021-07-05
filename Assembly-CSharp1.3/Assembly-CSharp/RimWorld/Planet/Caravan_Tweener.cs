using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020017B4 RID: 6068
	public class Caravan_Tweener
	{
		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x06008CC5 RID: 36037 RVA: 0x00328DA8 File Offset: 0x00326FA8
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x06008CC6 RID: 36038 RVA: 0x00328DB0 File Offset: 0x00326FB0
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x06008CC7 RID: 36039 RVA: 0x00328DC3 File Offset: 0x00326FC3
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x06008CC8 RID: 36040 RVA: 0x00328DE0 File Offset: 0x00326FE0
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008CC9 RID: 36041 RVA: 0x00328DFC File Offset: 0x00326FFC
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x00328E43 File Offset: 0x00327043
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x04005947 RID: 22855
		private Caravan caravan;

		// Token: 0x04005948 RID: 22856
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x04005949 RID: 22857
		private Vector3 lastTickSpringPos;

		// Token: 0x0400594A RID: 22858
		private const float SpringTightness = 0.09f;
	}
}
