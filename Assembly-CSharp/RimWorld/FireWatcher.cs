using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001264 RID: 4708
	public class FireWatcher
	{
		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x060066A9 RID: 26281 RVA: 0x00046286 File Offset: 0x00044486
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x060066AA RID: 26282 RVA: 0x0004628E File Offset: 0x0004448E
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

		// Token: 0x060066AB RID: 26283 RVA: 0x000462B0 File Offset: 0x000444B0
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x000462CA File Offset: 0x000444CA
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x001F9CCC File Offset: 0x001F7ECC
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

		// Token: 0x04004456 RID: 17494
		private Map map;

		// Token: 0x04004457 RID: 17495
		private float fireDanger = -1f;

		// Token: 0x04004458 RID: 17496
		private const int UpdateObservationsInterval = 426;

		// Token: 0x04004459 RID: 17497
		private const float BaseDangerPerFire = 0.5f;
	}
}
