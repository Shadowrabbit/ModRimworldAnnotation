using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BED RID: 3053
	public class FiringIncident : IExposable
	{
		// Token: 0x060047DB RID: 18395 RVA: 0x0017BA35 File Offset: 0x00179C35
		public FiringIncident()
		{
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0017BA48 File Offset: 0x00179C48
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0017BA73 File Offset: 0x00179C73
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x0017BA9C File Offset: 0x00179C9C
		public override string ToString()
		{
			string text = this.def.defName;
			if (this.parms != null)
			{
				text = text + ", parms=(" + this.parms.ToString() + ")";
			}
			if (this.source != null)
			{
				text = text + ", source=" + this.source;
			}
			if (this.sourceQuestPart != null)
			{
				text = text + ", sourceQuestPart=" + this.sourceQuestPart;
			}
			return text;
		}

		// Token: 0x04002C0A RID: 11274
		public IncidentDef def;

		// Token: 0x04002C0B RID: 11275
		public IncidentParms parms = new IncidentParms();

		// Token: 0x04002C0C RID: 11276
		public StorytellerComp source;

		// Token: 0x04002C0D RID: 11277
		public QuestPart sourceQuestPart;
	}
}
