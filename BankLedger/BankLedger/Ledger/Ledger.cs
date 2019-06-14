using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankLedger.User;
namespace BankLedger.Ledger
{
    class NPC
    {
        private List<Checkbook> accounts = new List<Checkbook>(); // keeps a running list of created account for each console session
        private bool login;
        private Checkbook currentAcc;
        public NPC() {
            this.login = false;
            this.currentAcc = null;
        }

        /*
         Function: runLedger
         Job: Flow control of the program and gives user options to perform tasks

             */
        public void runLedger()
        {
            
            bool quit = false;
            while (!quit)
            {
                // not logged in options
                if(this.login == false)
                {
                    // option 1 Login
                    // option 2 is Create account
                    int userSelection = notLoggedInSetup();
                    while (userSelection < 1 || userSelection > 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Please choose again.");
                        userSelection = notLoggedInSetup();
                    }
                   
                    switch (userSelection)
                    {
                        case 1:
                            // Login //
                            userLogin();
                            break;
                        case 2:
                            // create acc 
                            createAcc();
                            break;
                        case 3:
                            quit = true;
                            break;
                    }
                }
                // logged in options
                else
                {
                    // 1->5 Balance / Deposit/With/ History / Logout
                    int userSelection = loggedOptions();
                    while(userSelection < 1 || userSelection > 5)
                    {
                        Console.Clear();
                        Console.WriteLine("Please choose again.");
                        userSelection = loggedOptions();
                    }

                    switch (userSelection)
                    {
                        case 1:
                            checkBalance();
                            break;
                        case 2:
                            // deposit
                            makeDeposit();
                            break;
                        case 3:
                            //with
                            makeWithdrawal();
                            break;
                        case 4:
                            seeHistory();
                            break;
                        case 5:
                            // logout
                            logout();
                            break;
                    }
                }
            }
        }


        //Class Methods//
        /*
         Function: userLogin
         Job: Prompts user for their login info
         Params: None
         Returns: None ; alters bool value of logged in or not 
             */
        private void userLogin()
        {
            string userName;
            string userPass;
            int attempts = 3;
            Console.Write("Username :");
            userName = Console.ReadLine();
            // Could add some "protection" masking password from showing in console
            Console.Write("Password :");
            userPass = Console.ReadLine();
            Console.Clear();
            updateLogStatus(userName, userPass);

            while(this.login == false && attempts != 0)
            {
                Console.WriteLine("You have {0} attempts left.", attempts);
                Console.Write("Username :");
                userName = Console.ReadLine();
                // Could add some "protection" masking password from showing in console
                Console.Write("Password :");
                userPass = Console.ReadLine();
                Console.Clear();
                updateLogStatus(userName, userPass);
                attempts--;
            }
        }


        /*
         Function: createAcc
         Job: Lets the user create an account to login with 
         Params: none
         Returns: none; stores the new acc in class' Account to keep track of
             */
        public void createAcc()
        {
            string userName;
            string userPass;
            bool valid = false;
            Console.Write("Username :");
            userName = Console.ReadLine();
            valid = checkUser(userName); // if user name is taken valid = false
            while (!valid)
            {
                Console.WriteLine("Please pick a different username.");
                Console.Write("Username :");
                userName = Console.ReadLine();
                valid = checkUser(userName);
            }

            Console.Write("Password :");
            userPass = Console.ReadLine();
            Console.Clear();
            Console.Write("Starting Balance: ");
            string balance = Console.ReadLine();
            // https://social.msdn.microsoft.com/Forums/windows/en-US/84990ad2-5046-472b-b103-f862bfcd5dbc/how-to-check-string-is-number-or-not-in-c?forum=winforms //
            double conversion;
            bool numberInput = double.TryParse(balance, out conversion);
            while (!numberInput)
            {
                Console.Write("Please enter a numerical value. Starting Balance: ");
                balance = Console.ReadLine();
                numberInput = double.TryParse(balance, out conversion);
            }
            conversion = Convert.ToDouble(balance);
            Checkbook newUser = new Checkbook(userName, userPass, conversion);
            this.accounts.Add(newUser);
            Console.Clear();
        }
        /*
         Function: checkBalance()
         Job: Allows user to see their account balance, waits before clearing screen
         Params: None
         Returns: None; Console output
             */
        public void checkBalance()
        {
            Console.WriteLine("The current balance is $ {0}.", this.currentAcc.checkBalance());
            Console.WriteLine("Press Enter When Done.");
            string tmp = Console.ReadLine();
            Console.Clear();
        }

        /*
         Function: makeDeposit()
         Job: Prompts user for a deposit amount + description
         Parameters: None
         Returns: None
             */
        public void makeDeposit()
        {
            string Amount;
            string Describe;
            Console.Write("How much would you like to deposit? ");
            Amount = Console.ReadLine();
            Console.Write("Make a note about this deposit: ");
            Describe = Console.ReadLine();
            double value = Convert.ToDouble(Amount);
            //Update the acc balance and push a memo to update txHistory in user
            updateAccBalance(value);
            this.currentAcc.depositTransaction(Describe, value);
            Console.Clear();

        }
        /*
         Function: makeWithdrawal()
         Job: Prompts user for a withdrawal amount + description
         Parameters: None
         Returns: None
             */
        public void makeWithdrawal()
        {
            string Amount;
            string Describe;
            Console.Write("How much would you like to withdraw? ");
            Amount = Console.ReadLine();
            Console.Write("Make a note about this withdrawal: ");
            Describe = Console.ReadLine();
            double value = Convert.ToDouble(Amount);
            updateAccBalance(value * -1); // *-1 b/c we're withdrawing
            this.currentAcc.withdrawTransaction(Describe, value);
            Console.Clear();
        }
        /*
         Function: logout()
         Job: Logs the user out by nulling our currentAcc field aka the target acc all transaction go to; and setting the login (bool status) to false
         Parameters: None
         Returns: None
             */
        public void logout()
        {
            this.currentAcc = null;
            this.login = false;
            Console.Clear();
        }

        /*
         Function: seeHistory()
         Job: prints out all transaction history of current account
         Parameters: None
         Returns: None; console printout
             */
        public void seeHistory()
        {
            List<Tuple<string,double>> history = this.currentAcc.checkHistory();

            foreach (Tuple<string,double> memo in history)
            {
                Console.WriteLine(memo);
            }

            Console.WriteLine("Press Enter When Done.");
            string tmp = Console.ReadLine();
            Console.Clear();

        }


        //Util//

         // Utility Function which updates the account balance by calling on the User method//
        public void updateAccBalance(double amount)
        {
            this.currentAcc.updateBalance(amount);
        }
        // Handles letting the user pick options when not logged in returns 1-3 for option choice
        public int notLoggedInSetup()
        {
            string usrInput;
            int userInputInt;
            Console.WriteLine("Option 1 : Login");
            Console.WriteLine("Option 2 : Create Account");
            Console.WriteLine("Option 3 : Quit");
            // Do While loop incase user doesn't input any values
            do
            {
                Console.Write("Input option number then press enter : ");
                usrInput = Console.ReadLine();
            } while (usrInput == "");
            //usrInput = Console.ReadLine();
            userInputInt = Convert.ToInt32(usrInput);
            return userInputInt;
        }

        // Handles letting user pick options when they ARE logged in 1-5 choices
        public int loggedOptions()
        {
            string usrInput;
            int userInputInt;
            Console.WriteLine("Option 1 : Check Balance");
            Console.WriteLine("Option 2 : Deposit");
            Console.WriteLine("Option 3 : Withdrawal");
            Console.WriteLine("Option 4 : Transaction History");
            Console.WriteLine("Option 5 : Logout");
            do
            {
                Console.Write("Input option number then press enter : ");
                usrInput = Console.ReadLine();
            } while (usrInput == "");
            userInputInt = Convert.ToInt32(usrInput);
            return userInputInt;
        }

        //Checks if user and password matches 
        private void updateLogStatus(string name, string pass)
        {
            foreach (Checkbook person in this.accounts)
            {
                if(name == person.USERID)
                {
                    this.login = person.checkPassword(pass);
                    this.currentAcc = person;
                }
            }

            if (this.login)
            {
                Console.WriteLine("Login Successful");
            }
            else
                Console.WriteLine("Login Failed");
        }

        // checks to see if username already exists in our accounts list
        private bool checkUser(string name)
        {
            foreach (Checkbook person in this.accounts)
            {
                if (person.USERID == name)
                    return false;
            }

            return true;
        }
    }
}
