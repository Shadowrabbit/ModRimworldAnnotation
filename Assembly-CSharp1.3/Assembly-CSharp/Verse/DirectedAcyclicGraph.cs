using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200049B RID: 1179
	public class DirectedAcyclicGraph
	{
		// Token: 0x060023E8 RID: 9192 RVA: 0x000DFAB8 File Offset: 0x000DDCB8
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000DFAF9 File Offset: 0x000DDCF9
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000DFB10 File Offset: 0x000DDD10
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

		// Token: 0x060023EB RID: 9195 RVA: 0x000DFB64 File Offset: 0x000DDD64
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

		// Token: 0x060023EC RID: 9196 RVA: 0x000DFBD0 File Offset: 0x000DDDD0
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000DFBE0 File Offset: 0x000DDDE0
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

		// Token: 0x060023EE RID: 9198 RVA: 0x000DFC40 File Offset: 0x000DDE40
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

		// Token: 0x040016A7 RID: 5799
		private int numVertices;

		// Token: 0x040016A8 RID: 5800
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}
