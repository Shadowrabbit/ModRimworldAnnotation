using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCC RID: 7116
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06009CAF RID: 40111 RVA: 0x002DECA8 File Offset: 0x002DCEA8
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from c in base.Map.mapPawns.FreeColonists
				where c.equipment.Primary != null
				select c).Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsCount;
			}
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06009CB0 RID: 40112 RVA: 0x00068261 File Offset: 0x00066461
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x06009CB1 RID: 40113 RVA: 0x00068291 File Offset: 0x00066491
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x06009CB2 RID: 40114 RVA: 0x002DED04 File Offset: 0x002DCF04
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06009CB3 RID: 40115 RVA: 0x002DED60 File Offset: 0x002DCF60
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
