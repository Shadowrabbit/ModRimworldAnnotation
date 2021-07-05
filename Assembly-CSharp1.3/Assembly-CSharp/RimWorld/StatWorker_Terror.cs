using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001500 RID: 5376
	public class StatWorker_Terror : StatWorker
	{
		// Token: 0x06008015 RID: 32789 RVA: 0x002D5B74 File Offset: 0x002D3D74
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn;
			return base.ShouldShowFor(req) && (pawn = (req.Thing as Pawn)) != null && pawn.IsSlave;
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x002D5BA4 File Offset: 0x002D3DA4
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return ((Pawn)req.Thing).GetTerrorLevel();
		}
	}
}
