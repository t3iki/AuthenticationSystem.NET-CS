using System.Text.RegularExpressions;

static bool checkLength(string var, string name, int len)
{
    while (var.Length < len) return false;

    return true;
}

static bool findLogin(string[] players, string login)
{
    bool loginExists = false;

    foreach (string player in players)
    {
        string playerLogin = player.Split(" ")[0];

        if (playerLogin == login)
        {
            loginExists = true;
            break;
        }
        else
        {
            loginExists = false;
        }
    }

    return loginExists;
}

static void login(string[] players)
{
    Console.WriteLine("\n-Login-");

    Console.Write("Enter login -> ");
    string login = Console.ReadLine();

    bool loginExists = findLogin(players, login);

    int loginTries = 1;
    while (!loginExists && loginTries < 3)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("User does not exist");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter your login: ");
        login = Console.ReadLine();
        loginExists = findLogin(players, login);
        loginTries++;
    }

    if (loginTries == 3)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Too many tries");
        return;
    }

    if (loginExists)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.Write("Enter your password -> ");
        string password = Console.ReadLine();

        bool passMatch = false;
        int passTries = 1;

        foreach (string player in players)
        {
            string playerLogin = player.Split(" ")[0];

            if (playerLogin == login)
            {
                string truePassword = player.Split(" ")[1];
                if (password == truePassword) passMatch = true;
                break;
            }
        }

        while (!passMatch && passTries < 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong password");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter password again -> ");
            password = Console.ReadLine();

            foreach (string player in players)
            {
                string playerLogin = player.Split(" ")[0];

                if (playerLogin == login)
                {
                    string truePassword = player.Split(" ")[1];
                    if (password == truePassword) passMatch = true;
                    break;
                }
            }

            passTries++;
        }

        if (passTries == 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You've entered a password too many times");
            return;
        }

        if (passMatch)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome, " + login + " back!");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("This is your cute cat \n");
            Console.WriteLine("   |\\_._/|   ");
            Console.WriteLine("   | o o |    ");
            Console.WriteLine("   (  T  ) Hi, " + login + "!");
            Console.WriteLine("  .^`-^-'^.   ");
            Console.WriteLine("  `.  ;  .'  ");
            Console.WriteLine("  | | | | |   ");
            Console.WriteLine(" ((_((|))_))  ");

            Console.Write("Type any key to logout -> ");
            char answer = Console.ReadKey().KeyChar;

            switch (answer)
            {
                default:
                    return;
            }
        }
    }
}

static string[] register(string[] players)
{
    Console.WriteLine("\n-Register-");

    string login, password, email;

    Console.Write("Enter a login -> ");
    login = Console.ReadLine();

    bool loginIsOk = checkLength(login, "LOGIN", 3);
    bool loginExists = findLogin(players, login);

    while (loginExists || !loginIsOk)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (loginExists) Console.WriteLine("Login is already registered\n");
        if (!loginIsOk) Console.WriteLine("Login must be at least 4 characters\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter new login -> ");
        login = Console.ReadLine();

        loginIsOk = checkLength(login, "LOGIN", 4);
        loginExists = findLogin(players, login);
    }

    Console.ForegroundColor = ConsoleColor.Green;

    Console.Write("Enter a password -> ");
    password = Console.ReadLine();

    bool passIsOk = checkLength(password, "PASSWORD", 8); // false

    while (!passIsOk)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (!passIsOk) Console.WriteLine("Password must be at least 8 characters");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter new password -> ");
        password = Console.ReadLine();

        passIsOk = checkLength(password, "PASSWORD", 8);
    }

    Console.ForegroundColor = ConsoleColor.Green;

    Console.Write("Enter an email -> ");
    email = Console.ReadLine();

    var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

    bool match = regex.IsMatch(email);

    while (!match)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Not an e-mail");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter a new e-mail: ");
        email = Console.ReadLine();

        match = regex.IsMatch(email);
    }

    Console.ForegroundColor = ConsoleColor.Green;

    string acc = login + " " + password + " " + email;

    Array.Resize(ref players, players.Length + 1);
    players[players.Length - 1] = acc;

    string allPlayersTxt = "";
    foreach (string player in players)
    {
        allPlayersTxt += player + "\n";
    }

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("\nWelcome " + login + " to the STATZ.NET\n");

    File.WriteAllTextAsync("../../../db.txt", allPlayersTxt);

    return players;
}

string[] players = File.ReadAllLines("../../../db.txt");

char option = '_';

Console.ForegroundColor = ConsoleColor.Blue;
Console.Write("-Welcome to STATZ.NET-");

while (option != 'x')
{
    Console.ForegroundColor = ConsoleColor.Green;

    Console.Write(
         "\n---------------\n" +
         "Options: \n" +
         "1. Login\n" +
         "2. Register\n" +
         "x. Exit\n" +
         "Select your option -> ");

    option = Console.ReadKey().KeyChar;

    switch (option)
    {
        case '1':
            login(players);
            break;
        case '2':
            players = register(players);
            break;
        case 'x':
            Environment.Exit(0);
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nWrong option, choose ANOTHER ONE -> \n");
            break;
    }
}