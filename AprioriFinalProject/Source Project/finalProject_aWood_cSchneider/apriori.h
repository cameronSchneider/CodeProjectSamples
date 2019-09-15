#pragma once
#include "stack.h"
#include "itemset.h"
#include <iostream>
#include <fstream>
#include <sstream>

using namespace std;

class Apriori
{
private:
	
	double mMinSupport;
	int mNumTransactions;
	int mNumItems;
	int mSupportColumns;
	int mK;
	
	int** mDataset = NULL;
	int** mSupportTable = NULL;

	int* mTransactionLengths = NULL;
	int* mIndividualItemsetSupport = NULL;

	string mDataFileName;

	void calculateSupport();
	void clearOldSupportData();
	void copyCommonItems(int* common, Itemset &newSet);

public:
	Stack<Itemset> *mCk, *mLk;

	Apriori();
	Apriori(string fileName, double minSupport);
	~Apriori();

	void initializeDataset();

	void retrieveData();

	void calculateSupportTable();
	bool getIndividualItemsetSupport(bool *perItemSupport);
	void calculateIndividualSupport();
	void prune();

	void join();

	void generateFirstItemsets();
	void apriori_gen();

	void apriori_run();
};


int convertStringToInt(string str);
int getNumberCount(string str);
double convertStringToDouble(string str);
string extractString(string source, string start, string end);
void putNumbersInArray(string str, int *arr, int size);


//void generateFirstItemsets(int numTransactions, int numItems, Stack<Itemset> &L1, Stack<Itemset> &C1);
//void apriori_gen();

//void calculateSupportTable(Stack<Itemset> &Ck, int** &dataset, int* &transactionLengths, int* &individualSupport, int numTransactions, int k);
//bool getIndividualItemsetSupport(bool *perItemSupport, int size);
//void calculateIndividualSupport(Stack<Itemset> &Ck, int** &supportTable, int* &individualSupport, int numTransactions, int totalColumns);
//void prune(Stack<Itemset> &Ck, Stack<Itemset> &Lk);