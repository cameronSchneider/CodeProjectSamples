#include "itemset.h"


Itemset::Itemset()
{
	mTotalNumberOfItems = 0;
	mItems = NULL;
	mSupport = 0;
}

Itemset::Itemset(int numberOfItems)
{
	mTotalNumberOfItems = numberOfItems;

	mItems = new int[numberOfItems];

	mSupport = 0;
}

Itemset::~Itemset()
{
	mItems = NULL;
	delete mItems;
}

void Itemset::printItemsetToConsole() const
{
	for (int i = 0; i < mTotalNumberOfItems; i++)
	{
		cout << mItems[i] << " ";
	}
}

void Itemset::printItemsetToFile(ofstream& out) const
{
	out << "{";
	for (int i = 0; i < mTotalNumberOfItems; i++)
	{
		if(i != mTotalNumberOfItems - 1)
			out << mItems[i] << ", ";
		else
			out << mItems[i];
	}
	out << "} ";
}

ostream& operator<<(ostream& output, const Itemset &obj)
{
	obj.printItemsetToConsole();

	return output;
}

ofstream& operator<<(ofstream& out, const Itemset& obj)
{
	obj.printItemsetToFile(out);

	return out;
}

ofstream& operator<<(ofstream& out, const int& num)
{
	out << num;
	return out;
}