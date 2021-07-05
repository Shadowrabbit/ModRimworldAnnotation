using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA2 RID: 3490
	public static class PawnAttackGizmoUtility
	{
		// Token: 0x060050E7 RID: 20711 RVA: 0x001B11E9 File Offset: 0x001AF3E9
		public static IEnumerable<Gizmo> GetAttackGizmos(Pawn pawn)
		{
			if (PawnAttackGizmoUtility.ShouldUseMeleeAttackGizmo(pawn))
			{
				yield return PawnAttackGizmoUtility.GetMeleeAttackGizmo(pawn);
			}
			if (PawnAttackGizmoUtility.ShouldUseSquadAttackGizmo())
			{
				yield return PawnAttackGizmoUtility.GetSquadAttackGizmo(pawn);
			}
			yield break;
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x001B11F9 File Offset: 0x001AF3F9
		public static bool CanShowEquipmentGizmos()
		{
			return !PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons();
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x001B1203 File Offset: 0x001AF403
		private static bool ShouldUseSquadAttackGizmo()
		{
			return PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasRangedWeapon() && PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons();
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x001B1214 File Offset: 0x001AF414
		private static Gizmo GetSquadAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandSquadAttack".Translate();
			command_Target.defaultDesc = "CommandSquadAttackDesc".Translate();
			command_Target.hotKey = KeyBindingDefOf.Misc1;
			command_Target.icon = TexCommand.SquadAttack;
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.targetingParams.canTargetLocations = PawnAttackGizmoUtility.AllSelectedColonistsCanTargetLocations();
			string str;
			if (FloatMenuUtility.GetAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && pawn3.IsColonistPlayerControlled && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					string text;
					Action attackAction = FloatMenuUtility.GetAttackAction(pawn2, target, out text);
					if (attackAction != null)
					{
						attackAction();
					}
					else if (!text.NullOrEmpty())
					{
						Messages.Message(text, target.Thing, MessageTypeDefOf.RejectInput, false);
					}
				}
			};
			return command_Target;
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x001B12CD File Offset: 0x001AF4CD
		private static bool ShouldUseMeleeAttackGizmo(Pawn pawn)
		{
			return pawn.Drafted && (PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasRangedWeapon() || PawnAttackGizmoUtility.AtLeastOneSelectedColonistHasNoWeapon() || PawnAttackGizmoUtility.AtLeastTwoSelectedColonistsHaveDifferentWeapons());
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x001B12F0 File Offset: 0x001AF4F0
		private static Gizmo GetMeleeAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandMeleeAttack".Translate();
			command_Target.defaultDesc = "CommandMeleeAttackDesc".Translate();
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.hotKey = KeyBindingDefOf.Misc2;
			command_Target.icon = TexCommand.AttackMelee;
			string str;
			if (FloatMenuUtility.GetMeleeAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && pawn3.IsColonistPlayerControlled && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					string text;
					Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn2, target, out text);
					if (meleeAttackAction != null)
					{
						meleeAttackAction();
					}
					else if (!text.NullOrEmpty())
					{
						Messages.Message(text, target.Thing, MessageTypeDefOf.RejectInput, false);
					}
				}
			};
			return command_Target;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x001B139C File Offset: 0x001AF59C
		private static bool AtLeastOneSelectedColonistHasRangedWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled && pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x001B140C File Offset: 0x001AF60C
		private static bool AtLeastOneSelectedColonistHasNoWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled && (pawn.equipment == null || pawn.equipment.Primary == null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x001B1468 File Offset: 0x001AF668
		private static bool AtLeastTwoSelectedColonistsHaveDifferentWeapons()
		{
			if (Find.Selector.NumSelected <= 1)
			{
				return false;
			}
			ThingDef thingDef = null;
			bool flag = false;
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled)
				{
					ThingDef thingDef2;
					if (pawn.equipment == null || pawn.equipment.Primary == null)
					{
						thingDef2 = null;
					}
					else
					{
						thingDef2 = pawn.equipment.Primary.def;
					}
					if (!flag)
					{
						thingDef = thingDef2;
						flag = true;
					}
					else if (thingDef2 != thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x001B1500 File Offset: 0x001AF700
		private static bool AllSelectedColonistsCanTargetLocations()
		{
			using (List<object>.Enumerator enumerator = Find.Selector.SelectedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null && pawn.IsColonistPlayerControlled && pawn.Drafted)
					{
						if (pawn.equipment.Primary == null || pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
						{
							return false;
						}
						if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.targetParams.canTargetLocations)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
