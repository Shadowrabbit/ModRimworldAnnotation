using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5C RID: 7004
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06009A5E RID: 39518 RVA: 0x00066C9D File Offset: 0x00064E9D
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06009A5F RID: 39519 RVA: 0x00066CA4 File Offset: 0x00064EA4
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && (p.Faction == null || p.Faction == Faction.OfInsects) && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map) && !p.IsPrisonerInPrisonCell()
				select p;
			}
		}

		// Token: 0x06009A60 RID: 39520 RVA: 0x0006636A File Offset: 0x0006456A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
