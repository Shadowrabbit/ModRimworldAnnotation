using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200148B RID: 5259
	public static class ImpactSoundUtility
	{
		// Token: 0x06007DCD RID: 32205 RVA: 0x002C92F0 File Offset: 0x002C74F0
		public static void PlayImpactSound(Thing hitThing, ImpactSoundTypeDef ist, Map map)
		{
			if (ist == null)
			{
				return;
			}
			if (ist.playOnlyIfHitPawn && !(hitThing is Pawn))
			{
				return;
			}
			if (map == null)
			{
				Log.Warning("Can't play impact sound because map is null.");
				return;
			}
			SoundDef soundDef;
			if (hitThing is Pawn)
			{
				soundDef = ist.soundDef;
			}
			else
			{
				if (hitThing.Stuff != null)
				{
					soundDef = hitThing.Stuff.stuffProps.soundImpactStuff;
				}
				else
				{
					soundDef = hitThing.def.soundImpactDefault;
				}
				if (soundDef.NullOrUndefined())
				{
					soundDef = SoundDefOf.BulletImpact_Ground;
				}
			}
			if (!soundDef.NullOrUndefined())
			{
				soundDef.PlayOneShot(new TargetInfo(hitThing.PositionHeld, map, false));
			}
		}
	}
}
