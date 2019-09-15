#include "header.h"

/*
		  Author : Cameron Schneider
		  Project: Bank Account Final Project
	  Description: Controls all menu displaying and retrieval of choices

	Certification: located in main.cpp file
*/


/*
	 Pre: called from the main() function in main.cpp
 Purpose: to display a main menu of options
	Post: the menu is displayed
*/
void displayFirstMenu()
{
	cout << "1) Add a new account" << endl;
	cout << "2) Delete an existing account" << endl;
	cout << "3) Update information on existing account" << endl;
	cout << "4) Search account information" << endl;
	cout << "5) Deposit money into account" << endl;
	cout << "6) Withdraw money from account" << endl;
	cout << "7) Check balance on account" << endl;
	cout << "8) Logout" << endl;
}

/*
	 Pre: called from main() in main.cpp
 Purpose: to display a menu containing search options
	Post: the search menu is displayed
*/
void displayAccountSearchMenu()
{
	cout << "1) Search by Account Number" << endl;
	cout << "2) Search by Social Security Number" << endl;
	cout << "3) Search by Name" << endl;
	cout << "4) Search by Address" << endl;
	cout << "5) Search by Phone Number" << endl;
	cout << "6) Back to Main Menu" << endl;
}

/*
	Pre: called from updateAccount() in accountController.cpp
Purpose: to display a menu containing update options
   Post: the menu is displayed
*/

void displayAccountUpdateMenu()
{
	cout << "1) Update SSN" << endl;
	cout << "2) Update Name" << endl;
	cout << "3) Update Address" << endl;
	cout << "4) Update Phone Number" << endl;
	cout << "5) Back to Main Menu" << endl;
}

/*
	Pre: called from another function
Purpose: retrieves the user's menu choice
   Post: returns a string
*/
string getUserChoice()
{
	string userChoice;

	cout << "\nEnter choice: ";
	cin >> userChoice;

	return userChoice;
}