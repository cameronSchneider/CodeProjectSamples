#include "apriori.h"

Apriori::Apriori()
{
	cout << "ERROR: No data file provided!" << endl;
}

Apriori::Apriori(string fileName, double minSupport)
{
	mDataFileName = fileName;
	mMinSupport = minSupport;

	mK = 1;

	mCk = new Stack<Itemset>();
	mLk = new Stack<Itemset>();

	initializeDataset();
}

Apriori::~Apriori()
{
	mCk->clear();
	mLk->clear();

	delete mCk;
	delete mLk;

	for (int i = 0; i < mNumTransactions; i++)
	{
		mDataset[i] = NULL;
		delete[] mDataset[i];
	}
	mDataset = NULL;
	delete[] mDataset;

	mSupportTable = NULL;
	delete[] mSupportTable;

	mTransactionLengths = NULL;;
	delete[] mTransactionLengths;
	mIndividualItemsetSupport = NULL;
	delete[] mIndividualItemsetSupport;

}

void Apriori::initializeDataset()
{
	retrieveData();
	mMinSupport = (mMinSupport / 100.0) * mNumTransactions;
}

void Apriori::retrieveData()
{
	const string numTransactionsStartDelimiter = "D";
	const string numTransactionsEndDelimiter = "K";
	const string numItemsStartDelimiter = "N";
	const string numItemsEndDelimiter = "K";

	ifstream input(mDataFileName);

	if (input.fail())
		cout << "File doesn't exist" << endl;

	// This line finds the number of transactions in the dataset, which is a substring between the delimiters "D" and "K".
	// It then converts that substring to an integer and multiplies it by 1000 since the number of transactions is measured in 1000's.
	mNumTransactions = convertStringToDouble(extractString(mDataFileName, numTransactionsStartDelimiter, numTransactionsEndDelimiter)) * 1000;


	mNumItems = convertStringToDouble(extractString(mDataFileName, numItemsStartDelimiter, numItemsEndDelimiter)) * 1000;

	string currentTransaction;
	int lineNumber = 0;
	int i = 0;

	mTransactionLengths = new int[mNumTransactions];
	mDataset = new int *[mNumTransactions];

	while (!input.eof())
	{
		getline(input, currentTransaction);

		//count numbers in current and initialize dataset[lineNumber][count]
		if (!currentTransaction.empty())
		{
			mTransactionLengths[lineNumber] = getNumberCount(currentTransaction);
			mDataset[lineNumber] = new int[mTransactionLengths[lineNumber] - 1];

			//Insert each number in current individually into dataset[linenumber]
			putNumbersInArray(currentTransaction, mDataset[lineNumber], mTransactionLengths[lineNumber]);

			lineNumber++;
		}
	}
}

void Apriori::generateFirstItemsets()
{
	const int setSize = 1;
	int i = 0;

	for (i = 0; i < mNumItems; i++)
	{
		Itemset newSet(setSize);
		newSet.mItems[0] = i;
		mCk->push(newSet);
	}

	prune();
}

void Apriori::prune()
{
	mLk->clear();
	int i;
	int totalLengthCk = mCk->getCount();

	calculateSupport();

	for (i = 0; i < totalLengthCk; i++)
	{
		if ((*mCk)[i].getSupport() >= mMinSupport)
		{
			mLk->push((*mCk)[i]);
		}
	}

	clearOldSupportData();
}

void Apriori::calculateIndividualSupport()
{
	int i, j;

	mIndividualItemsetSupport = new int[mSupportColumns];

	for (i = 0; i < mSupportColumns; i++)
	{
		mIndividualItemsetSupport[i] = 0;
	}

	for (i = 0; i < mSupportColumns; i++)
	{
		for (j = 0; j < mNumTransactions; j++)
		{
			mIndividualItemsetSupport[i] += mSupportTable[j][i];
		}
	}
	
	for (i = 0; i < mSupportColumns; i++)
	{
		((*mCk)[i]).setSupport(mIndividualItemsetSupport[i]);
	}

}

void Apriori::calculateSupportTable()
{
	mSupportTable = new int *[mNumTransactions];
	bool* perItemSupport = new bool[mK];
	
	int i, j, m, currentColumn;

	mSupportColumns = mCk->getCount();

	// The support table will have the number of rows = the number of transactions and
	// the number of columns = the number of candidate itemsets (Ck.getCount()).

	for (i = 0; i < mNumTransactions; i++)
	{
		mSupportTable[i] = new int[mSupportColumns];
	}

	for (currentColumn = 0; currentColumn < mSupportColumns; currentColumn++) //This loop controls where we are in Ck, e.g what itemset is being tested
	{
		for (i = 0; i < mNumTransactions; i++)							   //This loop controls what transaction we are on
		{
			for (m = 0; m < mK; m++)										   //This loop controls where we are in each itemset
			{
				for (j = 0; j < mTransactionLengths[i]; j++)				   //This loop controls where we are in each transaction
				{
					//P.S. I overloaded the [] operator for the stack, used here
					if (mDataset[i][j] == ((*mCk)[currentColumn]).mItems[m])
					{
						//supportTable[i][currentColumn] = 1;
						perItemSupport[m] = true;
						break;
					}
					else
					{
						//supportTable[i][currentColumn] = 0;
						perItemSupport[m] = false;
					}
				}

				if (getIndividualItemsetSupport(perItemSupport))
				{
					mSupportTable[i][currentColumn] = 1;
				}
				else
				{
					mSupportTable[i][currentColumn] = 0;
				}
			}
		}
	}

	//UNCOMMENT THIS STUFF BELOW TO TEST THIS FUNCTION
	/*-------------------------------------------*/

	//Print sets
	/*cout << "  ";
	for(int i = 0; i < totalColumns/10; i++)
	{
		cout << "{";
		Ck[i].printItemsetToConsole();
		cout << "}  ";
	}
	cout << endl;*/

	//print support table?
	//print support table!
	/*for (i = 0; i < numTransactions/50; i++)
	{
		cout << i << " ";
		for (j = 0; j < totalColumns/10; j++)
		{
			cout << supportTable[i][j] << "        ";
		}
		cout << endl;
	}*/

	calculateIndividualSupport();

	perItemSupport = NULL;
	delete[] perItemSupport;
}

bool Apriori::getIndividualItemsetSupport(bool *perItemSupport)
{
	bool allTrue = true;

	for (int i = 0; i < mK; i++)
	{
		if (!perItemSupport[i])
		{
			allTrue = false;
			break;
		}
	}

	return allTrue;
}

int convertStringToInt(string str)
{
	int num = 0;
	stringstream ss(str);

	ss >> num;

	return num;
}

double convertStringToDouble(string str)
{
	double num = 0.0;
	stringstream ss(str);

	ss >> num;

	return num;
}

int getNumberCount(string str)
{
	string word;
	int count = 0;

	stringstream ss(str);

	while (ss >> word)
	{
		count++;
	}

	return count;
}

void putNumbersInArray(string str, int *arr, int size)
{
	int number = 0;

	stringstream ss(str);

	for (int i = 0; i < size; i++)
	{
		ss >> number;
		arr[i] = number;
	}

}

//SOURCE: http://blog.mrroa.com/2013/06/06/tiptrick-how-to-retrieve-a-sub-string-between-two-delimiters-using-c/
string extractString(string source, string start, string end)
{
	int endIndex;
	int startIndex = source.find(start);

	//If the starting part isn't found, then there cannot be a substring to extract
	if (startIndex == string::npos)
		return "";

	//Ading the length of th "start" string moves us to the beginning of the sub-string
	startIndex += start.length();

	//Find the end of the substring to extract
	endIndex = source.find(end, startIndex);

	// Return the substring between startIndex and endIndex. An empty string is returned
	// if the endIndex is invalid.
	return source.substr(startIndex, endIndex - startIndex);

	return "";
}

void Apriori::join()
{
	mCk->clear();
	mK++;
	int i, j, m, commonItemsCount = 0;
	int totalItemsLk = mLk->getCount();
	int* commonItems;

	for (i = 0; i < totalItemsLk; i++)
	{
		for (j = i + 1; j < totalItemsLk; j++)
		{
			Itemset newSet(mK);

			//Determine (k - 2) commmon items in mLk[i] and mLk[j], put into array.
			if ((mK - 2) > 0)
			{
				commonItems = new int[mK - 2];

				for (m = 0; m < mK - 2; m++)
				{
					if ((*mLk)[i].mItems[m] == (*mLk)[j].mItems[m])
					{
						commonItems[m] = (*mLk)[i].mItems[m];
						commonItemsCount++;
					}
				}

				//copy common items array into newSet ONLY if there are the correct amount of common items
				if(commonItemsCount == mK - 2)
					copyCommonItems(commonItems, newSet);
			}
			
			// copy last item mLk[i].mItems[k-2] into last slot of newSet (this is always performed)
			newSet.mItems[mK - 1] = (*mLk)[i].mItems[mK - 2];
			newSet.incrementCurrentNumItems();
			newSet.mItems[mK - 2] = (*mLk)[j].mItems[mK - 2];
			newSet.incrementCurrentNumItems();

			/*
			If the new set is the correct length mK, add it to mCk.

			This handles the case where, for some reason, mK - 2 is greater than 0 (i.e creating the size 3 or bigger sets),
			and we somehow couldn't find any common items, or not the right number of common items. Then, commonItems would
			either have less then mK - 2 items, or no items at all. We don't copy the common items when there aren't the right amount of them, so newSet would
			only have the very last item copied over, and would not be he right size.

			The 2-itemsets are unaffected since mk - 2 is 0.
			*/
			if(newSet.getCurrentNumItems() == mK)
				mCk->push(newSet);

			commonItems = NULL;
			commonItemsCount = 0;
		}
	}
}

void Apriori::copyCommonItems(int* common, Itemset &newSet)
{
	for (int i = 0; i < mK - 2; i++)
	{
		newSet.mItems[i] = common[i];
		newSet.incrementCurrentNumItems();
	}
}

void Apriori::calculateSupport()
{
	calculateSupportTable();
}

void Apriori::clearOldSupportData()
{
	int i;

	for (i = 0; i < mSupportColumns; i++)
	{
		mSupportTable[i] = NULL;
		delete[] mSupportTable[i];
	}
	mSupportTable = NULL;
	delete[] mSupportTable;

	mIndividualItemsetSupport = NULL;
	delete[] mIndividualItemsetSupport;
}

void Apriori::apriori_gen()
{
	join();
}

void Apriori::apriori_run()
{
	ofstream output;
	output.open("apriori_output.txt");

	cout << "Dataset loaded!" << endl;

	generateFirstItemsets();
	output << "L1: " << endl;
	mLk->displayToFile(output);
	output << endl << endl;

	while (mLk->getCount() >= 1)
	{
		apriori_gen();
		prune();

		if (mLk->getCount() != 0)
		{
			output << "L" << mK << ": " << endl;
			mLk->displayToFile(output);
			output << endl << endl;
		}
	}

	output.close();
}