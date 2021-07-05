using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000229 RID: 553
	public abstract class WeatherEvent
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000FB2 RID: 4018
		public abstract bool Expired { get; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0005976A File Offset: 0x0005796A
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00059779 File Offset: 0x00057979
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x00059780 File Offset: 0x00057980
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00059796 File Offset: 0x00057996
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FB8 RID: 4024
		public abstract void FireEvent();

		// Token: 0x06000FB9 RID: 4025
		public abstract void WeatherEventTick();

		// Token: 0x06000FBA RID: 4026 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x04000C5C RID: 3164
		protected Map map;
	}
}
