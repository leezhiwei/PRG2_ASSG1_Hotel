// Zhi Wei does 2,4,6
// Jia Xian does 1,3,5
using PRG2_ASSG1_Hotel;

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
{
    List<Room> AvailableRms = new List<Room>();
    foreach (Guest g in guestList)
    {
        foreach (Room r in g.HotelStay.RoomList)
        {
            Room ro = r;
            if (ro.IsAvail)
            {
                if (ro is DeluxeRoom)
                {
                    DeluxeRoom dr = (DeluxeRoom)ro;
                    dr.AdditionalBed = false;
                    ro = dr;
                }
                else if (ro is StandardRoom)
                {
                    StandardRoom sr = (StandardRoom)ro;
                    sr.RequireBreakfast = false;
                    sr.RequireWifi = false;
                    ro = sr;
                }
                AvailableRms.Add(ro);
            }
        }
    }
    return AvailableRms;
}
void DisplayGuest(List<Guest> guestList)
 {
    foreach (Guest g in guestList)
    {
        Console.WriteLine(g.ToString());
    }
}

void DisplayRoom(List<Room> roomList)
{
    foreach (Room r in roomList)
    {
        Console.WriteLine(r.ToString());
    }
}

void RegisterGuest()
{
    List<List<string>> guestlist = new List<List<string>>(); // init string-list var
    string[] guestdata = File.ReadAllLines("Guests.csv");
    for (int i = 1; i < guestdata.Length; i++)
    {
        List<string> values = guestdata[i].Split(',').ToList();
        //Adding data into list
        guestlist.Add(values);
    }

    string gName;
    string gPN;
    Console.WriteLine("Please enter the following information\n");
    Console.Write("Guest name: ");
    gName = Console.ReadLine();
    Console.Write("Guest passport number: ");
    gPN = Console.ReadLine();

    Stay stay = new Stay();
    Membership membership = new Membership("Ordinary", 0); //Creating a new membership object with ordinary status and 0 points
    Guest guest = new Guest(gName, gPN, stay, membership); //Creating new guest object
    guestList.Add(guest); //Adding guest to guestList

    string data = gName + "," + gPN + "," + membership.Status + "," + membership.Points; //Adding guest information into data
    using (StreamWriter sw = new StreamWriter("Guests.csv", true))
    {
        sw.WriteLine(data); //Appending data to Guest.csv
    }
    Console.WriteLine($"\nGuest Registration for {gName} is Successful!");
}
List<Guest> DisplayCIn(List<Guest> guestList)
{
    List<Guest> returnlist = new List<Guest>();
    int count = 0;
    foreach (Guest g in guestList)
    {
        if (!g.IsCheckedin)
        {
            count++;
            Console.WriteLine($"{count}) {g.ToString()}");
            returnlist.Add(g);
        }
    }
    return returnlist;
}
List<Guest> DisplayGuestsCIned(List<Guest> guestList)
{
    List<Guest> alreadycheckedin = new List<Guest>();
    int count = 0;
    foreach (Guest g in guestList)
    {
        if (g.IsCheckedin)
        {
            alreadycheckedin.Add(g);
            count++;
            Console.WriteLine($"{count}) {g.ToString()}");
        }
    }
    return alreadycheckedin;
}
Room AvailRoomSel()
{
    Room finalobj = null;
    DisplayRmCIn(availrooms);
    Console.Write("Please select a room: ");
    int choice = -1;
    Room chosenr = null;
    try
    {
        choice = Convert.ToInt32(Console.ReadLine());
    }
    catch
    {
        Console.WriteLine("Please enter a number.");
        return null;
    }
    try
    {
        chosenr = availrooms[choice - 1];
    }
    catch
    {
        Console.WriteLine("You have chosen an invalid option, which is not in list.");
        return null;
    }
    if (chosenr is StandardRoom)
    {
        StandardRoom r = (StandardRoom)chosenr;
        Console.Write("Do you want to have choice of WiFi (Y/N) ");
        string ch = Console.ReadLine();
        ch = ch.ToUpper();
        if (ch == "Y")
        {
            r.RequireWifi = true;
        }
        else if (ch == "N")
        {
            r.RequireWifi = false;
        }
        else
        {
            Console.WriteLine("You have typed in an invalid option.");
            return null;
        }
        Console.Write("Do you want to have choice of Breakfast (Y/N) ");
        ch = Console.ReadLine();
        ch = ch.ToUpper();
        if (ch == "Y")
        {
            r.RequireBreakfast = true;
        }
        else if (ch == "N")
        {
            r.RequireBreakfast = false;
        }
        else
        {
            Console.WriteLine("You have typed in an invalid option.");
            return null;
        }
        availrooms.RemoveAt(choice - 1);
        r.IsAvail = false;
        finalobj = r;
    }
    else if (chosenr is DeluxeRoom)
    {
        DeluxeRoom r = (DeluxeRoom)chosenr;
        Console.Write("Do you want to have additional beds (Y/N) ");
        string ch = Console.ReadLine();
        ch = ch.ToUpper();
        if (ch == "Y")
        {
            r.AdditionalBed = true;
        }
        else if (ch == "N")
        {
            r.AdditionalBed = false;
        }
        else
        {
            Console.WriteLine("You have typed in an invalid option.");
            return null;
        }
        availrooms.RemoveAt(choice - 1);
        r.IsAvail = false;
        finalobj = r;
    }
    return finalobj;
}
void DisplayRmCIn(List<Room> rlist)
{
    int count = 0;
    foreach (Room r in rlist)
    {
        count++;
        Console.WriteLine($"{count}) {r.ToString()}");
    }
}
void ShowAvailRoom(List<Room> rlist)
{
    foreach (Room r in rlist)
    {
        Console.WriteLine(r.ToString());
    }
}
void CheckIn(List<Guest> glist)
{
    int num = 0;
    while (true)
    {
        Room finalobj = null;
        Guest pickedguest = null;
        List <Guest> notcheckin = new List<Guest>(DisplayCIn(glist));
        DateTime cindate = DateTime.Now;
        DateTime coutdate = DateTime.Now;
        Console.Write("Enter the number in which you want to check in: ");
        try
        {
            num = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Please enter a number.");
            continue;
        }
        int index = num - 1;
        if (index < 0)
        {
            Console.WriteLine("Invalid number.");
            continue;
        }
        try
        {
            pickedguest = notcheckin[index];
        }
        catch
        {
            Console.WriteLine("Out of range of list");
            continue;
        }
        Console.Write("Please enter date of Check-In: ");
        try
        {
            cindate = Convert.ToDateTime(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid date format.");
            continue;
        }
        Console.Write("Please enter date of Check-Out: ");
        try
        {
            coutdate = Convert.ToDateTime(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid date format.");
            continue;
        }
        Stay stay = new Stay(cindate, coutdate);
        finalobj = AvailRoomSel();
        if (finalobj is null)
        {
            continue;
        }
        stay.AddRoom(finalobj);
        Console.Write("Do you want to add another room? (Y/N) ");
        string o = Console.ReadLine();
        o = o.ToUpper();
        if (o == "Y")
        {
            finalobj = AvailRoomSel();
            if (finalobj is null)
            {
                continue;
            }
            stay.AddRoom(finalobj);
        }
        else if (o == "N")
        {
            Console.WriteLine(); // Do nothing continue to next part
        }
        else
        {
            Console.WriteLine("You have selected invalid option.");
        }
        pickedguest.HotelStay = stay;
        pickedguest.IsCheckedin = true;
        Console.WriteLine("You have successfully checked-in!");
        break;
    }
}
void ExtendStay()
{
    List<Guest> checkedin = DisplayGuestsCIned(guestList);
    Guest pickedg = null;
    int choice = -1;
    Console.Write("Please enter which guest to extend stay: ");
    try
    {
        choice = Convert.ToInt32(Console.ReadLine());
    }
    catch
    {
        Console.WriteLine("Please enter a number.");
        return;
    }
    pickedg = checkedin[choice - 1];
    if (pickedg.IsCheckedin == false)
    {
        Console.WriteLine("You are not checked in yet.");
        return;
    }
    Stay s = pickedg.HotelStay;
    Console.Write("How many days do you want to extend stay by? ");
    int days = 0;
    try
    {
        days = Convert.ToInt32(Console.ReadLine());
    }
    catch
    {
        Console.WriteLine("Please enter a number.");
        return;
    }
    s.CheckOutDate = s.CheckOutDate.AddDays(days);
    guestList[choice - 1] = pickedg;
    Console.WriteLine($"Date has been updated to {s.CheckOutDate.ToString()}");
}

int entOpt;
InitData();
while (true)
{
    try
    {
        Console.WriteLine("------ Hotel Guest Management System ------");
        Console.WriteLine("[1]. Display all guests\n[2]. Display all available rooms\n[3]. Register guest\n[4]. Check-in guest\n[5]. Display all details for guest\n[6]. Extend days for stay\n[0]. Quit Hotal Guest Management System");
        Console.Write("-------------------------------------------\nPlease enter your option: ");
        entOpt = Convert.ToInt32(Console.ReadLine());
        if (entOpt == 0)
        {
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
            Console.WriteLine("\n------ Displaying all details for Guest ------\n");
        }
        else if (entOpt == 6)
        {
            Console.WriteLine("\n------ Extending days for stay ------\n");
            ExtendStay();
        }
        else
        {
            Console.WriteLine("\nPlease enter a numeric value from 0 - 6");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"\nIncorrect values! {ex.Message} Please try again with numeric values from 0 - 6");
    }
    catch
    {
        Console.WriteLine($"\nPlease enter the correct value format and try again. The option only accept numeric values from 0 - 6");
    }
}





