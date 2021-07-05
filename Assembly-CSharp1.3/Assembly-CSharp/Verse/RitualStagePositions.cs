using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000052 RID: 82
	public class RitualStagePositions : IExposable
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x00014F10 File Offset: 0x00013110
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn, PawnStagePosition>(ref this.positions, "positions", LookMode.Reference, LookMode.Deep, ref this.pawnListTmp, ref this.positionListTmp);
		}

		// Token: 0x04000121 RID: 289
		public Dictionary<Pawn, PawnStagePosition> positions = new Dictionary<Pawn, PawnStagePosition>();

		// Token: 0x04000122 RID: 290
		private List<Pawn> pawnListTmp;

		// Token: 0x04000123 RID: 291
		private List<PawnStagePosition> positionListTmp;
	}
}
