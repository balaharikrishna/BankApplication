using System.Text.RegularExpressions;

namespace BankApplication
{
    internal static class Miscellaneous
    {
        public static string reserveBankManagerName = "Technovert";
        public static string reserveBankManagerpassword = "Techno123@";
        public static string defaultCurrencyCode = "INR";
        public static short defaultCurrencyValue = 1;
        public static Regex passwordRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        public static Regex regex = new Regex("^[a-zA-Z]+$");
        public static Regex phoneNumberRegex = new Regex("^\\d{10}$");
        public static Regex exchangeRateRegex = new Regex(@"\b[A-Z]{3}\s\d{1,3}(,\d{3})*(\.\d+)?\b");
        public static Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        public static Regex dateRegex = new Regex(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$");// Enter a date(DD/MM/YYYY)
        public static string passwordComment = "Password Should Contain minimum 8 characters with atleast one uppercase ,one lowercase,one digit,one special character";

        //positions/keywords
        public static string customer = "Customer";
        public static string toCustomer = "ToCustomer";
        public static string staff = "Staff";
        public static string branchManager = "Branch Manager";
        public static string headManager = "Head Manager";
        public static string reserveBankManager = "Reserve Bank Manager";
        public static string mainPage = "Main Page";
        public static string back = "Back";

        //banks related
        public static string bank = "Bank";
        public static string branch = "Branch";

    }
}
