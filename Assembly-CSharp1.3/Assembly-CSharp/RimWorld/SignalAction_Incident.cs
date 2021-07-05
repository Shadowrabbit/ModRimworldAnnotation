using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109C RID: 4252
	public class SignalAction_Incident : SignalAction
	{
		// Token: 0x06006563 RID: 25955 RVA: 0x00223EC5 File Offset: 0x002220C5
		protected override void DoAction(SignalArgs args)
		{
			if (this.incident.Worker.CanFireNow(this.incidentParms))
			{
				this.incident.Worker.TryExecute(this.incidentParms);
			}
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x00223EF6 File Offset: 0x002220F6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
		}

		// Token: 0x04003917 RID: 14615
		public IncidentDef incident;

		// Token: 0x04003918 RID: 14616
		public IncidentParms incidentParms;
	}
}
