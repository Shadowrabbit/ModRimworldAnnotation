using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116B RID: 4459
	public static class SignalArgsUtility
	{
		// Token: 0x0600621B RID: 25115 RVA: 0x001EA800 File Offset: 0x001E8A00
		public static bool TryGetLookTargets(SignalArgs args, string name, out LookTargets lookTargets)
		{
			if (args.TryGetArg<LookTargets>(name, out lookTargets))
			{
				return true;
			}
			Thing t;
			if (args.TryGetArg<Thing>(name, out t))
			{
				lookTargets = t;
				return true;
			}
			WorldObject o;
			if (args.TryGetArg<WorldObject>(name, out o))
			{
				lookTargets = o;
				return true;
			}
			GlobalTargetInfo target;
			if (args.TryGetArg<GlobalTargetInfo>(name, out target))
			{
				lookTargets = target;
				return true;
			}
			TargetInfo target2;
			if (args.TryGetArg<TargetInfo>(name, out target2))
			{
				lookTargets = target2;
				return true;
			}
			lookTargets = LookTargets.Invalid;
			return false;
		}
	}
}
