using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC9 RID: 3017
	public class QuestPart_Incident : QuestPart
	{
		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x001759EA File Offset: 0x00173BEA
		// (set) Token: 0x060046A3 RID: 18083 RVA: 0x001759F2 File Offset: 0x00173BF2
		public MapParent MapParent
		{
			get
			{
				return this.mapParent;
			}
			set
			{
				this.mapParent = value;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x001759FB File Offset: 0x00173BFB
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

		// Token: 0x060046A5 RID: 18085 RVA: 0x00175A0C File Offset: 0x00173C0C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.incidentParms != null)
			{
				if (!this.incidentParms.forced)
				{
					Log.Error("QuestPart incident should always be forced but it's not. incident=" + this.incident);
					this.incidentParms.forced = true;
				}
				this.incidentParms.quest = this.quest;
				if (this.mapParent != null)
				{
					if (this.mapParent.HasMap)
					{
						this.incidentParms.target = this.mapParent.Map;
						if (this.incident.Worker.CanFireNow(this.incidentParms))
						{
							this.incident.Worker.TryExecute(this.incidentParms);
						}
						this.incidentParms.target = null;
						return;
					}
				}
				else if (this.incidentParms.target != null && this.incident.Worker.CanFireNow(this.incidentParms))
				{
					this.incident.Worker.TryExecute(this.incidentParms);
				}
			}
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x00175B28 File Offset: 0x00173D28
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

		// Token: 0x060046A7 RID: 18087 RVA: 0x00175B70 File Offset: 0x00173D70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x00175BCC File Offset: 0x00173DCC
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

		// Token: 0x04002B18 RID: 11032
		public string inSignal;

		// Token: 0x04002B19 RID: 11033
		public IncidentDef incident;

		// Token: 0x04002B1A RID: 11034
		private IncidentParms incidentParms;

		// Token: 0x04002B1B RID: 11035
		private MapParent mapParent;
	}
}
