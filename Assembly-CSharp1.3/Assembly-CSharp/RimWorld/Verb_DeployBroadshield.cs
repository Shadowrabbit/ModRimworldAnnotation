using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001510 RID: 5392
	public class Verb_DeployBroadshield : Verb
	{
		// Token: 0x06008068 RID: 32872 RVA: 0x002D7E75 File Offset: 0x002D6075
		protected override bool TryCastShot()
		{
			return Verb_DeployBroadshield.Deploy(base.ReloadableCompSource);
		}

		// Token: 0x06008069 RID: 32873 RVA: 0x002D7E84 File Offset: 0x002D6084
		public static bool Deploy(CompReloadable comp)
		{
			if (!ModLister.CheckRoyalty("Projectile interceptors"))
			{
				return false;
			}
			if (comp == null || !comp.CanBeUsed)
			{
				return false;
			}
			Pawn wearer = comp.Wearer;
			Map map = wearer.Map;
			int num = GenRadial.NumCellsInRadius(4f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = wearer.Position + GenRadial.RadialPattern[i];
				if (intVec.IsValid && intVec.InBounds(map))
				{
					Verb_DeployBroadshield.SpawnEffect(GenSpawn.Spawn(ThingDefOf.BroadshieldProjector, intVec, map, WipeMode.Vanish));
					comp.UsedOnce();
					return true;
				}
			}
			Messages.Message("AbilityNotEnoughFreeSpace".Translate(), wearer, MessageTypeDefOf.RejectInput, false);
			return false;
		}

		// Token: 0x0600806A RID: 32874 RVA: 0x002D7F38 File Offset: 0x002D6138
		private static void SpawnEffect(Thing projector)
		{
			FleckMaker.Static(projector.TrueCenter(), projector.Map, FleckDefOf.BroadshieldActivation, 1f);
			SoundDefOf.Broadshield_Startup.PlayOneShot(new TargetInfo(projector.Position, projector.Map, false));
		}
	}
}
