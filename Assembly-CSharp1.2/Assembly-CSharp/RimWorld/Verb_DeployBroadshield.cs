using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D85 RID: 7557
	public class Verb_DeployBroadshield : Verb
	{
		// Token: 0x0600A437 RID: 42039 RVA: 0x0006CDF2 File Offset: 0x0006AFF2
		protected override bool TryCastShot()
		{
			return Verb_DeployBroadshield.Deploy(base.ReloadableCompSource);
		}

		// Token: 0x0600A438 RID: 42040 RVA: 0x002FCEFC File Offset: 0x002FB0FC
		public static bool Deploy(CompReloadable comp)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shields are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 86573384, false);
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

		// Token: 0x0600A439 RID: 42041 RVA: 0x0006CDFF File Offset: 0x0006AFFF
		private static void SpawnEffect(Thing projector)
		{
			MoteMaker.MakeStaticMote(projector.TrueCenter(), projector.Map, ThingDefOf.Mote_BroadshieldActivation, 1f);
			SoundDefOf.Broadshield_Startup.PlayOneShot(new TargetInfo(projector.Position, projector.Map, false));
		}
	}
}
