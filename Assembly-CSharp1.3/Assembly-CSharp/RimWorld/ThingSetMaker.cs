using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD9 RID: 2777
	public abstract class ThingSetMaker
	{
		// Token: 0x0600415F RID: 16735 RVA: 0x0015F49C File Offset: 0x0015D69C
		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x0015F4B8 File Offset: 0x0015D6B8
		public List<Thing> Generate(ThingSetMakerParams parms)
		{
			List<Thing> list = new List<Thing>();
			ThingSetMaker.thingsBeingGeneratedNow.Add(list);
			try
			{
				ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
				this.Generate(parms2, list);
				this.PostProcess(list);
			}
			catch (Exception arg)
			{
				Log.Error("Exception while generating thing set: " + arg);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					list[i].Destroy(DestroyMode.Vanish);
					list.RemoveAt(i);
				}
			}
			finally
			{
				ThingSetMaker.thingsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0015F554 File Offset: 0x0015D754
		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		// Token: 0x06004163 RID: 16739
		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		// Token: 0x06004164 RID: 16740 RVA: 0x0015F570 File Offset: 0x0015D770
		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0015F58C File Offset: 0x0015D78C
		public IEnumerable<ThingDef> AllGeneratableThingsDebug(ThingSetMakerParams parms)
		{
			if (!this.CanGenerate(parms))
			{
				yield break;
			}
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			foreach (ThingDef thingDef in this.AllGeneratableThingsDebugSub(parms2).Distinct<ThingDef>())
			{
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
		{
			return 1f;
		}

		// Token: 0x06004167 RID: 16743
		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		// Token: 0x06004168 RID: 16744 RVA: 0x0015F5A4 File Offset: 0x0015D7A4
		private void PostProcess(List<Thing> things)
		{
			if (things.RemoveAll((Thing x) => x == null) != 0)
			{
				Log.Error(base.GetType() + " generated null things.");
			}
			this.ChangeDeadPawnsToTheirCorpses(things);
			for (int i = things.Count - 1; i >= 0; i--)
			{
				if (things[i].Destroyed)
				{
					Log.Error(base.GetType() + " generated destroyed thing " + things[i].ToStringSafe<Thing>());
					things.RemoveAt(i);
				}
				else if (things[i].stackCount <= 0)
				{
					Log.Error(string.Concat(new object[]
					{
						base.GetType(),
						" generated ",
						things[i].ToStringSafe<Thing>(),
						" with stackCount=",
						things[i].stackCount
					}));
					things.RemoveAt(i);
				}
			}
			this.Minify(things);
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x0015F6B0 File Offset: 0x0015D8B0
		private void Minify(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.Minifiable)
				{
					int stackCount = things[i].stackCount;
					things[i].stackCount = 1;
					MinifiedThing minifiedThing = things[i].MakeMinified();
					minifiedThing.stackCount = stackCount;
					things[i] = minifiedThing;
				}
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0015F718 File Offset: 0x0015D918
		private void ChangeDeadPawnsToTheirCorpses(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].ParentHolder is Corpse)
				{
					things[i] = (Corpse)things[i].ParentHolder;
				}
			}
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x0015F764 File Offset: 0x0015D964
		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x0015F781 File Offset: 0x0015D981
		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}

		// Token: 0x04002759 RID: 10073
		public ThingSetMakerParams fixedParams;

		// Token: 0x0400275A RID: 10074
		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();
	}
}
