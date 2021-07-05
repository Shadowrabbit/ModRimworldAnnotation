using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000964 RID: 2404
	public class Thought_Situational_WearingDesiredApparel : Thought_Situational
	{
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06003D32 RID: 15666 RVA: 0x001517A0 File Offset: 0x0014F9A0
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(((Precept_Apparel)this.sourcePrecept).apparelDef.Named("APPAREL")).CapitalizeFirst();
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06003D33 RID: 15667 RVA: 0x001517E4 File Offset: 0x0014F9E4
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(((Precept_Apparel)this.sourcePrecept).apparelDef.Named("APPAREL"), this.pawn.Named("PAWN")).CapitalizeFirst() + base.CausedByBeliefInPrecept;
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00151844 File Offset: 0x0014FA44
		protected override ThoughtState CurrentStateInternal()
		{
			Precept_Apparel precept_Apparel = this.sourcePrecept as Precept_Apparel;
			if (precept_Apparel == null || !this.HasApparel(precept_Apparel.apparelDef))
			{
				return ThoughtState.Inactive;
			}
			if (precept_Apparel.TargetGender != Gender.None && precept_Apparel.TargetGender != this.pawn.gender)
			{
				return ThoughtState.Inactive;
			}
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x001518AC File Offset: 0x0014FAAC
		private bool HasApparel(ThingDef thingDef)
		{
			using (List<Apparel>.Enumerator enumerator = this.pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
