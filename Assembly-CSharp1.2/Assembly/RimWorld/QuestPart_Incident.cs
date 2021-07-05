using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001151 RID: 4433
	public class QuestPart_Incident : QuestPart
	{
		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x0600614B RID: 24907 RVA: 0x0004303D File Offset: 0x0004123D
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x001E7690 File Offset: 0x001E5890
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.incidentParms != null)
			{
				if (!this.incidentParms.forced)
				{
					Log.Error("QuestPart incident should always be forced but it's not. incident=" + this.incident, false);
					this.incidentParms.forced = true;
				}
				this.incidentParms.quest = this.quest;
				if (this.mapParent != null)
				{
					if (this.mapParent.HasMap)
					{
						this.incidentParms.target = this.mapParent.Map;
						if (this.incident.Worker.CanFireNow(this.incidentParms, true))
						{
							this.incident.Worker.TryExecute(this.incidentParms);
						}
						this.incidentParms.target = null;
					}
				}
				else if (this.incidentParms.target != null && this.incident.Worker.CanFireNow(this.incidentParms, true))
				{
					this.incident.Worker.TryExecute(this.incidentParms);
				}
				this.incidentParms = null;
			}
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x001E77B8 File Offset: 0x001E59B8
		public void SetIncidentParmsAndRemoveTarget(IncidentParms value)
		{
			this.incidentParms = value;
			Map map = this.incidentParms.target as Map;
			if (map != null)
			{
				this.mapParent = map.Parent;
				this.incidentParms.target = null;
				return;
			}
			this.mapParent = null;
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x001E7800 File Offset: 0x001E5A00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x001E785C File Offset: 0x001E5A5C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.incident = IncidentDefOf.RaidEnemy;
				this.SetIncidentParmsAndRemoveTarget(new IncidentParms
				{
					target = Find.RandomPlayerHomeMap,
					points = 500f
				});
			}
		}

		// Token: 0x04004104 RID: 16644
		public string inSignal;

		// Token: 0x04004105 RID: 16645
		public IncidentDef incident;

		// Token: 0x04004106 RID: 16646
		private IncidentParms incidentParms;

		// Token: 0x04004107 RID: 16647
		private MapParent mapParent;
	}
}
