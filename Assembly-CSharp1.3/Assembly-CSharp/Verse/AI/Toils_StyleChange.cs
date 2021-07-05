using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005B9 RID: 1465
	public static class Toils_StyleChange
	{
		// Token: 0x06002AC4 RID: 10948 RVA: 0x00100BE0 File Offset: 0x000FEDE0
		public static Toil SetupLookChangeData()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.style.SetupNextLookChangeData(Toils_StyleChange.GetRandomStyleItemWeighted<HairDef>(actor, actor.story.hairDef, null), Toils_StyleChange.GetRandomStyleItemWeighted<BeardDef>(actor, actor.style.beardDef, null), Toils_StyleChange.GetRandomStyleItemWeighted<TattooDef>(actor, actor.style.FaceTattoo, new TattooType?(TattooType.Face)), Toils_StyleChange.GetRandomStyleItemWeighted<TattooDef>(actor, actor.style.BodyTattoo, new TattooType?(TattooType.Body)));
			};
			return toil;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x00100C1B File Offset: 0x000FEE1B
		public static Toil DoLookChange(TargetIndex StationIndex, Pawn pawn)
		{
			return Toils_General.WaitWith(StationIndex, 300, true, false).FailOnDespawnedOrNull(StationIndex).PlaySustainerOrSound(SoundDefOf.HairCutting, 1f).WithEffect(EffecterDefOf.HairCutting, StationIndex, new Color?(pawn.story.hairColor));
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x00100C5C File Offset: 0x000FEE5C
		public static Toil FinalizeLookChange()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				bool flag = false;
				if (actor.style.nextHairDef != null && actor.style.nextHairDef != actor.story.hairDef)
				{
					flag = true;
					actor.story.hairDef = actor.style.nextHairDef;
				}
				if (actor.style.beardDef != null && actor.style.nextBeardDef != actor.style.beardDef)
				{
					flag = true;
					actor.style.beardDef = actor.style.nextBeardDef;
				}
				if (actor.style.nextFaceTattooDef != null)
				{
					actor.style.FaceTattoo = actor.style.nextFaceTattooDef;
				}
				if (actor.style.nextBodyTatooDef != null)
				{
					actor.style.BodyTattoo = actor.style.nextBodyTatooDef;
				}
				actor.style.Notify_StyleItemChanged();
				actor.style.SetGraphicsDirty();
				if (flag)
				{
					actor.style.MakeHairFilth();
				}
			};
			return toil;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x00100C98 File Offset: 0x000FEE98
		private static T GetRandomStyleItemWeighted<T>(Pawn pawn, T current, TattooType? tattooType = null) where T : StyleItemDef
		{
			return (from x in DefDatabase<T>.AllDefs
			where PawnStyleItemChooser.WantsToUseStyle(pawn, x, tattooType)
			select x).RandomElementByWeight((T x) => PawnStyleItemChooser.TotalStyleItemLikelihood(x, pawn) * ((x == current) ? 0.05f : 1f));
		}
	}
}
