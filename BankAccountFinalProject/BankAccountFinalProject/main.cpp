#include "header.h"

/*
Author: Cameron Schneider
Class: CSI-140-01/02
Assignment: Bank Accounting Final Project
Date Assigned: 11-7-17
Due Date: 12-6-17

Description:	This program is a piece of software that allows bank tellers to perform a variety
		of standard teller operations from adding accounts, to searching for accounts, to managing
		money.


Certification of Authenticity:
I certify that this is entirely my own work, except where I have given
fully-documented references to the work of others. I understand the
definition and consequences of plagiarism and acknowledge that the assessor
of this assignment may, for the purpose of assessing this assignment:
- Reproduce this assignment and provide a copy to another member of
academic staff; and/or
- Communicate a copy of this assignment to a plagiarism checking
service (which may then retain a copy of this assignment on its
database for the purpose of future plagiarism checking)
*/



int main()
{
	string userMenuChoice;						//The general choice that the user makes to control menus

	createBalanceFiles();						//Create any missing balance files

	login();									//Call to a function in the accountController.cpp file to log into the system

	correctPreExistingPhoneNumberFormats();		/*Call to a function in the accountController.cpp file to correct phone number
													formats already in the accounts.dat file*/
	do
	{
		displayFirstMenu();						//Display the main menu (call to function in menuController.cpp file)
		userMenuChoice = getUserChoice();		//Get the user's choice for the main menu (call to function in menuController.cpp file


		//This switch decision controls which option happens depending on the user's choice
		switch (convertStr2Int(userMenuChoice))
		{

		// Add an account
/*------------------------------------------------------------------------------------------------*/
		case 1:
			addAccount();		//Call to a function in accountController.cpp file
			system("pause");
			system("cls");
			break;

		//Delete an account
/*------------------------------------------------------------------------------------------------*/
		case 2:
			deleteAccount();	//Call to a function in accountController.cpp file
			system("pause");
			system("cls");
			break;

		//Update an account's info
/*------------------------------------------------------------------------------------------------*/
		case 3:
			updateAccount();	//Call to a function in accountController.cpp file
			system("pause");
			system("cls");
			break;

		//Search for an account
/*------------------------------------------------------------------------------------------------*/
		case 4:
			system("cls");
			searchAccounts();	//Call to a function in accountController.cpp file
			system("cls");
			break;

		//Deposit money into an account
/*------------------------------------------------------------------------------------------------*/
		case 5:
			deposit();
			system("pause");
			system("cls");
			break;

		//Withdraw money from an account
/*------------------------------------------------------------------------------------------------*/
		case 6:
			withdraw();
			system("pause");
			system("cls");
			break;

		//Check the balance of an account
/*------------------------------------------------------------------------------------------------*/
		case 7:
			checkBalance();
			system("pause");
			system("cls");
			break;

		//Log out of the system
/*------------------------------------------------------------------------------------------------*/
		case 8:
			logout();			//Call to a function in the accountController.cpp file
			break;

		//Catch-all for invalid choices
/*------------------------------------------------------------------------------------------------*/
		default:
			cout << "Invalid Choice" << endl;
			system("pause");
			system("cls");
			break;
		}
	} while (true);		// while(true) forces the program to run until system is shut down
		
	system("pause");
}

/*
	Pre : @param intValue is an integer
	Post: returns intValue as a string
*/
string convertInt2Str(int intValue)
{
	stringstream stringValue;
	stringValue << intValue;
	return stringValue.str();
}

/*
Pre : @param stringValue is a string
Post: returns intValue as an int
*/
int convertStr2Int(string stringValue)
{
	int          intValue;
	stringstream str;
	str << stringValue;
	str >> intValue;
	return intValue;
}

/*
Pre : @param str is a string
Post: returns str as an all-uppercase string
*/
string toUpper(string str)
{
	int i, length;

	for (int i = 0, length = str.length(); i < length; i++)
	{
		str[i] = toupper(str[i]);
	}

	return str;
}

/* 
	Pre: The string to be converted in stored in stringValue
Purpose: Convert a string to double
   Post: The numeric value for the given string in doubleValue and
		 true if the string can be converted into double and false if
		 the string cannot be converted into double
*/
double convertStr2Double(string stringValue)
{
	double doubleValue;
	stringstream ss;
	ss << stringValue;
	ss >> doubleValue;
	
	return doubleValue;
}