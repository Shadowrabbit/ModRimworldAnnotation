using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200001C RID: 28
	public class LRUCache<K, V>
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000709D File Offset: 0x0000529D
		public LRUCache(int capacity)
		{
			this.capacity = capacity;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000070C4 File Offset: 0x000052C4
		public bool TryGetValue(K key, out V result)
		{
			LinkedListNode<ValueTuple<K, V>> linkedListNode;
			if (this.cache.TryGetValue(key, out linkedListNode))
			{
				result = linkedListNode.Value.Item2;
				this.WasUsed(linkedListNode);
				return true;
			}
			result = default(V);
			return false;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007104 File Offset: 0x00005304
		public void Add(K key, V value)
		{
			if (this.cache.Count > this.capacity)
			{
				this.RemoveLeastUsed();
			}
			LinkedListNode<ValueTuple<K, V>> linkedListNode = new LinkedListNode<ValueTuple<K, V>>(new ValueTuple<K, V>(key, value));
			this.cache.Add(key, linkedListNode);
			this.leastRecentList.AddLast(linkedListNode);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007150 File Offset: 0x00005350
		public void Clear()
		{
			this.cache.Clear();
			this.leastRecentList.Clear();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007168 File Offset: 0x00005368
		private void WasUsed(LinkedListNode<ValueTuple<K, V>> node)
		{
			this.leastRecentList.Remove(node);
			this.leastRecentList.AddLast(node);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007184 File Offset: 0x00005384
		private void RemoveLeastUsed()
		{
			LinkedListNode<ValueTuple<K, V>> first = this.leastRecentList.First;
			if (first != null)
			{
				this.leastRecentList.RemoveFirst();
				this.cache.Remove(first.Value.Item1);
			}
		}

		// Token: 0x04000045 RID: 69
		private readonly Dictionary<K, LinkedListNode<ValueTuple<K, V>>> cache = new Dictionary<K, LinkedListNode<ValueTuple<K, V>>>();

		// Token: 0x04000046 RID: 70
		private readonly LinkedList<ValueTuple<K, V>> leastRecentList = new LinkedList<ValueTuple<K, V>>();

		// Token: 0x04000047 RID: 71
		private readonly int capacity;
	}
}
