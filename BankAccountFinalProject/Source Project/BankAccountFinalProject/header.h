#pragma once

/*
	Author : Cameron Schneider
	Project: Bank Account Final Project

	Description:	The collection of functions and header files that must be shared by
				all cource code files.

	Certification: located in main.cpp file
*/

#include <iostream>
#include <string>
#include <sstream>
#include <fstream>
#include <cstdlib>
using namespace std;

// A static cstring that represents a temporary file used in multiple functions through the project
/*------------------------------------------------------------------------------------------------*/
static const char *TEMP_FILE = "temp.dat";
/*------------------------------------------------------------------------------------------------*/


	//Functions in main.cpp used for raw datatype manipulation
/*------------------------------------------------------------------------------------------------*/
int convertStr2Int(string stringValue);
string convertInt2Str(int intValue);
string toUpper(string str);
double convertStr2Double(string stringValue);
/*------------------------------------------------------------------------------------------------*/


	//Functions in loginController.cpp used for teller login/logout
/*------------------------------------------------------------------------------------------------*/
void login();
void logout();
/*------------------------------------------------------------------------------------------------*/


	//Functions in menuController.cpp used for any menu displaying
/*------------------------------------------------------------------------------------------------*/
void displayFirstMenu();
void displayAccountSearchMenu();
void displayAccountUpdateMenu();
string getUserChoice();
/*------------------------------------------------------------------------------------------------*/


	//Functions in accountController.cpp used for any account manipulation
/*------------------------------------------------------------------------------------------------*/
void addAccount();
void deleteAccount();
void updateAccount();
void searchAccounts();
void correctPreExistingPhoneNumberFormats();
void writeCorrectedPhoneNumsToFile();


void createBalanceFiles();	//Creates all balance files
/*------------------------------------------------------------------------------------------------*/

	//Functions in balanceController.cpp used for balance manipulation
/*------------------------------------------------------------------------------------------------*/
void checkBalance();
void deposit();
void withdraw();
/*------------------------------------------------------------------------------------------------*/