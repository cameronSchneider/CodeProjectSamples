#include <iostream>
using namespace std;

template<typename T>
void BubbleSort(T data[], int size)
{
	bool swapped = true;
	T temp;

	while (swapped)
	{
		swapped = false;
		for (int i = 0; i < size - 1; i++)
		{
			if (data[i] > data[i + 1])
			{
				temp = data[i + 1];
				data[i + 1] = data[i];
				data[i] = temp;
				swapped = true;
			}
		}
	}
}

template<typename T>
void InsertionSort(T data[], int size)
{
	T value;
	int j;

	for (int i = 1; i < size; i++)
	{
		value = data[i];
		j = i - 1;

		while (j >= 0 && data[j] > value)
		{
			data[j + 1] = data[j];
			j -= 1;
		}

		data[j + 1] = value;
	}
}

template<typename T>
void selectionSort(T data[], int size)
{
	T temp;
	int minIndex;

	for (int i = 0; i < size - 1; i++)
	{
		minIndex = i;

		for (int j = i + 1; j < size; j++)
		{
			if (data[j] < data[minIndex])
				minIndex = j;
		}

		if (minIndex != i)
		{
			temp = data[minIndex];
			data[minIndex] = data[i];
			data[i] = temp;
		}
	}
}

template<typename T>
void ShellSort(T data[], int size)
{
	T temp;
	int gap = size / 2;

	while (gap > 0)
	{
		for (int i = 0; i < size - gap; i++)
		{
			if (data[i] > data[i + gap])
			{
				temp = data[i + gap];
				data[i + gap] = data[i];
				data[i] = temp;
			}
		}

		gap = gap / 2;
	}

	BubbleSort(data, size);
}

template<typename T>
void merge(T data[], int lowerBound, int upperBound, int mid)
{
	int i, j, k;
	T* temp = new T[upperBound - lowerBound + 1];
	i = lowerBound;
	j = mid + 1;
	k = 0;

	while (i <= mid && j <= upperBound)
	{
		if (data[i] < data[j])
		{
			temp[k] = data[i];
			k++;
			i++;
		}
		else
		{
			temp[k] = data[j];
			k++;
			j++;
		}
	}

	while (i <= mid)
	{
		temp[k] = data[i];
		k++;
		i++;
	}

	while (j <= upperBound)
	{
		temp[k] = data[j];
		k++;
		j++;
	}

	for (i = lowerBound; i <= upperBound; i++)
	{
		data[i] = temp[i - lowerBound];
	}

	delete[] temp;
}

template<typename T>
void MergeSort(T data[], int lowerBound, int upperBound)
{
	int mid;

	if (lowerBound < upperBound)
	{
		mid = (lowerBound + upperBound) / 2;
		MergeSort(data, lowerBound, mid);
		MergeSort(data, mid + 1, upperBound);
		merge(data, lowerBound, upperBound, mid);
	}
}

template<typename T>
void QuickSort(T data[], int lowerBound, int upperBound)
{
	int i = lowerBound, j = upperBound;

	int pivot = data[(lowerBound + upperBound) / 2];
	T temp;

	while (i <= j)
	{
		while (data[i] < pivot)
		{
			i += 1;
		}

		while (data[j] > pivot)
		{
			j -= 1;
		}

		if (i <= j)
		{
			temp = data[i];
			data[i] = data[j];
			data[j] = temp;

			i += 1;
			j -= 1;
		}
	}

	if (lowerBound < j)
	{
		QuickSort(data, lowerBound, j);
	}
	if (i < upperBound)
	{
		QuickSort(data, i, upperBound);
	}
}

template<typename T>
void heapify(T list[], int size, int i)
{
	T temp;
	int largest = i;
	int left = 2 * i + 1;
	int right = 2 * i + 2;

	if (left < size && list[left] > list[largest])
		largest = left;

	if (right < size && list[right] > list[largest])
		largest = right;

	if (largest != i)
	{
		temp = list[i];
		list[i] = list[largest];
		list[largest] = temp;

		heapify(list, size, largest);
	}
}

template<typename T>
void heapSort(T list[], int size)
{
	T temp;

	for (int i = size / 2 - 1; i >= 0; i--)
	{
		heapify(list, size, i);
	}

	for (int i = size - 1; i >= 0; i--)
	{
		temp = list[0];
		list[0] = list[i];
		list[i] = temp;

		heapify(list, i, 0);
	}
}

template<typename T>
void printArray(T data[], int size)
{
	for (int i = 0; i < size; i++)
	{
		cout << data[i] << ", ";
	}
	cout << endl;
}

int main()
{
	int x[7] = { 7, 6, 4, 8, 1, 2, 3 };

	//BubbleSort(x, 7);
	//InsertionSort(x, 7);
	//selectionSort(x, 7);
	//ShellSort(x, 7);
	//MergeSort(x, 0, 6);
	QuickSort(x, 0, 6);
	//heapSort(x, 7);
	printArray(x, 7);

	system("pause");
	return 0;
}
