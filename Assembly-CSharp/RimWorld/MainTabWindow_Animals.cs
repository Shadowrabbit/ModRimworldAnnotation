using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B35 RID: 6965
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x06009960 RID: 39264 RVA: 0x00066329 File Offset: 0x00064529
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06009961 RID: 39265 RVA: 0x00066330 File Offset: 0x00064530
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		// Token: 0x06009962 RID: 39266 RVA: 0x0006636A File Offset: 0x0006456A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
