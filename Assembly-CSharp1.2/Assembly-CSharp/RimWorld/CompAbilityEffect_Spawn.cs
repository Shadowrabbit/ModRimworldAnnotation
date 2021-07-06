using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138E RID: 5006
	public class CompAbilityEffect_Spawn : CompAbilityEffect
	{
		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06006CA3 RID: 27811 RVA: 0x00049E23 File Offset: 0x00048023
		public new CompProperties_AbilitySpawn Props
		{
			get
			{
				return (CompProperties_AbilitySpawn)this.props;
			}
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x00049E30 File Offset: 0x00048030
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenSpawn.Spawn(this.Props.thingDef, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x00215CD0 File Offset: 0x00213ED0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map) || (target.Cell.GetFirstBuilding(this.parent.pawn.Map) != null && !this.Props.allowOnBuildings))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
