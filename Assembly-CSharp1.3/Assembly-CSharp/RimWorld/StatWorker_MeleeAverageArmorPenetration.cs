using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F6 RID: 5366
	public class StatWorker_MeleeAverageArmorPenetration : StatWorker
	{
		// Token: 0x06007FE7 RID: 32743 RVA: 0x002D4650 File Offset: 0x002D2850
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.IsWeapon && !thingDef.tools.NullOrEmpty<Tool>();
		}

		// Token: 0x06007FE8 RID: 32744 RVA: 0x002D4688 File Offset: 0x002D2888
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return 0f;
			}
			if (req.Thing != null)
			{
				Pawn attacker = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
				return (from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
				where x.verbProps.IsMeleeAttack
				select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, attacker, req.Thing, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedArmorPenetration(x.tool, attacker, req.Thing, null));
			}
			return (from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, null, thingDef, req.StuffDef, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedArmorPenetration(x.tool, null, thingDef, req.StuffDef, null));
		}

		// Token: 0x06007FE9 RID: 32745 RVA: 0x002D47C4 File Offset: 0x002D29C4
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return null;
			}
			Pawn currentWeaponUser = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
			IEnumerable<VerbUtility.VerbPropertiesWithSource> enumerable = from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (VerbUtility.VerbPropertiesWithSource verbPropertiesWithSource in enumerable)
			{
				float f = verbPropertiesWithSource.verbProps.AdjustedArmorPenetration(verbPropertiesWithSource.tool, currentWeaponUser, req.Thing, null);
				if (verbPropertiesWithSource.tool != null)
				{
					stringBuilder.AppendLine(string.Format("  {0} ({1})", verbPropertiesWithSource.tool.LabelCap, verbPropertiesWithSource.ToolCapacity.label));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("  {0}:", "StatsReport_NonToolAttack".Translate()));
				}
				stringBuilder.AppendLine("    " + f.ToStringPercent());
			}
			return stringBuilder.ToString();
		}
	}
}
