using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001372 RID: 4978
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x0600791E RID: 31006 RVA: 0x002AEB8E File Offset: 0x002ACD8E
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x0600791F RID: 31007 RVA: 0x002AEB95 File Offset: 0x002ACD95
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && (p.Faction == null || p.Faction == Faction.OfInsects) && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map) && !p.IsPrisonerInPrisonCell()
				select p;
			}
		}
	}
}
