using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x02000677 RID: 1655
	public class StateGraph
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002EDA RID: 11994 RVA: 0x001173B5 File Offset: 0x001155B5
		// (set) Token: 0x06002EDB RID: 11995 RVA: 0x001173C3 File Offset: 0x001155C3
		public LordToil StartingToil
		{
			get
			{
				return this.lordToils[0];
			}
			set
			{
				if (this.lordToils.Contains(value))
				{
					this.lordToils.Remove(value);
				}
				this.lordToils.Insert(0, value);
			}
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x001173ED File Offset: 0x001155ED
		public void AddToil(LordToil toil)
		{
			this.lordToils.Add(toil);
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x001173FB File Offset: 0x001155FB
		public void AddTransition(Transition transition, bool highPriority = false)
		{
			if (highPriority)
			{
				this.transitions.Insert(0, transition);
				return;
			}
			this.transitions.Add(transition);
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x0011741C File Offset: 0x0011561C
		public StateGraph AttachSubgraph(StateGraph subGraph)
		{
			for (int i = 0; i < subGraph.lordToils.Count; i++)
			{
				this.lordToils.Add(subGraph.lordToils[i]);
			}
			for (int j = 0; j < subGraph.transitions.Count; j++)
			{
				this.transitions.Add(subGraph.transitions[j]);
			}
			return subGraph;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x00117484 File Offset: 0x00115684
		public void ErrorCheck()
		{
			if (this.lordToils.Count == 0)
			{
				Log.Error("Graph has 0 lord toils.");
			}
			using (IEnumerator<LordToil> enumerator = this.lordToils.Distinct<LordToil>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LordToil toil = enumerator.Current;
					int num = (from s in this.lordToils
					where s == toil
					select s).Count<LordToil>();
					if (num != 1)
					{
						Log.Error(string.Concat(new object[]
						{
							"Graph has lord toil ",
							toil,
							" registered ",
							num,
							" times."
						}));
					}
				}
			}
			using (List<Transition>.Enumerator enumerator2 = this.transitions.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Transition trans = enumerator2.Current;
					int num2 = (from t in this.transitions
					where t == trans
					select t).Count<Transition>();
					if (num2 != 1)
					{
						Log.Error(string.Concat(new object[]
						{
							"Graph has transition ",
							trans,
							" registered ",
							num2,
							" times."
						}));
					}
				}
			}
			StateGraph.checkedToils = new HashSet<LordToil>();
			this.CheckForUnregisteredLinkedToilsRecursive(this.StartingToil);
			StateGraph.checkedToils = null;
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x00117610 File Offset: 0x00115810
		private void CheckForUnregisteredLinkedToilsRecursive(LordToil toil)
		{
			if (!this.lordToils.Contains(toil))
			{
				Log.Error("Unregistered linked lord toil: " + toil);
			}
			StateGraph.checkedToils.Add(toil);
			for (int i = 0; i < this.transitions.Count; i++)
			{
				Transition transition = this.transitions[i];
				if (transition.sources.Contains(toil) && !StateGraph.checkedToils.Contains(toil))
				{
					this.CheckForUnregisteredLinkedToilsRecursive(transition.target);
				}
			}
		}

		// Token: 0x04001CA7 RID: 7335
		public List<LordToil> lordToils = new List<LordToil>();

		// Token: 0x04001CA8 RID: 7336
		public List<Transition> transitions = new List<Transition>();

		// Token: 0x04001CA9 RID: 7337
		private static HashSet<LordToil> checkedToils;
	}
}
