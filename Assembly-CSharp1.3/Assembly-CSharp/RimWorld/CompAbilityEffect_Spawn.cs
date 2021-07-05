using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D63 RID: 3427
	public class CompAbilityEffect_Spawn : CompAbilityEffect
	{
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06004FA8 RID: 20392 RVA: 0x001AAA53 File Offset: 0x001A8C53
		public new CompProperties_AbilitySpawn Props
		{
			get
			{
				return (CompProperties_AbilitySpawn)this.props;
			}
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x001AAA60 File Offset: 0x001A8C60
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenSpawn.Spawn(this.Props.thingDef, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x001AAA94 File Offset: 0x001A8C94
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
