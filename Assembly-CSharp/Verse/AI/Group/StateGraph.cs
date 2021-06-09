using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x02000ADC RID: 2780
	public class StateGraph
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x00030EC1 File Offset: 0x0002F0C1
		// (set) Token: 0x060041B8 RID: 16824 RVA: 0x00030ECF File Offset: 0x0002F0CF
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

		// Token: 0x060041B9 RID: 16825 RVA: 0x00030EF9 File Offset: 0x0002F0F9
		public void AddToil(LordToil toil)
		{
			this.lordToils.Add(toil);
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x00030F07 File Offset: 0x0002F107
		public void AddTransition(Transition transition, bool highPriority = false)
		{
			if (highPriority)
			{
				this.transitions.Insert(0, transition);
				return;
			}
			this.transitions.Add(transition);
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x00188350 File Offset: 0x00186550
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

		// Token: 0x060041BC RID: 16828 RVA: 0x001883B8 File Offset: 0x001865B8
		public void ErrorCheck()
		{
			if (this.lordToils.Count == 0)
			{
				Log.Error("Graph has 0 lord toils.", false);
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
						}), false);
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
						}), false);
					}
				}
			}
			StateGraph.checkedToils = new HashSet<LordToil>();
			this.CheckForUnregisteredLinkedToilsRecursive(this.StartingToil);
			StateGraph.checkedToils = null;
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00188548 File Offset: 0x00186748
		private void CheckForUnregisteredLinkedToilsRecursive(LordToil toil)
		{
			if (!this.lordToils.Contains(toil))
			{
				Log.Error("Unregistered linked lord toil: " + toil, false);
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

		// Token: 0x04002D36 RID: 11574
		public List<LordToil> lordToils = new List<LordToil>();

		// Token: 0x04002D37 RID: 11575
		public List<Transition> transitions = new List<Transition>();

		// Token: 0x04002D38 RID: 11576
		private static HashSet<LordToil> checkedToils;
	}
}
