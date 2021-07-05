using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD4 RID: 3028
	public static class SignalArgsUtility
	{
		// Token: 0x0600471C RID: 18204 RVA: 0x00178520 File Offset: 0x00176720
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
