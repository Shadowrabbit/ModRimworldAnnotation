using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7A RID: 3194
	public static class FlickUtility
	{
		// Token: 0x06004A7B RID: 19067 RVA: 0x0018A174 File Offset: 0x00188374
		public static void UpdateFlickDesignation(Thing t)
		{
			bool flag = false;
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps != null)
			{
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						flag = true;
						break;
					}
				}
			}
			Designation designation = t.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick);
			if (flag && designation == null)
			{
				t.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Flick));
			}
			else if (!flag && designation != null)
			{
				designation.Delete();
			}
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.SwitchFlickingDesignation, Array.Empty<string>());
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x0018A224 File Offset: 0x00188424
		public static bool WantsToBeOn(Thing t)
		{
			CompFlickable compFlickable = t.TryGetComp<CompFlickable>();
			if (compFlickable != null && !compFlickable.SwitchIsOn)
			{
				return false;
			}
			CompSchedule compSchedule = t.TryGetComp<CompSchedule>();
			if (compSchedule != null && !compSchedule.Allowed)
			{
				return false;
			}
			if (t.TryGetComp<CompLightball>() != null && !t.IsRitualTarget())
			{
				return false;
			}
			CompAutoPowered compAutoPowered = t.TryGetComp<CompAutoPowered>();
			return compAutoPowered == null || compAutoPowered.WantsToBeOn;
		}
	}
}
