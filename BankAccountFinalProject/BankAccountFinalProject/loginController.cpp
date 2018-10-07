#include "header.h"

/*
Author : Cameron Schneider
Project: Bank Account Final Project
Description: Controls all log in and log out functionality

Certification: located in main.cpp file
*/

	//Helper functions
/*------------------------------------------------------------------------------------------------*/
void getUserID();
void getPassword();
bool validateIDAndPassword();
/*------------------------------------------------------------------------------------------------*/

string userID;
string userPassword;

ifstream tellerData;

//The file name containing all the teller ID's and passwords
const string BANK_TELLERS_FILE = ".\\data\\tellers.dat";

//This is initialized to true in order to allow the program to loop while logging in
bool isNotLoggedIn = true;


/*
	Pre: called from main() in main.cpp
Purpose: log the user out of the system
   Post: the user is logged out, system returns to login screen
*/
void logout()
{
	cout << "Goodbye, " << userID << endl;
	system("pause");
	system("cls");
	isNotLoggedIn = true;	//Reset to true because the user is now not logged in

	//Calls login() again to restart the process
	login();
}


/*
	Pre: called from main() in main.cpp and logout() in this file; user is NOT currently logged in.
Purpose: to log in the user successfully
   Post: the user is logged in
*/
void login()
{
	while (isNotLoggedIn)
	{
		cout << "Willkommen zu Commerzbank!" << endl << endl;

		getUserID();
		getPassword();

		//Make sure the given credentials are valid
		if (validateIDAndPassword())
		{
			isNotLoggedIn = false;
			system("cls");
			cout << "Welcome, " << userID << endl << endl;
		}
		else
		{
			cout << "User ID/Password Incorrect" << endl;
			system("pause");
			system("cls");
		}
	}

}


/*
	Pre: none
Purpose: retrieve the user's ID
   Post: userID contains the given string
*/
void getUserID()
{
	cout << "Enter User ID: ";
	cin >> userID;
}

/*
Pre: none
Purpose: retrieve the user's password
Post: userPassword contains the given string
*/
void getPassword()
{
	cout << "Enter Password: ";
	cin >> userPassword;
}


/*
	Pre: userID and userPassword are initialized
Purpose: validate that userID and userPassword are valid credentials
   Post: returns true if userID and userPassword are valid, false if not.
*/
bool validateIDAndPassword()
{
	tellerData.open(BANK_TELLERS_FILE);

	string fileUserName;
	string filePassword;
	string junk;
	bool tellerFound = false;

	if (tellerData.fail())
	{
		cout << "FATAIL ERROR: Input file not found: tellers.dat" << endl;
		system("pause");
		return tellerFound;
	}

	while(!tellerData.eof())
	{
		tellerData >> fileUserName;
		tellerData >> filePassword;
		getline(tellerData, junk);

		//Validate userID and usePassword
		if (fileUserName == userID && filePassword == userPassword)
		{
			tellerFound = true;
			break;
		}
	}

	tellerData.close();
	return tellerFound;
}