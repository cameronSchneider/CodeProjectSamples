#pragma once
#include <iostream>
#include <fstream>
using namespace std;

class Itemset
{
private:
	int mTotalNumberOfItems;
	int mCurrentNumItems = 0;
	int mSupport;

public:
	int *mItems;

	Itemset();
	Itemset(int numberOfItems);
	~Itemset();

	inline int getSupport()  const { return mSupport; };
	inline int getTotalNumItems() const { return mTotalNumberOfItems; };
	inline int getCurrentNumItems() const { return mCurrentNumItems; };
	inline void incrementCurrentNumItems() { mCurrentNumItems++; };
	inline void setSupport(int support) { mSupport = support; };

	//This is strictly to test with the console
	void printItemsetToConsole() const;

	//This is to output frequent itemsets to file
	void printItemsetToFile(ofstream &out) const;

	friend ostream& operator<<(ostream& out, const Itemset &obj);
	friend ofstream& operator<<(ofstream& out, const Itemset& set);
	friend ofstream& operator<<(ofstream& out, const int& num);
};