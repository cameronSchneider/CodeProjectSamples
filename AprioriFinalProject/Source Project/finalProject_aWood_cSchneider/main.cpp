#include "itemset.h"
#include "stack.h"
#include "apriori.h"

int main()
{
	string fileName = "T5.N0.1K.D1K.txt";
	string fileName2 = "T5.N0.01K.D0.01K.txt"; //enter 0.001 for the support with this file (anything from 0.001 to 0.009)
	double minSupport;

	cout << "Enter a minimum support percentage: ";
	cin >> minSupport;

	while (minSupport <= 0.0 || minSupport >= 100.0)
	{
		cout << "\tPlease enter a valid percentage greater than 0 but less than 100: ";
		cin >> minSupport;
	}

	Apriori test(fileName2, minSupport);

	//Itemset newSet1(2), newSet2(2), newSet3(2);S

	test.apriori_run();
	
	/*cout << "Dataset loaded! Press any button to run apriori: " << endl;
	system("pause");

	test.generateFirstItemsets();

	cout << "mLk 1: " << endl << endl << endl;
	
	test.mLk->display();

	test.join();

	cout << "mLk 2: " << endl << endl << endl;

	test.prune();
	test.mLk->display();

	test.join();

	cout << "mCk 3: " << endl << endl << endl;

	test.mCk->display();

	cout << "mLk 3: " << endl << endl << endl;

	test.prune();
	test.mLk->display();*/

	system("pause");
	return 0;
}