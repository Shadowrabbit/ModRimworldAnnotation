using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2E RID: 3374
	public class CompAbilityEffect_DropPawns : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004F13 RID: 20243 RVA: 0x001A81D5 File Offset: 0x001A63D5
		public new CompProperties_DropPawns Props
		{
			get
			{
				return (CompProperties_DropPawns)this.props;
			}
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x001A81E4 File Offset: 0x001A63E4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < this.Props.amount; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.Props.pawnKindDef, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
				list.Add(item);
			}
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.Map, list, 110, false, false, true, true);
		}
	}
}
