using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D84 RID: 7556
	public class Verb_Bombardment : Verb_CastBase
	{
		// Token: 0x0600A434 RID: 42036 RVA: 0x002FCE58 File Offset: 0x002FB058
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			bombardment.duration = 540;
			bombardment.instigator = this.caster;
			bombardment.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x0600A435 RID: 42037 RVA: 0x0006CDE8 File Offset: 0x0006AFE8
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x04006F58 RID: 28504
		public const int DurationTicks = 540;
	}
}
