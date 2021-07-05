using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C8 RID: 5064
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06007B27 RID: 31527 RVA: 0x002B7BB0 File Offset: 0x002B5DB0
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from c in base.Map.mapPawns.FreeColonists
				where c.equipment.Primary != null
				select c).Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsCount;
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06007B28 RID: 31528 RVA: 0x002B7C09 File Offset: 0x002B5E09
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x06007B29 RID: 31529 RVA: 0x002B7C39 File Offset: 0x002B5E39
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x06007B2A RID: 31530 RVA: 0x002B7C5C File Offset: 0x002B5E5C
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06007B2B RID: 31531 RVA: 0x002B7CB8 File Offset: 0x002B5EB8
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.Weapons)
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
