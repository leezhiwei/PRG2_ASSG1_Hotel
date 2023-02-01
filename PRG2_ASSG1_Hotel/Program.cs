// Zhi Wei does 2,4,6, Advanced A
// Jia Xian does 1,3,5, Advanced B
using PRG2_ASSG1_Hotel;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

List<Guest> guestList = new List<Guest>();
List<Room> availRooms = new List<Room>();
List<Room> occupiedrooms = new List<Room>();
List<Food> availableFoodOption = new List<Food>();
//Initialise Data from CSV
void InitData() 
{
    List<List<string>> roomlist = new List<List<string>>();
    List<List<string>> guestlist = new List<List<string>>(); // init string-list var
    List<List<string>> stayslist = new List<List<string>>();
    //Reading data from CSV files
    string[] roomdata = File.ReadAllLines("Rooms.csv");
    string[] guestdata = File.ReadAllLines("Guests.csv"); // readalllines of CSV
    string[] staysdata = File.ReadAllLines("Stays.csv");
    for (int i = 1; i < roomdata.Length; i++)
    {
        List<string> values = roomdata[i].Split(',').ToList();
        //Adding data into list
        roomlist.Add(values);
    }
    for (int j = 1; j < guestdata.Length; j++)
    {
        List<string> values = guestdata[j].Split(',').ToList();
        //Adding data into list
        guestlist.Add(values);
    }
    for (int k = 1; k < staysdata.Length; k++)
    {
        List<string> values = staysdata[k].Split(',').ToList();
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
                                // make standardroom obj, set bools
                                room.RequireWifi = Convert.ToBoolean(bookings[10]);
                                room.RequireBreakfast = Convert.ToBoolean(bookings[11]);
                                secroom = room; // set to secroom var

                            }
                            else if (rooms[0] == "Deluxe") // if type is deluxe
                            {
                                DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(r[1]), r[2], Convert.ToDouble(r[3]), !(Convert.ToBoolean(bookings[2])));
                                // make deluxeroom obj, set bools
                                room.AdditionalBed = Convert.ToBoolean(bookings[12]);
                                secroom = room; // set to secroom var
                            }
                        }
                    }
                }
                if (rooms[0] == "Standard") // if type is standard (for 1)
                {
                    StandardRoom room = new StandardRoom(Convert.ToInt32(rooms[1]), rooms[2], Convert.ToDouble(rooms[3]), !(Convert.ToBoolean(bookings[2])));
                    // make standardroom, obj, set bools
                    room.RequireWifi = Convert.ToBoolean(bookings[6]);
                    room.RequireBreakfast = Convert.ToBoolean(bookings[7]);
                    book.AddRoom(room); // add to stays
                    occupiedrooms.Add(room); // add to occupiedrooms

                }
                else if (rooms[0] == "Deluxe") // if type is deluxe (for 1)
                {
                    DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(rooms[1]), rooms[2], Convert.ToDouble(rooms[3]), !(Convert.ToBoolean(bookings[2])));
                    // make deluxeroom obj, set bools
                    room.AdditionalBed = Convert.ToBoolean(bookings[12]);
                    book.AddRoom(room); // add to stays
                    occupiedrooms.Add(room); // add to occupiedrooms
                }
                if (secroom is not null)
                { // if secroom not empty
                    book.AddRoom(secroom); // add stays
                    occupiedrooms.Add(secroom); // add occupiedrooms
                }
                foreach (List<string> guestprof in guestlist) // for guest profile in list
                {
                    if (guestprof[1] == bookings[1]) // matches
                    {
                        Membership m = new Membership(guestprof[2], Convert.ToInt32(guestprof[3]));
                        // new membership
                        Guest guestobj = new Guest(guestprof[0], guestprof[1], book, m);
                        // new guest
                        guestobj.IsCheckedin = Convert.ToBoolean(bookings[2]); // set bool
                        guestList.Add(guestobj); // add to guestlist
                    }
                }
            }
        }
    }
    availRooms = AvailRoom(); // get available room list
    foreach (List<string> ro in roomlist) // add in other avail rooms
    {
        bool isInList = false;
        bool occupied = false; // set booleans
        foreach (Room room in availRooms)
        {
            if (room.RoomNumber == Convert.ToInt32(ro[1])) // if already in
            {
                isInList = true; // set bool
                break; // break
            }
        }
        if (isInList) // if already in list
        {
            continue; // continue list
        }
        foreach (Room or in occupiedrooms)
        { // for room in occupiedrooms
            if (or.RoomNumber == Convert.ToInt32(ro[1])) // matches with loop
            {
                occupied = true; // occupied flag true
                break; // break loop
            }
        }
        if (occupied) // if occupied
        {
            continue; // continue loop
        }
        if (ro[0] == "Standard") // if type is standard
        { // else not occupied or already in list
            StandardRoom room = new StandardRoom(Convert.ToInt32(ro[1]), ro[2], Convert.ToDouble(ro[3]), true);
            availRooms.Add(room); //create and add

        }
        else if (ro[0] == "Deluxe") // if type is deluxe
        { // else not occupied or already in list
            DeluxeRoom room = new DeluxeRoom(Convert.ToInt32(ro[1]), ro[2], Convert.ToDouble(ro[3]), true);
            availRooms.Add(room); // create and add
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
 {  //Assignment Part 1, Displaying All Guests
    foreach (Guest g in guestList)
    {
        Console.WriteLine("Name: {0,-10} | Passport number: {1,-10} | Membership status: {2,-8} | Membership points: {3,-10}",g.Name,g.PassportNum,g.Member.Status,g.Member.Points);
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
Guest SearchG(List<Guest> guestList, string gName) //Created by Lim Jia Xian
{   //Searches for the name if it matches the records in guest list
    foreach (Guest g in guestList)
    {
        if (g.Name.ToUpper() == gName.ToUpper()) //If guest name is found
        {
            return g; //Return Guest Object
        }
    }
    return null;
}
Guest SearchPN(List<Guest> guestList, string gPN) //Created by Lim Jia Xian
{   //Searches for the passport number if it matches the records in guest list
    foreach (Guest g in guestList)
    {
        if (g.PassportNum.ToUpper() == gPN.ToUpper()) //If guest passport number is found
        {
            return g; //Return Guest Object
        }
    }
    return null;
}
bool ValidateNameCheck(string entName) //Created by Lim Jia Xian
{   //Function to check if name contains numbers or special characters
    Regex regex = new Regex(@"^[a-zA-z]+$");
    return regex.IsMatch(entName); //Returns false if name contains numbers or special characters, Returns true if name is within the range from A - Z
}
bool ValidatePassportNumCheck(string entPosNum) //Created by Lim Jia Xian
{   //Function to check if passport number is valid, containing 2 alphabets and 7 numbers tallying up to 9 characters in length.
    Regex regex = new Regex(@"^[A-Z0-9A-Z]{9}$");
    return regex.IsMatch(entPosNum); //Returns false if passport number is invalid, Returns true if passport number matches the proper values
}
void RegisterGuest() //Created by Lim Jia Xian
{   //Assignment Part 3, Registering Guest
    while (true)
    {
        string gName = "";
        string gPN = "";
        bool gDup = false;
        bool gPDup = false;
        Console.WriteLine("Please enter the following information\n");
        Console.Write("Guest name: ");
        gName = Console.ReadLine();

        if (ValidateNameCheck(gName) == false) //Checks if name entered was filled with numbers or special characters.
        {
            Console.WriteLine("\nName should not contain any numbers or special characters.\n");
            continue;
        }
        else if (gName.Length < 3 || gName.Trim().Length < 3) //If the name entered is empty or less than 3 characters or name filled with spaces
        {
            Console.WriteLine("\nThe guest name should not be empty or less than 3 characters in length. Please try again!\n");
            continue;
        }
        else //Continues if guest name was valid
        {
            Guest guestN = SearchG(guestList, gName); //Searches the guest name
            if (guestN != null) //Checks if the name is exactly the same with registered guest names
            {
                gDup = true; //Guest name duplicate changes to true
            }
        }
        
        Console.Write("Guest passport number: ");
        gPN = Console.ReadLine();

        if (ValidatePassportNumCheck(gPN) == false) //Checks if passport number is invalid and does not follow the format of 2 letters and 7 digits
        {
            Console.WriteLine("\nPassport number should start & end with upper-case letters with 7 numeric digits in between with a total of 9 characters in length. \nPlease try again.\n");
            continue;
        }
        else //Continues if passport number is valid
        {
            Guest guestPN = SearchPN(guestList, gPN); //Searches the guest passport number
            if (guestPN != null) //If passport number matches has been found
            {
                gPDup = true; //Guest passport num duplicate changes to true
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
            Guest guest = new Guest(gName, gPN.ToUpper(), stay, membership); //Creating new guest object
            guestList.Add(guest); //Adding guest to guestList
            string data = gName + "," + gPN.ToUpper() + "," + membership.Status + "," + membership.Points; //Adding guest information into data
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
    DisplayRmCIn(availRooms); // dont store the list as variable, no use
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
        chosenr = availRooms[choice - 1];
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
        availRooms.RemoveAt(choice - 1); // remove room from available list
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
        availRooms.RemoveAt(choice - 1); // remove the room from list
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
        if (cindate > coutdate)
        {// check if checkout date before checkin
            Console.WriteLine("Check-In date must be before check-out.");
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
{   //Assignment Part 5, Display all details of Guest
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

    Guest guestN = SearchG(guestList, gName); //Searches the guest name
    if (guestN != null) //Checks if the name matches any records in guest list, if so it will not return a null value. 
    {
        gFound = true; //Guest name duplicate changes to true
        Console.WriteLine($"\n--- All details of guest {guestN.Name} ---\n");
        Console.WriteLine($"Name: {guestN.Name}, Passport number: {guestN.PassportNum}, Membership {guestN.Member.ToString()}\n");
        if (guestN.HotelStay.CheckInDate == Convert.ToDateTime("1/1/0001 12:00:00 am") || guestN.HotelStay.CheckOutDate == Convert.ToDateTime("1/1/0001 12:00:00 am")) //Checks if check-in date and check-out date is invalid
        {
            Console.WriteLine("Guest has no stay information.");
        }
        else //If there are valid check-in & check-out dates, display hotel stay information with check in status
        {
            Console.WriteLine($"{guestN.HotelStay}");
            Console.WriteLine($"\nCheck in status: {guestN.IsCheckedin}");
        }
    }

    if (gFound == false) //If Guest found is false, display guest does not exist
    {
        Console.WriteLine($"\nName of Guest {gName} does not exist.\n");
    }
    Console.WriteLine();
}
void ExtendStay()
{ // Lee Zhi Wei
    List<Guest> checkedin = DisplayGuestsCIned(guestList); // get how many people checked-in
    Guest pickedg = null; // picked-guest variable to null
    int realindex = 0; // real index from real guestlist
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
    realindex = guestList.IndexOf(pickedg); // get real index from guestList
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
    guestList[realindex] = pickedg; // put back manipulated object
    Console.WriteLine($"Date has been updated to {s.CheckOutDate.ToString()}"); // print success msg
    return; // end the function
}
void CheckOutGuest() //Created by Lim Jia Xian
{   //Advanced Feature B, Check-Out Guest
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

    Guest guestN = SearchG(guestList, gName); //Searches the guest name
    if (guestN != null) //Checks if the name is exactly the same with registered guest names, if so it will not return a null value
    {
        gFound = true; //Guest name duplicate changes to true

        if (guestN.IsCheckedin == false) //Checks guest's check in status. If guest name found is not checked in, display the message
        {
            Console.WriteLine($"\nUnable to check out!\nGuest of the name {guestN.Name} is not checked in yet.\n");
            gFound = true; //Guest found is considered to be true, just that the guest is not able to check out at this moment.
        }
        else if (guestN.IsCheckedin == true)
        {
            gFound = true;
            Console.WriteLine($"\n--- All details of guest {guestN.Name} ---\n");
            Console.WriteLine($"Name: {guestN.Name}, Passport number: {guestN.PassportNum}\n");
            Console.WriteLine($"{guestN.HotelStay}"); //Displaying hotel stay information
            Console.WriteLine($"\nTotal bill amount: ${guestN.HotelStay.CalculateTotal()}");
            Console.WriteLine($"\nMembership {guestN.Member.ToString()}");
            if (guestN.Member.Status == "Silver" || guestN.Member.Status == "Gold") //If member status is silver or gold
            {
                Console.WriteLine("You are elligible to redeem points!");
                Console.Write("Enter the amount of points to offset the total bill amount: ");
                gPoints = Convert.ToInt32(Console.ReadLine());
                guestN.Member.RedeemPoints(gPoints);
                fBill = guestN.HotelStay.CalculateTotal() - gPoints; //Final bill is calculated by total - guest redeem points
                Console.WriteLine($"Final bill amount: ${fBill}");
                Console.WriteLine("\n----- Press ENTER to make payment -----\n");
                Console.ReadLine();
                guestN.Member.EarnPoints(fBill); //Earning points based off the final bill
                if ((fBill / 10) >= 1) //Checks if final bill / 10 returns a value more than 0
                {
                    double ePoints = fBill / 10; //Points earned = final bill / 10
                    Console.WriteLine($"Earned points: {ePoints}"); //Displaying points earned
                    if (guestN.Member.Status == "Silver") //Checks if member status is silver
                    {
                        if (guestN.Member.Points >= 200) //Checks if member points is 200 or more
                        {
                            guestN.Member.Status = "Gold"; //Promotes member status
                            Console.WriteLine($"Membership status has been promoted to {guestN.Member.Status}!");
                        }
                    }
                }
                else // if fbill / 10 returns a value lesser than 1, points earned = 0
                {
                    double ePoints = 0; //Earned points is zero
                    Console.WriteLine($"Earned points: {ePoints}");
                }
                Console.WriteLine($"\nCurrent membership points: {guestN.Member.Points}");
                guestN.IsCheckedin = false; //Update guest check in status to false
                Console.WriteLine($"\n--- Guest name {guestN.Name} Check-Out successful ---\n");
            }
            else
            {
                Console.WriteLine($"Final bill amount: ${guestN.HotelStay.CalculateTotal()}");
                Console.WriteLine("\n------ Press ENTER to make payment ------");
                Console.ReadLine();
                fBill = guestN.HotelStay.CalculateTotal(); //Calculating final bill
                guestN.Member.EarnPoints(fBill); //Earning points based off final bill
                if ((fBill / 10) >= 1) //Checks if final bill / 10 returns a value more than or equals to 1
                {
                    double ePoints = fBill / 10; //Points earned will be based off final bill / 10
                    Console.WriteLine($"Earned points: {ePoints}"); //Displaying points earned
                    if (guestN.Member.Status == "Silver") //Checks if member status is silver
                    {
                        if (guestN.Member.Points >= 200) //Checks if member points is 200 or more
                        {
                            guestN.Member.Status = "Gold"; //Promotes member status
                            Console.WriteLine($"Membership status has been promoted to {guestN.Member.Status}!");
                        }
                    }
                    else if (guestN.Member.Status == "Ordinary") //Checks if member status is ordinary
                    {
                        if (guestN.Member.Points >= 200) //Checks if member points is 200 or more
                        {
                            guestN.Member.Status = "Gold"; //Promotes member status
                            Console.WriteLine($"Membership status has been promoted to {guestN.Member.Status}!");
                        }
                        else if (guestN.Member.Points >= 100 && guestN.Member.Points < 200) //Checks if member points is 100 or more but less than 200
                        {
                            guestN.Member.Status = "Silver"; //Promotes member status
                            Console.WriteLine($"Membership status has been promoted to {guestN.Member.Status}!");
                        }
                    }
                }
                else // if fbill / 10 returns a value lesser than 1, points earned = 0
                {
                    double ePoints = 0; //Earned points is zero
                    Console.WriteLine($"Earned points: {ePoints}");
                }
                Console.WriteLine($"\nCurrent membership points: {guestN.Member.Points}");
                guestN.IsCheckedin = false; //Update guest check in status to false
                Console.WriteLine($"\n--- Guest name {guestN.Name} Check-Out successful ---\n");
            }
        }
    }

    if (gFound == false) //Guest is not found, displaying guest name does not exist
    {
        Console.WriteLine($"\nName of Guest {gName} does not exist.\n");
    }
    Console.WriteLine();
}
void MonthlyCharges()
{   //Advanced Feature A, Lee Zhi Wei
    List<string> months = new List<String>{"January", "February", "March", "April", "May",
    "June", "July", "August", "September", "October", "November", "December"}; // months list
    while (true)
    { // input loop
        double yearlytotal = 0; // init vars
        int year = 0;
        Console.Write("Enter the year: "); // enter year
        try
        { // if cannot convert to int
            year = Convert.ToInt32(Console.ReadLine());
        }
        catch
        { // error message, continue
            Console.WriteLine("Invalid year, you have typed in an unknown input, please input a number.");
            continue;
        }
        if (year <= 1000 || year >= 9999)
        { // if year less than 1000 and more than 999 (invalid years)
            Console.WriteLine("Invalid Year. Please try again.");  // error
            continue; // continue
        }
        
        bool leapyear = false; // boolean init
        if (year % 4  == 0)
        { // if leapyear set bool
            leapyear = true;
        }
        List<int> thirtyfirstmths = new List<int> { 1, 3, 5, 7, 8, 10, 12 }; // mths with 31 days
        for (int mth = 1; mth <= 12; mth++)
        { // for 12 months in year
            int endDay = 30; // default end day 30
            double totalcharge = 0; // totalcharge var init
            if (mth == 2)
            { // if feb
                if (leapyear)
                { // if leap year, 29
                    endDay = 29;
                }
                else
                { // else 28
                    endDay = 28;
                }
            }
            else if (thirtyfirstmths.Contains(mth))
            { // if in the list of 31 days year, set endday to 31
                endDay = 31;
            }
            DateTime mthStart = Convert.ToDateTime($"1/{mth}/{year}"); // first day of month
            DateTime mthEnd = Convert.ToDateTime($"{endDay}/{mth}/{year}"); // last day of month
            foreach (Guest g in guestList)
            { // foreach guest
                Stay s = g.HotelStay; // get stay object
                if (s.CheckOutDate <= mthEnd && s.CheckOutDate >= mthStart)
                { // if within period of month
                    totalcharge += s.CalculateTotal(); // add to total
                }
            }
            Console.WriteLine($"{months[mthEnd.Month - 1]} {year}: ${totalcharge:F2}");
            // write month year, eg Jan 2023
            yearlytotal += totalcharge; // add to yearly charge, eg total for year
        }
        Console.WriteLine($"Total: ${yearlytotal:F2}"); // print total for year
        break; // break out
    }
}
List<Food> InitFoodList()
{ // Lee Zhi Wei
    List<Food> foodList = new List<Food>(); // list of food
    Food f1 = new Food("Beef Wellington", 50);
    Food f2 = new Food("Carbonara Pasta", 25);
    Food f3 = new Food("Tomato Pasta (Vegeterian)", 20); // add in dishes
    Food f4 = new Food("Cheese-Baked Rice (Vegeterian)", 11);
    Food f5 = new Food("Chicken Rice", 15);
    foodList.Add(f1);
    foodList.Add(f2);
    foodList.Add(f3); // add into return list
    foodList.Add(f4);
    foodList.Add(f5);
    return foodList; // return list
}
void GuestOrderFood()
{   //Advanced Feature C, created by both students
    DisplayGuestName(guestList);
    string gName;
    bool gFound = false; // init variables
    int fdChoice;

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
        if (gName.ToUpper() == g.Name.ToUpper()) // upper case
        {
            gFound = true; // find the guest
            if (!g.IsCheckedin) // if not checked in
            {
                Console.WriteLine("Guest not checked in."); // error return
                return;
            }
            Console.Write("\n----------- Displaying Food Options ------------\n[1]. Chicken Rice, Price: $15\n[2]. Beef Wellington, Price: $50\n[3]. Carbonara Pasta, Price: $25\n[4]. Tomato Pasta (Vegetarian), Price: $20\n[5]. Cheese-Baked Rice (Vegetarian), Price: $11\n------------------------------------------------\nPlease enter your Food Choice: ");
            // food options
            try
            {
                fdChoice = Convert.ToInt32(Console.ReadLine()); // readline
            }
            catch
            {
                Console.WriteLine("Invalid input. Please try again."); // error
                break; // break the loop
            }
            if (fdChoice == 1) // chicken rice
            {
                int loop = 0; // how many qty, eg how much loop
                Console.WriteLine("Selected Chicken Rice"); // selected food
                Console.Write("Enter quantity: "); // quantity
                try
                { // try convert to number
                    loop = Convert.ToInt32(Console.ReadLine());
                }
                catch
                { // print error, break
                    Console.WriteLine("Input a number. You have entered other characters.");
                    break;
                }
                if (loop < 1)
                { // if qty less than 1, error break
                    Console.WriteLine("Input a valid number more than 1");
                    break;
                }
                if (loop == 1)
                { // if only 1
                    g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[4]); // add food
                    Console.WriteLine("Completed successfully."); // completed message
                }
                else
                {
                    for (int x = 0; x < loop; x++) // for loop
                    { // looping to add food object
                        g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[4]);
                    } // success message
                    Console.WriteLine("Completed successfully.");
                    break; // break
                }
            }
            else if (fdChoice == 2) // beef wellington
            {
                int loop = 0; // how many qty, eg how much loop
                Console.WriteLine("Selected Beef Wellington"); // selected food
                Console.Write("Enter quantity: "); // quantity
                try
                { // try convert to number
                    loop = Convert.ToInt32(Console.ReadLine());
                }
                catch
                { // print error, break
                    Console.WriteLine("Input a number. You have entered other characters.");
                    break;
                }
                if (loop < 1)
                { // if qty less than 1, error break
                    Console.WriteLine("Input a valid number more than 1");
                    break;
                }
                if (loop == 1)
                { // if only 1
                    g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[0]); // add food
                    Console.WriteLine("Completed successfully."); // completed message
                }
                else
                {
                    for (int x = 0; x < loop; x++) // for loop
                    { // looping to add food object
                        g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[0]);
                    } // success message
                    Console.WriteLine("Completed successfully.");
                    break; // break
                }
            }
            else if (fdChoice == 3) // carbonara
            {
                int loop = 0; // how many qty, eg how much loop
                Console.WriteLine("Selected Carbonara Pasta"); // selected food
                Console.Write("Enter quantity: "); // quantity
                try
                { // try convert to number
                    loop = Convert.ToInt32(Console.ReadLine());
                }
                catch
                { // print error, break
                    Console.WriteLine("Input a number. You have entered other characters.");
                    break;
                }
                if (loop < 1)
                { // if qty less than 1, error break
                    Console.WriteLine("Input a valid number more than 1");
                    break;
                }
                if (loop == 1)
                { // if only 1
                    g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[1]); // add food
                    Console.WriteLine("Completed successfully."); // completed message
                }
                else
                {
                    for (int x = 0; x < loop; x++) // for loop
                    { // looping to add food object
                        g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[1]);
                    } // success message
                    Console.WriteLine("Completed successfully.");
                    break; // break
                }
            }
            else if (fdChoice == 4) // tomato pasta
            {
                Console.WriteLine("Selected Tomato Pasta (Vegetarian)");
                int loop = 0; // how many qty, eg how much loop
                Console.WriteLine("Selected Carbonara Pasta"); // selected food
                Console.Write("Enter quantity: "); // quantity
                try
                { // try convert to number
                    loop = Convert.ToInt32(Console.ReadLine());
                }
                catch
                { // print error, break
                    Console.WriteLine("Input a number. You have entered other characters.");
                    break;
                }
                if (loop < 1)
                { // if qty less than 1, error break
                    Console.WriteLine("Input a valid number more than 1");
                    break;
                }
                if (loop == 1)
                { // if only 1
                    g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[2]); // add food
                    Console.WriteLine("Completed successfully."); // completed message
                }
                else
                {
                    for (int x = 0; x < loop; x++) // for loop
                    { // looping to add food object
                        g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[2]);
                    } // success message
                    Console.WriteLine("Completed successfully.");
                    break; // break
                }
            }
            else if (fdChoice == 5) // cheese baked rice
            {
                int loop = 0; // how many qty, eg how much loop
                Console.WriteLine("Selected Cheese-Baked Rice (Vegetarian)"); // selected food
                Console.Write("Enter quantity: "); // quantity
                try
                { // try convert to number
                    loop = Convert.ToInt32(Console.ReadLine());
                }
                catch
                { // print error, break
                    Console.WriteLine("Input a number. You have entered other characters.");
                    break;
                }
                if (loop < 1)
                { // if qty less than 1, error break
                    Console.WriteLine("Input a valid number more than 1");
                    break;
                }
                if (loop == 1)
                { // if only 1
                    g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[3]); // add food
                    Console.WriteLine("Completed successfully."); // completed message
                }
                else
                {
                    for (int x = 0; x < loop; x++) // for loop
                    { // looping to add food object
                        g.HotelStay.RoomServiceForStay.AddFood(availableFoodOption[3]);
                    } // success message
                    Console.WriteLine("Completed successfully.");
                    break; // break
                }
            }
            else // if above 5
            { // error
                Console.WriteLine("Please enter a numeric choice from 1 - 5 only");
                continue; // continue
            }
        }
    }

    if (gFound == false) // if no guest
    {
        Console.WriteLine($"\nName of Guest {gName} does not exist.\n"); // error
    }
}
void ShowRoomServiceObj()
{ // Lee Zhi Wei
    int choice = 0; // init choice
    Guest g = null; // init guest
    List<Guest> checkedin = DisplayGuestsCIned(guestList); // display checked in guest, put into list for select
    Console.Write("Select guest to display: "); // input
    try
    { // convert to int
        choice = Convert.ToInt32(Console.ReadLine());
    }
    catch
    { // if cannot print error, return
        Console.WriteLine("Please enter a number.");
        return;
    }
    try
    { // try get object
        g = checkedin[choice - 1]; 
    }
    catch
    { // if not print error
        Console.WriteLine("Out of range of list.");
        return;
    }
    Console.WriteLine(g.HotelStay.RoomServiceForStay.ToString()); // print room service list
    Console.WriteLine($"Total charge for RoomService: ${g.HotelStay.RoomServiceForStay.CalculateFoodCharges():F2}");
    // print total price
}
void MonthlyChargesWithRoomService()
{   //Advanced Feature A, Lee Zhi Wei
    List<string> months = new List<String>{"January", "February", "March", "April", "May",
    "June", "July", "August", "September", "October", "November", "December"}; // months list
    while (true)
    { // input loop
        double yearlytotal = 0; // init vars
        int year = 0;
        Console.Write("Enter the year: "); // enter year
        try
        { // if cannot convert to int
            year = Convert.ToInt32(Console.ReadLine());
        }
        catch
        { // error message, continue
            Console.WriteLine("Invalid year, you have typed in an unknown input, please input a number.");
            continue;
        }
        if (year <= 1000 || year >= 9999)
        { // if year less than 1000 and more than 999 (invalid years)
            Console.WriteLine("Invalid Year. Please try again.");  // error
            continue; // continue
        }

        bool leapyear = false; // boolean init
        if (year % 4 == 0)
        { // if leapyear set bool
            leapyear = true;
        }
        List<int> thirtyfirstmths = new List<int> { 1, 3, 5, 7, 8, 10, 12 }; // mths with 31 days
        for (int mth = 1; mth <= 12; mth++)
        { // for 12 months in year
            int endDay = 30; // default end day 30
            double totalcharge = 0; // totalcharge var init
            if (mth == 2)
            { // if feb
                if (leapyear)
                { // if leap year, 29
                    endDay = 29;
                }
                else
                { // else 28
                    endDay = 28;
                }
            }
            else if (thirtyfirstmths.Contains(mth))
            { // if in the list of 31 days year, set endday to 31
                endDay = 31;
            }
            DateTime mthStart = Convert.ToDateTime($"1/{mth}/{year}"); // first day of month
            DateTime mthEnd = Convert.ToDateTime($"{endDay}/{mth}/{year}"); // last day of month
            foreach (Guest g in guestList)
            { // foreach guest
                Stay s = g.HotelStay; // get stay object
                if (s.CheckOutDate <= mthEnd && s.CheckOutDate >= mthStart)
                { // if within period of month
                    totalcharge += s.CalculateTotalWithRoomService(); // add to total (with room charges)
                }
            }
            Console.WriteLine($"{months[mthEnd.Month - 1]} {year}: ${totalcharge:F2}");
            // write month year, eg Jan 2023
            yearlytotal += totalcharge; // add to yearly charge, eg total for year
        }
        Console.WriteLine($"Total: ${yearlytotal:F2}"); // print total for year
        break; // break out
    }
}

int entOpt;
InitData();
availableFoodOption = InitFoodList();
while (true)
{
    try
    {
        Console.WriteLine("------ Hotel Guest Management System ------");
        Console.WriteLine("[1].  Display all Guests\n[2].  Display all available rooms\n[3].  Register Guest\n[4].  Check-In Guest\n[5].  Display all details for guest\n[6].  Extend days for stay\n[7].  Display Monthly & Yearly charged amounts\n[8].  Check-Out Guest\n[9].  Room Service for Guests\n[10]. Display the RoomService Object\n[11]. Display Monthly & Yearly charged amounts (With RoomService)\n[0].  Quit Hotal Guest Management System");
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
            ShowAvailRoom(availRooms);
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
            Console.WriteLine("\n--- Displaying name of Guests ---\n");
            DisplayInfoguest();
        }
        else if (entOpt == 6)
        {
            Console.WriteLine("\n------ Extending days for stay ------\n");
            ExtendStay();
        }
        else if (entOpt == 7)
        {
            Console.WriteLine("\n---- Display monthly & Yearly charged amounts ----\n");
            MonthlyCharges();
        }
        else if (entOpt == 8)
        {
            Console.WriteLine("\n------ Guest Check-OUT ------\n");
            CheckOutGuest();
        }
        else if (entOpt == 9)
        {
            Console.WriteLine("\n--- Displaying name of Guests ---\n");
            GuestOrderFood();
        }
        else if (entOpt == 10)
        {
            Console.WriteLine("\n--- Display RoomService Object of Guest ----\n");
            ShowRoomServiceObj();
        }
        else if (entOpt == 11)
        {
            Console.WriteLine("\n---- Display monthly & Yearly charged amounts (With RoomService) ----\n");
            MonthlyChargesWithRoomService();
        }
        else
        {
            Console.WriteLine("\nPlease enter a numeric value from 0 - 11\n");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"\nIncorrect values! {ex.Message} Please try again with numeric values from 0 - 11\n");
    }
}