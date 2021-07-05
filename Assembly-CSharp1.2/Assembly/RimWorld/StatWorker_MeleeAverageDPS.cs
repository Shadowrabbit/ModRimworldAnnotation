using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D60 RID: 7520
	public class StatWorker_MeleeAverageDPS : StatWorker
	{
		// Token: 0x0600A37C RID: 41852 RVA: 0x002F9D60 File Offset: 0x002F7F60
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return false;
			}
			if (!thingDef.IsWeapon && !thingDef.isTechHediff)
			{
				return false;
			}
			List<VerbProperties> list;
			List<Tool> list2;
			this.GetVerbsAndTools(thingDef, out list, out list2);
			if (!list2.NullOrEmpty<Tool>())
			{
				return true;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsMeleeAttack)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A37D RID: 41853 RVA: 0x002F9DCC File Offset: 0x002F7FCC
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return 0f;
			}
			List<VerbProperties> verbProps;
			List<Tool> tools;
			this.GetVerbsAndTools(thingDef, out verbProps, out tools);
			if (req.Thing != null)
			{
				Pawn attacker = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
				float num = (from x in VerbUtility.GetAllVerbProperties(verbProps, tools)
				where x.verbProps.IsMeleeAttack
				select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, attacker, req.Thing, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeDamageAmount(x.tool, attacker, req.Thing, null));
				float num2 = (from x in VerbUtility.GetAllVerbProperties(verbProps, tools)
				where x.verbProps.IsMeleeAttack
				select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, attacker, req.Thing, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedCooldown(x.tool, attacker, req.Thing));
				if (num2 == 0f)
				{
					return 0f;
				}
				return num / num2;
			}
			else
			{
				float num3 = (from x in VerbUtility.GetAllVerbProperties(verbProps, tools)
				where x.verbProps.IsMeleeAttack
				select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight_NewTmp(x.tool, null, thingDef, req.StuffDef, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeDamageAmount_NewTmp(x.tool, null, thingDef, req.StuffDef, null));
				float num4 = (from x in VerbUtility.GetAllVerbProperties(verbProps, tools)
				where x.verbProps.IsMeleeAttack
				select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight_NewTmp(x.tool, null, thingDef, req.StuffDef, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedCooldown_NewTmp(x.tool, null, thingDef, req.StuffDef));
				if (num4 == 0f)
				{
					return 0f;
				}
				return num3 / num4;
			}
		}

		// Token: 0x0600A37E RID: 41854 RVA: 0x002F9FA4 File Offset: 0x002F81A4
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return null;
			}
			List<VerbProperties> verbProps;
			List<Tool> tools;
			this.GetVerbsAndTools(thingDef, out verbProps, out tools);
			Pawn currentWeaponUser = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
			IEnumerable<VerbUtility.VerbPropertiesWithSource> enumerable = from x in VerbUtility.GetAllVerbProperties(verbProps, tools)
			where x.verbProps.IsMeleeAttack
			select x;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (VerbUtility.VerbPropertiesWithSource verbPropertiesWithSource in enumerable)
			{
				float num = verbPropertiesWithSource.verbProps.AdjustedMeleeDamageAmount(verbPropertiesWithSource.tool, currentWeaponUser, req.Thing, null);
				float num2 = verbPropertiesWithSource.verbProps.AdjustedCooldown(verbPropertiesWithSource.tool, currentWeaponUser, req.Thing);
				if (verbPropertiesWithSource.tool != null)
				{
					stringBuilder.AppendLine(string.Format("  {0} ({1})", verbPropertiesWithSource.tool.LabelCap, verbPropertiesWithSource.ToolCapacity.label));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("  {0}:", "StatsReport_NonToolAttack".Translate()));
				}
				stringBuilder.AppendLine(string.Format("    {0} {1}", num.ToString("F1"), "DamageLower".Translate()));
				stringBuilder.AppendLine(string.Format("    {0} {1}", num2.ToString("F2"), "SecondsPerAttackLower".Translate()));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A37F RID: 41855 RVA: 0x002FA14C File Offset: 0x002F834C
		public static Pawn GetCurrentWeaponUser(Thing weapon)
		{
			if (weapon == null)
			{
				return null;
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = weapon.ParentHolder as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				return pawn_EquipmentTracker.pawn;
			}
			Pawn_ApparelTracker pawn_ApparelTracker = weapon.ParentHolder as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				return pawn_ApparelTracker.pawn;
			}
			return null;
		}

		// Token: 0x0600A380 RID: 41856 RVA: 0x002FA18C File Offset: 0x002F838C
		private void GetVerbsAndTools(ThingDef def, out List<VerbProperties> verbs, out List<Tool> tools)
		{
			verbs = def.Verbs;
			tools = def.tools;
			if (def.isTechHediff)
			{
				HediffDef hediffDef = this.FindTechHediffHediff(def);
				if (hediffDef == null)
				{
					return;
				}
				HediffCompProperties_VerbGiver hediffCompProperties_VerbGiver = hediffDef.CompProps<HediffCompProperties_VerbGiver>();
				if (hediffCompProperties_VerbGiver == null)
				{
					return;
				}
				verbs = hediffCompProperties_VerbGiver.verbs;
				tools = hediffCompProperties_VerbGiver.tools;
			}
		}

		// Token: 0x0600A381 RID: 41857 RVA: 0x002FA1D8 File Offset: 0x002F83D8
		private HediffDef FindTechHediffHediff(ThingDef techHediff)
		{
			List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].addsHediff != null && allDefsListForReading[i].IsIngredient(techHediff))
				{
					return allDefsListForReading[i].addsHediff;
				}
			}
			return null;
		}
	}
}
