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
            double conversion = Convert.ToDouble(balance);
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

        public void logout()
        {
            this.currentAcc = null;
            this.login = false;
            Console.Clear();
        }

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

        public void updateAccBalance(double amount)
        {
            this.currentAcc.updateBalance(amount);
        }
        public int notLoggedInSetup()
        {
            string usrInput;
            int userInputInt;
            Console.WriteLine("Option 1 : Login");
            Console.WriteLine("Option 2 : Create Account");
            Console.WriteLine("Option 3 : Quit");
            Console.Write("Input option number then press enter : ");
            usrInput = Console.ReadLine();
            userInputInt = Convert.ToInt32(usrInput);
            return userInputInt;
        }

        public int loggedOptions()
        {
            string usrInput;
            int userInputInt;
            Console.WriteLine("Option 1 : Check Balance");
            Console.WriteLine("Option 2 : Deposit");
            Console.WriteLine("Option 3 : Withdrawal");
            Console.WriteLine("Option 4 : Transaction History");
            Console.WriteLine("Option 5 : Logout");
            Console.Write("Input option number then press enter : ");
            usrInput = Console.ReadLine();
            userInputInt = Convert.ToInt32(usrInput);
            return userInputInt;
        }


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
