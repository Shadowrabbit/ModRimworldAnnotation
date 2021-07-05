using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001527 RID: 5415
	public abstract class RoomRequirement : IExposable
	{
		// Token: 0x060080D5 RID: 32981 RVA: 0x002D9E34 File Offset: 0x002D8034
		public bool MetOrDisabled(Room room, Pawn p = null)
		{
			return this.Disabled(room, p) || this.Met(room, p);
		}

		// Token: 0x060080D6 RID: 32982 RVA: 0x002D9E4C File Offset: 0x002D804C
		public virtual bool Disabled(Room room, Pawn p = null)
		{
			if (this.disablingPrecepts != null && p != null && p.Ideo != null)
			{
				foreach (Precept precept in p.Ideo.PreceptsListForReading)
				{
					if (this.disablingPrecepts.Contains(precept.def))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060080D7 RID: 32983
		public abstract bool Met(Room r, Pawn p = null);

		// Token: 0x060080D8 RID: 32984 RVA: 0x002D9ECC File Offset: 0x002D80CC
		public virtual string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x060080D9 RID: 32985 RVA: 0x002D9EDE File Offset: 0x002D80DE
		public string LabelCap(Room r = null)
		{
			return this.Label(r).CapitalizeFirst();
		}

		// Token: 0x060080DA RID: 32986 RVA: 0x002D9EEC File Offset: 0x002D80EC
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x060080DB RID: 32987 RVA: 0x002D9EF5 File Offset: 0x002D80F5
		public virtual bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.GetType() == other.GetType();
		}

		// Token: 0x060080DC RID: 32988 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool PlayerCanBuildNow()
		{
			return true;
		}

		// Token: 0x060080DD RID: 32989 RVA: 0x002D9F08 File Offset: 0x002D8108
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.labelKey, "labelKey", null, false);
		}

		// Token: 0x04005051 RID: 20561
		public List<PreceptDef> disablingPrecepts;

		// Token: 0x04005052 RID: 20562
		[NoTranslate]
		public string labelKey;
	}
}
