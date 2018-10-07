#include "header.h"
#include <vector>

/*
Author : Cameron Schneider
Project: Bank Account Final Project
Description: Controls all account-related functions as follows:
			-Adding accounts
			-Deleting accounts
			-Updating account info
			-Searching for accounts
			-Adding/removing balance files

Certification: located in main.cpp file

   References: fstream :: app found here-- http://www.cplusplus.com/reference/fstream/fstream/open/
*/

	//Helper functions used to search for accounts
/*------------------------------------------------------------------------------------------------*/
bool findAccountByNumber(string accountNumber);
bool findAccountBySSN(string accountSSN);
bool findAccountByName(string accountName);
bool findAccountByAddress(string accountAddress);
bool findAccountByPhoneNumber(string accountPhoneNumber);
/*------------------------------------------------------------------------------------------------*/

	//Helper functions used in adding new accounts to ensure no other accounts share the same
//		properties.
/*------------------------------------------------------------------------------------------------*/
bool checkForExistingAccountNumber(string accountNumber);
bool checkForExistingSSN(string ssn);
bool checkForExistingName(string name);
bool checkForExistingAddress(string address);
bool checkForExistingPhoneNumber(string phoneNum);
/*------------------------------------------------------------------------------------------------*/


	//Helper functions used in deleting accounts from the accounts.dat file
/*------------------------------------------------------------------------------------------------*/
void removeAccountFromFile(string accountNumber);
void eraseFileLine(string eraseLines[]);
/*------------------------------------------------------------------------------------------------*/


	//Helper functions used in updating account information in accounts.dat
/*------------------------------------------------------------------------------------------------*/
void makeUpdateDecision(string infoArray[]);
void updateSSN(string ssnToUpdate);
void updateName(string nameToUpdate);
void updateAddress(string addressToUpdate);
void updatePhoneNumber(string numToUpdate);
/*------------------------------------------------------------------------------------------------*/

	//Helper functions used to verify and ensure the formatting of SSN's, phone numbers,
//		and account numbers.
/*------------------------------------------------------------------------------------------------*/
void verifySSNFormat(string &ssn);
void verifyPhoneNumberFormat(string &phoneNum);
void verifyAccountNumberLength(string &accountNum);
/*------------------------------------------------------------------------------------------------*/


	//Helper functions used to fill or resize parallel vectors containing account info
/*------------------------------------------------------------------------------------------------*/
void populateAccountNums();
void populateAccountSSNs();
void populateAccountNames();
void populateAccountAddresses();
void populateAccountPhoneNumbers();

void populateAllVectors();
void resizeAllVectors();
/*------------------------------------------------------------------------------------------------*/


	//Helper function to close streams and remove/rename files in one step
/*------------------------------------------------------------------------------------------------*/
void closeAndRemove(ifstream &in, ofstream &out);
/*------------------------------------------------------------------------------------------------*/


	//Helper functions to make sure balance files are not overwritten, and to remove them when
	//accounts are deleted.
/*------------------------------------------------------------------------------------------------*/
bool checkIfBalanceFileExists(string acctNum);
void removeBalanceFile(string acctNum);
/*------------------------------------------------------------------------------------------------*/


	//A set of parallel vectors that contain the information of accounts
/*------------------------------------------------------------------------------------------------*/
vector<string> accountNums;
vector<string> accountSSNs;
vector<string> accountNames;
vector<string> accountAddresses;
vector<string> accountPhoneNumbers;
/*------------------------------------------------------------------------------------------------*/


	//The fstream objects for file manipulation
/*------------------------------------------------------------------------------------------------*/
ifstream accountsIn;
ofstream accountsOut;
/*------------------------------------------------------------------------------------------------*/


	//Constants related to requirements and standard file names
/*------------------------------------------------------------------------------------------------*/
const int MULTIPLE_Of_FIVE_DIVISOR = 5;		//Used in the populate<vector_name>() function algorithms
const int INFO_ARRAY_SIZE = 5;				//How many different info types there are (i.e number, SSN, Name, Address, Phone)

//The positions of account information in information arrays, and the
// line they occur on in accounts.dat
const int ACCT_NUMBER_POSITION   = 0;
const int ACCT_SSN_POSITION      = 1;
const int ACCT_NAME_POSITION     = 2;
const int ACCT_ADDRESS_POSITION  = 3;
const int ACCT_PHONENUM_POSITION = 4;


const int REQUIRED_PHONE_NUMBER_SIZE = 14;
const int REQUIRED_ACCOUNT_NUM_LENGTH = 5;
const char *CLIENT_ACCOUNTS_FILE = ".\\data\\accounts.dat";
const double INITIAL_BALANCE = 100.00;



/*------------------------------------------------------------------------------------------------*/
/*
	   Pre : called from the main() function in main.cpp, used to add new client accounts
	Purpose: adds a new account by asking the teller to input all-new information, verifies the info,
			 and adds a new dalance data file.
	   Post: a new account has been added to the accounts.dat file
*/
void addAccount()
{
	system("cls");
	
	string accountNumber;
	string ssn;
	string name;
	string address;
	string phoneNumber;

	// fill all the account data vectors with the current information
	populateAllVectors();

	// opens accounts.dat in append mode to append the added data to the file
	accountsOut.open(CLIENT_ACCOUNTS_FILE, fstream::app);

	if (accountsOut.fail())
	{
		cout << "Error: File not file not found: \data\accounts.dat" << endl;
	}
	else
	{
		/*------------------------------------------------------------------------------------------------*/
		cout << "Enter an Account Number: ";
		cin >> accountNumber;

		//The following statements verify that the number is 5 digits, and doesn't already exist
		verifyAccountNumberLength(accountNumber);

		while (checkForExistingAccountNumber(accountNumber))
		{
			cout << "An account with this number already exists! Enter a new number: ";
			cin >> accountNumber;
			verifyAccountNumberLength(accountNumber);
		}
		/*------------------------------------------------------------------------------------------------*/
		cout << "Enter a Social Security Number: ";
		cin.ignore();
		getline(cin, ssn);

		//The following statements verify that the SSN is in correct format, and doesn't already exist
		verifySSNFormat(ssn);

		while (checkForExistingSSN(ssn))
		{
			cout << "An account with this SSN already exists! Enter a new SSN: ";
			getline(cin, ssn);
			verifySSNFormat(ssn);
		}
		/*------------------------------------------------------------------------------------------------*/
		cout << "Enter a Name: ";
		getline(cin, name);

		//The following statements verify that the name doesn't already exist

		while (checkForExistingName(name))
		{
			cout << "An account with this name already exists! Enter a new name: ";
			getline(cin, name);
		}
		/*------------------------------------------------------------------------------------------------*/
		cout << "Enter an Address: ";
		getline(cin, address);

		//The following statements verify that the address doesn't already exist

		while (checkForExistingAddress(address))
		{
			cout << "An account with this address already exists! Enter a new address: ";
			getline(cin, address);
		}
		/*------------------------------------------------------------------------------------------------*/
		cout << "Enter a Phone Number: ";
		getline(cin, phoneNumber);

		//The following statements verify that the phone number is in correct format, and doesn't already exist
		verifyPhoneNumberFormat(phoneNumber);

		while (checkForExistingPhoneNumber(phoneNumber))
		{
			cout << "An account with this phone number already exists! Enter a new phone number: ";
			getline(cin, phoneNumber);
			verifyPhoneNumberFormat(phoneNumber);
		}

		// Append all user-given data into the file (it's already been verified)
		accountsOut << accountNumber << endl;
		accountsOut << ssn << endl;
		accountsOut << name << endl;
		accountsOut << address << endl;
		accountsOut << phoneNumber << endl;

		cout << "\nSuccessfully added account!" << endl;
	}
	//Close the file to save it, and resize all vectors to delete the information in them
	accountsOut.close();
	resizeAllVectors();

	createBalanceFiles();
}


/*
	Pre: called from main()
Purpose: deletes an account as specified by the teller
   Post: the specified account is removed from accounts.dat, and its balance file is deleted
*/
void deleteAccount()
{
	system("cls");
	string userNumberToDelete;
	string chooseToDelete;
	bool isValidResponse = false;

	cout << "Enter The account number to delete: ";
	cin >> userNumberToDelete;
	
	verifyAccountNumberLength(userNumberToDelete);

	if (!findAccountByNumber(userNumberToDelete))
		cout << "NO MATCHING ACCOUNT FOUND" << endl;
	else
	{
		cout << "\nDelete this account? (Y or N): ";
		cin >> chooseToDelete;

		chooseToDelete = toUpper(chooseToDelete);

		while (!isValidResponse)
		{
			if (chooseToDelete == "Y")
			{
				removeAccountFromFile(userNumberToDelete);
				removeBalanceFile(userNumberToDelete);
				cout << "Account deleted!" << endl;
				isValidResponse = true;
			}
			else if(chooseToDelete == "N")
			{
				isValidResponse = true;
				cout << "Returning to main menu..." << endl;
			}
			else
			{
				cout << "Invalid choice, please enter Y or N: ";
				cin >> chooseToDelete;
				chooseToDelete = toUpper(chooseToDelete);
			}
		}
		
	}
}


/*
	Pre: called from main()
Purpose: updates the specified type of account information on a specified account.
		 Allows any piece of info to be updated, excluding the account number, as that is
		 to essential to update.
   Post: the information of the account has been updated
*/
void updateAccount()
{
	system("cls");

	string accountToUpdate;
	string chooseToUpdate;
	string accountInfo[INFO_ARRAY_SIZE];
	bool isValidResponse = false;

	cout << "Enter the number off the account to update: ";
	cin >> accountToUpdate;

	verifyAccountNumberLength(accountToUpdate);

	if (!findAccountByNumber(accountToUpdate))
		cout << "NO MATCHING ACCOUNT FOUND" << endl;
	else
	{
		
		cout << "\n Update this account? (Y or N): ";
		cin >> chooseToUpdate;

		chooseToUpdate = toUpper(chooseToUpdate);

		while (!isValidResponse)
		{
			if (chooseToUpdate == "Y")
			{
				displayAccountUpdateMenu();
				populateAllVectors();
				string infoArray[5];

				for (int i = 0; i < accountNums.size(); i++)
				{
					if (accountNums[i] == accountToUpdate)
					{
						infoArray[ACCT_NUMBER_POSITION] = accountNums[i];
						infoArray[ACCT_SSN_POSITION] = accountSSNs[i];
						infoArray[ACCT_NAME_POSITION] = accountNames[i];
						infoArray[ACCT_ADDRESS_POSITION] = accountAddresses[i];
						infoArray[ACCT_PHONENUM_POSITION] = accountPhoneNumbers[i];
						break;
					}
				}

				makeUpdateDecision(infoArray);

				cout << "Account updated!" << endl;
				isValidResponse = true;
			}
			else if (chooseToUpdate == "N")
			{
				isValidResponse = true;
				cout << "Returning to main menu..." << endl;
			}
			else
			{
				cout << "Invalid choice, please enter Y or N: ";
				cin >> chooseToUpdate;
				chooseToUpdate = toUpper(chooseToUpdate);
			}
		}
	}
}

/*
	Pre: called from main() in main.cpp
Purpose: utilizes a user's menu choice by calling displayAccountSearchMenu() in menuController.cpp,
		 then performs the corresponding search.
   Post: if the account was found, the account is displayed; otherwise, display an error.
*/
void searchAccounts()
{
	string userSearchMenuChoice = "0";

	string accountNumberToFind;
	string ssnToFind;
	string nameToFind;
	string addressToFind;
	string phoneNumberToFind;

	while (userSearchMenuChoice != "6")
	{
		system("cls");

		displayAccountSearchMenu();
		userSearchMenuChoice = getUserChoice();

		switch (convertStr2Int(userSearchMenuChoice))
		{

	//Search by account number
/*------------------------------------------------------------------------------------------------*/
		case 1:
			system("cls");

			cin.ignore();
			cout << "Enter the Account Number to find: ";
			getline(cin, accountNumberToFind);

			//Make sure the teller is searching for a 5-digit number
			verifyAccountNumberLength(accountNumberToFind);

			if (!findAccountByNumber(accountNumberToFind))
				cout << "NO MATCH FOUND" << endl;

			system("pause");
			break;

	//Search by SSN
/*------------------------------------------------------------------------------------------------*/
		case 2:
			system("cls");

			cin.ignore();
			cout << "Enter the Account's SSN to find: ";
			getline(cin, ssnToFind);
			
			//Make sure teller is searching using correct SSN format
			verifySSNFormat(ssnToFind);

			if (!findAccountBySSN(ssnToFind))
				cout << "NO MATCH FOUND" << endl;

			system("pause");
			break;

	//Search by name
/*------------------------------------------------------------------------------------------------*/
		case 3:
			system("cls");

			cin.ignore();
			cout << "Enter the Account Name to find (full name): ";
			getline(cin, nameToFind);

			if (!findAccountByName(nameToFind))
				cout << "NO MATCH FOUND" << endl;

			system("pause");
			break;

	//Search by Address
/*------------------------------------------------------------------------------------------------*/
		case 4:
			system("cls");

			cin.ignore();
			cout << "Enter the Account Address to find (full address): ";
			getline(cin, addressToFind);

			if (!findAccountByAddress(addressToFind))
				cout << "NO MATCH FOUND" << endl;
			system("pause");
			break;

	//Serach by phone number
/*------------------------------------------------------------------------------------------------*/
		case 5:
			system("cls");

			cin.ignore();
			cout << "Enter the Account Phone Number to find: ";
			getline(cin, phoneNumberToFind);

			//Make sure teller is searching using correct phone number format
			verifyPhoneNumberFormat(phoneNumberToFind);

			if (!findAccountByPhoneNumber(phoneNumberToFind))
				cout << "NO MATCH FOUND" << endl;
			system("pause");
			break;
		}
	}
}
/*------------------------------------------------------------------------------------------------*/


	/*
		Pre: Each string parameter is the correct type of info
	Purpose: The following block of functions all assist in searching for accounts.
			 Each function is responsible for searching for an account by the information
			 specified in the function name.
	   Post: a boolean value is returned indicating if the account was found or not
	*/
/*------------------------------------------------------------------------------------------------*/
bool findAccountByNumber(string accountNumberQuery)
{
	populateAllVectors();

	for (int i = 0; i < accountNums.size(); i++)
	{
		if (accountNums[i] == accountNumberQuery)
		{
			system("cls");

			cout << accountNums[i] << endl;
			cout << accountSSNs[i] << endl;
			cout << accountNames[i] << endl;
			cout << accountAddresses[i] << endl;
			cout << accountPhoneNumbers[i] << endl;

			resizeAllVectors();

			return true;
		}
	}

	resizeAllVectors();

	return false;
}

bool findAccountBySSN(string accountSSNQuery)
{
	populateAllVectors();

	for (int i = 0; i < accountSSNs.size(); i++)
	{
		if (accountSSNs[i] == accountSSNQuery)
		{
			system("cls");

			cout << accountNums[i] << endl;
			cout << accountSSNs[i] << endl;
			cout << accountNames[i] << endl;
			cout << accountAddresses[i] << endl;
			cout << accountPhoneNumbers[i] << endl;

			resizeAllVectors();

			return true;
		}
	}

	resizeAllVectors();

	return false;
}

bool findAccountByName(string accountNameQuery)
{
	populateAllVectors();

	accountNameQuery = toUpper(accountNameQuery);

	for (int i = 0; i < accountNames.size(); i++)
	{
		string name = accountNames[i];
		name = toUpper(accountNames[i]);

		if (name == accountNameQuery)
		{
			system("cls");

			cout << accountNums[i] << endl;
			cout << accountSSNs[i] << endl;
			cout << accountNames[i] << endl;
			cout << accountAddresses[i] << endl;
			cout << accountPhoneNumbers[i] << endl;

			resizeAllVectors();

			return true;
		}
	}

	resizeAllVectors();

	return false;
}

bool findAccountByAddress(string accountAddressQuery)
{
	populateAllVectors();

	accountAddressQuery = toUpper(accountAddressQuery);

	for (int i = 0; i < accountAddresses.size(); i++)
	{
		string address = accountAddresses[i];
		address = toUpper(accountAddresses[i]);

		if (address == accountAddressQuery)
		{
			system("cls");

			cout << accountNums[i] << endl;
			cout << accountSSNs[i] << endl;
			cout << accountNames[i] << endl;
			cout << accountAddresses[i] << endl;
			cout << accountPhoneNumbers[i] << endl;

			resizeAllVectors();

			return true;
		}
	}

	resizeAllVectors();

	return false;
}

bool findAccountByPhoneNumber(string accountPhoneNumberQuery)
{
	populateAllVectors();

	for (int i = 0; i < accountPhoneNumbers.size(); i++)
	{
		if (accountPhoneNumbers[i] == accountPhoneNumberQuery)
		{
			system("cls");

			cout << accountNums[i] << endl;
			cout << accountSSNs[i] << endl;
			cout << accountNames[i] << endl;
			cout << accountAddresses[i] << endl;
			cout << accountPhoneNumbers[i] << endl;

			resizeAllVectors();

			return true;
		}
	}
	resizeAllVectors();

	return false;
}
/*------------------------------------------------------------------------------------------------*/


	/*
		Pre: the parameters have been declared in the calling function
	Purpose: The following block of functions ensure that SSNs, phone numbers, and
			 account numbers are in the correct format, as well as correcting
			 any pre-existing phone numbers that are incorrect.
	   Post: the specified piece of info is input in correct format, and all
			 pre-existing phone numbers are in correct format.
	*/
/*------------------------------------------------------------------------------------------------*/
void verifySSNFormat(string &ssn)
{
	const int REQUIRED_SSN_LENGTH = 11;
	const char REQUIRED_SSN_DASHES = '-';

	//The integer indices in the following boolean represent the position of the required element in
	// the correct SSN format
	bool isWrongFormat = ssn.size() != REQUIRED_SSN_LENGTH || ssn[3] != REQUIRED_SSN_DASHES || ssn[6] != REQUIRED_SSN_DASHES;

	while (isWrongFormat)
	{
		cout << "Invalid SSN format, please enter in xxx-xx-xxxx format: ";
		getline(cin, ssn);
	}
		
}

void verifyPhoneNumberFormat(string &phoneNum)
{
	const char REQUIRED_AREA_CODE_PARENTHESE_LEFT = '(';
	const char REQUIRED_AREA_CODE_PARENTHESE_RIGHT = ')';
	const char REQUIRED_SPACE = ' ';
	const char REQUIRED_DASH = '-';

	//The integer indices in the following boolean represent the position of the required element in the correct phone
	// number format
	bool isWrongFormat = phoneNum.size() != REQUIRED_PHONE_NUMBER_SIZE || phoneNum[0] != REQUIRED_AREA_CODE_PARENTHESE_LEFT || phoneNum[4] != REQUIRED_AREA_CODE_PARENTHESE_RIGHT
						|| phoneNum[5] != REQUIRED_SPACE || phoneNum[9] != REQUIRED_DASH;

	while (isWrongFormat)
	{
		cout << "Invalid Phone Number format, please enter in  (xxx) xxx-xxxx  format: ";
		getline(cin, phoneNum);
	}


}

void verifyAccountNumberLength(string &accountNum)
{
	while (accountNum.length() != REQUIRED_ACCOUNT_NUM_LENGTH)
	{
		cout << "Please enter a 5-number digit: ";
		cin >> accountNum;
	}
}

void correctPreExistingPhoneNumberFormats()
{
	populateAccountPhoneNumbers();

	string individualNumbers[10];	//10 is the length of each phone number prior to being corrected

	for (int i = 0; i < accountPhoneNumbers.size(); i++)
	{
		string currentPhoneNum = accountPhoneNumbers[i];


		//the integer indices in the following boolean represent the position of the required element in
		// the correct phone number format

		//BUG FIX: Had to "repeat" the formatting algorithm in this function to avoid any printing to screen.
			// This function happens behind the scenes.
		bool isCorrectFormat = currentPhoneNum.length() == REQUIRED_PHONE_NUMBER_SIZE && currentPhoneNum[0] == '(' && currentPhoneNum[4] == ')' &&
							   currentPhoneNum[5] == ' ' && currentPhoneNum[9] == '-';

		if (!isCorrectFormat)
		{
			for (int j = 0, k = 0; j < currentPhoneNum.length(); j++)
			{
				if (currentPhoneNum[j] != ' ')
				{
					individualNumbers[k] = currentPhoneNum[j];
					k++;
				}
			}

			//The integer indices in the following assignment are equivalent to the position of each number from the incorrect format
			//	Those numbers are assigned to the correct places now.
			currentPhoneNum = "(" + individualNumbers[0] + individualNumbers[1] + individualNumbers[2] + ")" +
							  " " + individualNumbers[3] + individualNumbers[4] + individualNumbers[5] + "-" +
									individualNumbers[6] + individualNumbers[7] + individualNumbers[8] + individualNumbers[9];
		}
		accountPhoneNumbers[i] = currentPhoneNum;
	}

	writeCorrectedPhoneNumsToFile();
}

void writeCorrectedPhoneNumsToFile()
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: writeCorrectedPhoneNumsToFile(); accountsController.cpp; line 727" << endl;
	}
	else
	{
		ofstream tempResult(TEMP_FILE);

		string inBuffer;	//The current line

		int i = 0;	 //The line position in accounts.dat
		int n = 0;	 //The index of the vector
		while (!accountsIn.eof())
		{
			getline(accountsIn, inBuffer);

			//This decision algorithm makes sure the correct lines are replaced
			if (i % MULTIPLE_Of_FIVE_DIVISOR == ACCT_PHONENUM_POSITION)
			{
				inBuffer = accountPhoneNumbers[n];
				n++;
			}

			//This decision avoids adding extra newlines at the end of the file
			// and rewrites all accounts.dat data plus the new data to a new file
			if (!accountsIn.eof())
				tempResult << inBuffer << endl;
			else
				tempResult << inBuffer;

			i++;
		}

		//The following deletes the old accounts.dat file and creates a new one with the
		// updated data.
		closeAndRemove(accountsIn, tempResult);
	}
}
/*------------------------------------------------------------------------------------------------*/


	/*
		Pre: each string parameter is the correct type of info
	Purpose: The following block of functions all make sure that there is not an 
			 account that already exists when adding new accounts. They ensure that
			 accounts are not added with the same info as another.
	   Post: a boolean value is returned indicating if an account exists with the
			 specified info, or not.
	*/
/*------------------------------------------------------------------------------------------------*/
bool checkForExistingAccountNumber(string accountNumber)
{
	for (string number : accountNums)
	{
		if (accountNumber == number)
			return true;
	}
	return false;
}

bool checkForExistingSSN(string accountSSN)
{
	for (string ssn : accountSSNs)
	{
		if (accountSSN == ssn)
			return true;
	}
	return false;
}

bool checkForExistingName(string accountName)
{
	for (string name : accountNames)
	{
		if (accountName == name)
			return true;
	}
	return false;
}

bool checkForExistingAddress(string accountAddress)
{
	for (string address : accountAddresses)
	{
		if (accountAddress == address)
			return true;
	}
	return false;
}

bool checkForExistingPhoneNumber(string phoneNum)
{
	for (string num : accountPhoneNumbers)
	{
		if (phoneNum == num)
			return true;
	}
	return false;
}
/*------------------------------------------------------------------------------------------------*/


	/*
		Pre: all vectors are empty
	Purpose: The following block of functions all populate their respective vectors,
			 as specified in the function name. The last 2 functions deal with all vectors
			 as a whole.
	   Post: the specified vector(s) is populated with the respective data
	*/
/*------------------------------------------------------------------------------------------------*/
void populateAccountNums()
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: populateAccountNums(); accountsController.cpp; line 841" << endl;
	}
	else
	{
		string fileAccountNumber;

		/*
		The following algorithm finds each account number and puts it into a vector for later use.
		It does so by searching through the accounts.dat file. Each account number is located on
		a line number that is a multiple of 5, starting from 0; therefore, it takes each line that is a
		multiple of 5 and puts it into the vector.
		*/

		int i = 0;   //The line position in accounts.dat
		int n = 0;	 //The index of the vector
		while (!accountsIn.eof())
		{
			getline(accountsIn, fileAccountNumber);

			if (i % MULTIPLE_Of_FIVE_DIVISOR == ACCT_NUMBER_POSITION)
			{
				accountNums.push_back("");
				accountNums[n] = fileAccountNumber;
				n++;
			}

			i++;
		}
	}
	accountsIn.close();
}

void populateAccountSSNs()
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: populateAccountSSNs(); accountsController.cpp; line 879" << endl;
	}
	else
	{

		string fileAccountSSN;

		/*
		The following algorithm finds each account SSN and puts it into a vector for later use.
		It does so by searching through the accounts.dat file. Each account SSN is located on
		a line number that has a remainder of 1 when divided by 5, starting from 0; therefore, it takes each
		line number that has a remainder of 1 and puts it into the vector.
		*/

		int i = 0;	 //The line position in accounts.dat
		int n = 0;	 //The index of the vector
		while (!accountsIn.eof())
		{
			getline(accountsIn, fileAccountSSN);

			if (i % MULTIPLE_Of_FIVE_DIVISOR == ACCT_SSN_POSITION)
			{
				accountSSNs.push_back("");
				accountSSNs[n] = fileAccountSSN;
				n++;
			}

			i++;
		}
	}
	accountsIn.close();
}

void populateAccountNames()
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: populateAccountNames(); accountsController.cpp; line 918" << endl;
	}
	else
	{

		string fileAccountName;

		/*
		The following algorithm finds each account Name and puts it into a vector for later use.
		It does so by searching through the accounts.dat file. Each account Name is located on
		a line number that has a remainder of 2 when divided by 5, starting from 0; therefore, it takes each
		line number that has a remainder of 2 and puts it into the vector.
		*/

		int i = 0;	 //The line position in accountss.dat
		int n = 0;	 //The index of the vector
		while (!accountsIn.eof())
		{
			getline(accountsIn, fileAccountName);

			if (i % MULTIPLE_Of_FIVE_DIVISOR == ACCT_NAME_POSITION)
			{
				accountNames.push_back("");
				accountNames[n] = fileAccountName;
				n++;
			}

			i++;
		}
	}
	accountsIn.close();
}

void populateAccountAddresses()
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: populateAccountAddresses(); accountsController.cpp; line 957" << endl;
	}
	else
	{

		/*
		The following algorithm finds each account Address and puts it into a vector for later use.
		It does so by searching through the accounts.dat file. Each account Address is located on
		a line number that has a remainder of 3 when divided by 5, starting from 0; therefore, it takes each
		line number that has a remainder of 3 and puts it into the vector.
		*/

		string fileAccountAddress;

		int i = 0;	 //The line position in accounts.dat
		int n = 0;	 //the index of the vector
		while (!accountsIn.eof())
		{
			getline(accountsIn, fileAccountAddress);

			if (i % MULTIPLE_Of_FIVE_DIVISOR == ACCT_ADDRESS_POSITION)
			{
				accountAddresses.push_back("");
				accountAddresses[n] = fileAccountAddress;
				n++;
			}

			i++;
		}
	}
	accountsIn.close();
}

void populateAccountPhoneNumbers()
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: populateAccountPhoneNumbers(); accountsController.cpp; line 996" << endl;
	}
	else
	{

		string fileAccountPhoneNumber;

		/*
		The following algorithm finds each account Phone Number and puts it into a vector for later use.
		It does so by searching through the accounts.dat file. Each account Phone Number is located on
		a line number that has a remainder of 4 when divided by 5, starting from 0; therefore, it takes each
		line number that has a remainder of 4 and puts it into the vector.
		*/

		int i = 0;	 //The line position in accounts.dat
		int n = 0;	 //The index of the vector
		while (!accountsIn.eof())
		{
			getline(accountsIn, fileAccountPhoneNumber);

			if (i % MULTIPLE_Of_FIVE_DIVISOR == ACCT_PHONENUM_POSITION)
			{
				accountPhoneNumbers.push_back("");
				accountPhoneNumbers[n] = fileAccountPhoneNumber;
				n++;
			}

			i++;
		}
	}
	accountsIn.close();
}

void populateAllVectors()
{
	populateAccountNums();
	populateAccountSSNs();
	populateAccountNames();
	populateAccountAddresses();
	populateAccountPhoneNumbers();
}

void resizeAllVectors()
{
	accountNums.resize(0);
	accountSSNs.resize(0);
	accountNames.resize(0);
	accountAddresses.resize(0);
	accountPhoneNumbers.resize(0);
}
/*------------------------------------------------------------------------------------------------*/



/*------------------------------------------------------------------------------------------------*/
	/*
		Pre: the string parameter is an account number
	Purpose: this function sets the values on an info[] array and sends it
			 to another function to overwrite accounts.dat, excluding the info[]
			 array data.
	   Post: the info[] array has been sent to eraseFileLine()
	*/
void removeAccountFromFile(string accountNumber)
{
	populateAllVectors();

	string accountsInfo[INFO_ARRAY_SIZE];

	/*
		This algorithm takes a given account number, then takes that and all information 
		attached to that account and puts it into an array. It then passes that array to the
		functionthat deletes said information.
	*/

	for (int i = 0; i < accountNums.size(); i++)
	{
		if (accountNums[i] == accountNumber)
		{
			accountsInfo[ACCT_NUMBER_POSITION] = accountNums[i];
			accountsInfo[ACCT_SSN_POSITION] = accountSSNs[i];
			accountsInfo[ACCT_NAME_POSITION] = accountNames[i];
			accountsInfo[ACCT_ADDRESS_POSITION] = accountAddresses[i];
			accountsInfo[ACCT_PHONENUM_POSITION] = accountPhoneNumbers[i];
			break;
		}
	}

	eraseFileLine(accountsInfo);

	resizeAllVectors();
}


	/*
		Pre: the array is of size 5, and contains all information types of an account
	Purpose: This function overwrites accounts.dat excluding the account passed to it
			 in the parameter array, efectively deleting the account.
	   Post: the account has been reoved from accounts.dat
	*/
void eraseFileLine(string eraseLines[])
{
	string currentLine;
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: eraseFileLine(); accountsController.cpp; line 1101" << endl;
	}
	else
	{

		ofstream tempResult(TEMP_FILE);

		/*
			The following algorithm rewrites the information in accounts.dat to a temp file,
			excluding the account information that was passed to it in an array. That account
			is not written in because it is "deleted".
		*/

		int i = 0;
		while (!accountsIn.eof())
		{
			getline(accountsIn, currentLine);

			//The decision controlling whether or not to rewrite the data
			if (currentLine != eraseLines[i] && !accountsIn.eof())
				tempResult << currentLine << endl;
			else
				i++;
		}

		closeAndRemove(accountsIn, tempResult);
	}
}
/*------------------------------------------------------------------------------------------------*/


	/*
		Pre: each string parameter is the correct info type.
	Purpose: The following block of functions handle updating account information as specified
			 by the makeUpdateDecision() function.
	   Post: the information of the specified account has been updated, and accounts.dat
			 has been overwritten with the new info.
	*/
/*------------------------------------------------------------------------------------------------*/
void makeUpdateDecision(string infoArray[])
{
	string userChoice = getUserChoice();

	//Switch to decide which update to execute

	switch (convertStr2Int(userChoice))
	{
	case 1:
		updateSSN(infoArray[ACCT_SSN_POSITION]);
		break;
	case 2:
		updateName(infoArray[ACCT_NAME_POSITION]);
		break;
	case 3:
		updateAddress(infoArray[ACCT_ADDRESS_POSITION]);
		break;
	case 4:
		updatePhoneNumber(infoArray[ACCT_PHONENUM_POSITION]);
		break;
	case 5:
		cout << "Returning to Main Menu..." << endl;
		break;
	default:
		cout << "Invalid choice!" << endl;
		break;
	}
}

void updateSSN(string ssnToUpdate)
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);
	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: updateSSN(); accountsController.cpp; line 1176" << endl;
	}
	else
	{

		string currentLine;
		string newSSN;

		ofstream tempResult(TEMP_FILE);

		//Retrieve a new SSN
		cout << "Enter a new SSN: ";
		cin.ignore();
		getline(cin, newSSN);

		verifySSNFormat(newSSN);

		/*
			The following algorithm searches the accounts.dat file for the given SSN,
			then rewrites the information in accounts.dat to a temp file, overwriting the old
			SSN (given) with the new SSN.
		*/

		int i = 0;
		while (!accountsIn.eof())
		{
			getline(accountsIn, currentLine);

			if (currentLine == ssnToUpdate)
				tempResult << newSSN << endl;
			else
			{
				//This decision avoids adding an extra newline at the end of temp.dat
				if (!accountsIn.eof())
					tempResult << currentLine << endl;
				else
					tempResult << currentLine;
			}
		}

		//Delete and rename once again, effectively updating accounts.dat
		closeAndRemove(accountsIn, tempResult);
	}
}

void updateName(string nameToUpdate)
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: updateName(); accountsController.cpp; line 1226" << endl;
	}
	else
	{

		string currentLine;
		string newName;

		ofstream tempResult(TEMP_FILE);

		cout << "Enter a new Name: ";
		cin.ignore();
		getline(cin, newName);

		/*
		The following algorithm searches the accounts.dat file for the given name,
		then rewrites the information in accounts.dat to a temp file, overwriting the old
		name (given) with the new name.
		*/

		int i = 0;
		while (!accountsIn.eof())
		{
			getline(accountsIn, currentLine);

			if (currentLine == nameToUpdate)
				tempResult << newName << endl;
			else
			{
				//This decision avoids adding extra newlines at the end of temp.dat
				if (!accountsIn.eof())
					tempResult << currentLine << endl;
				else
					tempResult << currentLine;
			}
		}

		//Delete and rename, updating accounts.dat
		closeAndRemove(accountsIn, tempResult);
	}
}

void updateAddress(string addressToUpdate)
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: updateAddress(); accountsController.cpp; line 1274" << endl;
	}
	else
	{

		string currentLine;
		string newAddress;

		ofstream tempResult(TEMP_FILE);

		cout << "Enter a new Address: ";
		cin.ignore();
		getline(cin, newAddress);

		/*
		The following algorithm searches the accounts.dat file for the given address,
		then rewrites the information in accounts.dat to a temp file, overwriting the old
		address (given) with the new address.
		*/

		int i = 0;
		while (!accountsIn.eof())
		{
			getline(accountsIn, currentLine);

			if (currentLine == addressToUpdate)
				tempResult << newAddress << endl;
			else
			{
				//this decision avoids adding extra newlines to the end of temp.dat
				if (!accountsIn.eof())
					tempResult << currentLine << endl;
				else
					tempResult << currentLine;
			}
		}

		//Delete and rename, updating accounts.dat
		closeAndRemove(accountsIn, tempResult);
	}
}

void updatePhoneNumber(string numberToUpdate)
{
	accountsIn.open(CLIENT_ACCOUNTS_FILE);

	if (accountsIn.fail())
	{
		cout << "Error: File not found: \data\accounts.dat" << endl << "\tFunction: updatePhoneNumber(); accountsController.cpp; line 1322" << endl;
	}
	else
	{

		string currentLine;
		string newPhoneNumber;

		ofstream tempResult(TEMP_FILE);

		cout << "Enter a new Phone Number: ";
		cin.ignore();
		getline(cin, newPhoneNumber);

		verifyPhoneNumberFormat(newPhoneNumber);

		/*
		The following algorithm searches the accounts.dat file for the given phone number,
		then rewrites the information in accounts.dat to a temp file, overwriting the old
		phone number (given) with the new phone number.
		*/

		int i = 0;
		while (!accountsIn.eof())
		{
			getline(accountsIn, currentLine);

			if (currentLine == numberToUpdate)
				tempResult << newPhoneNumber << endl;
			else
			{
				//This decision avoids adding extra newlines to the and of temp.dat
				if (!accountsIn.eof())
					tempResult << currentLine << endl;
				else
					tempResult << currentLine;
			}
		}

		//Delete and rename, updating accounts.dat
		closeAndRemove(accountsIn, tempResult);
	}
}
/*------------------------------------------------------------------------------------------------*/


	/*
		Pre: the two fstream objects are initialized in the calling function
	Purpose: to close the given files, then remove accounts.dat, and rename temp.dat to accounts.dat
			 in order to "overwrite" accounts.dat.
	   Post: the fstreams have been closed, and accounts.dat has been overwritten.
	*/
/*------------------------------------------------------------------------------------------------*/
void closeAndRemove(ifstream &in, ofstream &out)
{
	in.close();
	out.close();

	remove(CLIENT_ACCOUNTS_FILE);
	rename(TEMP_FILE, CLIENT_ACCOUNTS_FILE);
}
/*------------------------------------------------------------------------------------------------*/



/*------------------------------------------------------------------------------------------------*/

	/*
		Pre: called from either main() or addAccount()
	Purpose: to create all balance files that aren't already made.
	   Post: all balance files are created
	*/
void createBalanceFiles()
{
	populateAccountNums();
	string currentFileName;
	string path = ".\\data\\";

	for (string num : accountNums)
	{
		if (num != "" && !checkIfBalanceFileExists(num))
		{
			currentFileName = num + ".dat";
			path += currentFileName;

			ofstream newbalanceFile(path);

			if (newbalanceFile.fail())
			{
					cout << "Error: Directory: \data\temp.dat" << endl << "\tFunction: createBalanceFiles(); accountsController.cpp; line 1400" << endl;
					break;
			}

			newbalanceFile << INITIAL_BALANCE;

			if (newbalanceFile.fail())
			{
				cout << "Failed to create output file in data folder" << endl;
			}

			newbalanceFile.close();
			path = ".\\data\\";
		}
	}

	resizeAllVectors();
}


/*
	Pre: the string parameter is an account number
Purpose: to make sure a balance file for the specified account does not already exist,
		 preventing balance data from being overwritten
   Post: a boolean value is returned indicating if a balance file for the specified account
		 exists or not.
*/
bool checkIfBalanceFileExists(string acctNum)
{
	string path = ".\\data\\";
	string currentFileName;

	ifstream testOpen;
	currentFileName = acctNum + ".dat";
	path += currentFileName;

	testOpen.open(path);

	if (!testOpen.fail())
	{
		testOpen.close();
		return true;
	}
	else
	{
		testOpen.close();
		return false;
	}
}


/*
	Pre: the string parameter is an account number
Purpose: removes the balance file associated with the given account, to finish
		 deleting the account.
   Post: the specified balance file is deleted
*/
void removeBalanceFile(string acctNum)
{
	const char *FILE_TO_REMOVE;
	string path = ".\\data\\";
	string fileName = acctNum + ".dat";

	path += fileName;

	FILE_TO_REMOVE = path.c_str();

	remove(FILE_TO_REMOVE);
}
/*------------------------------------------------------------------------------------------------*/