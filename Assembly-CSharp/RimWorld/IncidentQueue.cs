using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118B RID: 4491
	public class IncidentQueue : IExposable
	{
		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060062FF RID: 25343 RVA: 0x000441B3 File Offset: 0x000423B3
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06006300 RID: 25344 RVA: 0x001EDB34 File Offset: 0x001EBD34
		public string DebugQueueReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (QueuedIncident queuedIncident in this.queuedIncidents)
				{
					stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.FireTick - Find.TickManager.TicksGame).ToString() + " ticks)");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x000441C0 File Offset: 0x000423C0
		public IEnumerator GetEnumerator()
		{
			foreach (QueuedIncident queuedIncident in this.queuedIncidents)
			{
				yield return queuedIncident;
			}
			List<QueuedIncident>.Enumerator enumerator = default(List<QueuedIncident>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06006302 RID: 25346 RVA: 0x000441CF File Offset: 0x000423CF
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x000441DC File Offset: 0x000423DC
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x000441F4 File Offset: 0x000423F4
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x001EDBC4 File Offset: 0x001EBDC4
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
		{
			QueuedIncident qi = new QueuedIncident(new FiringIncident(def, null, parms), fireTick, retryDurationTicks);
			this.Add(qi);
			return true;
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x001EDBEC File Offset: 0x001EBDEC
		public void IncidentQueueTick()
		{
			for (int i = this.queuedIncidents.Count - 1; i >= 0; i--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[i];
				if (!queuedIncident.TriedToFire)
				{
					if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
					{
						bool flag = Find.Storyteller.TryFire(queuedIncident.FiringIncident);
						queuedIncident.Notify_TriedToFire();
						if (flag || queuedIncident.RetryDurationTicks == 0)
						{
							this.queuedIncidents.Remove(queuedIncident);
						}
					}
				}
				else if (queuedIncident.FireTick + queuedIncident.RetryDurationTicks <= Find.TickManager.TicksGame)
				{
					this.queuedIncidents.Remove(queuedIncident);
				}
				else if (Find.TickManager.TicksGame % 833 == Rand.RangeSeeded(0, 833, queuedIncident.FireTick))
				{
					bool flag2 = Find.Storyteller.TryFire(queuedIncident.FiringIncident);
					queuedIncident.Notify_TriedToFire();
					if (flag2)
					{
						this.queuedIncidents.Remove(queuedIncident);
					}
				}
			}
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x001EDCE0 File Offset: 0x001EBEE0
		public void Notify_MapRemoved(Map map)
		{
			this.queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
		}

		// Token: 0x04004251 RID: 16977
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();
	}
}
