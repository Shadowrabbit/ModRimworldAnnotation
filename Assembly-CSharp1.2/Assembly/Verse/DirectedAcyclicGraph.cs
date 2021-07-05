using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200080E RID: 2062
	public class DirectedAcyclicGraph
	{
		// Token: 0x060033E6 RID: 13286 RVA: 0x00151318 File Offset: 0x0014F518
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x00028B39 File Offset: 0x00026D39
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0015135C File Offset: 0x0014F55C
		public List<int> TopologicalSort()
		{
			bool[] array = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
			}
			List<int> result = new List<int>();
			for (int j = 0; j < this.numVertices; j++)
			{
				if (!array[j])
				{
					this.TopologicalSortInner(j, array, result);
				}
			}
			return result;
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x001513B0 File Offset: 0x0014F5B0
		private void TopologicalSortInner(int v, bool[] visited, List<int> result)
		{
			visited[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num])
				{
					this.TopologicalSortInner(num, visited, result);
				}
			}
			result.Add(v);
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x00028B4D File Offset: 0x00026D4D
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0015141C File Offset: 0x0014F61C
		public int FindCycle()
		{
			bool[] array = new bool[this.numVertices];
			bool[] array2 = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
				array2[i] = false;
			}
			for (int j = 0; j < this.numVertices; j++)
			{
				if (this.IsCyclicInner(j, array, array2))
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0015147C File Offset: 0x0014F67C
		private bool IsCyclicInner(int v, bool[] visited, bool[] history)
		{
			visited[v] = true;
			history[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num] && this.IsCyclicInner(num, visited, history))
				{
					return true;
				}
				if (history[num])
				{
					return true;
				}
			}
			history[v] = false;
			return false;
		}

		// Token: 0x040023FA RID: 9210
		private int numVertices;

		// Token: 0x040023FB RID: 9211
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}
