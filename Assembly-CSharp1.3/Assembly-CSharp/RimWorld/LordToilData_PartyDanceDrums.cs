using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CA RID: 2250
	public class LordToilData_PartyDanceDrums : LordToilData_Gathering
	{
		// Token: 0x06003B40 RID: 15168 RVA: 0x0014AF57 File Offset: 0x00149157
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn, Building>(ref this.playedInstruments, "playedInstruments", LookMode.Reference, LookMode.Reference, ref this.tmpPawns, ref this.tmpUsedInstruments);
		}

		// Token: 0x04002043 RID: 8259
		public Dictionary<Pawn, Building> playedInstruments = new Dictionary<Pawn, Building>();

		// Token: 0x04002044 RID: 8260
		private List<Pawn> tmpPawns;

		// Token: 0x04002045 RID: 8261
		private List<Building> tmpUsedInstruments;
	}
}
