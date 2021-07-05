using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F7 RID: 6135
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x06008F1F RID: 36639 RVA: 0x00334C3D File Offset: 0x00332E3D
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06008F20 RID: 36640 RVA: 0x00334C44 File Offset: 0x00332E44
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06008F21 RID: 36641 RVA: 0x00334C52 File Offset: 0x00332E52
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
