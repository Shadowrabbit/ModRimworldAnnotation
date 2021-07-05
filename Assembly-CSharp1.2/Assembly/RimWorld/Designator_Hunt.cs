using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A3 RID: 6563
	public class Designator_Hunt : Designator
	{
		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x06009114 RID: 37140 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x06009115 RID: 37141 RVA: 0x000614A0 File Offset: 0x0005F6A0
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x06009116 RID: 37142 RVA: 0x0029BDE0 File Offset: 0x00299FE0
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

		// Token: 0x06009117 RID: 37143 RVA: 0x000614A7 File Offset: 0x0005F6A7
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

		// Token: 0x06009118 RID: 37144 RVA: 0x0029BE6C File Offset: 0x0029A06C
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.HuntablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06009119 RID: 37145 RVA: 0x0029BEBC File Offset: 0x0029A0BC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.AnimalOrWildMan() && !pawn.IsPrisonerInPrisonCell() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600911A RID: 37146 RVA: 0x0029BF24 File Offset: 0x0029A124
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x0600911B RID: 37147 RVA: 0x0029BF78 File Offset: 0x0029A178
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

		// Token: 0x0600911C RID: 37148 RVA: 0x000614E2 File Offset: 0x0005F6E2
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

		// Token: 0x0600911D RID: 37149 RVA: 0x0029C020 File Offset: 0x0029A220
		private void ShowDesignationWarnings(Pawn pawn)
		{
			float manhunterOnDamageChance = pawn.RaceProps.manhunterOnDamageChance;
			float manhunterOnDamageChance2 = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			if (manhunterOnDamageChance >= 0.015f)
			{
				Messages.Message("MessageAnimalsGoPsychoHunted".Translate(pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(), manhunterOnDamageChance2.ToStringPercent(), pawn.Named("ANIMAL")).CapitalizeFirst(), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x04005C17 RID: 23575
		private List<Pawn> justDesignated = new List<Pawn>();
	}
}
