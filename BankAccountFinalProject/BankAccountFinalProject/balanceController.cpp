#include "header.h"
#include <iomanip>


	//Helper functions
/*------------------------------------------------------------------------------------------------*/
double getNumbers(string &acctNum, bool &userStop);								//Used by checkBalance
double getNumbers(string &acctNum, string &acctBalanceFile, bool &userStop);	//Overload for extra functionality
/*------------------------------------------------------------------------------------------------*/

ifstream balanceIn;

const char *ORIGINAL_BALANCE_FILE;	//Used by anything that removes/renames files

/*
	Pre: called from main()
Purpose: displays the balance of given account
   Post: the account's balance is displayed
*/
void checkBalance()
{
	system("cls");

	string acctNum;

	bool userStop = false;	//used in case 'verlassen' is given to stop

	double accountBalanceDouble;

	//getNumbers() returns a double, but also modifies acctNum and userStop.
	accountBalanceDouble = getNumbers(acctNum, userStop);

	// If the user hasn't stopped toe process, display the info
	if (!userStop)
		cout << "The balance for account " <<  acctNum << " is: $" << fixed << setprecision(2) << accountBalanceDouble << endl;
}


/*
	Pre: called from main()
Purpose: allows the teller to deposit money into an account
   Post: the account's .dat balance file is updated, and the new info is displayed
*/
void deposit()
{
	system("cls");

	//Temporary output file
	ofstream tempResult(TEMP_FILE);
	
	string acctNum;
	string balanceFile;
	
	bool userStop = false;	//used in case 'verlassen' is given to stop

	double accountBalanceDouble;
	double amountToDeposit;

	//This calls an overloaded getNumbers() function
	accountBalanceDouble = getNumbers(acctNum, balanceFile, userStop);


	// If the user hasn't stopped toe process, display the info
	if (!userStop)
	{
		cout << "\nEnter the amount to deposit: $";
		cin >> amountToDeposit;

		//The const must be assigned to a c_string for it to work
		ORIGINAL_BALANCE_FILE = balanceFile.c_str();

		accountBalanceDouble += amountToDeposit;

		tempResult << accountBalanceDouble;

		//Close, remove, rename the files to overwrite it.
		balanceIn.close();
		tempResult.close();

		remove(ORIGINAL_BALANCE_FILE);
		rename(TEMP_FILE, ORIGINAL_BALANCE_FILE);

		//Display the info
		cout << "\nThe new balance of account " << acctNum << " is $" << fixed << setprecision(2) << accountBalanceDouble << endl;
	}
}


/*
	Pre: called from main()
Purpose: allows the teller to withdraw money from an account
   Post: the account's .dat balance file is updated, and the new info is displayed
*/
void withdraw()
{
	system("cls");

	//Temporary output file
	ofstream tempResult(TEMP_FILE);

	string acctNum;
	string balanceFile;

	bool userStop = false;	//used in case 'verlassen' is given to stop

	double accountBalanceDouble;
	double amountToWithdraw;

	//This calls an overloaded getNumbers() function
	accountBalanceDouble = getNumbers(acctNum, balanceFile, userStop);

	// If the user hasn't stopped toe process, display the info
	if (!userStop)
	{
		cout << "\nEnter the amount to withdraw: $";
		cin >> amountToWithdraw;

		//The const must be assigned to a c_string for it to work
		ORIGINAL_BALANCE_FILE = balanceFile.c_str();

		accountBalanceDouble -= amountToWithdraw;

		tempResult << accountBalanceDouble;

		//Close, remove, rename the files to overwrite it.
		balanceIn.close();
		tempResult.close();

		remove(ORIGINAL_BALANCE_FILE);
		rename(TEMP_FILE, ORIGINAL_BALANCE_FILE);

		//Display the info
		cout << "\nThe new balance of account " << acctNum << " is $" << fixed << setprecision(2) << accountBalanceDouble << endl;
	}
}


/*
	Pre: acctNum and userStop are declared in the function that calls getNumbers()
Purpose: retrieves the account number from the user, determines if the user used 'verlassen' to stop, opens the
		 correct balance file, and returns the number in the balance file.
   Post: a double is returned, while acctNum and userSTop are modified
*/
double getNumbers(string &acctNum, bool &userStop)
{
	string accountBalanceString;
	double accountBalanceDouble;

	//The following loop verifies that the account balance file exists
	// and that the user hasn't stopped the process with 'verlassen'
	do
	{
		cout << "Enter the account number ('verlassen' to stop): ";
		cin >> acctNum;

		if (acctNum == "verlassen")
		{
			//the calling function needs to know this value in order to execute a decision
			userStop = true;
			break;
		}

		//append .dat to the given account number to create the file name
		string balanceFile = ".\\data\\" + acctNum + ".dat";

		balanceIn.open(balanceFile);

		//If the balance file isn't open, display an error, else return the double in the file
		if (balanceIn.fail())
		{
			cout << "\n\tAccount balance file not found!" << endl << endl;
		}
		else
		{
			balanceIn >> accountBalanceString;

			accountBalanceDouble = convertStr2Double(accountBalanceString);

			balanceIn.close();

			return accountBalanceDouble;
		}

	} while (balanceIn.fail());
	
	//This return is only used when the user uses verlassen. It is never put into a file.
	return 0.0;
}


/*
	Pre: acctNum and userStop are declared in the function that calls getNumbers()
Purpose: retrieves the account number from the user, determines if the user used 'verlassen' to stop, opens the
		 correct balance file, and returns the number in the balance file.
   Post: a double is returned, while acctNum, acctBalanceFile, and userSTop are modified
*/
double getNumbers(string &acctNum, string &acctBalanceFile, bool &userStop)
{
	string accountBalanceString;
	double accountBalanceDouble;

	//The following loop verifies that the account balance file exists
	// and that the user hasn't stopped the process with 'verlassen'
	do
	{
		cout << "Enter the account number ('verlassen' to stop): ";
		cin >> acctNum;

		if (acctNum == "verlassen")
		{
			//the calling function needs to know this value in order to execute a decision
			userStop = true;
			break;
		}

		/*
		append .dat to the given account number to create the file name
		This is assigned to the reference variable because the calling function
		needs to know what the file name is (this is why the overload exists).
		*/
		acctBalanceFile = ".\\data\\" + acctNum + ".dat";

		balanceIn.open(acctBalanceFile);

		//If the balance file isn't open, display an error, else return the double in the file
		if (balanceIn.fail())
		{
			cout << "\n\tAccount balance file not found!" << endl << endl;
		}
		else
		{
			balanceIn >> accountBalanceString;

			accountBalanceDouble = convertStr2Double(accountBalanceString);

			balanceIn.close();

			return accountBalanceDouble;
		}

	} while (balanceIn.fail());

	//This return is only used when the user uses verlassen. It is never put into a file.
	return 0.0;
}