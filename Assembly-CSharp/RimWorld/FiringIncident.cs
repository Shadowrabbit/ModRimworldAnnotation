using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001186 RID: 4486
	public class FiringIncident : IExposable
	{
		// Token: 0x060062E4 RID: 25316 RVA: 0x0004409C File Offset: 0x0004229C
		public FiringIncident()
		{
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x000440AF File Offset: 0x000422AF
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x000440DA File Offset: 0x000422DA
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x060062E7 RID: 25319 RVA: 0x001ED61C File Offset: 0x001EB81C
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

		// Token: 0x04004227 RID: 16935
		public IncidentDef def;

		// Token: 0x04004228 RID: 16936
		public IncidentParms parms = new IncidentParms();

		// Token: 0x04004229 RID: 16937
		public StorytellerComp source;

		// Token: 0x0400422A RID: 16938
		public QuestPart sourceQuestPart;
	}
}
