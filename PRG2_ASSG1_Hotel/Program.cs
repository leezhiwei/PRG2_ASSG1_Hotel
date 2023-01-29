// Zhi Wei does 2,4,6
// Jia Xian does 1,3,5
using PRG2_ASSG1_Hotel;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

List<Guest> guestList = new List<Guest>();
List<Room> availrooms = new List<Room>();
List<Room> occupiedrooms = new List<Room>();
//Initialise Data from csv files
void InitData()
{
    List<List<string>> roomlist = new List<List<string>>();
    List<List<string>> guestlist = new List<List<string>>(); // init string-list var
    List<List<string>> stayslist = new List<List<string>>();
    //Reading data from csv
    string[] roomdata = File.ReadAllLines("Rooms.csv");
    string[] guestdata = File.ReadAllLines("Guests.csv"); // readalllines of CSV
    string[] staysdata = File.ReadAllLines("Stays.csv");
    for (int i = 1; i < roomdata.Length; i++)
    {
        List<string> values = roomdata[i].Split(',').ToList();
        //Adding data into list
        roomlist.Add(values);
    }
    for (int i = 1; i < guestdata.Length; i++)
    {
        List<string> values = guestdata[i].Split(',').ToList();
        //Adding data into list
        guestlist.Add(values);
    }
    for (int i = 1; i < staysdata.Length; i++)
    {
        List<string> values = staysdata[i].Split(',').ToList();
        //Adding data into list
        stayslist.Add(values);
    }
    foreach (List<string> bookings in stayslist) // iterate through bookinglist
    {
        Stay book = new Stay(Convert.ToDateTime(bookings[3]), Convert.ToDateTime(bookings[4]));
        Room secroom = null;
        foreach (List<string> rooms in roomlist) // iterate through roomslist
        {
            if (rooms[1] == bookings[5]) // if roomno matches first one
            {
                if (bookings[9] != "") // where roomnum (2nd one) is not empty
                {
                    foreach (List<string> r in roomlist) // iterate again
                    {
                        if (r[1] == bookings[9]) // if iterate, matches
                        {
                            if (r[0] == "Standard") // if type is standard
                            {
                                StandardRoom room = new StandardRoom(Convert.ToInt32(r[1]), r[2], Convert.ToDouble(r[3]), !(Convert.ToBoolean(bookings[2])));
                                room.RequireWifi = Convert.ToBoolean(bookings[10]);
                                room.RequireBreakfast = Convert.ToBoolean(bookings[11]);
                                secroom = room;

                            }
                            else if (rooms[0] == "Deluxe") // if type is deluxe
                            {
                                DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(r[1]), r[2], Convert.ToDouble(r[3]), !(Convert.ToBoolean(bookings[2])));
                                room.AdditionalBed = Convert.ToBoolean(bookings[12]);
                                secroom = room;
                            }
                        }
                    }
                }
                if (rooms[0] == "Standard") // if type is standard
                {
                    StandardRoom room = new StandardRoom(Convert.ToInt32(rooms[1]), rooms[2], Convert.ToDouble(rooms[3]), !(Convert.ToBoolean(bookings[2])));
                    room.RequireWifi = Convert.ToBoolean(bookings[6]);
                    room.RequireBreakfast = Convert.ToBoolean(bookings[7]);
                    book.AddRoom(room);
                    occupiedrooms.Add(room);

                }
                else if (rooms[0] == "Deluxe") // if type is deluxe
                {
                    DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(rooms[1]), rooms[2], Convert.ToDouble(rooms[3]), !(Convert.ToBoolean(bookings[2])));
                    room.AdditionalBed = Convert.ToBoolean(bookings[12]);
                    book.AddRoom(room);
                    occupiedrooms.Add(room);
                }
                if (secroom is not null)
                {
                    book.AddRoom(secroom);
                    occupiedrooms.Add(secroom);
                }
                foreach (List<string> guestprof in guestlist)
                {
                    if (guestprof[1] == bookings[1])
                    {
                        Membership m = new Membership(guestprof[2], Convert.ToInt32(guestprof[3]));
                        Guest guestobj = new Guest(guestprof[0], guestprof[1], book, m);
                        guestobj.IsCheckedin = Convert.ToBoolean(bookings[2]);
                        guestList.Add(guestobj);
                    }
                }
            }
        }
    }
    availrooms = AvailRoom();
    foreach (List<string> ro in roomlist) // add in other avail rooms
    {
        bool isInList = false;
        bool occupied = false;
        foreach (Room room in availrooms)
        {
            if (room.RoomNumber == Convert.ToInt32(ro[1]))
            {
                isInList = true;
                break;
            }
        }
        if (isInList)
        {
            continue;
        }
        foreach (Room or in occupiedrooms)
        {
            if (or.RoomNumber == Convert.ToInt32(ro[1]))
            {
                occupied = true;
                break;
            }
        }
        if (occupied)
        {
            continue;
        }
        if (ro[0] == "Standard") // if type is standard
        {
            StandardRoom room = new StandardRoom(Convert.ToInt32(ro[1]), ro[2], Convert.ToDouble(ro[3]), true);
            availrooms.Add(room);

        }
        else if (ro[0] == "Deluxe") // if type is deluxe
        {
            DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(ro[1]), ro[2], Convert.ToDouble(ro[3]), true);
            availrooms.Add(room);
        }
    }
}
List<Room> AvailRoom()
{ // Lee Zhi Wei
    List<Room> AvailableRms = new List<Room>(); // new ReturnList
    foreach (Guest g in guestList)
    { // foreach guest in guestlist
        foreach (Room r in g.HotelStay.RoomList)
        { // foreach room in Stay.RoomList
            Room ro = r; // need to reassign to local var, if not will give error (cannot assign to foreach object)
            if (ro.IsAvail)
            { // if available
                if (ro is DeluxeRoom)
                { // if deluxe
                    DeluxeRoom dr = (DeluxeRoom)ro; // cast to DeluxeRoom object
                    dr.AdditionalBed = false; // set AdditonalBed to false
                    ro = dr; // put back to initial variable
                }
                else if (ro is StandardRoom) 
                { // else if standard
                    StandardRoom sr = (StandardRoom)ro; // cast to standard room object
                    sr.RequireBreakfast = false; // set breakfast to false
                    sr.RequireWifi = false; // set wifi to false
                    ro = sr; // put back to initial variable
                }
                AvailableRms.Add(ro); // add to returnlist
            }
        }
    }
    return AvailableRms; // return the list
}
void DisplayGuest(List<Guest> guestList) //Created by Lim Jia Xian
 {  //Assignment Part 1
    foreach (Guest g in guestList)
    {
        Console.WriteLine(g.ToString());
    }
    Console.WriteLine();
}
void DisplayGuestName(List<Guest> guestList) //Created by Lim Jia Xian
{   //Print guuest names
    int k = 0;
    foreach (Guest g in guestList)
    {
        k++;
        Console.WriteLine($"{k}. {g.Name}");
    }
    Console.WriteLine();
}
bool ValidateNameCheck(string entName) //Created by Lim Jia Xian
{   //Function to check if name contains numbers or special characters
    Regex regex = new Regex(@"^[a-zA-z]+$");
    return regex.IsMatch(entName); //Returns false if name contains numbers or special characters, Returns true if name is within the range from A - Z
}
void RegisterGuest() //Created by Lim Jia Xian
{   //Assignment Part 3
    while (true)
    {
        string gName;
        string gPN;
        bool gDup = false;
        bool gPDup = false;
        Console.WriteLine("Please enter the following information\n");
        Console.Write("Guest name: ");
        gName = Console.ReadLine();

        if (ValidateNameCheck(gName) == false) //Checks if name entered was filled with numbers.
        {
            Console.WriteLine("\nName should not contain any numbers or special characters.\n");
            continue;
        }
        else if (gName == null || gName.Length < 3 || gName.Trim().Length < 3) //If the name entered is empty or less than 3 characters or name filled with spaces
        {
            Console.WriteLine("\nThe guest name should not be empty or less than 3 characters in length. Please try again!\n");
            continue;
        }
        else
        {
            foreach (Guest g in guestList)
            {
                if (g.Name.ToUpper() == gName.ToUpper()) //Checks if the name is exactly the same with registered guest names
                {
                    gDup = true; //Guest name duplicate changes to true
                    break;
                }
            }
        }
        
        Console.Write("Guest passport number: ");
        gPN = Console.ReadLine();
        if (gPN.Trim().Length < 5) //Checks if guest passport number is empty or contains spaces or less than 5 characters in length
        {
            Console.WriteLine("Passport number cannot be empty or less than 5 characters in length. Please try again!\n");
            continue;
        }
        else
        {
            foreach (Guest g in guestList)
            {
                if (g.PassportNum.ToUpper() == gPN.ToUpper()) //Checks if the passport num is exactly the same with registered passport nums
                {
                    gPDup = true; //Guest passport num duplicate changes to true
                    break;
                }
            }
        }
        
        if (gDup == true && gPDup == true) //Checks if both guest name & passport num has already existed
        {
            Console.WriteLine("\nA Guest with that name & passport number has already been registered.\nPlease try again later with a unique name & passport number.\n");
            break;
        }
        else if (gPDup == true) //Checks if guest passport num already existed
        {
            Console.WriteLine("\nA Guest with that passport number has already been registered.\nPlease try again later with a unique passport number.\n");
            break;
        }
        else if (gDup == true) //Checks if guest name already existed
        {
            Console.WriteLine($"\nA Guest with the name of {gName} has already been registered.\nPlease try again later with a unique name.\n");
            break;
        }
        else //Continues if there is no issues with name or passport num
        {
            Stay stay = new Stay(); //Creating empty stay object
            Membership membership = new Membership("Ordinary", 0); //Creating a new membership object with ordinary status and 0 points
            Guest guest = new Guest(gName, gPN, stay, membership); //Creating new guest object
            guestList.Add(guest); //Adding guest to guestList
            string data = gName + "," + gPN + "," + membership.Status + "," + membership.Points; //Adding guest information into data
            using (StreamWriter sw = new StreamWriter("Guests.csv", true))
            {
                sw.WriteLine(data); //Appending data to Guest.csv
            }
            Console.WriteLine($"\nGuest Registration for {gName} is Successful!\n");
            break;
        }
    }
}
List<Guest> DisplayCIn(List<Guest> guestList)
{ // Lee Zhi Wei
    List<Guest> returnlist = new List<Guest>(); // list to return, init
    int count = 0; // count for index + 1
    foreach (Guest g in guestList) // foreach guest
    {
        if (!g.IsCheckedin) // if not checked-in
        {
            count++; // add 1 to count
            Console.WriteLine($"{count}) {g.ToString()}"); // print out index num + 1 and details
            // eg 1) Micheal ...
            returnlist.Add(g); // add guest to returnlist
        }
    }
    return returnlist; // return the list
}
List<Guest> DisplayGuestsCIned(List<Guest> guestList)
{ // Lee Zhi Wei
    List<Guest> alreadycheckedin = new List<Guest>(); // new list for returning
    int count = 0; // init variable for count.
    foreach (Guest g in guestList)
    { // foreach guest object in guestlist
        if (g.IsCheckedin) // if checked-in
        {
            alreadycheckedin.Add(g); // add to list
            count++; // increment count
            Console.WriteLine($"{count}) {g.ToString()}"); // print out number and tostring
        }
    }
    return alreadycheckedin; // return list
}
Room AvailRoomSel()
{ // Lee Zhi Wei
    Room finalobj = null; // final room object to return
    DisplayRmCIn(availrooms); // dont store the list as variable, no use
    Console.Write("Please select a room: "); // prompt to select room
    int choice = -1; // init with invalid value.
    Room chosenr = null; // init the chosen room variable, from list
    try
    { // try convert choice to number
        choice = Convert.ToInt32(Console.ReadLine());
    }
    catch
    { // print error, return null (continues loop)
        Console.WriteLine("Please enter a number.");
        return null;
    }
    try
    { // try to get object from list
        chosenr = availrooms[choice - 1];
    }
    catch
    { // if unable, print error, return null (continue loop)
        Console.WriteLine("You have chosen an invalid option, which is not in list.");
        return null;
    }
    if (chosenr is StandardRoom)
    { // if room is StandardRoom
        StandardRoom r = (StandardRoom)chosenr; // cast to StandardRoom
        Console.Write("Do you want to have choice of WiFi (Y/N): "); // input prompt for wifi
        string ch = Console.ReadLine(); // get choice
        ch = ch.ToUpper(); // upper case it
        if (ch == "Y") // if yes
        {
            r.RequireWifi = true; // set true
        }
        else if (ch == "N") // if no
        {
            r.RequireWifi = false; // set false
        }
        else // if invalid
        { // print error, return null (continue loop)
            Console.WriteLine("You have typed in an invalid option.");
            return null;
        }
        Console.Write("Do you want to have choice of Breakfast (Y/N): "); // prompt for breakfast
        ch = Console.ReadLine(); // reinit ch variable
        ch = ch.ToUpper(); // upper case
        if (ch == "Y") // yes, set true
        {
            r.RequireBreakfast = true;
        }
        else if (ch == "N") // no set false
        {
            r.RequireBreakfast = false;
        }
        else // else invalid, return nulll (continue loop)
        {
            Console.WriteLine("You have typed in an invalid option.");
            return null;
        }
        availrooms.RemoveAt(choice - 1); // remove room from available list
        r.IsAvail = false; // set room object to not avail
        finalobj = r; // put into finalobj for return
    }
    else if (chosenr is DeluxeRoom)
    { // if chosenroom is DeluxeRoom
        DeluxeRoom r = (DeluxeRoom)chosenr; // cast to DeluxeRoom
        Console.Write("Do you want to have additional beds (Y/N): "); // prompt for additional beds
        string ch = Console.ReadLine(); // put choice to variable
        ch = ch.ToUpper(); // upper case it
        if (ch == "Y")
        { // yes, set true
            r.AdditionalBed = true;
        }
        else if (ch == "N")
        { // no, set false
            r.AdditionalBed = false;
        }
        else
        { // invalid, print error return null (continue loop)
            Console.WriteLine("You have typed in an invalid option.");
            return null;
        }
        availrooms.RemoveAt(choice - 1); // remove the room from list
        r.IsAvail = false; // set available to false
        finalobj = r; // put into final object
    }
    return finalobj; // return the final object
}
void DisplayRmCIn(List<Room> rlist)
{ // Lee Zhi Wei
    int count = 0; // init count variable 
    foreach (Room r in rlist)
    { // foreach room object
        count++; // increment count
        Console.WriteLine($"{count}) {r.ToString()}"); // print out count and tostring of room obj
    }
}
void ShowAvailRoom(List<Room> rlist)
{ // Lee Zhi Wei
    foreach (Room r in rlist) // foreach loop, iterate through each room object.
    {
        Console.WriteLine(r.ToString()); // run tostring methods
    }
    Console.WriteLine(); // print \n
}
void CheckIn(List<Guest> glist)
{ // Lee Zhi Wei
    int num = 0; // input choice var, init
    while (true) // input prompt, while loop
    {
        Room finalobj = null; // init the final room object to add in
        Guest pickedguest = null; // init the guest object variable.
        List <Guest> notcheckin = new List<Guest>(DisplayCIn(glist)); // display all the non-checkedin guests.
        DateTime cindate = DateTime.Now; // init datetime vars with now, since cannot put null.
        DateTime coutdate = DateTime.Now;
        Console.Write("Enter the number in which you want to check in: ");// prompt
        try
        { // try to convert the user input to num
            num = Convert.ToInt32(Console.ReadLine());
        }
        catch
        { // print error, continue loop.
            Console.WriteLine("Please enter a number.");
            continue;
        }
        int index = num - 1; // index of the list from input
        if (index < 0) // if -ve index, (cannot use in C#)
        {
            Console.WriteLine("Invalid number."); // print error
            continue; // continue loop
        }
        try
        { // try to get guest object
            pickedguest = notcheckin[index];
        }
        catch
        { // if unable, print error, continue loop
            Console.WriteLine("Out of range of list");
            continue;
        }
        Console.Write("Please enter date of Check-In: "); // check-in prompt
        try
        { // try to turn into datetime
            cindate = Convert.ToDateTime(Console.ReadLine());
        }
        catch
        { // if not, print error, continue loop
            Console.WriteLine("Invalid date format.");
            continue;
        }
        Console.Write("Please enter date of Check-Out: "); // check-out prompt
        try
        { // try turn into datetime
            coutdate = Convert.ToDateTime(Console.ReadLine());
        }
        catch
        { // if not print error, continue loop
            Console.WriteLine("Invalid date format.");
            continue;
        }
        Stay stay = new Stay(cindate, coutdate); // make new stay object
        finalobj = AvailRoomSel(); // call the available room method
        if (finalobj is null) // if failed (object is null)
        {
            continue; // continue loop
        }
        stay.AddRoom(finalobj); // add the room to stay object
        Console.Write("Do you want to add another room? (Y/N): "); // prompt for another
        string o = Console.ReadLine(); // string read from console
        o = o.ToUpper(); // upper case it
        if (o == "Y") // if yes
        {
            finalobj = AvailRoomSel(); // run again
            if (finalobj is null) // if failed
            {
                continue; // continue loop
            }
            stay.AddRoom(finalobj); // add object to stay
        }
        else if (o == "N") // if no
        {
            Console.WriteLine(); // Do nothing continue to next part
        }
        else // else invalid choice
        {
            Console.WriteLine("You have selected invalid option."); // print error 
            continue; // continue loop
        }
        pickedguest.HotelStay = stay; // add in stay object
        pickedguest.IsCheckedin = true; // checked in to true
        Console.WriteLine("You have successfully checked-in!\n"); // print success
        break; // break out of loop, upon success
    }
}
void DisplayInfoguest() //Created by Lim Jia Xian
{   //Assignment Part 5
    DisplayGuestName(guestList);
    string gName;
    bool gFound = false;
    
    while (true)
    {
        Console.Write("\nPlease enter Guest name: ");
        gName = Console.ReadLine();
        if (ValidateNameCheck(gName) == false) //Checks if name entered was filled with numbers.
        {
            Console.WriteLine("\nName should not contain any numbers or special characters.\n");
            continue;
        }
        break;
    }

    foreach (Guest g in guestList)
    {
        if (gName == g.Name)
        {
            gFound= true;
            Console.WriteLine($"\n--- All details of guest {gName} ---\n");
            Console.WriteLine($"Name: {g.Name}, Passport number: {g.PassportNum}\n");
            if (g.HotelStay.CheckInDate == Convert.ToDateTime("1/1/0001 12:00:00 am") || g.HotelStay.CheckOutDate == Convert.ToDateTime("1/1/0001 12:00:00 am"))
            {
                Console.WriteLine("Guest has no stay information.");
            }
            else
            {
                Console.WriteLine($"{g.HotelStay}");
                Console.WriteLine($"\nCheck in status: {g.IsCheckedin}");
            }
            break;
        }
        else
        {
            gFound= false;
        }
    }
    if (gFound == false)
    {
        Console.WriteLine($"\nName of Guest {gName} does not exist.\n");
    }
    Console.WriteLine();
}
void ExtendStay()
{ // Lee Zhi Wei
    List<Guest> checkedin = DisplayGuestsCIned(guestList); // get how many people checked-in
    Guest pickedg = null; // picked-guest variable to null
    int choice = -1; // set choice 
    Console.Write("Please enter which guest to extend stay: "); // prompt
    try
    { // try convert to int
        choice = Convert.ToInt32(Console.ReadLine());
    }
    catch
    { // if cannot, print error, return nothing, end method
        Console.WriteLine("Please enter a number.");
        return;
    }
    try
    {
        pickedg = checkedin[choice - 1]; // try to get picked-guest into object
    }
    catch
    { // if unable, print error, return method.
        Console.WriteLine("Please select a selection from the list");
        return;
    }
    if (pickedg.IsCheckedin == false)
    { // if not checked in, print error, return fn.
        Console.WriteLine("You are not checked in yet.");
        return;
    }
    Stay s = pickedg.HotelStay; // store the stay from guest into temp var
    Console.Write("How many days do you want to extend stay by? "); // prompt for how many days to extend
    int days = 0; // int var
    try
    { // try convert input to int
        days = Convert.ToInt32(Console.ReadLine());
    }
    catch
    { // if unable, print error
        Console.WriteLine("Please enter a number.");
        return;
    }
    if (days <= 0)
    { // if days less than zero, invalid, print error, return fn
        Console.WriteLine("Days cannot be zero or negative, please try again.");
        return;
    }
    s.CheckOutDate = s.CheckOutDate.AddDays(days); // add days to check-out date
    guestList[choice - 1] = pickedg; // put back manipulated object
    Console.WriteLine($"Date has been updated to {s.CheckOutDate.ToString()}"); // print success msg
    return; // end the function
}

void CheckOutGuest() //Created by Lim Jia Xian
{
    DisplayGuestName(guestList);
    int gPoints;
    double fBill;
    string gName;
    bool gFound = false;

    while (true)
    {
        Console.Write("\nPlease enter Guest name: ");
        gName = Console.ReadLine();
        if (ValidateNameCheck(gName) == false) //Checks if name entered was filled with numbers.
        {
            Console.WriteLine("\nName should not contain any numbers or special characters.\n");
            continue;
        }
        break;
    }

    foreach (Guest g in guestList)
    {
        if (gName == g.Name && g.IsCheckedin == false)
        {
            Console.WriteLine($"\nUnable to check out!\nGuest of the name {gName} is not checked in yet.\n");
            gFound = true; //Guest found is considered to be true, just that the guest is not able to check out at this moment.
            break;
        }
        else
        {
            if (gName == g.Name)
            {
                gFound = true;
                Console.WriteLine($"\n--- All details of guest {gName} ---\n");
                Console.WriteLine($"Name: {g.Name}, Passport number: {g.PassportNum}\n");
                Console.WriteLine($"{g.HotelStay}");
                Console.WriteLine($"\nTotal bill amount: ${g.HotelStay.CalculateTotal()}");
                Console.WriteLine($"\nMembership Status: ${g.Member.ToString()}");
                if (g.Member.Status == "Sliver" || g.Member.Status == "Gold")
                {
                    Console.WriteLine("You are elligible to redeem points!");
                    Console.Write("Enter the amount of points to offset the total bill amount: ");
                    gPoints = Convert.ToInt32(Console.ReadLine());
                    g.Member.RedeemPoints(gPoints);
                    fBill = g.HotelStay.CalculateTotal() - gPoints;
                    Console.WriteLine($"Final bill amount: ${fBill}");
                    g.Member.EarnPoints(fBill);
                    if ((fBill / 10) > 0)
                    {
                        double ePoints = fBill / 10;
                        Console.WriteLine($"Earned points: {ePoints}");
                    }
                    else
                    {
                        double ePoints = 0;
                        Console.WriteLine($"Earned points:{ePoints}");
                    }
                    Console.WriteLine($"\nCurrent membership points: {g.Member.Points}");
                    if (g.Member.Points < 200)
                    {
                        if (g.Member.Points > 100 && g.Member.Points < 200)
                        {
                            Console.WriteLine($"Membership status promoted to Sliver!");
                        }
                        else if (g.Member.Points > 100 && g.Member.Points >= 200)
                        {
                            Console.WriteLine($"Membership status promoted to Gold!");
                        }
                    }
                    g.IsCheckedin = false;
                }
                else
                {
                    Console.WriteLine($"Final bill amount: ${g.HotelStay.CalculateTotal()}");
                    fBill = g.HotelStay.CalculateTotal();
                    g.Member.EarnPoints(fBill);
                    if ((fBill / 10) > 0)
                    {
                        double ePoints = fBill / 10;
                        Console.WriteLine($"Earned points: {ePoints}");
                    }
                    else
                    {
                        double ePoints = 0;
                        Console.WriteLine($"Earned points:{ePoints}");
                    }
                    Console.WriteLine($"\nCurrent membership points: {g.Member.Points}");
                    if (g.Member.Points < 200)
                    {
                        if (g.Member.Points > 100 && g.Member.Points < 200)
                        {
                            Console.WriteLine($"Membership status promoted to Sliver!");
                        }
                        else if (g.Member.Points > 100 && g.Member.Points >= 200)
                        {
                            Console.WriteLine($"Membership status promoted to Gold!");
                        }
                    }
                    g.IsCheckedin = false;
                }
                break;
            }
            else
            {
                gFound = false;
            }
        }
    }
    if (gFound == false)
    {
        Console.WriteLine($"\nName of Guest {gName} does not exist.\n");
    }
    Console.WriteLine();
}


int entOpt;
InitData();
while (true)
{
    try
    {
        Console.WriteLine("------ Hotel Guest Management System ------");
        Console.WriteLine("[1]. Display all guests\n[2]. Display all available rooms\n[3]. Register guest\n[4]. Check-in guest\n[5]. Check-out guest\n[6]. Display all details for guest\n[7]. Extend days for stay\n[8]. Display monthly & Yearly charged amounts\n[0]. Quit Hotal Guest Management System");
        Console.Write("-------------------------------------------\nPlease enter your option: ");
        entOpt = Convert.ToInt32(Console.ReadLine());
        if (entOpt == 0)
        {
            Console.WriteLine("\n-- Quiting Hotel Management Application --\n");
            break;
        }
        else if (entOpt == 1)
        {
            Console.WriteLine("\n------ Displaying all guests ------\n");
            DisplayGuest(guestList);
        }
        else if (entOpt == 2)
        {
            Console.WriteLine("\n------ Displaying all available rooms ------\n");
            ShowAvailRoom(availrooms);
        }
        else if (entOpt == 3)
        {
            Console.WriteLine("\n------------- Register Guest --------------\n");
            RegisterGuest();
        }
        else if (entOpt == 4)
        {
            Console.WriteLine("\n------ Guest Check-IN ------\n");
            CheckIn(guestList);
        }
        else if (entOpt == 5)
        {
            Console.WriteLine("\n------ Guest Check-OUT ------\n");
            CheckOutGuest();
        }
        else if (entOpt == 6)
        {
            Console.WriteLine("\n--- Displaying name of guests ---\n");
            DisplayInfoguest();
        }
        else if (entOpt == 7)
        {
            Console.WriteLine("\n------ Extending days for stay ------\n");
            ExtendStay();
        }
        else if (entOpt == 8)
        {
            Console.WriteLine("\n---- Display monthly & Yearly charged amounts ----\n");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("\nPlease enter a numeric value from 0 - 8\n");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"\nIncorrect values! {ex.Message} Please try again with numeric values from 0 - 8\n");
    }
}





