using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {

	T[] items;
	int count;

	public Heap (int MaximumHeapSize) {
		items = new T[MaximumHeapSize];
	}

	public void Add (T item) {
		item.HeapIndex = count;
		items[count] = item;
		SortUp(item);
		count++;
	}

	public T RemoveFirst () {
		T firstItem = items[0];
		count--;
		items[0] = items[count];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}

	public T RemoveAt (int index) {
		T item = items[index];
		count--;
		items[index] = items[count];
		items[index].HeapIndex = index;
		SortDown(items[index]);
		return item;
	}

	public T this[int index] {
		get {
			return items[index];
		}
	}

	public void UpdateItem (T item) {
		SortUp(item);
	}

	public bool Contains (T item) {
		return Equals(items[item.HeapIndex], item);
	}

	public int Count {
		get {
			return count;
		}
	}

	void SortDown (T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < count) {
				swapIndex = childIndexLeft;

				if (childIndexRight < count) {
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap(item, items[swapIndex]);
				}
				else {
					return;
				}
			}
			else {
				return;
			}
		}
	}

	void SortUp (T item) {
		int parentIndex = Mathf.FloorToInt((item.HeapIndex-1)/2);

		while (true) {
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				Swap (item, parentItem);
			}
			else {break;}
			parentIndex = Mathf.FloorToInt((item.HeapIndex-1)/2);
		}
	}

	void Swap (T item, T withItem) {
		items[item.HeapIndex] = withItem;
		items[withItem.HeapIndex] = item;
		int iAindex = item.HeapIndex;
		item.HeapIndex = withItem.HeapIndex;
		withItem.HeapIndex = iAindex;
	}

}

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}
