using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF2 RID: 3058
	public class IncidentQueue : IExposable
	{
		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060047F6 RID: 18422 RVA: 0x0017C1E3 File Offset: 0x0017A3E3
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x060047F7 RID: 18423 RVA: 0x0017C1F0 File Offset: 0x0017A3F0
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

		// Token: 0x060047F8 RID: 18424 RVA: 0x0017C280 File Offset: 0x0017A480
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

		// Token: 0x060047F9 RID: 18425 RVA: 0x0017C28F File Offset: 0x0017A48F
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0017C29C File Offset: 0x0017A49C
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0017C2B4 File Offset: 0x0017A4B4
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0017C2F0 File Offset: 0x0017A4F0
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
		{
			QueuedIncident qi = new QueuedIncident(new FiringIncident(def, null, parms), fireTick, retryDurationTicks);
			this.Add(qi);
			return true;
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0017C318 File Offset: 0x0017A518
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

		// Token: 0x060047FE RID: 18430 RVA: 0x0017C40C File Offset: 0x0017A60C
		public void Notify_MapRemoved(Map map)
		{
			this.queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
		}

		// Token: 0x04002C3D RID: 11325
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();
	}
}
