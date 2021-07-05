using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C79 RID: 3193
	public class FireWatcher
	{
		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004A76 RID: 19062 RVA: 0x0018A0AD File Offset: 0x001882AD
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004A77 RID: 19063 RVA: 0x0018A0B5 File Offset: 0x001882B5
		public bool LargeFireDangerPresent
		{
			get
			{
				if (this.fireDanger < 0f)
				{
					this.UpdateObservations();
				}
				return this.fireDanger > 90f;
			}
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0018A0D7 File Offset: 0x001882D7
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0018A0F1 File Offset: 0x001882F1
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0018A10C File Offset: 0x0018830C
		private void UpdateObservations()
		{
			this.fireDanger = 0f;
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.Fire);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				this.fireDanger += 0.5f + fire.fireSize;
			}
		}

		// Token: 0x04002D3D RID: 11581
		private Map map;

		// Token: 0x04002D3E RID: 11582
		private float fireDanger = -1f;

		// Token: 0x04002D3F RID: 11583
		private const int UpdateObservationsInterval = 426;

		// Token: 0x04002D40 RID: 11584
		private const float BaseDangerPerFire = 0.5f;
	}
}
