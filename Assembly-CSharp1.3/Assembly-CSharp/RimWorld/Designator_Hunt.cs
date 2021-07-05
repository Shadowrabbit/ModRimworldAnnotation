using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B0 RID: 4784
	public class Designator_Hunt : Designator
	{
		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06007248 RID: 29256 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06007249 RID: 29257 RVA: 0x00262A82 File Offset: 0x00260C82
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x0600724A RID: 29258 RVA: 0x00262A8C File Offset: 0x00260C8C
		public Designator_Hunt()
		{
			this.defaultLabel = "DesignatorHunt".Translate();
			this.defaultDesc = "DesignatorHuntDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x0600724B RID: 29259 RVA: 0x00262B18 File Offset: 0x00260D18
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.HuntablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateHuntable".Translate();
			}
			return true;
		}

		// Token: 0x0600724C RID: 29260 RVA: 0x00262B54 File Offset: 0x00260D54
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.HuntablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600724D RID: 29261 RVA: 0x00262BA4 File Offset: 0x00260DA4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.AnimalOrWildMan() && !pawn.IsPrisonerInPrisonCell() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600724E RID: 29262 RVA: 0x00262C0C File Offset: 0x00260E0C
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x0600724F RID: 29263 RVA: 0x00262C60 File Offset: 0x00260E60
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					this.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind));
				}
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06007250 RID: 29264 RVA: 0x00262D08 File Offset: 0x00260F08
		private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return (Pawn)thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007251 RID: 29265 RVA: 0x00262D20 File Offset: 0x00260F20
		private void ShowDesignationWarnings(Pawn pawn)
		{
			float manhunterOnDamageChance = pawn.RaceProps.manhunterOnDamageChance;
			float manhunterOnDamageChance2 = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			if (manhunterOnDamageChance >= 0.015f)
			{
				Messages.Message("MessageAnimalsGoPsychoHunted".Translate(pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(), manhunterOnDamageChance2.ToStringPercent(), pawn.Named("ANIMAL")).CapitalizeFirst(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
		}

		// Token: 0x04003EC1 RID: 16065
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
