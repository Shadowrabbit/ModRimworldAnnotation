using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031A RID: 794
	public abstract class WeatherEvent
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001438 RID: 5176
		public abstract bool Expired { get; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x00014932 File Offset: 0x00012B32
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00014941 File Offset: 0x00012B41
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x000CE1E8 File Offset: 0x000CC3E8
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00014948 File Offset: 0x00012B48
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600143E RID: 5182
		public abstract void FireEvent();

		// Token: 0x0600143F RID: 5183
		public abstract void WeatherEventTick();

		// Token: 0x06001440 RID: 5184 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x04000FE7 RID: 4071
		protected Map map;
	}
}
